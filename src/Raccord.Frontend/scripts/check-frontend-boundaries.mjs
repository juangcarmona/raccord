import { readdir, readFile } from "node:fs/promises";
import path from "node:path";
import process from "node:process";
import { fileURLToPath } from "node:url";

const sourceRoot = path.resolve("src");
const layerNames = new Set(["app", "features", "shared"]);

function layerOf(filePath) {
    const relative = path.relative(sourceRoot, filePath).split(path.sep);
    if (relative[0] === "features") return { name: "features", feature: relative[1] };
    if (layerNames.has(relative[0])) return { name: relative[0] };
    return null;
}

function importedLayer(sourcePath, specifier) {
    if (!specifier.startsWith(".")) return null;
    const target = path.resolve(path.dirname(sourcePath), specifier);
    return layerOf(target);
}

export function findBoundaryViolations(files) {
    const violations = [];
    const importPattern = /(?:import|export)\s+(?:[\s\S]*?\s+from\s+)?["'](\.[^"']+)["']/g;

    for (const file of files) {
        const source = layerOf(file.path);
        if (!source) continue;
        for (const match of file.content.matchAll(importPattern)) {
            const target = importedLayer(file.path, match[1]);
            if (!target) continue;
            const forbidden =
                (source.name === "shared" && target.name !== "shared") ||
                (source.name === "features" && target.name !== "shared" && target.feature !== source.feature);
            if (forbidden) {
                violations.push(
                    `${path.relative(process.cwd(), file.path)} imports ${target.name}${target.feature ? `/${target.feature}` : ""}`,
                );
            }
        }
    }
    return violations;
}

async function sourceFiles(directory) {
    const entries = await readdir(directory, { withFileTypes: true });
    const files = [];
    for (const entry of entries) {
        const filePath = path.join(directory, entry.name);
        if (entry.isDirectory()) files.push(...(await sourceFiles(filePath)));
        else if (/\.(?:ts|tsx)$/.test(entry.name))
            files.push({ path: filePath, content: await readFile(filePath, "utf8") });
    }
    return files;
}

if (path.resolve(process.argv[1]) === fileURLToPath(import.meta.url)) {
    const violations = findBoundaryViolations(await sourceFiles(sourceRoot));
    if (violations.length > 0) {
        console.error("Frontend dependency boundary violations:");
        for (const violation of violations) console.error(`- ${violation}`);
        process.exitCode = 1;
    }
}
