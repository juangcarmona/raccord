import assert from "node:assert/strict";
import { readdir, readFile } from "node:fs/promises";
import { describe, it } from "node:test";
import { fileURLToPath } from "node:url";
import path from "node:path";
import { resolveLocale, rootLocalePath } from "./locale.mjs";

const siteDirectory = path.dirname(fileURLToPath(import.meta.url));
const outputDirectory = path.join(siteDirectory, "dist");

describe("public site build", () => {
    it("emits the root page and complete Spanish and English locales", async () => {
        const files = (await readdir(outputDirectory)).sort();

        assert.deepEqual(files, ["en", "es", "images", "index.html", "locale.mjs", "styles.css"]);
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
            /src="\.\/locale\.mjs"/,
        );
        assert.ok(
            (await readFile(path.join(outputDirectory, "styles.css"), "utf8"))
                .length > 0,
        );
        const imageFiles = await readdir(path.join(outputDirectory, "images"));
        assert.deepEqual(imageFiles, ["invoice-issued.png"]);
        const homepage = await readFile(
            path.join(outputDirectory, "index.html"),
            "utf8",
        );
        assert.match(homepage, /src="\.\.\/images\/invoice-issued\.png"/);
        assert.match(homepage, /alt="Factura emitida de ejemplo/);

        const spanishPage = await readFile(
            path.join(outputDirectory, "es", "index.html"),
            "utf8",
        );
        const englishPage = await readFile(
            path.join(outputDirectory, "en", "index.html"),
            "utf8",
        );
        assert.match(spanishPage, /<html lang="es">/);
        assert.match(englishPage, /<html lang="en">/);
        assert.match(spanishPage, /hreflang="en" href="https:\/\/raccord\.dev\/en\/"/);
        assert.match(englishPage, /hreflang="es" href="https:\/\/raccord\.dev\/es\/"/);
        assert.match(spanishPage, /href="\.\.\/en\/" lang="en"/);
        assert.match(englishPage, /href="\.\.\/es\/" lang="es"/);
        assert.match(spanishPage, /aria-label="Selector de idioma"/);
        assert.match(englishPage, /aria-label="Language selector"/);
        assert.match(spanishPage, /Raccord no afirma cumplir/);
        assert.match(englishPage, /Raccord does not claim legal or/);
        assert.doesNotMatch(spanishPage, /locale\.mjs/);
        assert.doesNotMatch(englishPage, /locale\.mjs/);
        assert.deepEqual(await readdir(path.join(outputDirectory, "es")), ["index.html"]);
        assert.deepEqual(await readdir(path.join(outputDirectory, "en")), ["index.html"]);
    });

    it("selects the first supported browser language and falls back to Spanish", () => {
        assert.equal(resolveLocale(["en-GB", "es-ES"]), "en");
        assert.equal(resolveLocale(["fr-FR", "es-ES", "en-US"]), "es");
        assert.equal(resolveLocale(["fr-FR", "de-DE"]), "es");
        assert.equal(resolveLocale([]), "es");
        assert.equal(rootLocalePath(["en-GB", "es-ES"]), "/en/");
        assert.equal(rootLocalePath(["fr-FR", "es-ES"]), "/es/");
        assert.equal(rootLocalePath(["fr-FR"]), "/es/");
    });
});
