import assert from "node:assert/strict";
import { readdir, readFile } from "node:fs/promises";
import { describe, it } from "node:test";
import { fileURLToPath } from "node:url";
import path from "node:path";

const siteDirectory = path.dirname(fileURLToPath(import.meta.url));
const outputDirectory = path.join(siteDirectory, "dist");

describe("public site build", () => {
    it("emits only the public homepage and its stylesheet", async () => {
        const files = (await readdir(outputDirectory)).sort();

        assert.deepEqual(files, ["index.html", "styles.css"]);
        const configText = await readFile(
            path.join(siteDirectory, "wrangler.jsonc"),
            "utf8",
        );
        const hostingConfig = JSON.parse(configText.replace(/,\s*(?=})/g, ""));

        assert.equal(hostingConfig.name, "raccord-public-site");
        assert.equal(hostingConfig.pages_build_output_dir, "./dist");
        assert.match(
            await readFile(path.join(outputDirectory, "index.html"), "utf8"),
            /<html lang="es">/,
        );
        assert.match(
            await readFile(path.join(outputDirectory, "index.html"), "utf8"),
            /href="\.\/styles\.css"/,
        );
        assert.ok(
            (await readFile(path.join(outputDirectory, "styles.css"), "utf8"))
                .length > 0,
        );
    });
});
