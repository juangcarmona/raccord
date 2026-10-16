<!-- pdac-scope: cited -->

Product pages and task guides must reflect the product areas and the people responsible for operating, billing and maintaining an installation.

<!-- pdac:cite id="BC-PROJECTS" digest="sha256:11cadbc8906365210716689609d66738e29fc576822aa2e8e7a925a8bae0933e" -->
<!-- pdac:cite id="BC-INVOICING" digest="sha256:386343a8a3613a73a2ab60d1b5767cbe7d880fb8ded0d2d1a7c5dee68fa2a567" -->
<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->

## 1. Static site foundation

- [x] 1.1 Create a bilingual static page renderer and route content for `/es/` and `/en/`; verify `npm run build` emits every planned route.
- [x] 1.2 Add shared accessible navigation, page metadata, matching locale links, skip links and responsive design; verify generated pages have correct language and navigation labels.

## 2. Product discovery

- [x] 2.1 Replace the homepage with the agreed audience, no-license-fee proposition, operating-cost note, maturity statement and focused calls to action; verify both locale pages include the claims and no compliance promise.
- [x] 2.2 Add the ten-area product map and explicit availability note; verify each bounded context appears in Spanish and English without an unsupported implementation status.
- [x] 2.3 Present the existing invoice screenshot with synthetic-data caption, alt text and full-size link; verify the referenced image exists in the build.

## 3. Manuals and hosting

- [x] 3.1 Add task-based invoice issue and PDF-download instructions grounded in the successful browser journey; verify each locale includes prerequisites, the demonstrated sequence and an accounting caution.
- [x] 3.2 Add Docker Compose installation and Azure deployment tutorials based on their operations guides; verify setup security, development-versus-production distinction and source links.
- [x] 3.3 Add backup and restore tutorial and self-hosting options page; verify database-plus-documents, off-host backup, infrastructure costs and operational responsibility are explained.

## 4. Engineering overview

- [x] 4.1 Add an accessible runtime diagram and plain-language domain explanation; verify browser, web application/API, PostgreSQL, document storage and administrator boundary are present in both locales.
- [x] 4.2 Explain .NET Aspire only as a development environment; verify all public pages direct production setup to Compose or Azure.

## 5. Build and content verification

- [x] 5.1 Extend the Node build tests to cover all routes, locale peer links, local navigation, image assets and required notices; verify `npm run verify` passes.
- [x] 5.2 Run `npx -y @fission-ai/openspec@1.12.0 validate full-public-site-and-user-manuals --strict` and `npx -y @prodshape/cli@0.22.0 citations verify --provider openspec`; resolve every diagnostic before marking the plan complete.
- [x] 5.3 Review `docs/operations/process-ledger.md` and record a durable lesson only if the implementation revealed a repeatable process gap; verify the ledger change is tied to an observed issue.
