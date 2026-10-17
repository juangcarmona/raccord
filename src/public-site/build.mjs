import { cp, mkdir, rm, writeFile } from "node:fs/promises";
import { fileURLToPath } from "node:url";
import path from "node:path";
import { renderPage, routes } from "./render.mjs";

const sourceDirectory = path.dirname(fileURLToPath(import.meta.url));
const outputDirectory = path.join(sourceDirectory, "dist");

await rm(outputDirectory, { recursive: true, force: true });
await mkdir(outputDirectory, { recursive: true });
await cp(
    path.join(sourceDirectory, "styles.css"),
    path.join(outputDirectory, "styles.css"),
);
await cp(
    path.join(sourceDirectory, "images"),
    path.join(outputDirectory, "images"),
    { recursive: true },
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
    `<!doctype html><html lang="es"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1"><title>Raccord</title><script type="module" src="./locale.mjs"></script></head><body><a href="/es/">Raccord</a></body></html>`,
);

console.log(`Static homepage built at ${outputDirectory}`);
