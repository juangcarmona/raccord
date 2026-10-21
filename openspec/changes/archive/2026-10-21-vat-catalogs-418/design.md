<!-- pdac-scope: cited -->

## Context

The Invoicing module currently calculates VAT for invoice lines but has no persistent class or scheme catalog. The issue requires administrators to configure catalogs, expose stable scheme references to Contacts and Projects, initialize a new Spanish installation with an editable standard-rate default, and preserve rates after use. The accepted product contract is `CHG-FINANCE-CATALOGS-417-419-001` on PR #420.

<!-- pdac:cite id="TERM-WORKSPACE" digest="sha256:76a1adeba2ddd9f048c0e6c171f0fe224f6ea85609ca288a61424b7522e30b92" -->

The product defines one workspace as the company's own installation. The catalog is therefore scoped to that installation's database; this change does not introduce multiple workspace identities inside one installation.

<!-- pdac:cite id="UC-FIN-CONFIGURE-VAT" digest="sha256:3c05cb10f788f77cb6752fca0c806d089d618a79be13a6cd2a2d4cb911388ab7" -->
<!-- pdac:cite id="TERM-VAT-SCHEME" digest="sha256:d7fbf3cf62480bae491180a199e8fbe4455b25712d349af2b203173abf42095f" -->
<!-- pdac:cite id="BR-FIN-USED-RATE-FROZEN" digest="sha256:b33134bd0f2519dfb830cd37b214eab8e6c9e8609b7a9bf63c9db8e2e6cb8a95" -->

## Goals / Non-Goals

**Goals:** Deliver one vertical Invoicing catalog slice with domain validation, durable configuration, permission-checked API, settings UI, stable catalog contracts, and one real-browser journey. Ensure the default value is named and source-dated in code but only initializes a new installation; persisted schemes are the source of truth after setup.

**Non-Goals:** Determine tax treatment for a particular operation, prefill reduced or zero rates, update rates automatically, represent VAT exemptions or reverse-charge disclosures on issued invoices, make legal-compliance claims, or implement the contact commercial-default form owned by #300.

## Decisions

### Keep VAT catalog behavior in the Invoicing bounded context

Implement the catalog through the existing Invoicing module seam and its public contracts. Keep the domain types independent of EF Core and HTTP; place persistence behind the module-owned contract and current repository adapter boundary. Compose endpoints in the root host and let the frontend consume only the generated OpenAPI client. Do not let a route reach persistence directly.

### Store the standard-rate bootstrap as a named source-dated constant

Define one immutable project constant containing the default percentage, territory, official source URL, and verification date for the Spanish standard-rate scheme. The installation initializer uses it only when the installation has no VAT catalog. It writes the source and date into the initial configuration and does not run an upsert over an administrator's later edits. A later verified legal change updates the bootstrap constant for new installations and creates a new default scheme for an existing installation through an explicit catalog operation; it never rewrites a used scheme.

### Persist stable scheme identities and freeze used rates

Store classes and schemes as separate records with stable identifiers. Schemes contain their kind and either per-class rates, one fixed rate, or reverse charge. Catalog reads return identifiers and display data. The existing project billable-line write path must persist the selected scheme identity together with the effective percentage snapshot when the line is first saved; invoice draft lines carry both forward. This is the usage record that makes a rate immutable, rather than an inferred relationship from a display name or percentage. Contacts save their default reference through the separate #300 use case.

Register scheme use in the same database transaction that saves the billable line. Rate edits/default changes and first-use registration must serialize on the scheme row (using the existing persistence concurrency approach) so either the rate update wins before the line captures its snapshot, or the line records usage first and the edit is rejected. A rate update must never race past a committed use. Keep the saved percentage snapshot on the commercial record so a replacement scheme cannot alter historic calculations.

Append audit events for catalog creation, catalog edits, default changes, and initial catalog setup through the existing `IAuditAppender`/`IAuditTrail` path. The audit append and catalog mutation share one unit of work: a failed mutation leaves neither a catalog change nor an event. Include actor, action, entity identity and relevant before/after values; never update or delete an audit event.

### Use the repository's existing dependency and test patterns

Add no production dependency. Follow current permission policy registration and audit infrastructure, preserve workspace scoping, and verify the configured OpenAPI contract. TDD starts at domain and public API seams, with integration coverage against real PostgreSQL and a browser E2E against the Aspire AppHost. The audit implementation follows the generic audited-action requirement and existing transaction contract; it does not introduce a new audit product surface.

## Risks / Trade-offs

- A source-dated constant can become stale. It is deliberately a new-installation default rather than an automatic updater; a release that changes it must first reverify the official source and date, and an existing installation must explicitly create a replacement scheme.
- Catalog configuration is not a legal determination. A user must assign the appropriate class and scheme for each operation; the standard class does not make all items taxable at 21%.
- Splitting module contracts, persistence, API and UI requires a vertical slice across several assemblies. Keep the public surface limited to the use cases in #418 and avoid invoice exemption behavior in #325.
- Used-rate immutability must be checked under concurrent writes and by recording real scheme use on a saved project billable line, not only by toggling a catalog test flag or checking the settings UI.
