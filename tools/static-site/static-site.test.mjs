import assert from "node:assert/strict";
import {
    existsSync,
    mkdtempSync,
    mkdirSync,
    readFileSync,
    rmSync,
    writeFileSync,
} from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import test from "node:test";
import {
    buildReport,
    decideRun,
    escapeHtml,
    finalizeSite,
} from "./static-site.mjs";

function temporaryDirectory() {
    return mkdtempSync(join(tmpdir(), "raccord-static-site-"));
}

test("escapes HTML metacharacters in dynamic values", () => {
    assert.equal(
        escapeHtml(`<script a="&">'`),
        "&lt;script a=&quot;&amp;&quot;&gt;&#39;",
    );
});

test("builds an empty report that explains missing journey evidence", () => {
    const root = temporaryDirectory();
    try {
        const report = buildReport({
            artifactsDirectory: join(root, "missing"),
            outputDirectory: join(root, "report"),
            metadata: {
                runId: "7",
                sha: "abc123",
                runUrl: "https://github.com/example/run/7",
                date: "2026-10-13T00:00:00Z",
                overallStatus: "success",
            },
        });
        const html = readFileSync(join(root, "report", "index.html"), "utf8");
        assert.equal(report.journeys.length, 0);
        assert.equal(report.workflowStatus, "success");
        assert.equal(report.overallStatus, "failure");
        assert.match(html, /No journey result was recorded/);
        assert.match(html, /Workflow result: <strong class="status failure">failure/);
        assert.match(html, /No journey evidence captured/);
        assert.match(html, /id="journey-nav"/);
        assert.match(html, /mobile-nav-toggle/);
        assert.match(html, /data-appearance="dark"/);
        assert.match(html, /data-appearance="light"/);
        assert.match(html, /abc123/);
    } finally {
        rmSync(root, { recursive: true, force: true });
    }
});

test("escapes report metadata before inserting it into the page", () => {
    const root = temporaryDirectory();
    try {
        buildReport({
            artifactsDirectory: join(root, "missing"),
            outputDirectory: join(root, "report"),
            metadata: {
                sha: "<script>alert(1)</script>",
                runUrl: "https://example.test/?a=1&b=2",
            },
        });
        const html = readFileSync(join(root, "report", "index.html"), "utf8");
        assert.match(html, /&lt;script&gt;alert\(1\)&lt;\/script&gt;/);
        assert.doesNotMatch(html, /<script>alert\(1\)/);
        assert.match(html, /a=1&amp;b=2/);
    } finally {
        rmSync(root, { recursive: true, force: true });
    }
});

test("groups passing, failing, and flaky journeys with linked evidence", () => {
    const root = temporaryDirectory();
    try {
        const artifacts = join(root, "artifacts");
        const output = join(root, "report");
        mkdirSync(artifacts);
        for (const [name, status] of [
            ["customer-register", "passed"],
            ["invoice-finalize", "failed"],
            ["project-billable-line", "flaky"],
        ]) {
            writeFileSync(
                join(artifacts, `${name}.json`),
                JSON.stringify({ name, status }),
            );
        }
        writeFileSync(
            join(artifacts, "customer-register-checkpoint.png"),
            "png",
        );
        writeFileSync(join(artifacts, "customer-register.webm"), "webm");
        writeFileSync(join(artifacts, "invoice-finalize-failure.png"), "png");
        writeFileSync(join(artifacts, "invoice-finalize-failure.zip"), "zip");
        writeFileSync(
            join(artifacts, "project-billable-line-checkpoint.png"),
            "png",
        );

        const report = buildReport({
            artifactsDirectory: artifacts,
            outputDirectory: output,
            metadata: {},
        });
        const html = readFileSync(join(output, "index.html"), "utf8");
        assert.deepEqual(report.counts, {
            passed: 1,
            failed: 1,
            flaky: 1,
            unknown: 0,
        });
        assert.match(html, /<video controls/);
        assert.match(html, /Viewport/);
        assert.match(html, /journey-search/);
        assert.match(html, /\(desktop\)/);
        assert.match(html, /assets\/invoice-finalize-failure\.png/);
        assert.match(html, /Playwright trace/);
        assert.equal(
            readFileSync(
                join(output, "assets", "customer-register.webm"),
                "utf8",
            ),
            "webm",
        );
    } finally {
        rmSync(root, { recursive: true, force: true });
    }
});

