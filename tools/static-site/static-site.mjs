#!/usr/bin/env node
import {
    copyFileSync,
    mkdirSync,
    readFileSync,
    readdirSync,
    rmSync,
    statSync,
    writeFileSync,
    createWriteStream,
} from "node:fs";
import { dirname, join, relative, resolve, sep } from "node:path";
import { pathToFileURL } from "node:url";
import { Readable } from "node:stream";
import { pipeline } from "node:stream/promises";

const MAX_REPORTS = 14;
const MAX_AGE_DAYS = 30;
const MAX_SITE_BYTES = 800 * 1024 * 1024;
const SAFE_PATH = /^[A-Za-z0-9._/-]+$/;

export function escapeHtml(value) {
    return String(value).replace(
        /[&<>"']/g,
        (character) =>
            ({
                "&": "&amp;",
                "<": "&lt;",
                ">": "&gt;",
                '"': "&quot;",
                "'": "&#39;",
            })[character],
    );
}

function safeRelativePath(path) {
    if (
        !SAFE_PATH.test(path) ||
        path.startsWith("/") ||
        path.split("/").includes("..")
    ) {
        throw new Error(`Unsafe site path: ${path}`);
    }
    return path;
}

async function fetchFile(url, destination) {
    const response = await fetch(url);
    if (!response.ok)
        throw new Error(`Could not restore ${url}: HTTP ${response.status}`);
    mkdirSync(dirname(destination), { recursive: true });
    await pipeline(
        Readable.fromWeb(response.body),
        createWriteStream(destination),
    );
}

export async function restoreSite(baseUrl, siteDirectory) {
    const root = resolve(siteDirectory);
    mkdirSync(root, { recursive: true });
    const manifestUrl = new URL(
        "site-manifest.json",
        `${baseUrl.replace(/\/$/, "")}/`,
    );
    let response;
    try {
        response = await fetch(manifestUrl);
    } catch (error) {
        throw new Error(
            `Could not read the current Pages site at ${manifestUrl}: ${error.message}`,
        );
    }
    if (response.status === 404) return false;
    if (!response.ok)
        throw new Error(
            `Could not read ${manifestUrl}: HTTP ${response.status}`,
        );
    const manifest = await response.json();
    if (!Array.isArray(manifest.files))
        throw new Error("Invalid site manifest: files must be an array.");

    for (const rawPath of manifest.files) {
        const path = safeRelativePath(rawPath);
        const destination = resolve(root, path);
        if (!destination.startsWith(`${root}${sep}`))
            throw new Error(`Site path escapes output: ${path}`);
        await fetchFile(
            new URL(path, `${baseUrl.replace(/\/$/, "")}/`),
            destination,
        );
    }
    writeFileSync(
        join(root, "site-manifest.json"),
        JSON.stringify(manifest, null, 2),
    );
    return true;
}

export async function decideRun(baseUrl, sha, eventName, force) {
    const base = `${baseUrl.replace(/\/$/, "")}/`;
    const [manifestResponse, historyResponse] = await Promise.all([
        fetch(new URL("site-manifest.json", base)),
        fetch(new URL("e2e/history.json", base)),
    ]);
    if (!manifestResponse.ok && manifestResponse.status !== 404) {
        throw new Error(
            `Could not read site manifest: HTTP ${manifestResponse.status}`,
        );
    }
    const hasSite = manifestResponse.ok;
    const history = historyResponse.ok
        ? await historyResponse.json()
        : { reports: [] };
    if (!historyResponse.ok && historyResponse.status !== 404) {
        throw new Error(
            `Could not read E2E history: HTTP ${historyResponse.status}`,
        );
    }
    const reports = Array.isArray(history.reports) ? history.reports : [];
    const latestSha = reports[0]?.sha ?? "";
    const runE2e =
        eventName === "schedule"
            ? latestSha !== sha
            : eventName === "workflow_dispatch"
              ? force === "true"
              : false;
    const cutoff = Date.now() - MAX_AGE_DAYS * 24 * 60 * 60 * 1000;
    const needsPrune =
        reports.some((report) => Date.parse(report.date) < cutoff) ||
        reports.length > MAX_REPORTS;
    const publish = eventName === "push" || runE2e || needsPrune || !hasSite;
    return { runE2e, publish, needsPrune, hasSite, latestSha };
}

function getFiles(directory) {
    if (
        !directory ||
        !statSync(directory, { throwIfNoEntry: false })?.isDirectory()
    )
        return [];
    return readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
        const fullPath = join(directory, entry.name);
        if (entry.isDirectory()) return getFiles(fullPath);
        return entry.isFile() ? [fullPath] : [];
    });
}

function displayName(name) {
    return name
        .replace(/-/g, " ")
        .replace(/\b[a-z]/g, (letter) => letter.toUpperCase());
}

