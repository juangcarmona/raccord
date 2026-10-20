<!-- pdac-scope: cited -->

## Why

Raccord's public site currently presents one narrow invoice workflow and sends prospective users to engineering material for the rest. Audiovisual production and equipment-rental teams need to see how the product fits their work, understand what the MVP offers, and learn how to run it themselves without reading source code.

## What Changes

- Replace the single-page introduction with a bilingual static site for product discovery, task-based manuals, self-hosting, and a plain-language engineering overview.
- Present the ten bounded contexts as a connected product map. Distinguish product-model scope from capabilities backed by current implementation and successful browser evidence; do not label an area available solely because it appears in the model.
- Show the existing invoice screenshot with its provenance, fictional-data disclosure, useful alternative text, caption, and full-size link.
- Turn the maintained Docker Compose, Azure, backup and restore guidance into reader-oriented tutorials; identify .NET Aspire as the development environment.
- Explain the no-license-fee model and make clear that hosting, infrastructure, maintenance, backups, and operations can still cost money.
- Keep legal, tax, self-hosting security and product maturity wording grounded in repository guidance. Do not introduce a hosted demo or application instance.
- Extend static build checks to cover every route, locale, navigation target and local image.

## Capabilities

### New Capabilities

- `public-product-guides`: Product overview, screenshot presentation, manuals, self-hosting guidance and engineering overview on the public site.

### Modified Capabilities

- `public-homepage`: Homepage navigation, calls to action, localization and product maturity presentation.

## Product definition impact

This is a documentation and presentation change. It does not alter product behavior or the canonical model. The affected product areas are the ten bounded contexts listed below, plus the operations-coordinator, billing-administrator and installation-administrator audiences and their project, invoicing and installation journeys. Contexts and audiences reviewed but not treated as proof of implementation status: the public inventory ticket #338 depends on the still-open audit #277. Only workflow evidence that is present in the repository and can be verified is presented as demonstrated product UI.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:b63537573b968a16a585370ae0e4a98ec688de6e31e42e8b7e2dad5c89d3f474" -->
<!-- pdac:cite id="BC-PROJECTS" digest="sha256:11cadbc8906365210716689609d66738e29fc576822aa2e8e7a925a8bae0933e" -->
<!-- pdac:cite id="BC-COMMERCIAL" digest="sha256:cecf438d86ba351619417a2951b5dbc53aea0c70dbb07a0dd509e80ab4391adc" -->
<!-- pdac:cite id="BC-INVOICING" digest="sha256:386343a8a3613a73a2ab60d1b5767cbe7d880fb8ded0d2d1a7c5dee68fa2a567" -->
<!-- pdac:cite id="BC-EQUIPMENT" digest="sha256:0e324b394e1d89bacbfa13bc9f312f0592c710fe99f8d9c515dc9c999025bf82" -->
<!-- pdac:cite id="BC-WAREHOUSE" digest="sha256:2604b7b7573cfde1759e4b8c0cb9c5c76a188520fac77862a06344f8d08ca2ff" -->
<!-- pdac:cite id="BC-CREW" digest="sha256:424bcce4c1dcf63261bb06395d5889f8e2e931cb7499ef4feb96c3417418fefe" -->
<!-- pdac:cite id="BC-DOCUMENTS" digest="sha256:36cfde6671027e37541e237722980667ca47ad36a8fe2f6dd8712dd75f286a54" -->
<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->
<!-- pdac:cite id="BC-COMPLIANCE" digest="sha256:92fbac72cbcb8e640f0c87ff7da3629698b6bb3ee7cf5bdf742db249ca898dc0" -->
<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->
<!-- pdac:cite id="JRN-PRJ-DELIVER-PRODUCTION-WORK" digest="sha256:4bfc3ca3d7a769f83fa042d2a9ec4750afa3003434795f23d2225b7b3a519bfb" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->
<!-- pdac:cite id="JRN-OPERATE-INSTALLATION" digest="sha256:113619d8648d3bb2d2e7e05eeccc24a2fa6499dde56e525b4642286ada27787c" -->

## Impact

- `src/public-site/`: generated locale routes, translated page content, shared navigation and styling, and a stronger static-content verification suite.
- `openspec/specs/public-homepage/` and new `public-product-guides` capability specification.
- No application/API behavior, production dependencies, DNS records, hosted application, or deployment credentials change.
- Existing manuals remain the operational source; public tutorials explain them in plain language and link to exact repository documents when technical reference is needed.

## Product definition drift

None. The pages explain model areas but will not present unverified requirements as shipped functionality.
