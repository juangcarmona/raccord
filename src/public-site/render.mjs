import { getSiteContent } from "./site-content.mjs";

const routes = {
    home: "",
    product: "producto/",
    manuals: "manuales/",
    hosting: "autoalojamiento/",
    engineering: "arquitectura/",
    invoice: "manuales/emitir-factura/",
    install: "manuales/instalar-compose/",
    azure: "manuales/desplegar-azure/",
    backup: "manuales/copias-restauracion/",
};

const esc = (value) =>
    String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;");

function header(t, locale, depth, routeKey) {
    const localeUp = "../".repeat(depth - 1);
    const rootUp = "../".repeat(depth);
    const active = (key) => (key === locale ? ' aria-current="page"' : "");
    return `<a class="skip-link" href="#contenido">${locale === "es" ? "Saltar al contenido" : "Skip to content"}</a>
<header class="site-header"><a class="wordmark" href="${localeUp}index.html" aria-label="Raccord">Raccord<span>.</span></a>
<nav class="primary-nav" aria-label="${locale === "es" ? "Navegación principal" : "Main navigation"}"><a href="${localeUp}producto/">${t.nav.product}</a><a href="${localeUp}manuales/">${t.nav.manuals}</a><a href="${localeUp}autoalojamiento/">${t.nav.hosting}</a><a href="${localeUp}arquitectura/">${t.nav.engineering}</a></nav>
<a class="header-cta" href="https://github.com/juangcarmona/raccord">${t.github}<span aria-hidden="true"> ↗</span></a>
<nav class="language-switcher" aria-label="${locale === "es" ? "Selector de idioma" : "Language selector"}"><a href="${rootUp}es/${routes[routeKey]}" lang="es"${active("es")}>ES</a><a href="${rootUp}en/${routes[routeKey]}" lang="en"${active("en")}>EN</a></nav></header>`;
}

function cards(items, className = "card-grid") {
    return `<div class="${className}">${items.join("")}</div>`;
}

function linkCard(href, title, text, eyebrow = "") {
    return `<a class="guide-card" href="${href}">${eyebrow ? `<span class="card-eyebrow">${esc(eyebrow)}</span>` : ""}<h3>${esc(title)}</h3><p>${esc(text)}</p><span class="card-arrow" aria-hidden="true">↗</span></a>`;
}

function imageFigure(t, assets) {
    return `<figure class="product-shot"><a class="screenshot-link" href="${assets}images/invoice-issued.png" aria-label="${esc(t.screenshotTitle)}"><img src="${assets}images/invoice-issued.png" alt="${esc(t.screenshotAlt)}" width="1280" height="900" loading="eager" decoding="async" /></a><figcaption><strong>${esc(t.screenshotTitle)}</strong><span>${esc(t.screenshotCaption)} <a href="${assets}images/invoice-issued.png">${localeText(t.language, "Abrir imagen completa", "Open full-size image")}</a></span></figcaption></figure>`;
}

function localeText(locale, es, en) {
    return locale === "es" ? es : en;
}