export function buildReport({ artifactsDirectory, outputDirectory, metadata }) {
    mkdirSync(outputDirectory, { recursive: true });
    const assetsDirectory = join(outputDirectory, "assets");
    mkdirSync(assetsDirectory, { recursive: true });
    const files = getFiles(resolve(artifactsDirectory));
    const groups = new Map();

    for (const path of files) {
        const filename = path.split(sep).at(-1);
        if (!/^[A-Za-z0-9][A-Za-z0-9._-]*$/.test(filename)) continue;
        const result = filename.match(/^([A-Za-z0-9-]+)\.json$/);
        if (result) {
            try {
                const value = JSON.parse(readFileSync(path, "utf8"));
                if (
                    /^[A-Za-z0-9-]+$/.test(value.name) &&
                    ["passed", "failed", "flaky"].includes(value.status)
                ) {
                    groups.set(value.name, {
                        name: value.name,
                        status: value.status,
                        viewport: value.viewport ?? "desktop",
                        assets: [],
                    });
                }
            } catch {
                // A truncated result file should not hide the artifacts or other journeys.
            }
        }
    }

    const assetRecords = [];
    for (const path of files) {
        const filename = path.split(sep).at(-1);
        const match = filename.match(
            /^([A-Za-z0-9-]+?)(-failure|-checkpoint)?\.(png|webm|zip)$/,
        );
        if (!match) continue;
        const [, journeyName, variant = "", extension] = match;
        const group = groups.get(journeyName) ?? {
            name: journeyName,
            status: variant === "-failure" ? "failed" : "unknown",
            viewport: "desktop",
            assets: [],
        };
        groups.set(journeyName, group);
        const assetName = `${journeyName}${variant}.${extension}`;
        const destination = join(assetsDirectory, assetName);
        copyFileSync(path, destination);
        const type =
            extension === "png"
                ? variant === "-failure"
                    ? "failure screenshot"
                    : "screenshot"
                : extension === "webm"
                  ? "video"
                  : "trace";
        const asset = { path: `assets/${assetName}`, type, name: assetName };
        group.assets.push(asset);
        assetRecords.push(asset);
    }

    const journeyRecords = [...groups.values()].sort((left, right) =>
        left.name.localeCompare(right.name),
    );
    const counts = Object.fromEntries(
        ["passed", "failed", "flaky", "unknown"].map((status) => [
            status,
            journeyRecords.filter((journey) => journey.status === status)
                .length,
        ]),
    );
    const journeysHtml = journeyRecords
        .map((journey) => {
            const assetsHtml = journey.assets
                .map((asset) => {
                    const path = escapeHtml(asset.path);
                    const name = escapeHtml(asset.name);
                    if (asset.type === "video") {
                        return `<article class="asset"><video controls preload="none"><source src="${path}" type="video/webm"></video><a href="${path}">${name}</a></article>`;
                    }
                    if (asset.type.includes("screenshot")) {
                        return `<a class="image" href="${path}"><img loading="lazy" src="${path}" alt="${escapeHtml(asset.type)} for ${escapeHtml(displayName(journey.name))}"><span>${escapeHtml(asset.type)}</span></a>`;
                    }
                    return `<p><a href="${path}">Playwright trace: ${name}</a> (download and open with the trace viewer)</p>`;
                })
                .join("");
            return `<section class="journey" data-status="${journey.status}" data-viewport="${escapeHtml(journey.viewport)}" data-name="${escapeHtml(journey.name.toLowerCase())}"><h2><span class="badge ${journey.status}">${journey.status}</span> ${escapeHtml(displayName(journey.name))} <small>(${escapeHtml(journey.viewport)})</small></h2><div class="assets">${assetsHtml || "<p>No screenshot, video, or trace was produced.</p>"}</div></section>`;
        })
        .join("");

    const runUrl = escapeHtml(metadata.runUrl ?? "#");
    const sha = escapeHtml(metadata.sha ?? "unknown");
    const date = escapeHtml(metadata.date ?? new Date().toISOString());
    const overall = escapeHtml(metadata.overallStatus ?? "unknown");
    const emptyMessage =
        journeyRecords.length === 0
            ? "No journey result was recorded. The runner may have failed before test discovery; inspect the linked workflow run."
            : "";
    const html = `<!doctype html><html lang="en"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="robots" content="noindex,nofollow"><title>Raccord E2E · ${date}</title><style>
*{box-sizing:border-box}body{margin:0;background:#10151d;color:#e6edf3;font:16px/1.5 system-ui,sans-serif;padding:clamp(1rem,4vw,3rem)}main{max-width:1100px;margin:auto}a{color:#80c8ff}h1{margin-bottom:.2rem}.meta,.summary{color:#aeb8c4}.summary{display:flex;flex-wrap:wrap;gap:.6rem;margin:1.2rem 0}.summary span,.badge{border:1px solid #364352;border-radius:999px;padding:.2rem .65rem}.journey{border-top:1px solid #364352;padding:1.25rem 0}.journey h2{font-size:1.2rem}.badge{font-size:.75rem;text-transform:uppercase;margin-right:.5rem}.passed{color:#67d99c}.failed{color:#ff827d}.flaky{color:#f5c451}.unknown{color:#b4c4d5}.assets{display:grid;grid-template-columns:repeat(auto-fit,minmax(min(100%,320px),1fr));gap:1rem}.image,.asset{min-width:0}.image img,video{display:block;width:100%;height:auto;border:1px solid #364352;border-radius:.5rem;background:#090d12}.image span{display:block;color:#aeb8c4;font-size:.85rem;margin-top:.3rem}.run{word-break:break-all}.empty{padding:1rem;border:1px solid #855f24;border-radius:.5rem}</style></head><body><main><h1>Raccord nightly browser journeys</h1><p class="meta">Tested commit <code class="run">${sha}</code> · ${date} · workflow result: <strong>${overall}</strong></p><p><a href="${runUrl}">Open this GitHub Actions run</a> · <a href="../../index.html">All retained reports</a> · <a href="../../../index.html">Raccord snapshots</a></p><div class="summary">${Object.entries(
        counts,
    )
        .map(
            ([status, count]) =>
                `<span class="${status}">${count} ${status}</span>`,
        )
        .join(
            "",
        )}</div>${emptyMessage ? `<p class="empty">${escapeHtml(emptyMessage)}</p>` : ""}${journeysHtml}</main></body></html>`;
    const summaryEnd =
        html.indexOf("</div>", html.indexOf('<div class="summary">')) + 6;
    const filters =
        '<div class="filters" style="display:flex;gap:.75rem;flex-wrap:wrap;margin:1rem 0"><label>Journey <input id="journey-search" type="search" placeholder="Filter by name"></label><label>Status <select id="status-filter"><option value="">All</option><option>passed</option><option>failed</option><option>flaky</option><option>unknown</option></select></label><label>Viewport <select id="viewport-filter"><option value="">All</option><option value="desktop">Desktop</option></select></label></div>';
    const script = `<script>const controls=[document.querySelector('#journey-search'),document.querySelector('#status-filter'),document.querySelector('#viewport-filter')];function filterJourneys(){const [name,status,viewport]=controls.map(control=>control.value.toLowerCase());for(const card of document.querySelectorAll('.journey'))card.hidden=!(card.dataset.name.includes(name)&&(!status||card.dataset.status===status)&&(!viewport||card.dataset.viewport===viewport))}controls.forEach(control=>control.addEventListener('input',filterJourneys));</script>`;
    const galleryHtml =
        `${html.slice(0, summaryEnd)}${filters}${html.slice(summaryEnd)}`.replace(
            "</main></body></html>",
            `${script}</main></body></html>`,
        );
    writeFileSync(join(outputDirectory, "index.html"), galleryHtml);
    const reportManifest = {
        ...metadata,
        counts,
        journeys: journeyRecords,
        files: ["index.html", ...assetRecords.map((asset) => asset.path)],
    };
    writeFileSync(
        join(outputDirectory, "report.json"),
        JSON.stringify(reportManifest, null, 2),
    );
    return reportManifest;
}

