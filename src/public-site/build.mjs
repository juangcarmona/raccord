import { cp, mkdir, rm } from "node:fs/promises";
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

console.log(`Static homepage built at ${outputDirectory}`);