test("runs scheduled and post-merge E2E only when main has advanced beyond the last report", async () => {
    const now = new Date().toISOString();
    let latestSha = "same-sha";
    const server = await import("node:http").then(({ createServer }) =>
        createServer((request, response) => {
            if (request.url === "/site-manifest.json") {
                response.writeHead(200, { "content-type": "application/json" });
                response.end(JSON.stringify({ files: [] }));
            } else if (request.url === "/e2e/history.json") {
                response.writeHead(200, { "content-type": "application/json" });
                response.end(
                    JSON.stringify({
                        reports: [{ sha: latestSha, date: now }],
                    }),
                );
            } else {
                response.writeHead(404).end();
            }
        }),
    );
    await new Promise((resolve) => server.listen(0, "127.0.0.1", resolve));
    try {
        const address = server.address();
        const baseUrl = `http://127.0.0.1:${address.port}`;
        const unchanged = await decideRun(
            baseUrl,
            "same-sha",
            "schedule",
            "false",
        );
        const advanced = await decideRun(
            baseUrl,
            "new-sha",
            "schedule",
            "false",
        );
        assert.equal(unchanged.runE2e, false);
        assert.equal(unchanged.publish, false);
        assert.equal(advanced.runE2e, true);
        assert.equal(advanced.publish, true);

        const mergedCommit = await decideRun(
            baseUrl,
            "merged-sha",
            "push",
            "false",
        );
        assert.equal(mergedCommit.runE2e, true);
        assert.equal(mergedCommit.publish, true);

        latestSha = "merged-sha";
        const duplicatePush = await decideRun(
            baseUrl,
            "merged-sha",
            "push",
            "false",
        );
        assert.equal(duplicatePush.runE2e, false);
        assert.equal(duplicatePush.publish, true);

        const alreadyReported = await decideRun(
            baseUrl,
            "merged-sha",
            "schedule",
            "false",
        );
        assert.equal(alreadyReported.runE2e, false);
        assert.equal(alreadyReported.publish, false);
    } finally {
        await new Promise((resolve, reject) =>
            server.close((error) => (error ? reject(error) : resolve())),
        );
    }
});