function directoryBytes(directory) {
    return getFiles(directory).reduce(
        (total, path) => total + statSync(path).size,
        0,
    );
}

export function finalizeSite(siteDirectory) {
    const root = resolve(siteDirectory);
    const reportsDirectory = join(root, "e2e", "reports");
    const historyPath = join(root, "e2e", "history.json");
    let history = [];
    try {
        history = JSON.parse(readFileSync(historyPath, "utf8")).reports ?? [];
    } catch {
        history = [];
    }

    const currentReportPath = join(
        reportsDirectory,
        process.env.E2E_RUN_ID ?? "",
    );
    if (
        process.env.E2E_RUN_ID &&
        statSync(currentReportPath, { throwIfNoEntry: false })?.isDirectory()
    ) {
        const report = JSON.parse(
            readFileSync(join(currentReportPath, "report.json"), "utf8"),
        );
        history = history.filter((item) => item.runId !== report.runId);
        history.push({
            runId: report.runId,
            date: report.date,
            sha: report.sha,
            status: report.overallStatus,
            runUrl: report.runUrl,
            path: `reports/${report.runId}/index.html`,
            files: report.files.map(
                (path) => `reports/${report.runId}/${path}`,
            ),
        });
    }

    const cutoff = Date.now() - MAX_AGE_DAYS * 24 * 60 * 60 * 1000;
    history = history
        .filter((report) => Date.parse(report.date) >= cutoff)
        .sort((left, right) => Date.parse(right.date) - Date.parse(left.date))
        .slice(0, MAX_REPORTS);
    const retainedIds = new Set(history.map((report) => String(report.runId)));
    if (statSync(reportsDirectory, { throwIfNoEntry: false })?.isDirectory()) {
        for (const item of readdirSync(reportsDirectory, {
            withFileTypes: true,
        })) {
            if (item.isDirectory() && !retainedIds.has(item.name)) {
                rmSync(join(reportsDirectory, item.name), {
                    recursive: true,
                    force: true,
                });
            }
        }
    }
    while (
        directoryBytes(reportsDirectory) > MAX_SITE_BYTES &&
        history.length > 1
    ) {
        const removed = history.pop();
        rmSync(join(reportsDirectory, String(removed.runId)), {
            recursive: true,
            force: true,
        });
    }
    if (directoryBytes(reportsDirectory) > MAX_SITE_BYTES) {
        throw new Error(
            "The newest E2E report alone exceeds the 800 MiB site evidence budget.",
        );
    }

    mkdirSync(join(root, "e2e"), { recursive: true });
    writeFileSync(historyPath, JSON.stringify({ reports: history }, null, 2));
    const latest = history[0];
    const latestHtml = latest
        ? `<!doctype html><html lang="en"><meta charset="utf-8"><meta name="robots" content="noindex,nofollow"><meta http-equiv="refresh" content="0;url=${escapeHtml(latest.path)}"><title>Latest Raccord E2E report</title><p><a href="${escapeHtml(latest.path)}">Open the latest nightly report (${escapeHtml(latest.date)})</a></p></html>`
        : '<!doctype html><html lang="en"><meta charset="utf-8"><title>Raccord E2E</title><p>No nightly report has been published yet.</p></html>';
    writeFileSync(join(root, "e2e", "latest.html"), latestHtml);
    const list = history
        .map(
            (report) =>
                `<li><a href="${escapeHtml(report.path)}">${escapeHtml(report.date)} · ${escapeHtml(report.sha.slice(0, 8))} · ${escapeHtml(report.status)}</a></li>`,
        )
        .join("");
    writeFileSync(
        join(root, "e2e", "index.html"),
        `<!doctype html><html lang="en"><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>Raccord E2E reports</title><body><main><h1>Nightly E2E reports</h1><p><a href="latest.html">Latest report</a> · <a href="../">Raccord snapshots</a></p><ul>${list || "<li>No reports published yet.</li>"}</ul><p>Reports are public and retained for at most 30 days (up to 14 runs).</p></main></body></html>`,
    );
    writeFileSync(
        join(root, "index.html"),
        `<!doctype html><html lang="en"><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>Raccord engineering snapshots</title><body><main><h1>Raccord engineering snapshots</h1><ul><li><a href="architecture/">Architecture (arc42)</a></li><li><a href="model/">Product model</a></li><li><a href="e2e/">Nightly browser journey evidence</a></li></ul></main></body></html>`,
    );

    const siteFiles = getFiles(root)
        .map((path) => relative(root, path).split(sep).join("/"))
        .filter((path) => path !== "site-manifest.json")
        .sort();
    writeFileSync(
        join(root, "site-manifest.json"),
        JSON.stringify(
            { generatedAt: new Date().toISOString(), files: siteFiles },
            null,
            2,
        ),
    );
    return { reports: history, files: siteFiles };
}

if (
    process.argv[1] &&
    import.meta.url === pathToFileURL(resolve(process.argv[1])).href
) {
    const [command, ...args] = process.argv.slice(2);
    if (command === "restore") {
        restoreSite(args[0], args[1]);
    } else if (command === "report") {
        const [
            artifactsDirectory,
            outputDirectory,
            runId,
            sha,
            runUrl,
            date,
            overallStatus,
        ] = args;
        const metadata = { runId, sha, runUrl, date, overallStatus };
        buildReport({ artifactsDirectory, outputDirectory, metadata });
    } else if (command === "finalize") {
        finalizeSite(args[0]);
    } else if (command === "decide") {
        const [baseUrl, sha, eventName, force] = args;
        decideRun(baseUrl, sha, eventName, force)
            .then((decision) => process.stdout.write(JSON.stringify(decision)))
            .catch((error) => {
                process.stderr.write(`${error.message}\n`);
                process.exitCode = 1;
            });
    } else {
        process.stderr.write(
            "Usage: static-site.mjs <restore|report|finalize|decide> ...\n",
        );
        process.exitCode = 2;
    }
}
