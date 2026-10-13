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
                overallStatus: "failure",
            },
        });
        const html = readFileSync(join(root, "report", "index.html"), "utf8");
        assert.equal(report.journeys.length, 0);
        assert.match(html, /No journey result was recorded/);
        assert.match(html, /workflow result: <strong>failure/);
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

test("runs nightly only when main has advanced beyond the last report", async () => {
    const now = new Date().toISOString();
    const server = await import("node:http").then(({ createServer }) =>
        createServer((request, response) => {
            if (request.url === "/site-manifest.json") {
                response.writeHead(200, { "content-type": "application/json" });
                response.end(JSON.stringify({ files: [] }));
            } else if (request.url === "/e2e/history.json") {
                response.writeHead(200, { "content-type": "application/json" });
                response.end(
                    JSON.stringify({
                        reports: [{ sha: "same-sha", date: now }],
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
        mkdirSync(join(root, "e2e", "reports", "22"), { recursive: true });
        writeFileSync(
            join(root, "e2e", "reports", "22", "index.html"),
            "report",
        );
        writeFileSync(
            join(root, "e2e", "reports", "22", "report.json"),
            JSON.stringify({
                runId: "22",
                date: new Date().toISOString(),
                sha: "deadbeef",
                overallStatus: "success",
                runUrl: "https://github.com/example/run/22",
                files: ["index.html"],
            }),
        );
        process.env.E2E_RUN_ID = "22";
        finalizeSite(root);
        const latest = readFileSync(join(root, "e2e", "latest.html"), "utf8");
        const manifest = JSON.parse(
            readFileSync(join(root, "site-manifest.json"), "utf8"),
        );
        assert.match(latest, /reports\/22\/index\.html/);
        assert.ok(manifest.files.includes("e2e/reports/22/index.html"));
        assert.equal(manifest.files.includes("architecture"), false);
    } finally {
        if (priorRun === undefined) delete process.env.E2E_RUN_ID;
        else process.env.E2E_RUN_ID = priorRun;
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
