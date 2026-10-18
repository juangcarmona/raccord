import assert from "node:assert/strict";
import { access, readFile } from "node:fs/promises";
import { describe, it } from "node:test";
import { fileURLToPath } from "node:url";
import path from "node:path";
import { routes } from "./render.mjs";
import { resolveLocale, rootLocalePath } from "./locale.mjs";

const siteDirectory = path.dirname(fileURLToPath(import.meta.url));
const outputDirectory = path.join(siteDirectory, "dist");
const pageKeys = Object.keys(routes);

async function fileExists(file) {
    try {
        await access(file);
        return true;
    } catch {
        return false;
    }
}

describe("public site build", () => {
    it("publishes the brand mark and uses it on localized architecture pages", async () => {
        assert.equal(
            await fileExists(path.join(outputDirectory, "raccord-mark.svg")),
            true,
        );

        for (const locale of ["es", "en"]) {
            const architecturePath = path.join(
                outputDirectory,
                locale,
                routes.engineering,
                "index.html",
            );
            const html = await readFile(architecturePath, "utf8");
            assert.match(
                html,
                /<img class="brand-mark" src="\.\.\/\.\.\/raccord-mark\.svg" alt="" aria-hidden="true"\s*\/>/,
            );
            assert.match(html, /class="diagram-node node-db"/);
            assert.match(html, /class="diagram-node node-files"/);
        }
    });

    it("keeps architecture nodes on distinct desktop and mobile grid tracks", async () => {
        const styles = await readFile(
            path.join(outputDirectory, "styles.css"),
            "utf8",
        );
        assert.match(
            styles,
            /\.node-db\s*\{\s*grid-column:\s*3;\s*grid-row:\s*3;/,
        );
        assert.match(
            styles,
            /\.node-files\s*\{\s*grid-column:\s*4;\s*grid-row:\s*3;/,
        );
        assert.match(
            styles,
            /\.node-web,\s*\.node-db,\s*\.node-files\s*\{\s*grid-column:\s*1;\s*grid-row:\s*auto;/,
        );
    });

    it("emits every page in Spanish and English", async () => {
        for (const locale of ["es", "en"]) {
            for (const key of pageKeys) {
                const pagePath = path.join(
                    outputDirectory,
                    locale,
                    routes[key],
                    "index.html",
                );
                assert.equal(
                    await fileExists(pagePath),
                    true,
                    `${locale}/${routes[key]} exists`,
                );
                const html = await readFile(pagePath, "utf8");
                assert.match(html, new RegExp(`<html lang="${locale}">`));
                assert.match(html, /<main id="contenido">/);
                assert.match(html, /class="skip-link"/);
                assert.match(html, /hreflang="es"/);
                assert.match(html, /hreflang="en"/);
                assert.match(
                    html,
                    /aria-label="(?:Selector de idioma|Language selector)"/,
                );

                for (const match of html.matchAll(/(?:href|src)="([^"]+)"/g)) {
                    const href = match[1];
                    if (
                        href.startsWith("#") ||
                        href.startsWith("https://") ||
                        href.startsWith("mailto:")
                    ) {
                        continue;
                    }
                    const destination = path.resolve(
                        path.dirname(pagePath),
                        href,
                    );
                    const normalized = href.endsWith("/")
                        ? path.join(destination, "index.html")
                        : destination;
                    assert.equal(
                        await fileExists(normalized),
                        true,
                        `${locale}/${routes[key]} -> ${href}`,
                    );
                }
            }
        }
    });

    it("presents all ten product areas and the evidence boundary in both locales", async () => {
        const labels = {
            es: [
                "Contactos",
                "Proyectos",
                "Comercial",
                "Facturación",
                "Equipos",
                "Almacén",
                "Equipo humano y transporte",
                "Documentos y comunicación",
                "Espacio de trabajo",
                "Control y trazabilidad",
            ],
            en: [
                "Contacts",
                "Projects",
                "Commercial",
                "Invoicing",
                "Equipment",
                "Warehouse",
                "Crew and transport",
                "Documents and communication",
                "Workspace",
                "Compliance and audit",
            ],
        };
        for (const locale of ["es", "en"]) {
            const product = await readFile(
                path.join(
                    outputDirectory,
                    locale,
                    routes.product,
                    "index.html",
                ),
                "utf8",
            );
            for (const label of labels[locale])
                assert.ok(
                    product.includes(label),
                    `${locale} product page contains ${label}`,
                );
            assert.match(product, /invoice-issued\.png/);
            assert.match(product, /fictional|ficticios/i);
            assert.match(product, /full-size image|imagen completa/i);
            assert.match(product, /availability|disponibilidad/i);
            assert.doesNotMatch(
                product,
                /VERI\*FACTU compliance|cumple VERI\*FACTU/i,
            );
            assert.equal(
                [...product.matchAll(/class="area-features"/g)].length,
                10,
            );
            assert.equal([...product.matchAll(/<li>/g)].length, 30);
            assert.equal(
                [...product.matchAll(/Product model scope|Ámbito del modelo/g)]
                    .length,
                10,
            );
        }
    });

    it("keeps the value proposition and maturity disclosure accurate in both locales", async () => {
        const spanish = await readFile(
            path.join(outputDirectory, "es", "index.html"),
            "utf8",
        );
        const english = await readFile(
            path.join(outputDirectory, "en", "index.html"),
            "utf8",
        );
        assert.match(spanish, /Sin cuota de licencia/);
        assert.match(spanish, /pueden tener un coste/);
        assert.match(english, /no licence fee/i);
        assert.match(english, /may still cost money/i);
        assert.match(spanish, /MVP está implementado/);
        assert.match(english, /MVP is implemented/i);
        assert.match(spanish, /No hay demo alojada/);
        assert.match(english, /There is no hosted demo/i);
        assert.match(
            spanish,
            /no afirma cumplir requisitos legales o fiscales/i,
        );
        assert.match(english, /does not claim legal or tax compliance/i);
    });

    it("keeps every tutorial in both languages and labels the development path", async () => {
        for (const locale of ["es", "en"]) {
            for (const key of ["invoice", "install", "azure", "backup"]) {
                const page = await readFile(
                    path.join(
                        outputDirectory,
                        locale,
                        routes[key],
                        "index.html",
                    ),
                    "utf8",
                );
                assert.match(page, /<ol class="numbered-steps">/);
                if (key !== "invoice")
                    assert.match(
                        page,
                        /Maintained technical references|Referencias técnicas mantenidas/,
                    );
            }
            const architecture = await readFile(
                path.join(
                    outputDirectory,
                    locale,
                    routes.engineering,
                    "index.html",
                ),
                "utf8",
            );
            assert.match(architecture, /PostgreSQL/);
            assert.match(architecture, /NET Aspire/);
        }
    });

    it("keeps the root locale selector behavior", () => {
        assert.equal(resolveLocale(["en-GB", "es-ES"]), "en");
        assert.equal(resolveLocale(["fr-FR", "es-ES", "en-US"]), "es");
        assert.equal(resolveLocale(["fr-FR", "de-DE"]), "es");
        assert.equal(resolveLocale([]), "es");
        assert.equal(rootLocalePath(["en-GB", "es-ES"]), "/en/");
        assert.equal(rootLocalePath(["fr-FR", "es-ES"]), "/es/");
        assert.equal(rootLocalePath(["fr-FR"]), "/es/");
    });

    it("emits a styled language handoff with both no-script destinations and shared tokens", async () => {
        const root = await readFile(
            path.join(outputDirectory, "index.html"),
            "utf8",
        );
        const styles = await readFile(
            path.join(outputDirectory, "styles.css"),
            "utf8",
        );
        assert.match(root, /href="\.\/styles\.css"/);
        assert.match(root, /href="\/es\/"/);
        assert.match(root, /href="\/en\/"/);
        assert.match(root, /src="\/raccord-mark\.svg"/);
        assert.match(root, /lang="es"/);
        assert.match(root, /lang="en"/);
        assert.match(styles, /--raccord-ink:\s*#0b1020/);
        assert.match(
            root,
            /<nav aria-label="Idioma \/ Language">[\s\S]*?<\/nav>/,
        );
    });
});
