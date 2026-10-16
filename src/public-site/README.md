# Public site assets

Product screenshots must come from a successful Playwright journey in `.github/workflows/ci.yml`. The job sets `PLAYWRIGHT_CAPTURE_EVIDENCE=true` and uploads `playwright-artifacts/` for seven days, including successful checkpoints. `InvoiceDocumentJourney_Should_DownloadFromInvoicePage` issues a synthetic invoice and downloads its PDF; it saves the publication-safe screenshot as `invoice-publication.png` after masking the account email, recipient name and tax identifier. The committed invoice screenshot came from [CI run 38889875378](https://github.com/juangcarmona/raccord/actions/runs/38889875378), at `tests/Raccord.EndToEndTests/bin/Release/net10.0/playwright-artifacts/invoice-publication.png`; its `invoice-document-download.json` result says `passed`.

Before publishing an image, confirm its matching `invoice-document-download.json` result says `passed`, inspect the image for fictional data and legibility, and check that it contains no secrets, traces or failure state. Commit the reviewed image under `src/public-site/images/` so the asset remains available after the CI artifact expires.

To refresh a screenshot locally, follow the E2E instructions in `docs/operations/local-development.md`, run that focused journey with `PLAYWRIGHT_CAPTURE_EVIDENCE=true`, inspect the checkpoint, and replace the committed image only after the journey passes. If Docker is unavailable locally, use the matching successful CI artifact.

## Locales and translation review

The Spanish page in `index.html` is the source for public-site copy. English lives in `locales/en/index.html`; `build.mjs` emits `/es/` and `/en/`, and the root URL directs visitors to Spanish unless their browser language matches an available locale. Keep the language selector, page metadata, canonical and `hreflang` links, navigation, image descriptions, and every maturity or safety notice complete in each locale. A locale is not ready to publish while any page is incomplete or mixes languages.

When Spanish product claims, security guidance, or legal and tax disclosures change, update English in the same pull request. A fluent human reviewer checks each translation, and the product owner checks capability statements and disclosures against the verified inventory in issue [#245](https://github.com/juangcarmona/raccord/issues/245). Machine translation can help draft wording, but it does not replace review. Run the public-site build and locale journey before merging.

## Language rollout

The initial rollout is Spanish first, then English. French is the next candidate, followed by German. The 2024 EU language survey reports French as the most commonly spoken EU foreign language after English (11%) and German next (10%). Global estimates also put French farther ahead: the Organisation internationale de la Francophonie counted 321 million French speakers in 2022, while the Goethe-Institut estimates roughly 130 million German speakers. These global counts use different source methods, so use them as directional evidence; revisit priorities using visitor demand and translation review capacity.

After French and German, phase the remaining official EU languages by the latest EU survey on language knowledge and use, then by visitor demand and the availability of a fluent reviewer. Recheck the order when a phase is planned. The official-language set is listed by the [European Union](https://european-union.europa.eu/principles-countries-history/languages_en).

Sources: [European Commission, 2024 Eurobarometer results](https://ec.europa.eu/commission/presscorner/api/files/document/print/fen/ip_24_2686/IP_24_2686_EN.pdf), [Organisation internationale de la Francophonie, French speakers worldwide](https://observatoireancien.francophonie.org/qui-parle-francais-dans-le-monde/), and [Goethe-Institut, German speakers worldwide](https://www.goethe.de/prj/dlp/en/magazin-sprache/22772878.html).
