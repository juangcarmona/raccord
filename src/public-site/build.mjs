import { cp, mkdir, readFile, rm, writeFile } from "node:fs/promises";
import { fileURLToPath } from "node:url";
import path from "node:path";

const sourceDirectory = path.dirname(fileURLToPath(import.meta.url));
const outputDirectory = path.join(sourceDirectory, "dist");

await rm(outputDirectory, { recursive: true, force: true });
await mkdir(outputDirectory, { recursive: true });
await cp(
    path.join(sourceDirectory, "index.html"),
    path.join(outputDirectory, "index.html"),
);
await cp(
    path.join(sourceDirectory, "styles.css"),
    path.join(outputDirectory, "styles.css"),
);
await cp(
    path.join(sourceDirectory, "images"),
    path.join(outputDirectory, "images"),
    { recursive: true },
);
await mkdir(path.join(outputDirectory, "es"), { recursive: true });
await cp(
    path.join(sourceDirectory, "index.html"),
    path.join(outputDirectory, "es", "index.html"),
);
await cp(
    path.join(sourceDirectory, "locales", "en"),
    path.join(outputDirectory, "en"),
    { recursive: true },
);
await cp(
    path.join(sourceDirectory, "locale.mjs"),
    path.join(outputDirectory, "locale.mjs"),
);

const rootHtmlPath = path.join(outputDirectory, "index.html");
const rootHtml = await readFile(rootHtmlPath, "utf8");
await writeFile(
    rootHtmlPath,
    rootHtml.replace(
        "</body>",
        '    <script type="module" src="./locale.mjs"></script>\n    </body>',
    ),
);

console.log(`Static homepage built at ${outputDirectory}`);
