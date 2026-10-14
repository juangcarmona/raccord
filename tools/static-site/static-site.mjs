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
import { fileURLToPath, pathToFileURL } from "node:url";
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
    const workflowStatus = metadata.overallStatus ?? "unknown";
    // A successful workflow without journey records is not a successful report.
    const overallStatus = journeyRecords.length === 0 ? "failure" : workflowStatus;
    const overall = escapeHtml(overallStatus);
    const emptyMessage =
        journeyRecords.length === 0
            ? "No journey result was recorded. The runner may have failed before test discovery; inspect the linked workflow run."
            : "";
    const html = `<!doctype html><html lang="en"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="robots" content="noindex,nofollow"><title>Raccord E2E · ${date}</title><style>
:root{color-scheme:dark;--ink:#0b1020;--surface:#151c2b;--line:#30394b;--cloud:#f5f7fa;--muted:#aab5c7;--cyan:#2de2e6;--coral:#ff6b5e}*{box-sizing:border-box}body{margin:0;background:var(--ink);color:var(--cloud);font:16px/1.55 system-ui,-apple-system,"Segoe UI",sans-serif;padding:clamp(1rem,4vw,3.5rem)}main{max-width:1120px;margin:auto}a{color:var(--cyan);text-underline-offset:.2em}a:focus-visible,button:focus-visible,input:focus-visible,select:focus-visible{outline:3px solid var(--cyan);outline-offset:3px}.topline{display:flex;align-items:center;gap:.8rem;color:var(--muted);font-size:.9rem}.brand{width:2.5rem;height:2.5rem;border-radius:.55rem}h1{font-size:clamp(2rem,5vw,3.35rem);line-height:1.08;letter-spacing:-.035em;margin:2rem 0 .7rem}.meta,.lede{color:var(--muted)}.lede{max-width:68ch}.run{overflow-wrap:anywhere}.status{font-weight:700;text-transform:uppercase;letter-spacing:.08em}.status.failure{color:#ff948d}.status.success{color:#79e2ad}.summary{display:flex;flex-wrap:wrap;gap:.65rem;margin:1.5rem 0}.summary span,.badge{border:1px solid var(--line);border-radius:999px;padding:.3rem .8rem}.summary span{display:inline-flex;gap:.35em}.passed{color:#79e2ad}.failed{color:#ff948d}.flaky{color:#ffd071}.unknown{color:#c1ccdc}.filters{display:flex;gap:1rem;flex-wrap:wrap;margin:1.4rem 0 2rem}.filters label{display:grid;gap:.35rem;color:var(--muted);font-size:.9rem}.filters input,.filters select{font:inherit;color:var(--cloud);background:var(--surface);border:1px solid var(--line);border-radius:.4rem;padding:.55rem .7rem}.journey{border-top:1px solid var(--line);padding:1.5rem 0}.journey h2{font-size:1.25rem}.journey h2 small{font-size:.85rem;color:var(--muted);font-weight:400}.badge{font-size:.72rem;text-transform:uppercase;letter-spacing:.06em}.assets{display:grid;grid-template-columns:repeat(auto-fit,minmax(min(100%,340px),1fr));gap:1rem}.image,.asset{min-width:0}.image img,video{display:block;width:100%;height:auto;border:1px solid var(--line);border-radius:.4rem;background:#080b12}.image span{display:block;color:var(--muted);font-size:.85rem;margin-top:.4rem}.empty{padding:1.25rem;border:1px solid #9a4b45;border-left:4px solid var(--coral);border-radius:.45rem;background:#24191e}.footer{border-top:1px solid var(--line);margin-top:3rem;padding-top:1.25rem;color:var(--muted);font-size:.9rem}</style></head><body><main><div class="topline"><img class="brand" src="../../../assets/raccord-mark.svg" alt=""><span>Raccord · Browser journey evidence</span></div><h1>Nightly browser journeys</h1><p class="lede">A recorded view of the user journeys exercised against this revision.</p><p class="meta">Tested commit <code class="run">${sha}</code> · ${date} · workflow result: <strong class="status ${overall}">${overall}</strong></p><p><a href="${runUrl}">Open this GitHub Actions run</a> · <a href="../../../index.html">Engineering snapshots</a></p><div class="summary">${Object.entries(
        counts,
    )
        .map(
            ([status, count]) =>
                `<span class="${status}">${count} ${status}</span>`,
        )
        .join(
            "",
        )}</div>${emptyMessage ? `<p class="empty"><strong>No journey evidence captured.</strong><br>${escapeHtml(emptyMessage)} The report is marked as a failure, regardless of the workflow’s reported status.</p>` : ""}${journeysHtml}<footer class="footer">Screenshots, videos, and traces are retained with this run. Earlier runs are listed below when available.</footer></main></body></html>`;
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
        workflowStatus,
        overallStatus,
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
    if (latest) {
        const reportPath = join(root, "e2e", latest.path);
        const earlierReports = history
            .slice(1)
            .map(
                (report) =>
                    `<li><a href="../${escapeHtml(String(report.runId))}/index.html">${escapeHtml(report.date)} · ${escapeHtml(report.sha.slice(0, 8))}</a> <span>${escapeHtml(report.status)}</span></li>`,
            )
            .join("");
        const archive = earlierReports
            ? `<section class="archive" aria-labelledby="archive-title"><h2 id="archive-title">Earlier runs</h2><ul>${earlierReports}</ul><p>Reports are retained for up to 30 days (maximum 14 runs).</p></section>`
            : "";
        if (archive && statSync(reportPath, { throwIfNoEntry: false })?.isFile()) {
            const reportHtml = readFileSync(reportPath, "utf8");
            writeFileSync(
                reportPath,
                reportHtml.replace("</main>", `${archive}</main>`),
            );
        }
    }
    const latestHtml = latest
        ? `<!doctype html><html lang="en"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="robots" content="noindex,nofollow"><meta http-equiv="refresh" content="0;url=${escapeHtml(latest.path)}"><title>Latest Raccord E2E report</title></head><body><p>Opening the <a href="${escapeHtml(latest.path)}">latest nightly report (${escapeHtml(latest.date)})</a>…</p></body></html>`
        : '<!doctype html><html lang="en"><meta charset="utf-8"><title>Raccord E2E</title><p>No nightly report has been published yet.</p></html>';
    writeFileSync(join(root, "e2e", "latest.html"), latestHtml);
    writeFileSync(
        join(root, "e2e", "index.html"),
        '<!doctype html><html lang="en"><meta charset="utf-8"><meta http-equiv="refresh" content="0;url=latest.html"><title>Raccord E2E</title><body><p><a href="latest.html">Open latest browser journey report</a></p></body></html>',
    );
    const sourceRoot = resolve(dirname(fileURLToPath(import.meta.url)), "../..");
    mkdirSync(join(root, "assets"), { recursive: true });
    copyFileSync(
        join(sourceRoot, "docs", "design", "raccord-mark.svg"),
        join(root, "assets", "raccord-mark.svg"),
    );
    const latestLink = latest ? "e2e/latest.html" : "e2e/";
    const latestRun = latest
        ? `<p class="meta">Latest run · ${escapeHtml(latest.date)} · <span class="status ${escapeHtml(latest.status)}">${escapeHtml(latest.status)}</span></p>`
        : '<p class="meta">No browser journey report has been published yet.</p>';
    writeFileSync(
        join(root, "index.html"),
        `<!doctype html><html lang="en"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="theme-color" content="#0b1020"><title>Raccord · Engineering snapshots</title><style>
:root{color-scheme:light;--ink:#0b1020;--cyan:#2de2e6;--coral:#ff6b5e;--cloud:#f5f7fa;--slate:#667085;--line:#dfe4ec;--white:#fff}*{box-sizing:border-box}body{margin:0;background:var(--cloud);color:var(--ink);font:16px/1.55 system-ui,-apple-system,"Segoe UI",sans-serif}a{color:#075f83;text-underline-offset:.2em}a:focus-visible{outline:3px solid #087eaa;outline-offset:4px}.shell{max-width:1160px;margin:auto;padding:clamp(1.1rem,5vw,4rem)}header{display:flex;align-items:center;gap:.9rem}.mark{width:3rem;height:3rem;border-radius:.65rem}.wordmark{font-weight:750;letter-spacing:.02em}main{padding-top:clamp(3.5rem,10vw,7rem)}.eyebrow{font-size:.78rem;text-transform:uppercase;letter-spacing:.15em;font-weight:700;color:#526074}.hero{max-width:780px}h1{font-size:clamp(2.6rem,7vw,5.6rem);line-height:.99;letter-spacing:-.06em;margin:.8rem 0 1.2rem}.intro{font-size:clamp(1.05rem,2vw,1.25rem);color:#475467;max-width:62ch}.latest{margin:2.5rem 0;background:var(--ink);color:var(--white);padding:clamp(1.3rem,4vw,2.2rem);border-radius:.7rem;display:flex;align-items:center;justify-content:space-between;gap:1.5rem;flex-wrap:wrap}.latest .eyebrow,.latest .meta{color:#bac6d8}.latest h2{margin:.25rem 0;font-size:1.5rem}.status{font-weight:750;text-transform:uppercase;letter-spacing:.07em}.status.success{color:#87e8b4}.status.failure{color:#ff9a91}.button{display:inline-block;background:var(--cyan);color:var(--ink);text-decoration:none;font-weight:750;padding:.75rem 1rem;border-radius:.35rem}.button:hover{background:#75f1f2}.section-label{margin:3rem 0 1rem;font-size:1.15rem}.grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(min(100%,260px),1fr));gap:1rem}.card{display:block;background:var(--white);border:1px solid var(--line);border-radius:.55rem;padding:1.35rem;color:var(--ink);text-decoration:none;transition:border-color .15s,transform .15s}.card:hover{border-color:#0781a5;transform:translateY(-2px)}.card .eyebrow{color:#667085}.card h3{font-size:1.25rem;margin:.65rem 0 .35rem}.card p{color:#526074;margin:0}.arrow{display:inline-block;margin-top:1.1rem;color:#075f83;font-weight:700}footer{margin-top:4rem;padding-top:1.25rem;border-top:1px solid var(--line);color:var(--slate);font-size:.9rem}@media(prefers-reduced-motion:reduce){*,*::before,*::after{scroll-behavior:auto!important;transition:none!important}}</style></head><body><div class="shell"><header><img class="mark" src="assets/raccord-mark.svg" alt=""><span class="wordmark">RACCORD</span></header><main><section class="hero"><p class="eyebrow">Engineering snapshots</p><h1>How Raccord is built, in the open.</h1><p class="intro">Architecture, product shape, and real browser journeys from the latest application revision—kept together in one place.</p></section><section class="latest" aria-labelledby="latest-title"><div><p class="eyebrow">Latest browser run</p><h2 id="latest-title">Nightly journey evidence</h2>${latestRun}</div><a class="button" href="${latestLink}">View latest report <span aria-hidden="true">→</span></a></section><h2 class="section-label">Explore the project</h2><nav class="grid" aria-label="Engineering snapshots"><a class="card" href="architecture/"><span class="eyebrow">System design</span><h3>Architecture</h3><p>System boundaries, runtime structure, and key engineering decisions.</p><span class="arrow">Explore architecture →</span></a><a class="card" href="model/"><span class="eyebrow">Product definition</span><h3>Product model</h3><p>The current product shape, workflows, and domain concepts.</p><span class="arrow">Explore product model →</span></a><a class="card" href="${latestLink}"><span class="eyebrow">Quality evidence</span><h3>Browser journeys</h3><p>Screenshots and videos captured while exercising the running application.</p><span class="arrow">See the latest run →</span></a></nav></main><footer>Public engineering material for Raccord · Browser reports are retained for up to 30 days (maximum 14 runs).</footer></div></body></html>`,
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
