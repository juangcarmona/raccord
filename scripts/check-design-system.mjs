#!/usr/bin/env node
import { readFile } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

export async function checkDesignSystem(
    root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), ".."),
) {
    const read = (file) => readFile(path.join(root, file), "utf8");
    const errors = [];
    let canonical;
    try {
        canonical = await read("docs/design/tokens.css");
    } catch {
        return ["Missing canonical token source: docs/design/tokens.css"];
    }
    const tokenValues = new Set(
        [...canonical.matchAll(/(--raccord-[\w-]+)\s*:\s*([^;]+);/g)].map(
            (match) => match[2].trim().toLowerCase(),
        ),
    );
    const tokens = new Set(
        [...canonical.matchAll(/(--raccord-[\w-]+)\s*:/g)].map(
            (match) => match[1],
        ),
    );

    const colorValues = (name) =>
        [
            ...canonical.matchAll(
                new RegExp(`${name}\\s*:\\s*(#[0-9a-f]{3,8})\\s*;`, "gi"),
            ),
        ].map((match) => match[1]);
    const luminance = (hex) => {
        const digits = hex.slice(1);
        const normalized =
            digits.length === 3
                ? [...digits].map((digit) => digit + digit)
                : digits.match(/../g);
        const [red, green, blue] = normalized.map((channel) => {
            const value = parseInt(channel, 16) / 255;
            return value <= 0.04045
                ? value / 12.92
                : ((value + 0.055) / 1.055) ** 2.4;
        });
        return 0.2126 * red + 0.7152 * green + 0.0722 * blue;
    };
    const contrast = (foreground, background) => {
        const first = luminance(foreground);
        const second = luminance(background);
        return (
            (Math.max(first, second) + 0.05) / (Math.min(first, second) + 0.05)
        );
    };
    for (const required of [
        "--raccord-ink",
        "--raccord-selected",
        "--raccord-action",
        "--raccord-action-hover",
        "--raccord-cloud",
        "--raccord-white",
        "--raccord-slate",
        "--raccord-font-sans",
        "--raccord-space-4",
        "--raccord-radius-md",
        "--raccord-focus-width",
        "--raccord-success",
        "--raccord-warning",
        "--raccord-error",
    ]) {
        if (!tokens.has(required))
            errors.push(`Canonical source is missing ${required}`);
    }
    const contrastPairs = [
        ["--raccord-ink", "--raccord-cloud", 4.5],
        ["--raccord-slate", "--raccord-cloud", 4.5],
        ["--raccord-action", "--raccord-cloud", 4.5],
        ["--raccord-white", "--raccord-action", 4.5],
        ["--raccord-white", "--raccord-action-hover", 4.5],
        ["--raccord-success", "--raccord-success-soft", 4.5],
        ["--raccord-warning", "--raccord-warning-soft", 4.5],
        ["--raccord-error", "--raccord-error-soft", 4.5],
        ["--raccord-selected", "--raccord-cloud", 3],
    ];
    for (const [foreground, background, minimum] of contrastPairs) {
        const foregroundValues = colorValues(foreground);
        const backgroundValues = colorValues(background);
        if (!foregroundValues.length || !backgroundValues.length) continue;
        for (const theme of [0, 1]) {
            const ratio = contrast(
                foregroundValues[Math.min(theme, foregroundValues.length - 1)],
                backgroundValues[Math.min(theme, backgroundValues.length - 1)],
            );
            if (ratio < minimum)
                errors.push(
                    `${foreground} on ${background} contrast is ${ratio.toFixed(2)}:1 in ${theme ? "dark" : "light"} theme; expected at least ${minimum}:1`,
                );
        }
    }
    const consumers = [
        [
            "app",
            "src/Raccord.Frontend/src/styles.css",
            /@import\s+url\(["']?\.\.\/\.\.\/\.\.\/docs\/design\/tokens\.css["']?\)/,
        ],
        [
            "public site",
            "src/public-site/build.mjs",
            /docs\/design\/tokens\.css/,
        ],
        ["public site styles", "src/public-site/styles.css", /--raccord-/],
        [
            "engineering report",
            "tools/static-site/static-site.mjs",
            /docs\/design\/tokens\.css/,
        ],
        [
            "architecture snapshot",
            "tools/arc42-snapshot/generate.mjs",
            /resolve\(docsDir, ['"]\.\.\/\.\.\/design\/tokens\.css['"]\)/,
        ],
        [
            "architecture snapshot styles",
            "tools/arc42-snapshot/shell.mjs",
            /var\(--raccord-/,
        ],
    ];
    const sharedDefinitions = new RegExp(
        `(?:^|[;{\\s])(--raccord-[\\w-]+)\\s*:`,
        "gm",
    );
    const aliases = new Map([
        [
            "src/Raccord.Frontend/src/styles.css",
            {
                "--bg": "var(--raccord-cloud)",
                "--surface": "var(--raccord-white)",
                "--text": "var(--raccord-ink)",
                "--muted": "var(--raccord-slate)",
                "--border": "var(--raccord-border)",
                "--accent": "var(--raccord-action)",
                "--accent-text": "var(--raccord-white)",
                "--danger": "var(--raccord-error)",
                "--warning-bg": "var(--raccord-warning-soft)",
                "--warning-border": "var(--raccord-warning)",
                "--ok": "var(--raccord-success)",
                "--focus": "var(--raccord-selected)",
                "--focus-width": "var(--raccord-focus-width)",
                "--focus-offset": "var(--raccord-focus-offset)",
                "--space-4": "var(--raccord-space-4)",
                "--radius": "var(--raccord-radius-sm)",
            },
        ],
        [
            "tools/static-site/static-site.mjs",
            {
                "--bg": "var(--raccord-cloud)",
                "--panel": "var(--raccord-selected-soft)",
                "--surface": "var(--raccord-white)",
                "--ink": "var(--raccord-ink)",
                "--text": "var(--raccord-ink)",
                "--muted": "var(--raccord-slate)",
                "--line": "var(--raccord-border)",
                "--accent": "var(--raccord-action)",
                "--pass": "var(--raccord-success)",
                "--fail": "var(--raccord-error)",
                "--flaky": "var(--raccord-warning)",
            },
        ],
        [
            "src/public-site/styles.css",
            {
                "--ink": "var(--raccord-ink)",
                "--paper": "var(--raccord-cloud)",
                "--paper-raised": "var(--raccord-white)",
                "--line": "var(--raccord-border)",
                "--accent": "var(--raccord-action)",
                "--accent-hover": "var(--raccord-action-hover)",
                "--focus-width": "var(--raccord-focus-width)",
                "--focus-offset": "var(--raccord-focus-offset)",
                "--space-4": "var(--raccord-space-4)",
                "--radius-md": "var(--raccord-radius-md)",
            },
        ],
        [
            "tools/arc42-snapshot/shell.mjs",
            {
                "--bg": "var(--raccord-cloud)",
                "--panel": "var(--raccord-selected-soft)",
                "--raise": "var(--raccord-white)",
                "--ink": "var(--raccord-ink)",
                "--accent": "var(--raccord-action)",
                "--space-4": "var(--raccord-space-4)",
                "--radius-md": "var(--raccord-radius-md)",
                "--focus-width": "var(--raccord-focus-width)",
            },
        ],
    ]);
    for (const [surface, file, required] of consumers) {
        let source;
        try {
            source = await read(file);
        } catch {
            errors.push(`${surface}: missing consumer ${file}`);
            continue;
        }
        if (!required.test(source))
            errors.push(`${surface}: does not load the canonical token source`);
        for (const [, token] of source.matchAll(sharedDefinitions)) {
            if (tokens.has(token))
                errors.push(`${surface}: redefines shared token ${token}`);
        }
        for (const [alias, value] of Object.entries(aliases.get(file) ?? {})) {
            const declaration = new RegExp(`${alias}\\s*:\\s*([^;\\n}]+)`, "m")
                .exec(source)?.[1]
                ?.trim();
            if (declaration !== value)
                errors.push(`${surface}: ${alias} must map to ${value}`);
        }
    }
    const publicCss = await read("src/public-site/styles.css");
    if (
        /--(?:lime|forest|peach)\s*:|#(?:c5eb68|9bc23b|e89d76|111917)\b|rgba?\(\s*(?:197,\s*235,\s*104|232,\s*157,\s*118)\b/i.test(
            publicCss,
        )
    )
        errors.push("public site: legacy forest/lime palette remains");
    const report = await read("tools/static-site/static-site.mjs");
    if (/--(?:bg|accent|pass):#(?:fbfbfa|0b5cad|16804a)\b/i.test(report))
        errors.push("engineering report: legacy shared palette remains");
    const exceptions = await read("docs/design/token-exceptions.md");
    if (
        !exceptions.includes("Mermaid") ||
        !exceptions.includes("canonical values")
    )
        errors.push("Design-specific exceptions must be documented");
    const diagramConfig = await read("tools/arc42-snapshot/render.mjs");
    for (const [, color] of diagramConfig.matchAll(
        /(?:Color|Text|Border|lineColor|background):\s*'?(#[0-9a-f]{3,8})'?/gi,
    )) {
        if (!tokenValues.has(color.toLowerCase()))
            errors.push(
                `architecture diagrams: ${color} is not a canonical token value`,
            );
    }
    return errors;
}

if (
    process.argv[1] &&
    path.resolve(process.argv[1]) === fileURLToPath(import.meta.url)
) {
    const errors = await checkDesignSystem();
    if (errors.length) {
        console.error(errors.map((error) => `✗ ${error}`).join("\n"));
        process.exitCode = 1;
    } else {
        console.log("Design tokens are aligned across app and both sites.");
    }
}
