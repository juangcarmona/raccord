<!-- pdac-scope: cited -->

## Context

See `proposal.md` for the motivation and `specs/` for the user-visible contract. The current site is a hand-authored Spanish and English homepage emitted to `/es/` and `/en/`; `src/public-site/build.mjs` copies a fixed list of files. The current committed product image is the publication-safe invoice screenshot. Issue #338 makes detailed capability labels dependent on the open audit #277.

The content map depends on the ten bounded contexts and the audiences who coordinate operations, billing and installation. The product model describes the areas; it does not prove their current UI status.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:c9c5f006e7a0904b8ac8303a008358ba52bc1203747dba3e03340500cd2f86d7" -->
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

## Goals / Non-Goals

**Goals:** Make product discovery and first-run learning feel like a coherent public product site; keep the content navigable, bilingual and usable without client-side JavaScript.

**Non-Goals:** Re-audit the 97 historical issues in #277, create a hosted service, change application behavior, or deploy to production.

## Decisions

1. **Generate static pages from a small bilingual content module.** This keeps route output framework-free and easy to inspect, while shared render functions keep headers, footers, accessibility hooks and layout consistent. Separate authored HTML files were considered but would duplicate the same navigation and structural changes across locales and routes.
2. **Use stable locale-prefixed routes.** `/es/` and `/en/` remain the language roots; product, manual, hosting and architecture pages live below each root. Each route has a peer in the other locale and its selector points to the corresponding peer. Existing locale detection remains only on `/`.
3. **Treat model areas and implementation evidence as different content.** The product page describes each bounded context as an area of responsibility. A visible note says the inventory is still under review, while the existing screenshot and invoice tutorial are specifically identified as demonstrated workflow evidence. The site will not infer availability from model content, issue status or a screenshot unrelated to the claim.
4. **Keep user tutorials in the site, technical references as secondary links.** The manuals explain customer/project/invoice, Docker Compose, Azure, and backup/restore tasks in everyday wording. Maintained operations documents remain linked for exact commands and deeper reference. .NET Aspire is explicitly marked as development-only.
5. **Use native CSS for the system illustration and visual language.** A semantic diagram built from HTML and CSS avoids an image dependency and remains responsive and accessible. System fonts avoid remote font requests and third-party tracking.
6. **Extend build verification rather than introduce a front-end framework.** The existing Node build emits static route files; Node's built-in test runner checks route completeness, locale metadata, local links, image assets and safety disclosures. No production dependency is added.

## Risks / Trade-offs

- [Risk] The detailed availability audit is incomplete, so the product map can feel broader than the currently evidenced UI. → Keep the availability note alongside the map and show only the invoice workflow as screenshot-backed.
- [Risk] Machine-written English may need fluent review. → Keep the translation parallel and complete, then request editorial review before a production release.
- [Risk] Tutorial details can drift from the actual interface. → Limit the user workflow to the current successful browser journey and keep version-sensitive command details in maintained operational documentation.
- [Risk] Cloud deployment costs vary by region and configuration. → Describe cost categories rather than promising a fixed price.

## Migration Plan

The build emits a new static route tree under `dist/`; it removes only generated output before rebuilding. No application schema, API or customer data migrates. Rollback is a content revert and a rebuild. Production publication is outside this change's local implementation steps.

## Open Questions

None that affect the planned routes or implementation. A fluent English review and the full capability audit remain separate publication evidence.