function home(t, up, assets) {
    const steps = t.steps
        .map(
            (step, i) =>
                `<li><span class="step-number">0${i + 1}</span><span>${esc(step)}</span></li>`,
        )
        .join("");
    return `<main id="contenido"><section class="hero hero-home"><div class="hero-copy-block"><p class="eyebrow"><span class="eyebrow-dot"></span>${esc(t.badge)}</p><h1>${esc(t.heroTitle)}</h1><p class="hero-lede">${esc(t.heroText)}</p><div class="actions"><a class="button button-primary" href="${up}producto/">${esc(t.primary)} <span aria-hidden="true">→</span></a><a class="button button-quiet" href="${up}manuales/">${esc(t.secondary)}</a></div><p class="hero-note">${esc(t.heroNote)}</p></div><div class="hero-art" aria-hidden="true"><div class="orbit orbit-one"></div><div class="orbit orbit-two"></div><div class="hero-art-core"><span>R</span></div><div class="orbit-label label-project">${esc(t.heroLabelProjects)} <i>↗</i></div><div class="orbit-label label-invoice">${esc(t.heroLabelInvoicing)} <i>✓</i></div><div class="orbit-label label-control">${esc(t.heroLabelHosting)} <i>⌂</i></div><div class="hero-art-caption">${esc(t.heroArtCaption)}</div></div></section>
<section class="value-band"><div class="value-mark" aria-hidden="true">∞</div><div><p class="eyebrow">${esc(localeText(t.language, "Sin cuota de licencia", "No licence fee"))}</p><h2>${esc(t.promiseTitle)}</h2><p>${esc(t.promise)}</p></div><a class="text-link" href="${up}autoalojamiento/">${esc(localeText(t.language, "Ver formas de alojarlo", "Explore hosting options"))} <span aria-hidden="true">→</span></a></section>
<section class="section section-workflow"><div class="section-heading"><p class="eyebrow">${esc(localeText(t.language, "Un flujo que puedes ver", "A workflow you can see"))}</p><h2>${esc(t.workflowTitle)}</h2><p>${esc(t.workflowText)}</p></div><ol class="workflow-list">${steps}</ol>${imageFigure(t, assets)}<a class="text-link" href="${up}manuales/emitir-factura/">${esc(t.manualInvoice)} <span aria-hidden="true">→</span></a></section>
<section class="section section-areas"><div class="section-heading"><p class="eyebrow">${esc(localeText(t.language, "De un solo lugar", "One connected system"))}</p><h2>${esc(t.areasTitle)}</h2><p>${esc(t.areasText)}</p></div>${cards(
        t.boundedContexts
            .slice(0, 4)
            .map(
                (item) =>
                    `<article class="area-card"><span class="area-index">${String(t.boundedContexts.indexOf(item) + 1).padStart(2, "0")}</span><h3>${esc(item[t.language][0])}</h3><p>${esc(item[t.language][1])}</p></article>`,
            ),
        "area-grid",
    )}
<p class="scope-note">${esc(t.areasNote)}</p><a class="text-link" href="${up}producto/">${esc(t.domainLinks)} <span aria-hidden="true">→</span></a></section>
<section class="section section-manuals"><div class="section-heading"><p class="eyebrow">${esc(localeText(t.language, "Para cada rol", "For every role"))}</p><h2>${esc(t.manualsTitle)}</h2><p>${esc(t.manualsText)}</p></div>${cards([linkCard(`${up}manuales/emitir-factura/`, t.manualInvoice, t.manualInvoiceDesc, localeText(t.language, "Facturación", "Billing")), linkCard(`${up}manuales/instalar-compose/`, t.manualInstall, t.manualInstallDesc, localeText(t.language, "Administración", "Administration")), linkCard(`${up}manuales/desplegar-azure/`, t.manualAzure, t.manualAzureDesc, "Azure")], "guide-grid")}<a class="text-link" href="${up}manuales/">${esc(localeText(t.language, "Ver todos los manuales", "Browse all manuals"))} <span aria-hidden="true">→</span></a></section>
<section class="final-cta"><div><p class="eyebrow">${esc(localeText(t.language, "Siguiente paso", "Next step"))}</p><h2>${esc(t.hostingTitle)}</h2><p>${esc(t.hostingText)}</p></div><a class="button button-primary" href="${up}autoalojamiento/">${esc(localeText(t.language, "Elegir una ruta", "Choose a path"))} <span aria-hidden="true">→</span></a></section>
<section class="maturity-strip"><span class="status-dot"></span><div><strong>${esc(t.maturityTitle)}</strong><p>${esc(t.maturity)}</p></div><a href="${up}arquitectura/">${esc(t.engineeringTitle)} →</a></section></main>`;
}

function product(t, up) {
    const all = t.boundedContexts.map(
        (item, i) =>
            `<article class="area-card area-card-large" id="${item.id}"><span class="area-index">${String(i + 1).padStart(2, "0")}</span><div><p class="card-eyebrow">${esc(t.productLabel)}</p><h2>${esc(item[t.language][0])}</h2><p>${esc(item[t.language][1])}</p><ul class="area-features">${item.features[t.language].map((feature) => `<li>${esc(feature)}</li>`).join("")}</ul></div></article>`,
    );
    return `<main id="contenido"><section class="page-hero"><p class="eyebrow">${esc(t.nav.product)}</p><h1>${esc(t.productTitle)}</h1><p class="hero-lede">${esc(t.productIntro)}</p><a class="button button-primary" href="${up}manuales/">${esc(t.nav.manuals)} <span aria-hidden="true">→</span></a></section><section class="section"><div class="product-map">${all.join("")}</div><div class="scope-note scope-note-large"><strong>${esc(t.availability)}</strong><p>${esc(t.areasNote)}</p></div><div class="inline-callout"><h2>${esc(t.workflowTitle)}</h2><p>${esc(t.workflowText)}</p>${imageFigure(t, "../../")}</div></section></main>`;
}

