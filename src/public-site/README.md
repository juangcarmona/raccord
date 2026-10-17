# Raccord public site

The site is a static Spanish and English website built from `site-content.mjs` and `render.mjs`. The build emits locale roots at `/es/` and `/en/`, with product, manuals, self-hosting and architecture pages beneath both. `build.mjs` writes only to `dist/`; the root page chooses a locale from browser language and falls back to Spanish.

Run `npm run verify` from this directory to build the site and check every route, local link, screenshot reference, locale, product-area label and required maturity disclosure.

## Product screenshots

Screenshots must come from a successful Playwright journey in `.github/workflows/ci.yml`. That job sets `PLAYWRIGHT_CAPTURE_EVIDENCE=true` and uploads `playwright-artifacts/`, including successful checkpoints. `InvoiceDocumentJourney_Should_DownloadFromInvoicePage` creates a synthetic customer and project, issues an invoice, downloads its PDF and saves the publication-safe screenshot as `invoice-publication.png` after masking the account email, recipient name and tax identifier. The committed screenshot in `images/invoice-issued.png` came from [CI run 38889875378](https://github.com/juangcarmona/raccord/actions/runs/38889875378); its matching `invoice-document-download.json` result says `passed`.

Before publishing an image, confirm its matching result says `passed`, inspect it for fictional data and legibility, and check that it contains no secrets, traces or failure state. Keep the reviewed asset in `images/` so it remains available after the CI artifact expires.

## Content and translations

The content module holds matched Spanish and English copy. Shared renderer code supplies page structure, navigation and metadata. Keep the available routes equivalent in both languages; update titles, image descriptions, captions, language links and all maturity or safety notices together.

The product page is a map of the ten Product Definition areas, not a claim that every modeled capability is currently available. The detailed inventory remains under review in issue #338, which depends on the audit in #277. Present a capability as implemented only when the repository includes current evidence for it. The invoice workflow is currently the only workflow shown as a product screenshot.

Manuals are written for users and installation administrators. Keep technical commands and detailed operational guarantees aligned with `docs/operations/self-hosting.md`, `docs/operations/azure-deployment.md` and `docs/operations/backup-and-restore.md`; the site tutorials are a readable path into those references.

Machine translation may help draft copy, but it does not replace a fluent English review. The product owner checks capability statements and legal or tax disclosures against current implementation evidence.

## Publication

The build and deployment boundary is documented in [`docs/operations/public-site-deployment.md`](../../docs/operations/public-site-deployment.md). The repository's public-site workflow checks the static build; it does not deploy to Cloudflare Pages. Verify the production project and preview before deployment. Do not change DNS records without recording the current zone and approving the exact change.
