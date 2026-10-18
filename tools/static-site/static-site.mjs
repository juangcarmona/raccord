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
const DESIGN_TOKENS = readFileSync(
    resolve(
        dirname(fileURLToPath(import.meta.url)),
        "../../docs/design/tokens.css",
    ),
    "utf8",
);

function includeDesignTokens(html) {
    return html.replaceAll("<style>", `<style>${DESIGN_TOKENS}\n`);
}

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
        eventName === "schedule" || eventName === "push"
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

const BOUNDED_CONTEXTS = [
    { id: "contacts", title: "Contacts" },
    { id: "projects", title: "Projects" },
    { id: "equipment", title: "Equipment" },
    { id: "warehouse", title: "Warehouse" },
    { id: "crew", title: "Crew and transport" },
    { id: "commercial", title: "Commercial" },
    { id: "invoicing", title: "Invoicing" },
    { id: "compliance", title: "Compliance and audit" },
    { id: "documents", title: "Documents and communication" },
    { id: "workspace", title: "Workspace" },
];
const JOURNEY_AREAS = new Set([
    ...BOUNDED_CONTEXTS.map(({ id }) => id),
    "cross-cutting",
]);

export function buildReport({
    artifactsDirectory,
    outputDirectory,
    metadata,
    existingReport,
}) {
    mkdirSync(outputDirectory, { recursive: true });
    const assetsDirectory = join(outputDirectory, "assets");
    mkdirSync(assetsDirectory, { recursive: true });
    const files = existingReport ? [] : getFiles(resolve(artifactsDirectory));
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
                    const areas = Array.isArray(value.areas)
                        ? [
                              ...new Set(
                                  value.areas.filter((area) =>
                                      JOURNEY_AREAS.has(area),
                                  ),
                              ),
                          ]
                        : [];
                    groups.set(value.name, {
                        name: value.name,
                        status: value.status,
                        viewport: value.viewport ?? "desktop",
                        areas,
                        evidenceMode:
                            value.evidenceMode === "api" ? "api" : "browser",
                        assets: [],
                    });
                }
            } catch {
                // A truncated result file should not hide the artifacts or other journeys.
            }
        }
    }

    const supplementalAssets = new Map();
    for (const path of files.filter((file) => file.endsWith(".json"))) {
        try {
            const result = JSON.parse(readFileSync(path, "utf8"));
            for (const asset of result.supplementalAssets ?? []) {
                if (typeof asset === "string")
                    supplementalAssets.set(asset, result.name);
            }
        } catch {
            // The result parser above already treats malformed manifests as unclassified.
        }
    }

    const assetRecords = [];
    for (const path of files) {
        const filename = path.split(sep).at(-1);
        const match = filename.match(/^([A-Za-z0-9-]+)\.(png|webm|zip)$/);
        if (!match) continue;
        const explicitJourney = supplementalAssets.get(filename);
        let journeyName = explicitJourney ?? match[1];
        const extension = match[2];
        let variant = "";
        for (const suffix of explicitJourney
            ? []
            : ["-failure", "-checkpoint", "-mobile", "-publication"]) {
            if (journeyName.endsWith(suffix)) {
                journeyName = journeyName.slice(0, -suffix.length);
                variant = suffix;
                break;
            }
        }
        const group = groups.get(journeyName) ?? {
            name: journeyName,
            status: variant === "-failure" ? "failed" : "unknown",
            viewport: "desktop",
            areas: [],
            evidenceMode: "browser",
            assets: [],
        };
        groups.set(journeyName, group);
        if (group.evidenceMode === "api" && extension !== "zip") continue;
        const assetName = explicitJourney
            ? filename
            : `${journeyName}${variant}.${extension}`;
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

    const journeyRecords =
        existingReport?.journeys ??
        [...groups.values()].sort((left, right) =>
            left.name.localeCompare(right.name),
        );
    const counts =
        existingReport?.counts ??
        Object.fromEntries(
            ["passed", "failed", "flaky", "unknown"].map((status) => [
                status,
                journeyRecords.filter((journey) => journey.status === status)
                    .length,
            ]),
        );
    const journeysHtml = journeyRecords
        .map((journey, index) => {
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
            const evidenceNote =
                journey.evidenceMode === "api"
                    ? '<p class="evidence-mode">API journey: assertions exercise the AppHost API. Visual capture is omitted; use the result and Playwright trace as evidence.</p>'
                    : "";
            return `<section class="journey" id="journey-${index}" data-status="${journey.status}" data-viewport="${escapeHtml(journey.viewport)}" data-name="${escapeHtml(journey.name.toLowerCase())}"><h2><span class="badge ${journey.status}">${journey.status}</span> ${escapeHtml(displayName(journey.name))} <small>(${escapeHtml(journey.viewport)})</small></h2>${evidenceNote}<div class="assets">${assetsHtml || "<p>No screenshot, video, or trace was produced.</p>"}</div></section>`;
        })
        .join("");

    const statusIcon = (status) => {
        const paths = {
            passed: '<path d="m3 8 3.2 3.2L13 4.5"/>',
            failed: '<path d="m4 4 8 8M12 4l-8 8"/>',
            flaky: '<path d="M8 2v6l4 2"/><circle cx="8" cy="8" r="6"/>',
            unknown:
                '<circle cx="8" cy="8" r="6"/><path d="M6.3 6a1.8 1.8 0 1 1 3 1.3c-.9.7-1.3 1-1.3 2M8 11.7v.1"/>',
        };
        return `<svg viewBox="0 0 16 16" aria-hidden="true">${paths[status] ?? paths.unknown}</svg>`;
    };
    const navItem = (journey, index) =>
        `<li><a href="#journey-${index}" data-status="${journey.status}" title="${escapeHtml(displayName(journey.name))}"><span class="nav-icon">${statusIcon(journey.status)}</span><span class="nav-title">${escapeHtml(displayName(journey.name))}</span><span class="nav-code">${journey.status === "passed" ? "PASS" : journey.status === "failed" ? "FAIL" : journey.status.toUpperCase()}</span></a></li>`;
    const indicesByArea = (area) =>
        journeyRecords.flatMap((journey, index) =>
            (Array.isArray(journey.areas) ? journey.areas : []).includes(area)
                ? [navItem(journey, index)]
                : [],
        );
    const navGroup = (id, title, items) =>
        `<details class="nav-group" data-area="${id}" open><summary><span class="nav-group-title">${escapeHtml(title)}</span></summary>${items.length ? `<ul class="nav-list">${items.join("")}</ul>` : '<p class="nav-empty">No evidence in this run</p>'}</details>`;
    const navGroups = BOUNDED_CONTEXTS.map(({ id, title }) => {
        const items = indicesByArea(id);
        return navGroup(id, title, items);
    });
    const crossCuttingItems = indicesByArea("cross-cutting");
    navGroups.push(
        navGroup("cross-cutting", "Cross-cutting", crossCuttingItems),
    );
    const unclassifiedItems = journeyRecords.flatMap((journey, index) =>
        (Array.isArray(journey.areas) ? journey.areas : []).some((area) =>
            JOURNEY_AREAS.has(area),
        )
            ? []
            : [navItem(journey, index)],
    );
    if (unclassifiedItems.length) {
        navGroups.push(
            navGroup("unclassified", "Unclassified", unclassifiedItems),
        );
    }
    const viewportOptions = [
        ...new Set(journeyRecords.map((journey) => journey.viewport)),
    ]
        .sort()
        .map(
            (viewport) =>
                `<option value="${escapeHtml(viewport.toLowerCase())}">${escapeHtml(viewport.charAt(0).toUpperCase() + viewport.slice(1))}</option>`,
        )
        .join("");

    const runUrl = escapeHtml(metadata.runUrl ?? "#");
    const sha = escapeHtml(metadata.sha ?? "unknown");
    const date = escapeHtml(metadata.date ?? new Date().toISOString());
    const workflowStatus =
        existingReport?.workflowStatus ?? metadata.overallStatus ?? "unknown";
    // A successful workflow without journey records is not a successful report.
    const overallStatus =
        existingReport?.overallStatus ??
        (journeyRecords.length === 0 ? "failure" : workflowStatus);
    const overall = escapeHtml(overallStatus);
    const emptyMessage =
        journeyRecords.length === 0
            ? "No journey result was recorded. The runner may have failed before test discovery; inspect the linked workflow run."
            : "";
    const summary = Object.entries(counts)
        .map(
            ([status, count]) =>
                `<span class="count ${status}"><strong>${count}</strong> ${status}</span>`,
        )
        .join("");
    const html = `<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width,initial-scale=1">
  <meta name="robots" content="noindex,nofollow">
  <meta name="color-scheme" content="light dark">
  <title>Raccord · Browser journeys · ${date}</title>
  <style>
:root{--bg:var(--raccord-cloud);--panel:var(--raccord-selected-soft);--surface:var(--raccord-white);--ink:var(--raccord-ink);--text:var(--raccord-ink);--muted:var(--raccord-slate);--line:var(--raccord-border);--line-strong:var(--raccord-slate);--accent:var(--raccord-action);--accent-soft:var(--raccord-selected-soft);--pass:var(--raccord-success);--fail:var(--raccord-error);--flaky:var(--raccord-warning);--unknown:var(--raccord-slate);--space-1:var(--raccord-space-1);--space-2:var(--raccord-space-2);--space-3:var(--raccord-space-3);--space-4:var(--raccord-space-4);--space-6:var(--raccord-space-6);--space-8:var(--raccord-space-8);--radius-sm:var(--raccord-radius-sm);--radius-md:var(--raccord-radius-md);--radius-lg:var(--raccord-radius-lg);--focus-width:var(--raccord-focus-width);--focus-offset:var(--raccord-focus-offset);--shadow:0 14px 38px color-mix(in srgb,var(--raccord-ink) 7%,transparent)}


*{box-sizing:border-box}html,body{height:100%}body{margin:0;display:flex;flex-direction:column;overflow:hidden;background:var(--bg);color:var(--text);font:var(--raccord-text-sm)/var(--raccord-leading) var(--raccord-font-sans)}a{color:var(--accent);text-decoration:none}a:hover{text-decoration:underline}[hidden]{display:none!important}:focus-visible{outline:var(--focus-width) solid var(--accent);outline-offset:var(--focus-offset)}.site-header{display:flex;align-items:center;gap:.8rem;min-height:3.5rem;flex:none;padding:.45rem .9rem;background:var(--surface);border-bottom:1px solid var(--line)}.brand{width:1.8rem;height:1.8rem;object-fit:contain}.brand-title{font-weight:700;color:var(--ink)}.revision{font: .75rem var(--raccord-font-mono);color:var(--muted);border:1px solid var(--line);border-radius:var(--radius-sm);padding:.12rem .45rem}.spacer{flex:1}.iconbtn,.appearance button{display:inline-flex;align-items:center;justify-content:center;width:2rem;height:2rem;border:1px solid var(--line-strong);border-radius:var(--radius-sm);background:var(--surface);color:var(--muted);cursor:pointer}.iconbtn:hover,.appearance button:hover,.appearance button[aria-pressed=true]{color:var(--ink);background:var(--accent-soft);border-color:var(--accent)}.appearance{display:flex;gap:.2rem}.appearance svg{width:1rem;height:1rem;fill:none;stroke:currentColor;stroke-width:1.5;stroke-linecap:round;stroke-linejoin:round}.appearance .sun{fill:currentColor;stroke:none}.appearance .moon{fill:currentColor;stroke:none}.mobile-nav-toggle,#nav-close,#nav-backdrop{display:none}#shell{position:relative;display:flex;flex:1;min-height:0}aside.nav{position:relative;z-index:2;flex:none;width:17.5rem;overflow-y:auto;padding:.8rem .65rem;background:var(--panel);border-right:1px solid var(--line);transition:width .18s,padding .18s}.nav-heading{display:flex;align-items:center;justify-content:space-between;gap:.5rem;margin:0 0 .65rem;padding:0 .2rem}.nav-heading h2,.nav-group h3,.nav-group-title{margin:0;color:var(--muted);font-size:.7rem;text-transform:uppercase;letter-spacing:.09em}.nav-group{margin-top:1rem}.nav-group h3{margin:.6rem .35rem}.nav-group>summary{cursor:pointer;margin:.6rem .35rem}.nav-group>summary:focus-visible{border-radius:2px}.nav-list{list-style:none;margin:0;padding:0}.nav-list li{border-bottom:1px solid var(--line)}.nav-list a{display:flex;align-items:center;gap:.55rem;padding:.38rem .4rem;border-left:2px solid transparent;color:var(--text);font-size:.85rem}.nav-list a:hover,.nav-list a[aria-current]{background:var(--surface);border-left-color:var(--accent);color:var(--ink);text-decoration:none}.nav-icon{display:inline-flex;flex:none;width:1rem;height:1rem;color:var(--unknown)}.nav-icon svg{width:100%;height:100%;fill:none;stroke:currentColor;stroke-width:1.7;stroke-linecap:round;stroke-linejoin:round}.nav-list a[data-status=passed] .nav-icon{color:var(--pass)}.nav-list a[data-status=failed] .nav-icon{color:var(--fail)}.nav-list a[data-status=flaky] .nav-icon{color:var(--flaky)}.nav-list a[data-status=unknown] .nav-icon{color:var(--unknown)}.nav-title{flex:1;min-width:0}.nav-code{display:none;font:600 .56rem/1 var(--raccord-font-mono);color:var(--muted)}main#main{flex:1;min-width:0;overflow-y:auto;padding:clamp(1rem,3vw,2.5rem)}.content{max-width:78rem;margin:auto}.eyebrow{margin:0;color:var(--muted);font-size:.72rem;text-transform:uppercase;letter-spacing:.12em;font-weight:700}.hero{padding:0 0 1.25rem;border-bottom:1px solid var(--line)}h1{margin:.35rem 0;font-size:clamp(1.8rem,4vw,2.6rem);line-height:1.15;letter-spacing:-.035em;color:var(--ink)}.lede,.meta{margin:.35rem 0;color:var(--muted)}.run{overflow-wrap:anywhere}.runlinks{display:flex;flex-wrap:wrap;gap:.8rem;margin-top:.8rem}.summary{display:flex;flex-wrap:wrap;gap:.55rem;margin:1rem 0}.count,.badge{display:inline-flex;align-items:center;gap:.35rem;border:1px solid var(--line);border-radius:999px;padding:.26rem .68rem;font-size:.8rem}.count strong{font-variant-numeric:tabular-nums}.passed{--status:var(--pass)}.failed{--status:var(--fail)}.flaky{--status:var(--flaky)}.unknown{--status:var(--unknown)}.count,.badge{color:var(--status,var(--muted))}.filters{display:flex;align-items:end;gap:.65rem;flex-wrap:wrap;padding:.8rem;background:var(--panel);border:1px solid var(--line);border-radius:var(--radius-md)}.filters label{display:grid;gap:.25rem;color:var(--muted);font-size:.78rem}.filters input,.filters select{min-width:9rem;max-width:100%;font:inherit;color:var(--text);background:var(--surface);border:1px solid var(--line-strong);border-radius:var(--radius-sm);padding:.42rem .55rem}.journey{margin:1rem 0;padding:1rem;border:1px solid var(--line);border-radius:var(--radius-md);background:var(--surface);scroll-margin-top:1rem}.journey h2{display:flex;align-items:center;gap:.55rem;margin:0 0 .85rem;font-size:1.1rem;color:var(--ink)}.journey h2 small{margin-left:auto;color:var(--muted);font-size:.78rem;font-weight:400}.badge{font-size:.65rem;text-transform:uppercase;letter-spacing:.06em}.assets{display:grid;grid-template-columns:repeat(auto-fit,minmax(min(100%,320px),1fr));gap:.8rem}.image,.asset{min-width:0}.image img,video{display:block;width:100%;height:auto;border:1px solid var(--line);border-radius:5px;background:var(--raccord-cloud)}.image span{display:block;margin-top:.35rem;color:var(--muted);font-size:.8rem}.empty{padding:.85rem 1rem;border:1px solid color-mix(in srgb,var(--fail) 55%,var(--line));border-left:3px solid var(--fail);border-radius:5px;background:color-mix(in srgb,var(--fail) 8%,var(--surface))}.footer{margin-top:2rem;padding:1rem 0;border-top:1px solid var(--line);color:var(--muted);font-size:.82rem}body.nav-collapsed aside.nav{width:4.1rem;padding:.8rem .45rem}body.nav-collapsed .nav-heading{justify-content:center}body.nav-collapsed .nav-heading h2,body.nav-collapsed .nav-title,body.nav-collapsed .nav-group h3,body.nav-collapsed .nav-group-title{display:none}body.nav-collapsed .nav-list a{flex-direction:column;gap:.16rem;padding:.45rem .1rem}body.nav-collapsed .nav-icon{width:1.2rem;height:1.2rem}body.nav-collapsed .nav-code{display:block}@media(max-width:900px){.mobile-nav-toggle{display:inline-flex}aside.nav{position:absolute;inset:0 auto 0 0;z-index:25;width:min(19rem,86vw);transform:translateX(-105%);box-shadow:var(--shadow);transition:transform .2s}body.mobile-nav-open aside.nav{transform:translateX(0)}#nav-close{display:inline-flex}#nav-backdrop{position:absolute;inset:0;z-index:24;display:block;border:0;background:color-mix(in srgb,var(--raccord-ink) 50%,transparent)}body.nav-collapsed aside.nav{width:min(19rem,86vw);padding:.8rem .65rem}body.nav-collapsed .nav-heading{justify-content:space-between}body.nav-collapsed .nav-heading h2,body.nav-collapsed .nav-title,body.nav-collapsed .nav-group h3,body.nav-collapsed .nav-group-title{display:block}body.nav-collapsed .nav-list a{flex-direction:row;gap:.55rem;padding:.38rem .4rem}body.nav-collapsed .nav-icon{width:1rem;height:1rem}body.nav-collapsed .nav-code{display:none}main#main{padding:1rem}}@media(max-width:520px){.site-header{gap:.45rem}.revision{font-size:.65rem}.brand-title{font-size:.9rem}}@media print{body{display:block;overflow:visible}aside.nav,.site-header .appearance,.site-header .iconbtn{display:none}main#main{overflow:visible}}@media(prefers-reduced-motion:reduce){*,*::before,*::after{scroll-behavior:auto!important;transition:none!important}}
  .status.failure{color:var(--fail)}.status.success{color:var(--pass)}.archive{margin-top:2rem;padding:1rem;border:1px solid var(--line);border-radius:var(--radius-md);background:var(--panel)}.archive h2{margin:0 0 .6rem;font-size:1rem;color:var(--ink)}.archive ul{margin:0;padding-left:1.2rem}.archive li{padding:.2rem 0}.archive li span{margin-left:.4rem;color:var(--muted);font-size:.8rem}.brand-title{white-space:nowrap}@media(max-width:520px){.revision{display:none}.brand-title{font-size:.84rem}}
    .nav-empty{margin:.3rem .45rem;color:var(--muted);font-size:.78rem}
  </style>
</head>
<body>
  <header class="site-header">
    <button type="button" id="mobile-nav-toggle" class="iconbtn mobile-nav-toggle" aria-controls="journey-nav" aria-expanded="false" aria-label="Open journey navigation" title="Open navigation">☰</button>
    <img class="brand" src="../../../assets/raccord-mark.svg" alt="">
    <span class="brand-title">Raccord · E2E</span>
    <span class="revision" title="Tested commit ${sha}">${sha.slice(0, 7)}</span>
    <span class="spacer"></span>
    <div class="appearance" role="group" aria-label="Appearance">
      <button type="button" data-appearance="auto" aria-label="Use system appearance" title="System"><svg viewBox="0 0 16 16" aria-hidden="true"><rect x="2" y="2" width="12" height="9" rx="1"/><path d="M6 14h4M8 11v3"/></svg></button>
      <button type="button" data-appearance="light" aria-label="Use light appearance" title="Light"><svg viewBox="0 0 16 16" aria-hidden="true" class="sun"><circle cx="8" cy="8" r="3"/><path d="M8 1v2M8 13v2M1 8h2m10 0h2M3 3l1.4 1.4m7.2 7.2L13 13m0-10-1.4 1.4m-7.2 7.2L3 13"/></svg></button>
      <button type="button" data-appearance="dark" aria-label="Use dark appearance" title="Dark"><svg viewBox="0 0 16 16" aria-hidden="true" class="moon"><path d="M13.5 10.2A6 6 0 0 1 5.8 2.5 6 6 0 1 0 13.5 10.2Z"/></svg></button>
    </div>
    <a class="iconbtn" href="../../../index.html" aria-label="Engineering snapshots home" title="Engineering snapshots">⌂</a>
  </header>
  <div id="shell">
    <aside class="nav" id="journey-nav" aria-label="Browser journey navigation">
      <div class="nav-heading"><h2>Journeys</h2><button type="button" class="iconbtn" id="nav-collapse" aria-label="Collapse navigation" aria-expanded="true" title="Collapse navigation">◧</button><button type="button" class="iconbtn" id="nav-close" aria-label="Close navigation" title="Close navigation">×</button></div>
      <nav aria-label="Report sections">
        <ul class="nav-list"><li><a href="#report-overview" data-status="unknown"><span class="nav-icon"><svg viewBox="0 0 16 16" aria-hidden="true"><path d="m2 7 6-5 6 5v7H9v-4H7v4H2z"/></svg></span><span class="nav-title">Run overview</span><span class="nav-code">HOME</span></a></li></ul>
        ${navGroups.join("")}
      </nav>
      <div id="archive-nav"></div>
    </aside>
    <button type="button" id="nav-backdrop" aria-label="Close navigation" hidden></button>
    <main id="main"><div class="content">
      <section class="hero" id="report-overview">
        <p class="eyebrow">Nightly E2E · ${date}</p>
        <h1>Browser journeys</h1>
        <p class="lede">Screenshots, videos, and traces captured from the application at the tested revision.</p>
        <p class="meta">Commit <code class="run">${sha}</code> · Workflow result: <strong class="status ${overall}">${overall}</strong></p>
        <div class="runlinks"><a href="${runUrl}">Open this GitHub Actions run</a><a href="../../../index.html">All engineering snapshots</a></div>
      </section>
      <div class="summary" aria-label="Journey results">${summary}</div>
      <div class="filters" aria-label="Filter journey results">
        <label>Journey <input id="journey-search" type="search" placeholder="Filter by name"></label>
        <label>Status <select id="status-filter"><option value="">All statuses</option><option>passed</option><option>failed</option><option>flaky</option><option>unknown</option></select></label>
        <label>Viewport <select id="viewport-filter"><option value="">All viewports</option>${viewportOptions}</select></label>
      </div>
      ${emptyMessage ? `<p class="empty"><strong>No journey evidence captured.</strong><br>${escapeHtml(emptyMessage)} The report is marked as a failure, regardless of the workflow’s reported status.</p>` : ""}
      ${journeysHtml}
      <div id="archive-content"></div>
      <footer class="footer">Screenshots, videos, and traces are retained with this run. Earlier runs are listed below when available.</footer>
    </div></main>
  </div>
  <script>
  (()=>{
    const root=document.documentElement, body=document.body;
    const buttons=[...document.querySelectorAll('[data-appearance]')];
    const applyAppearance=choice=>{if(choice==='auto')root.removeAttribute('data-appearance');else root.setAttribute('data-appearance',choice);buttons.forEach(button=>button.setAttribute('aria-pressed',String(button.dataset.appearance===choice)));try{localStorage.setItem('raccord-appearance',choice)}catch{}};
    let appearance='auto';try{appearance=localStorage.getItem('raccord-appearance')||'auto'}catch{}applyAppearance(appearance);
    buttons.forEach(button=>button.addEventListener('click',()=>applyAppearance(button.dataset.appearance)));
    const closeNav=()=>{body.classList.remove('mobile-nav-open');document.getElementById('nav-backdrop').hidden=true;document.getElementById('mobile-nav-toggle').setAttribute('aria-expanded','false')};
    document.getElementById('mobile-nav-toggle').addEventListener('click',event=>{body.classList.add('mobile-nav-open');event.currentTarget.setAttribute('aria-expanded','true');document.getElementById('nav-backdrop').hidden=false});
    document.getElementById('nav-close').addEventListener('click',closeNav);document.getElementById('nav-backdrop').addEventListener('click',closeNav);
    document.getElementById('nav-collapse').addEventListener('click',event=>{body.classList.toggle('nav-collapsed');const expanded=!body.classList.contains('nav-collapsed');event.currentTarget.setAttribute('aria-expanded',String(expanded));event.currentTarget.setAttribute('aria-label',expanded?'Collapse navigation':'Expand navigation')});
        const nav=document.querySelector('#journey-nav nav');
        const syncNav=()=>{const current=location.hash||'#report-overview';for(const link of nav.querySelectorAll('a')){if(link.getAttribute('href')===current)link.setAttribute('aria-current','location');else link.removeAttribute('aria-current')}};
        nav.addEventListener('click',event=>{if(event.target.closest('a'))closeNav()});
        window.addEventListener('hashchange',syncNav);syncNav();
    document.addEventListener('keydown',event=>{if(event.key==='Escape'&&body.classList.contains('mobile-nav-open'))closeNav()});
    const controls=[document.querySelector('#journey-search'),document.querySelector('#status-filter'),document.querySelector('#viewport-filter')];
    const filter=()=>{const [name,status,viewport]=controls.map(control=>control.value.toLowerCase());for(const journey of document.querySelectorAll('.journey'))journey.hidden=!(journey.dataset.name.includes(name)&&(!status||journey.dataset.status===status)&&(!viewport||journey.dataset.viewport.toLowerCase()===viewport))};
    controls.forEach(control=>control.addEventListener('input',filter));
  })();
  </script>
</body>
</html>`;
    writeFileSync(
        join(outputDirectory, "index.html"),
        includeDesignTokens(html),
    );
    const reportManifest = {
        ...metadata,
        workflowStatus,
        overallStatus,
        counts,
        journeys: journeyRecords,
        files: existingReport?.files ?? [
            "index.html",
            ...assetRecords.map((asset) => asset.path),
        ],
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

function renderLatestRun(latest, report) {
    if (!latest) {
        return '<p class="run-message">No browser journey report has been published yet. The report page will appear after the next run.</p>';
    }

    const status = ["success", "failure"].includes(latest.status)
        ? latest.status
        : "unknown";
    const journeys = Array.isArray(report?.journeys) ? report.journeys : null;
    const counts = report?.counts;
    const parsedDate = latest.date ? new Date(latest.date) : null;
    const canonicalDate =
        parsedDate && !Number.isNaN(parsedDate.getTime())
            ? parsedDate.toISOString()
            : null;
    const time = canonicalDate
        ? `<time class="run-time" data-local-time datetime="${escapeHtml(canonicalDate)}">${escapeHtml(canonicalDate)} UTC</time>`
        : '<span class="run-time">Run time unavailable</span>';
    let runUrl;
    try {
        const parsedUrl = new URL(latest.runUrl);
        if (parsedUrl.protocol === "https:") runUrl = parsedUrl.href;
    } catch {
        runUrl = undefined;
    }
    const commit =
        latest.sha && runUrl
            ? `<a class="commit" href="${escapeHtml(runUrl)}" aria-label="Open workflow for commit ${escapeHtml(latest.sha)}">${escapeHtml(latest.sha.slice(0, 8))}</a>`
            : "";
    let resultMessage;
    if (journeys?.length === 0) {
        resultMessage = `<p class="run-message no-results"><strong>No journeys were recorded.</strong> The workflow reported ${escapeHtml(report.workflowStatus ?? latest.status)}; inspect the report for the discovery or setup failure.</p>`;
    } else if (journeys === null) {
        resultMessage =
            '<p class="run-message">Journey details are unavailable in this report.</p>';
    } else {
        const recorded = journeys.length;
        const totals = [
            ["passed", counts?.passed ?? 0],
            ["failed", counts?.failed ?? 0],
            ["flaky", counts?.flaky ?? 0],
            ["unknown", counts?.unknown ?? 0],
        ]
            .filter(([, count]) => count > 0)
            .map(([name, count]) => `${count} ${name}`)
            .join(" · ");
        resultMessage = `<p class="run-message">${recorded} ${recorded === 1 ? "journey" : "journeys"} recorded${totals ? ` · ${escapeHtml(totals)}` : ""}</p>`;
    }

    const label = journeys?.length === 0 ? "No results" : status;
    const statusClass = journeys?.length === 0 ? "no-results" : status;
    return `<p class="run-meta">${time}${commit ? ` <span aria-hidden="true">·</span> ${commit}` : ""} <span aria-hidden="true">·</span> <span class="status ${statusClass}">${label}</span></p>${resultMessage}`;
}

function renderLandingPage({ latestRun, latestLink }) {
    return includeDesignTokens(`<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width,initial-scale=1">
  <meta name="color-scheme" content="light dark">
  <meta name="theme-color" content="var(--raccord-cloud)">
  <title>Raccord · Engineering snapshots</title>
  <style>
:root{--bg:var(--raccord-cloud);--panel:var(--raccord-selected-soft);--surface:var(--raccord-white);--ink:var(--raccord-ink);--text:var(--raccord-ink);--muted:var(--raccord-slate);--line:var(--raccord-border);--line-strong:var(--raccord-slate);--accent:var(--raccord-selected);--accent-soft:var(--raccord-selected-soft);--mark:var(--raccord-warning-soft);--success:var(--raccord-success);--failure:var(--raccord-error);--space-1:var(--raccord-space-1);--space-2:var(--raccord-space-2);--space-3:var(--raccord-space-3);--space-4:var(--raccord-space-4);--space-6:var(--raccord-space-6);--space-8:var(--raccord-space-8);--radius-sm:var(--raccord-radius-sm);--radius-md:var(--raccord-radius-md);--radius-lg:var(--raccord-radius-lg);--focus-width:var(--raccord-focus-width);--focus-offset:var(--raccord-focus-offset);--sans:system-ui,-apple-system,"Segoe UI",Roboto,Ubuntu,Cantarell,"Helvetica Neue",Arial,sans-serif}


*{box-sizing:border-box}body{margin:0;background:var(--bg);color:var(--text);font:16px/1.55 var(--sans);-webkit-text-size-adjust:100%}a{color:var(--accent);text-underline-offset:.2em}a:focus-visible,button:focus-visible{outline:2px solid var(--accent);outline-offset:3px}.shell{max-width:1240px;margin:auto;padding:clamp(1rem,3vw,2.5rem)}header{display:flex;align-items:center;gap:.8rem;min-height:2.6rem}.mark{width:2rem;height:2rem;border-radius:.4rem}.wordmark{font-weight:750;letter-spacing:.03em;color:var(--ink)}.spacer{flex:1}.appearance{display:flex;gap:.25rem;padding:.2rem;border:1px solid var(--line);border-radius:.4rem;background:var(--panel)}.appearance button{font:inherit;font-size:.78rem;color:var(--muted);background:transparent;border:0;border-radius:.25rem;padding:.32rem .5rem;cursor:pointer}.appearance button[aria-pressed=true]{color:var(--ink);background:var(--surface);box-shadow:0 0 0 1px var(--line)}main{padding-top:clamp(2.5rem,7vw,5rem)}.eyebrow{font-size:.74rem;text-transform:uppercase;letter-spacing:.14em;font-weight:750;color:var(--muted)}.hero{max-width:850px}h1{font-size:clamp(2.7rem,7.3vw,6.2rem);line-height:.98;letter-spacing:-.065em;color:var(--ink);margin:.6rem 0 1rem}.intro{font-size:clamp(1.05rem,2vw,1.3rem);color:var(--text);max-width:64ch;margin:0}.latest{margin:clamp(2rem,5vw,3.5rem) 0;background:var(--panel);color:var(--ink);padding:clamp(1.25rem,3vw,2rem);border:1px solid var(--line);border-radius:.65rem;display:flex;align-items:center;justify-content:space-between;gap:1.5rem;flex-wrap:wrap}.latest .eyebrow{margin:0 0 .35rem}.latest h2{margin:0;font-size:1.45rem}.run-meta{display:flex;align-items:center;gap:.5rem;flex-wrap:wrap;margin:.65rem 0 0;color:var(--muted);font-variant-numeric:tabular-nums}.run-time{white-space:normal}.commit{font-family:ui-monospace,SFMono-Regular,Consolas,monospace;font-size:.9em}.status{font-size:.77rem;font-weight:750;text-transform:uppercase;letter-spacing:.06em;color:var(--success)}.status.failure,.status.no-results,.status.unknown{color:var(--failure)}.run-message{margin:.35rem 0 0;color:var(--text);max-width:70ch}.run-message.no-results{color:var(--failure)}.button{display:inline-block;background:var(--accent);color:var(--raccord-white);text-decoration:none;font-weight:750;padding:.72rem 1rem;border-radius:.35rem;white-space:nowrap}.button:hover{filter:brightness(1.12)}.section-label{margin:2.5rem 0 .9rem;font-size:1.1rem;color:var(--ink)}.grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:1rem}.card{display:flex;flex-direction:column;align-items:flex-start;min-height:220px;background:var(--surface);border:1px solid var(--line);border-radius:.55rem;padding:clamp(1rem,2.5vw,1.4rem);color:var(--ink);text-decoration:none;transition:border-color .15s,transform .15s}.card:hover{border-color:var(--accent);transform:translateY(-2px)}.card .eyebrow{margin:0;color:var(--muted)}.card h3{font-size:1.25rem;margin:.55rem 0 .3rem;color:var(--ink)}.card p{color:var(--text);margin:0}.arrow{margin-top:auto;padding-top:1rem;color:var(--accent);font-weight:700}footer{margin-top:3rem;padding-top:1rem;border-top:1px solid var(--line);color:var(--muted);font-size:.9rem}@media(max-width:760px){.grid{grid-template-columns:1fr}.card{min-height:0}.latest{align-items:flex-start}.latest .button{white-space:normal}}@media(prefers-reduced-motion:reduce){*,*::before,*::after{scroll-behavior:auto!important;transition:none!important}}
  </style>
  <style>.button{color:var(--bg)}@media(min-width:601px) and (max-width:900px){.grid{grid-template-columns:repeat(2,minmax(0,1fr))}}@media(max-width:600px){.grid{grid-template-columns:1fr}.card{min-height:0}}</style>
</head>
<body>
  <div class="shell">
    <header><img class="mark" src="assets/raccord-mark.svg" alt=""><span class="wordmark">RACCORD</span><span class="spacer"></span><div class="appearance" role="group" aria-label="Color theme"><button type="button" data-appearance-set="auto" aria-pressed="true">Auto</button><button type="button" data-appearance-set="light" aria-pressed="false">Light</button><button type="button" data-appearance-set="dark" aria-pressed="false">Dark</button></div></header>
    <main id="main"><section class="hero"><p class="eyebrow">Engineering snapshots</p><h1>How Raccord is built, in the open.</h1><p class="intro">Architecture, product shape, and real browser journeys from the latest application revision—kept together in one place.</p></section>
      <section class="latest" aria-labelledby="latest-title"><div><p class="eyebrow">Latest browser run</p><h2 id="latest-title">Nightly journey evidence</h2>${latestRun}</div><a class="button" href="${latestLink}">View latest report <span aria-hidden="true">→</span></a></section>
      <h2 class="section-label">Explore the project</h2><nav class="grid" aria-label="Engineering snapshots"><a class="card" href="architecture/"><span class="eyebrow">System design</span><h3>Architecture</h3><p>System boundaries, runtime structure, and key engineering decisions.</p><span class="arrow">Explore architecture →</span></a><a class="card" href="model/"><span class="eyebrow">Product definition</span><h3>Product model</h3><p>The current product shape, workflows, and domain concepts.</p><span class="arrow">Explore product model →</span></a><a class="card" href="${latestLink}"><span class="eyebrow">Quality evidence</span><h3>Browser journeys</h3><p>Screenshots and videos captured while exercising the running application.</p><span class="arrow">See the latest run →</span></a></nav>
    </main><footer>Public engineering material for Raccord · Browser reports are retained for up to 30 days (maximum 14 runs).</footer>
  </div>
  <script>
    (()=>{const root=document.documentElement;const buttons=[...document.querySelectorAll('[data-appearance-set]')];const apply=choice=>{if(choice==='auto')root.removeAttribute('data-appearance');else root.setAttribute('data-appearance',choice);buttons.forEach(button=>button.setAttribute('aria-pressed',String(button.dataset.appearanceSet===choice)));try{localStorage.setItem('raccord-appearance',choice)}catch{}};let choice='auto';try{choice=localStorage.getItem('raccord-appearance')||'auto'}catch{}apply(choice);buttons.forEach(button=>button.addEventListener('click',()=>apply(button.dataset.appearanceSet)));for(const time of document.querySelectorAll('time[data-local-time]')){const date=new Date(time.dateTime);if(Number.isNaN(date.getTime()))continue;try{const zone=Intl.DateTimeFormat().resolvedOptions().timeZone;time.textContent=new Intl.DateTimeFormat(undefined,{dateStyle:'medium',timeStyle:'short'}).format(date);if(zone)time.title=zone;time.setAttribute('aria-label',time.textContent+' in your local time zone')}catch{time.textContent=date.toISOString().replace('T',' ').replace(/\\.\\d{3}Z$/,' UTC')}}})();
  </script>
</body>
</html>`);
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

    // Correct retained pre-guard reports too; publishing the new site should not
    // continue to present an old zero-journey run as successful.
    for (const item of history) {
        const reportDirectory = join(reportsDirectory, String(item.runId));
        const reportJsonPath = join(reportDirectory, "report.json");
        try {
            const report = JSON.parse(readFileSync(reportJsonPath, "utf8"));
            if (
                Array.isArray(report.journeys) &&
                report.journeys.length === 0
            ) {
                report.workflowStatus ??= report.overallStatus;
                report.overallStatus = "failure";
                item.status = "failure";
                writeFileSync(reportJsonPath, JSON.stringify(report, null, 2));
                const reportHtmlPath = join(reportDirectory, "index.html");
                if (
                    statSync(reportHtmlPath, {
                        throwIfNoEntry: false,
                    })?.isFile()
                ) {
                    const reportHtml = readFileSync(reportHtmlPath, "utf8");
                    writeFileSync(
                        reportHtmlPath,
                        reportHtml.replace(
                            /workflow result: <strong(?: class="[^"]*")?>([^<]+)<\/strong>/,
                            'workflow result: <strong style="color:var(--raccord-error)">failure</strong>',
                        ),
                    );
                }
            }
        } catch {
            // A missing or malformed legacy manifest does not discard its history entry.
        }
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

    // Re-render retained runs from their manifests so a visual update reaches
    // old evidence too, without requiring a fresh E2E execution or re-copying assets.
    for (const report of history) {
        const reportDirectory = join(reportsDirectory, String(report.runId));
        const manifestPath = join(reportDirectory, "report.json");
        if (!statSync(manifestPath, { throwIfNoEntry: false })?.isFile())
            continue;
        const manifest = JSON.parse(readFileSync(manifestPath, "utf8"));
        buildReport({
            outputDirectory: reportDirectory,
            metadata: manifest,
            existingReport: manifest,
        });
    }

    mkdirSync(join(root, "e2e"), { recursive: true });
    writeFileSync(historyPath, JSON.stringify({ reports: history }, null, 2));
    const latest = history[0];
    let stableReportHtml;
    if (latest) {
        const reportPath = join(root, "e2e", latest.path);
        const earlierReports = history.slice(1);
        const archiveFor = (hrefPrefix) =>
            earlierReports
                .map(
                    (report) =>
                        `<li><a href="${hrefPrefix}${escapeHtml(String(report.runId))}/index.html">${escapeHtml(report.date)} · ${escapeHtml(report.sha.slice(0, 8))}</a> <span>${escapeHtml(report.status)}</span></li>`,
                )
                .join("");
        const archiveNavFor = (hrefPrefix) => {
            const items = archiveFor(hrefPrefix);
            return items
                ? `<section class="nav-group archive-nav"><h3>Run history</h3><ul class="nav-list">${earlierReports
                      .map(
                          (report) =>
                              `<li><a href="${hrefPrefix}${escapeHtml(String(report.runId))}/index.html" data-status="${escapeHtml(report.status)}"><span class="nav-icon"><svg viewBox="0 0 16 16" aria-hidden="true"><circle cx="8" cy="8" r="6"/><path d="M8 4v4l3 2"/></svg></span><span class="nav-title">${escapeHtml(report.date.slice(0, 10))} · ${escapeHtml(report.sha.slice(0, 7))}</span></a></li>`,
                      )
                      .join("")}</ul></section>`
                : "";
        };
        const archiveContentFor = (hrefPrefix) => {
            const items = archiveFor(hrefPrefix);
            return items
                ? `<section class="archive" aria-labelledby="archive-title"><h2 id="archive-title">Earlier runs</h2><ul>${items}</ul><p>Reports are retained for up to 30 days (maximum 14 runs).</p></section>`
                : "";
        };
        const reportHtml = readFileSync(reportPath, "utf8");
        const currentReportHtml = reportHtml
            .replace('<div id="archive-nav"></div>', archiveNavFor("../"))
            .replace(
                '<div id="archive-content"></div>',
                archiveContentFor("../"),
            );
        if (statSync(reportPath, { throwIfNoEntry: false })?.isFile()) {
            writeFileSync(reportPath, currentReportHtml);
        }
        stableReportHtml = reportHtml
            .replace('<div id="archive-nav"></div>', archiveNavFor("reports/"))
            .replace(
                '<div id="archive-content"></div>',
                archiveContentFor("reports/"),
            )
            .replace(
                'src="../../../assets/raccord-mark.svg"',
                'src="../assets/raccord-mark.svg"',
            )
            .replaceAll('href="../../../index.html"', 'href="../index.html"')
            .replaceAll(
                'href="assets/',
                `href="reports/${escapeHtml(String(latest.runId))}/assets/`,
            )
            .replaceAll(
                'src="assets/',
                `src="reports/${escapeHtml(String(latest.runId))}/assets/`,
            );
    }
    const noReportsHtml = includeDesignTokens(
        `<!doctype html><html lang="en"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><meta name="color-scheme" content="light dark"><title>Raccord · Browser journeys</title><style>:root{--bg:var(--raccord-cloud);--surface:var(--raccord-white);--ink:var(--raccord-ink);--text:var(--raccord-ink);--muted:var(--raccord-slate);--line:var(--raccord-border);--accent:var(--raccord-action);--space-1:var(--raccord-space-1);--space-2:var(--raccord-space-2);--space-3:var(--raccord-space-3);--space-4:var(--raccord-space-4);--space-6:var(--raccord-space-6);--space-8:var(--raccord-space-8);--radius-sm:var(--raccord-radius-sm);--radius-md:var(--raccord-radius-md);--radius-lg:var(--raccord-radius-lg);--focus-width:var(--raccord-focus-width);--focus-offset:var(--raccord-focus-offset);}*{box-sizing:border-box}body{margin:0;background:var(--bg);color:var(--text);font:var(--raccord-text-sm)/var(--raccord-leading) var(--raccord-font-sans);padding:1rem}main{max-width:60rem;margin:10vh auto;padding:2rem;background:var(--surface);border:1px solid var(--line);border-radius:var(--radius-lg)}h1{color:var(--ink)}p{color:var(--muted)}a{color:var(--accent)}button{margin-right:.3rem;padding:.5rem .75rem;color:var(--text);background:var(--surface);border:1px solid var(--line);border-radius:var(--radius-sm);cursor:pointer}</style></head><body><main><p>Raccord · Browser journeys</p><h1>No nightly report yet</h1><p>The next scheduled browser journey run will publish its screenshots and videos here.</p><p><a href="../index.html">Engineering snapshots</a></p><div aria-label="Appearance"><button data-theme="auto">System</button><button data-theme="light">Light</button><button data-theme="dark">Dark</button></div></main><script>document.querySelectorAll('[data-theme]').forEach(button=>button.addEventListener('click',()=>{const theme=button.dataset.theme;if(theme==='auto')document.documentElement.removeAttribute('data-appearance');else document.documentElement.setAttribute('data-appearance',theme)}))</script></body></html>`,
    );
    const latestHtml = stableReportHtml ?? noReportsHtml;
    writeFileSync(join(root, "e2e", "latest.html"), latestHtml);
    writeFileSync(join(root, "e2e", "index.html"), latestHtml);
    const sourceRoot = resolve(
        dirname(fileURLToPath(import.meta.url)),
        "../..",
    );
    mkdirSync(join(root, "assets"), { recursive: true });
    copyFileSync(
        join(sourceRoot, "docs", "design", "raccord-mark.svg"),
        join(root, "assets", "raccord-mark.svg"),
    );
    const latestLink = latest ? "e2e/latest.html" : "e2e/";
    let latestReport;
    if (latest) {
        const manifestPath = join(
            root,
            "e2e",
            latest.path.replace(/index\.html$/, "report.json"),
        );
        try {
            latestReport = JSON.parse(readFileSync(manifestPath, "utf8"));
        } catch {
            latestReport = undefined;
        }
    }
    const latestRun = renderLatestRun(latest, latestReport);
    writeFileSync(
        join(root, "index.html"),
        renderLandingPage({ latestRun, latestLink }),
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