function guideIndex(t, up) {
    return `<main id="contenido"><section class="page-hero"><p class="eyebrow">${esc(t.nav.manuals)}</p><h1>${esc(t.manualTitle)}</h1><p class="hero-lede">${esc(t.manualIntro)}</p></section><section class="section"><div class="guide-grid">${linkCard(`${up}manuales/emitir-factura/`, t.manualInvoice, t.manualInvoiceDesc, localeText(t.language, "Producción y facturación", "Production and billing"))}${linkCard(`${up}manuales/instalar-compose/`, t.manualInstall, t.manualInstallDesc, "Docker Compose")}${linkCard(`${up}manuales/desplegar-azure/`, t.manualAzure, t.manualAzureDesc, "Azure")}${linkCard(`${up}manuales/copias-restauracion/`, t.manualBackup, t.manualBackupDesc, localeText(t.language, "Administración", "Administration"))}</div><div class="scope-note"><strong>${esc(t.availability)}</strong><p>${esc(t.maturity)}</p></div></section></main>`;
}

function hosting(t, up) {
    const options = t.hostOptions
        .map(
            ([title, body]) =>
                `<article class="hosting-card"><span class="card-eyebrow">${esc(t.nav.hosting)}</span><h2>${esc(title)}</h2><p>${esc(body)}</p></article>`,
        )
        .join("");
    return `<main id="contenido"><section class="page-hero"><p class="eyebrow">${esc(t.nav.hosting)}</p><h1>${esc(t.hostingTitlePage)}</h1><p class="hero-lede">${esc(t.hostingIntro)}</p><div class="cost-callout"><strong>${esc(localeText(t.language, "Sin licencia · con costes de operación", "No licence fee · operating costs may apply"))}</strong><p>${esc(t.hostingCosts)}</p></div></section><section class="section"><div class="hosting-grid">${options}</div><div class="hosting-next"><div><p class="eyebrow">${esc(t.manualInstall)}</p><h2>${esc(localeText(t.language, "Empieza por la ruta que mejor conoces", "Start with the path you know best"))}</h2><p>${esc(t.hostingText)}</p></div><div class="actions"><a class="button button-primary" href="${up}manuales/instalar-compose/">Docker Compose →</a><a class="button button-quiet" href="${up}manuales/desplegar-azure/">Azure →</a></div></div></section></main>`;
}

function engineering(t, up) {
    return `<main id="contenido"><section class="page-hero"><p class="eyebrow">${esc(t.nav.engineering)}</p><h1>${esc(t.architectureTitle)}</h1><p class="hero-lede">${esc(t.architectureIntro)}</p></section><section class="section"><h2 class="section-title">${esc(t.diagramTitle)}</h2><div class="system-diagram" role="img" aria-label="${esc(t.diagramBrowser)} → ${esc(t.diagramWeb)} → ${esc(t.diagramDb)} y ${esc(t.diagramFiles)}"><div class="diagram-node node-browser"><span>01</span><strong>${esc(t.diagramBrowser)}</strong></div><div class="diagram-arrow" aria-hidden="true">→</div><div class="diagram-node node-web"><span>02</span><strong>${esc(t.diagramWeb)}</strong></div><div class="diagram-branches"><span aria-hidden="true">↙</span><span aria-hidden="true">↘</span></div><div class="diagram-node node-db"><span>03A</span><strong>${esc(t.diagramDb)}</strong></div><div class="diagram-node node-files"><span>03B</span><strong>${esc(t.diagramFiles)}</strong></div></div><p class="diagram-caption">${esc(t.diagramOps)}</p><div class="domain-explainer"><div><p class="eyebrow">${esc(t.nav.product)}</p><h2>${esc(t.domainTitle)}</h2><p>${esc(t.domainText)}</p></div><a class="button button-secondary" href="${up}producto/">${esc(t.domainLinks)} <span aria-hidden="true">→</span></a></div><div class="development-note"><span class="dev-icon" aria-hidden="true">⌘</span><div><h2>${esc(t.devTitle)}</h2><p>${esc(t.devText)}</p></div></div></section></main>`;
}

