# Provider application and evidence

<!-- pdac-scope: cited -->

## Context

The owner approved the original written product intent and recommendations in #437. Recovery uses the six immutable files from PR #399 at dfc162a7f697caa03d9cd6a542bb982a32fa016c, reconciled against the current accepted model rather than the historical epic branch.

<!-- pdac:cite id="FR-WSP-030" digest="sha256:3106f257093a9439e1653c31c1876a6945d04ea9e4880be382388d0a9c41253d" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e" -->
<!-- pdac:cite id="BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION" digest="sha256:d6491901cbe82003e5ce323b7a979d0a2a6c33be2383d97e5db0f65920206371" -->
<!-- pdac:cite id="SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION" digest="sha256:1c6c3ce42e36bc172f3b6e66bc595e0223a0673b03bfe62dba9978176a0f21f7" -->

## Decisions

Use the installed ProductShape CLI0.26.1/core0.25.0 for the complete provider graph, overlay validation, dry-run and explicit application. Keep the same CHG identity and four-additions/one-modification scope. Preserve the current empty applies-to list on the existing assignment rule; its Contacts use cases already own governed-by links. Preserve the approved concurrent settings-edit conflict and future-only allocation intent.

Pin each unchanged direct consumer with its actual content digest, the modified rule digest and a specific rationale. ProductShape validates all four ledger entries. Remove completed proposed copies after checking references, retaining the applied change.md decision record and immutable original source.

The native serializer formats the complete Workspace inspect/impact/citation object to528689 bytes, beyond its524288-byte bound. Compact serialization of the same complete object is348604 bytes. This branch does not alter Lighthouse or disguise owner-supplied data as an authenticated native report. The blocked native result remains separate from actual operator provider validation.

## Architecture and Boundaries

ADR-0018 keeps settings in Workspace and contact creation in Contacts. ADR-0012 retains independent installation databases. No module, production dependency, credential or runtime behavior changes in this product-only slice. The model is accepted into main only after a separately authorized merge; dependent runtime keeps future.

## Verification

Require zero overlay diagnostics before application, an exact provider diff with four added artifacts and one modified rule, zero accepted-model diagnostics after application, current citations and strict OpenSpec/Markdown validation. Forge checks bind the published PR head separately. Browser runtime qualification and feature acceptance occur during the later implementation.
