import { cp, mkdir, readFile, rm, writeFile } from "node:fs/promises";
import { fileURLToPath } from "node:url";
import path from "node:path";
import { renderPage, routes } from "./render.mjs";

const sourceDirectory = path.dirname(fileURLToPath(import.meta.url));
const outputDirectory = path.join(sourceDirectory, "dist");
const repositoryRoot = path.resolve(sourceDirectory, "../..");
const sharedTokens = await readFile(
    path.join(repositoryRoot, "docs/design/tokens.css"),
    "utf8",
);
const siteStyles = await readFile(
    path.join(sourceDirectory, "styles.css"),
    "utf8",
);

await rm(outputDirectory, { recursive: true, force: true });
await mkdir(outputDirectory, { recursive: true });
await writeFile(
    path.join(outputDirectory, "styles.css"),
    `${sharedTokens}\n${siteStyles}`,
);
await cp(
    path.join(sourceDirectory, "images"),
    path.join(outputDirectory, "images"),
    { recursive: true },
);
await cp(
    path.join(repositoryRoot, "src/Raccord.Frontend/public/raccord-mark.svg"),
    path.join(outputDirectory, "raccord-mark.svg"),
);
const pageKeys = Object.keys(routes);
for (const locale of ["es", "en"]) {
    for (const key of pageKeys) {
        const route = routes[key];
        const pageDirectory = path.join(outputDirectory, locale, route);
        await mkdir(pageDirectory, { recursive: true });
        await writeFile(
            path.join(pageDirectory, "index.html"),
            renderPage(locale, key),
        );
    }
}
await cp(
    path.join(sourceDirectory, "locale.mjs"),
    path.join(outputDirectory, "locale.mjs"),
);

await writeFile(
    path.join(outputDirectory, "index.html"),
    `<!doctype html><html lang="es"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1"><meta name="color-scheme" content="light dark"><title>Raccord · Idioma / Language</title><link rel="stylesheet" href="./styles.css"><script type="module" src="./locale.mjs"></script></head><body><main class="locale-handoff"><a class="wordmark" href="/es/" aria-label="Raccord"><img class="brand-mark" src="/raccord-mark.svg" alt="" aria-hidden="true" />Raccord<span>.</span></a><h1>Elige tu idioma <span aria-hidden="true">/</span> Choose your language</h1><nav aria-label="Idioma / Language"><a lang="es" href="/es/">Español</a><a lang="en" href="/en/">English</a></nav></main></body></html>`,
);

console.log(`Static homepage built at ${outputDirectory}`);