function numberedGuide(t, key, title, intro, steps, warning, up, source) {
    const list = steps
        .map(
            (step, i) =>
                `<li><span class="guide-step-number">${String(i + 1).padStart(2, "0")}</span><div>${esc(step)}</div></li>`,
        )
        .join("");
    const sourceReference = source
        ? `<div class="source-links"><h2>${esc(t.sourceDocs)}</h2><a href="${source}">${esc(key)} <span aria-hidden="true">↗</span></a></div>`
        : "";
    const humanNote =
        key === t.manualInvoice
            ? ""
            : `<p class="human-note">${esc(t.note)}</p>`;
    return `<main id="contenido"><section class="page-hero guide-hero"><a class="back-link" href="${up}manuales/">← ${esc(t.back)}</a><p class="eyebrow">${esc(t.nav.manuals)}</p><h1>${esc(title)}</h1><div class="prerequisite"><strong>${esc(t.prereq)}</strong><p>${esc(intro)}</p></div></section><section class="section guide-body"><ol class="numbered-steps">${list}</ol><aside class="warning-callout"><h2>${esc(t.warningTitle)}</h2><p>${esc(warning)}</p></aside>${sourceReference}${humanNote}</section></main>`;
}

function invoiceGuide(t, up) {
    return numberedGuide(
        t,
        t.manualInvoice,
        t.page.invoice,
        t.invoicePrereq,
        t.invoiceSteps,
        t.note,
        up,
        "",
    );
}
function installGuide(t, up) {
    return numberedGuide(
        t,
        t.manualInstall,
        t.page.install,
        t.manualInstallDesc,
        t.installSteps,
        t.installWarning,
        up,
        "https://github.com/juangcarmona/raccord/blob/main/docs/operations/self-hosting.md",
    );
}
function azureGuide(t, up) {
    return numberedGuide(
        t,
        t.manualAzure,
        t.page.azure,
        t.manualAzureDesc,
        t.azureSteps,
        t.azureWarning,
        up,
        "https://github.com/juangcarmona/raccord/blob/main/docs/operations/azure-deployment.md",
    );
}
function backupGuide(t, up) {
    return numberedGuide(
        t,
        t.manualBackup,
        t.page.backup,
        t.manualBackupDesc,
        t.backupSteps,
        t.backupWarning,
        up,
        "https://github.com/juangcarmona/raccord/blob/main/docs/operations/backup-and-restore.md",
    );
}

const renderers = {
    home,
    product,
    manuals: guideIndex,
    hosting,
    engineering,
    invoice: invoiceGuide,
    install: installGuide,
    azure: azureGuide,
    backup: backupGuide,
};

export function renderPage(locale, routeKey) {
    const t = getSiteContent(locale);
    const route = routes[routeKey];
    const depth = route ? route.split("/").filter(Boolean).length + 1 : 1;
    const up = "../".repeat(depth - 1);
    const localeUp = up;
    const assetRoot = "../".repeat(depth);
    const title =
        routeKey === "home"
            ? "Raccord — " + t.badge
            : `${t.page[routeKey] ?? t.nav.product} — Raccord`;
    const pageBody = renderers[routeKey](t, up, assetRoot);
    const html = `<!doctype html><html lang="${locale}"><head><meta charset="utf-8"/><meta name="viewport" content="width=device-width, initial-scale=1"/><meta name="description" content="${esc(t.heroText)}"/><meta name="theme-color" content="#10171c"/><title>${esc(title)}</title><link rel="stylesheet" href="${assetRoot}styles.css"/><link rel="canonical" href="https://raccord.dev/${locale}/${route}"/><link rel="alternate" hreflang="es" href="https://raccord.dev/es/${route}"/><link rel="alternate" hreflang="en" href="https://raccord.dev/en/${route}"/><link rel="alternate" hreflang="x-default" href="https://raccord.dev/"/></head><body>${header(t, locale, depth, routeKey)}${pageBody}<footer class="site-footer"><a class="wordmark" href="${localeUp}index.html">Raccord<span>.</span></a><p>${esc(t.footer)}</p><div><a href="${localeUp}producto/">${esc(t.nav.product)}</a><a href="${localeUp}manuales/">${esc(t.nav.manuals)}</a><a href="https://github.com/juangcarmona/raccord">GitHub ↗</a></div></footer></body></html>`;
    return html;
}

export { routes };