test("finalizes a stable latest link and bounds retained report history", () => {
    const root = temporaryDirectory();
    const priorRun = process.env.E2E_RUN_ID;
    try {
        const artifacts = join(root, "artifacts-22");
        mkdirSync(artifacts, { recursive: true });
        writeFileSync(
            join(artifacts, "customer-register.json"),
            JSON.stringify({
                name: "customer-register",
                status: "passed",
                viewport: "mobile",
            }),
        );
        writeFileSync(join(artifacts, "customer-register-checkpoint.png"), "png");
        buildReport({
            artifactsDirectory: artifacts,
            outputDirectory: join(root, "e2e", "reports", "22"),
            metadata: {
                runId: "22",
                sha: "deadbeef",
                overallStatus: "success",
                runUrl: "https://github.com/example/run/22",
                date: new Date().toISOString(),
            },
        });
        writeFileSync(join(root, "e2e", "reports", "22", "index.html"), "legacy report page");
        const olderArtifacts = join(root, "artifacts-21");
        mkdirSync(olderArtifacts, { recursive: true });
        writeFileSync(
            join(olderArtifacts, "project-list.json"),
            JSON.stringify({
                name: "project-list",
                status: "passed",
                viewport: "desktop",
            }),
        );
        mkdirSync(join(root, "e2e", "reports", "21"), { recursive: true });
        writeFileSync(
            join(root, "e2e", "reports", "21", "index.html"),
            "<main>No journey result was recorded. workflow result: <strong>success</strong></main>",
        );
        writeFileSync(
            join(root, "e2e", "reports", "21", "report.json"),
            JSON.stringify({
                runId: "21",
                overallStatus: "success",
                journeys: [],
            }),
        );
        writeFileSync(
            join(root, "e2e", "history.json"),
            JSON.stringify({
                reports: [
                    {
                        runId: "21",
                        date: new Date(Date.now() - 60_000).toISOString(),
                        sha: "cafebabe",
                        status: "success",
                        runUrl: "https://github.com/example/run/21",
                        path: "reports/21/index.html",
                        files: ["reports/21/index.html"],
                    },
                ],
            }),
        );
        process.env.E2E_RUN_ID = "22";
        finalizeSite(root);
        const latest = readFileSync(join(root, "e2e", "latest.html"), "utf8");
        const manifest = JSON.parse(
            readFileSync(join(root, "site-manifest.json"), "utf8"),
        );
        assert.match(latest, /Raccord · E2E/);
        assert.match(latest, /data-appearance/);
        assert.match(latest, /mobile-nav-toggle/);
        assert.match(latest, /reports\/21\/index\.html/);
        assert.match(latest, /src="\.\.\/assets\/raccord-mark\.svg"/);
        assert.match(
            latest,
            /src="reports\/22\/assets\/customer-register-checkpoint\.png"/,
        );
        assert.doesNotMatch(latest, /http-equiv="refresh"/);
        assert.ok(manifest.files.includes("e2e/reports/22/index.html"));
        assert.ok(manifest.files.includes("assets/raccord-mark.svg"));
        assert.equal(manifest.files.includes("architecture"), false);
        const landing = readFileSync(join(root, "index.html"), "utf8");
        assert.match(landing, /How Raccord is built, in the open/);
        assert.match(landing, /href="e2e\/latest\.html"/);
        assert.match(landing, /assets\/raccord-mark\.svg/);
        assert.match(landing, /data-local-time datetime="/);
        assert.match(landing, /data-appearance-set="auto"/);
        assert.match(landing, /data-appearance-set="dark"/);
        assert.match(landing, /localStorage\.setItem\('raccord-appearance'/);
        assert.match(landing, /journey recorded · 1 passed/);
        assert.match(landing, /max-width:900px/);
        assert.match(landing, /max-width:600px/);
        assert.match(landing, /prefers-reduced-motion:reduce/);
        const landingScript = landing.match(/<script>([\s\S]*?)<\/script>/)?.[1];
        assert.ok(landingScript);
        assert.doesNotThrow(() => new Function(landingScript));
        const reportsIndex = readFileSync(
            join(root, "e2e", "index.html"),
            "utf8",
        );
        assert.match(reportsIndex, /id="journey-nav"/);
        assert.doesNotMatch(reportsIndex, /url=latest\.html/);
        assert.doesNotMatch(reportsIndex, /All retained reports/);
        const report = readFileSync(
            join(root, "e2e", "reports", "22", "index.html"),
            "utf8",
        );
        assert.match(report, /Earlier runs/);
        assert.match(report, /archive-nav/);
        assert.match(report, /\.\.\/21\/index\.html/);
        const olderReport = readFileSync(
            join(root, "e2e", "reports", "21", "index.html"),
            "utf8",
        );
        assert.match(olderReport, /id="journey-nav"/);
        assert.match(olderReport, /data-appearance="dark"/);
        assert.doesNotMatch(olderReport, /legacy older report page/);
        assert.match(report, /failure/);
        assert.match(report, /failure/);
        const archivedReport = JSON.parse(
            readFileSync(
                join(root, "e2e", "reports", "21", "report.json"),
                "utf8",
            ),
        );
        assert.equal(archivedReport.workflowStatus, "success");
        assert.equal(archivedReport.overallStatus, "failure");
        assert.match(
            readFileSync(
                join(root, "e2e", "reports", "21", "index.html"),
                "utf8",
            ),
            /Workflow result: <strong class="status failure">failure<\/strong>/,
        );
    } finally {
        if (priorRun === undefined) delete process.env.E2E_RUN_ID;
        else process.env.E2E_RUN_ID = priorRun;
        rmSync(root, { recursive: true, force: true });
    }
});

test("labels an empty latest run as missing evidence instead of successful", () => {
    const root = temporaryDirectory();
    try {
        const runId = "empty-run";
        const date = "2026-10-15T05:48:53Z";
        const reportDirectory = join(root, "e2e", "reports", runId);
        mkdirSync(reportDirectory, { recursive: true });
        writeFileSync(
            join(reportDirectory, "report.json"),
            JSON.stringify({
                runId,
                date,
                sha: "0123456789abcdef",
                runUrl: "https://github.com/example/run/empty-run",
                workflowStatus: "success",
                overallStatus: "failure",
                counts: { passed: 0, failed: 0, flaky: 0, unknown: 0 },
                journeys: [],
                files: [],
            }),
        );
        writeFileSync(
            join(root, "e2e", "history.json"),
            JSON.stringify({
                reports: [
                    {
                        runId,
                        date,
                        sha: "0123456789abcdef",
                        status: "failure",
                        runUrl: "https://github.com/example/run/empty-run",
                        path: `reports/${runId}/index.html`,
                        files: [],
                    },
                ],
            }),
        );

        finalizeSite(root);

        const landing = readFileSync(join(root, "index.html"), "utf8");
        assert.match(landing, /No journeys were recorded/);
        assert.match(landing, /workflow reported success/);
        assert.match(landing, />No results</);
        assert.match(landing, /2026-10-15T05:48:53\.000Z UTC/);
        assert.match(landing, /new Intl\.DateTimeFormat\(undefined/);
    } finally {
        rmSync(root, { recursive: true, force: true });
    }
});

test("prunes aged and excess report directories while preserving the recent window", () => {
    const root = temporaryDirectory();
    const priorRun = process.env.E2E_RUN_ID;
    try {
        const reports = join(root, "e2e", "reports");
        mkdirSync(reports, { recursive: true });
        const history = [];
        for (let index = 0; index < 16; index += 1) {
            const runId = String(100 + index);
            mkdirSync(join(reports, runId));
            writeFileSync(
                join(reports, runId, "index.html"),
                `report ${runId}`,
            );
            const date = new Date(
                Date.now() - index * 24 * 60 * 60 * 1000,
            ).toISOString();
            writeFileSync(
                join(reports, runId, "report.json"),
                JSON.stringify({
                    runId,
                    date,
                    sha: runId,
                    overallStatus: "success",
                    runUrl: "#",
                    files: ["index.html"],
                }),
            );
            history.push({
                runId,
                date,
                sha: runId,
                status: "success",
                runUrl: "#",
                path: `reports/${runId}/index.html`,
                files: [`reports/${runId}/index.html`],
            });
        }
        writeFileSync(
            join(root, "e2e", "history.json"),
            JSON.stringify({ reports: history }),
        );
        delete process.env.E2E_RUN_ID;
        finalizeSite(root);
        const retained = JSON.parse(
            readFileSync(join(root, "e2e", "history.json"), "utf8"),
        ).reports;
        assert.equal(retained.length, 14);
        assert.equal(retained[0].runId, "100");
        assert.equal(retained.at(-1).runId, "113");
        assert.equal(existsSync(join(reports, "114")), false);
        assert.equal(existsSync(join(reports, "115")), false);
    } finally {
        if (priorRun === undefined) delete process.env.E2E_RUN_ID;
        else process.env.E2E_RUN_ID = priorRun;
        rmSync(root, { recursive: true, force: true });
    }
});
