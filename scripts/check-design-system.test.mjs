import { afterEach, beforeEach, test } from "node:test";
import assert from "node:assert/strict";
import { mkdtemp, mkdir, readFile, rm, writeFile } from "node:fs/promises";
import os from "node:os";
import path from "node:path";
import { checkDesignSystem } from "./check-design-system.mjs";

let root;

beforeEach(async () => {
    root = await mkdtemp(path.join(os.tmpdir(), "raccord-design-system-"));
    const files = {
        "docs/design/tokens.css":
            ":root { --raccord-ink: #0b1020; --raccord-cloud: #f5f7fa; --raccord-slate: #667085; --raccord-white: #ffffff; --raccord-selected: #0b7fbf; --raccord-selected-soft: #edf6fc; --raccord-action: #075a88; --raccord-action-hover: #08699e; --raccord-font-sans: system-ui; --raccord-space-4: 1rem; --raccord-radius-md: .5rem; --raccord-focus-width: 3px; --raccord-success: #1d7a4f; --raccord-success-soft: #e3f5ee; --raccord-warning: #8a5a00; --raccord-warning-soft: #fff3d6; --raccord-error: #b3261e; --raccord-error-soft: #fde3e0; }",
        "src/Raccord.Frontend/src/styles.css":
            '@import url("../../../docs/design/tokens.css"); :root { --bg: var(--raccord-cloud); --surface: var(--raccord-white); --text: var(--raccord-ink); --muted: var(--raccord-slate); --border: var(--raccord-border); --accent: var(--raccord-action); --danger: var(--raccord-error); --accent-text: var(--raccord-white); --warning-bg: var(--raccord-warning-soft); --warning-border: var(--raccord-warning); --ok: var(--raccord-success); --focus: var(--raccord-selected); --focus-width: var(--raccord-focus-width); --focus-offset: var(--raccord-focus-offset); --space-4: var(--raccord-space-4); --radius: var(--raccord-radius-sm); }',
        "src/public-site/build.mjs": 'readFile("docs/design/tokens.css")',
        "src/public-site/styles.css":
            ":root { --ink: var(--raccord-ink); --paper: var(--raccord-cloud); --paper-raised: var(--raccord-white); --line: var(--raccord-border); --accent: var(--raccord-action); --accent-hover: var(--raccord-action-hover); --focus-width: var(--raccord-focus-width); --focus-offset: var(--raccord-focus-offset); --space-4: var(--raccord-space-4); --radius-md: var(--raccord-radius-md); }",
        "tools/static-site/static-site.mjs":
            'readFileSync(resolve(root, "docs/design/tokens.css")); :root{--bg:var(--raccord-cloud);--panel:var(--raccord-selected-soft);--surface:var(--raccord-white);--ink:var(--raccord-ink);--text:var(--raccord-ink);--muted:var(--raccord-slate);--line:var(--raccord-border);--accent:var(--raccord-action);--pass:var(--raccord-success);--fail:var(--raccord-error);--flaky:var(--raccord-warning)}',
        "tools/arc42-snapshot/generate.mjs":
            'resolve(docsDir, "../../design/tokens.css")',
        "tools/arc42-snapshot/shell.mjs":
            "--bg: var(--raccord-cloud); --panel: var(--raccord-selected-soft); --raise: var(--raccord-white); --ink: var(--raccord-ink); --accent: var(--raccord-action); --space-4: var(--raccord-space-4); --radius-md: var(--raccord-radius-md); --focus-width: var(--raccord-focus-width)",
        "tools/arc42-snapshot/render.mjs": "primaryColor: '#0b1020'",
        "docs/design/token-exceptions.md": "Mermaid mirrors canonical values.",
    };
    for (const [file, contents] of Object.entries(files)) {
        const target = path.join(root, file);
        await mkdir(path.dirname(target), { recursive: true });
        await writeFile(target, contents);
    }
});

afterEach(async () => rm(root, { recursive: true, force: true }));

test("accepts consumers that load canonical tokens", async () => {
    assert.deepEqual(await checkDesignSystem(root), []);
});

test("rejects a missing surface token import", async () => {
    await writeFile(path.join(root, "src/public-site/build.mjs"), "export {};");
    assert.ok(
        (await checkDesignSystem(root)).some((error) =>
            error.includes("public site: does not load"),
        ),
    );
});

test("rejects a redefined canonical token regardless of the chosen value", async () => {
    await writeFile(
        path.join(root, "src/public-site/styles.css"),
        ":root { --raccord-ink: #111111; }",
    );
    assert.ok(
        (await checkDesignSystem(root)).some((error) =>
            error.includes("redefines shared token --raccord-ink"),
        ),
    );
});

test("rejects a shared alias mapped to the wrong canonical token", async () => {
    const file = path.join(root, "src/public-site/styles.css");
    await writeFile(
        file,
        ":root { --ink: var(--raccord-selected); --paper: var(--raccord-cloud); --paper-raised: var(--raccord-white); --line: var(--raccord-border); --accent: var(--raccord-action); --accent-hover: var(--raccord-action-hover); --focus-width: var(--raccord-focus-width); --focus-offset: var(--raccord-focus-offset); --space-4: var(--raccord-space-4); --radius-md: var(--raccord-radius-md); }",
    );
    assert.ok(
        (await checkDesignSystem(root)).some((error) =>
            error.includes("--ink must map to var(--raccord-ink)"),
        ),
    );
});

test("rejects a token value that breaks the documented contrast floor", async () => {
    const file = path.join(root, "docs/design/tokens.css");
    const source = await readFile(file, "utf8");
    await writeFile(
        file,
        source.replace(
            "--raccord-action: #075a88",
            "--raccord-action: #f5f7fa",
        ),
    );
    assert.ok(
        (await checkDesignSystem(root)).some((error) =>
            error.includes("--raccord-action on --raccord-cloud contrast"),
        ),
    );
});

test("rejects legacy palette drift in a surface stylesheet", async () => {
    await writeFile(
        path.join(root, "src/public-site/styles.css"),
        ":root { --forest: #111917; }",
    );
    assert.ok(
        (await checkDesignSystem(root)).some((error) =>
            error.includes("legacy forest/lime palette"),
        ),
    );
});

test("rejects an outdated literal required by the diagram renderer", async () => {
    const file = path.join(root, "tools/arc42-snapshot/render.mjs");
    await writeFile(file, "primaryColor: '#010101'");
    assert.ok(
        (await checkDesignSystem(root)).some((error) =>
            error.includes("architecture diagrams: #010101"),
        ),
    );
});
