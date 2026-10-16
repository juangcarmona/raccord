# Public site assets

Product screenshots must come from a successful Playwright journey in `.github/workflows/ci.yml`. The job sets `PLAYWRIGHT_CAPTURE_EVIDENCE=true` and uploads `playwright-artifacts/` for seven days, including successful checkpoints. `InvoiceDocumentJourney_Should_DownloadFromInvoicePage` issues a synthetic invoice and downloads its PDF; it saves the publication-safe screenshot as `invoice-publication.png` after masking the test login, recipient name and tax identifier. Its matching journey JSON must say `passed` before the image is considered for publication.

Before publishing an image, confirm its matching `invoice-document-download.json` result says `passed`, inspect the image for fictional data and legibility, and check that it contains no secrets, traces or failure state. Commit the reviewed image under `src/public-site/images/` so the asset remains available after the CI artifact expires.

To refresh a screenshot locally, follow the E2E instructions in `docs/operations/local-development.md`, run that focused journey with `PLAYWRIGHT_CAPTURE_EVIDENCE=true`, inspect the checkpoint, and replace the committed image only after the journey passes. If Docker is unavailable locally, use the matching successful CI artifact.
