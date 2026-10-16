<!-- pdac-scope: cited -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->

## Context

The repository has a serialized GitHub Pages workflow named `engineering-site.yml` for architecture snapshots, the product model, and nightly browser evidence. It watches application paths under `src/` while excluding `src/public-site/`, whose independent workflow validates the public homepage. ADR-0024 assigns one publisher to the engineering artifact.

## Goals / Non-Goals

**Goals:**

- Keep the homepage's editable source under `src/public-site/`.
- Deliver Spanish copy while keeping page content separable for later translations.
- Clearly state the current first-slice maturity and avoid implying authentication or legal compliance is implemented.

**Non-Goals:**

- Change application behavior, engineering snapshot content, or the serialized GitHub Pages publisher; issue #247 owns workflow and hosting boundaries.
- Deploy to `raccord.dev` or modify DNS, certificates, or live hosting configuration.
- Add a product demo, application access, analytics, or a contact form.
- Deliver translated pages in this slice.

## Decisions

1. **Place the site at `src/public-site/`.** This follows the repository's source organization and gives the engineering workflow a precise path to exclude. A root-level `public/` directory was considered but rejected because it would make the requested source-under-`src/` convention less consistent.
2. **Build a static site with no application runtime dependency.** A visitor can read project information without contacting Raccord's application, which remains unauthenticated. The independent build and hosting mechanics are tracked in issue #247.
3. **Keep the initial content Spanish and translation-ready.** Spanish matches the initial market; content and layout roles will not assume additional languages are already available.
4. **Treat brand assets as exploratory.** Use existing visual tokens conservatively and avoid presenting the concept mark as a finalized trademark identity.

## Risks / Trade-offs

- [Risk] Homepage copy may describe planned features as available → [Mitigation] Limit claims to the implemented slice and link visitors to current project documentation.
- [Risk] Public copy may be read as a security or compliance assurance → [Mitigation] State that application authentication is not implemented and do not claim legal or VERI\*FACTU compliance.
- [Risk] The Cloudflare Git integration requires account setup outside the repository → [Mitigation] Document the project root, build output, preview behavior, and production-disabled state; defer custom-domain binding and production publication.

## Migration Plan

No data migration is needed. This change adds static source files; issue #247 prepares the independent build and hosting configuration. Production publication and domain binding remain disabled until explicit Owner approval.

## Open Questions

None.

## Test Impact

There are no application API, database, or product end-to-end journey changes. The static site implementation will verify its build output, navigation and link targets, responsive rendering, keyboard use, and accessibility expectations. Pull request evidence will include rendered desktop and mobile screenshots. Repository CI and ProductShape/OpenSpec gates will run for the files and tools introduced by the implementation.
