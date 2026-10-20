# Architecture decision records

This directory holds Raccord's Architecture Decision Records (ADRs), one file per decision, named `NNNN-<slug>.md`. Each record keeps its status, date, context, consequences and rejected alternatives in the record itself.

Records are immutable in spirit. To reverse or replace a decision, write a new record with the next number and mark the old one `Superseded by ADR-NNNN`; never rewrite an accepted record. The record template lives with the ADR skill. Accepted decisions that shape the current architecture are also indexed in [arc42 section 9](../architecture/arc42/09-architecture-decisions.md).

| ADR      | Title                                                                                                                                                              | Status                 | Date       |
| -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ---------------------- | ---------- |
| ADR-0001 | [Start with a modular monolith](0001-modular-monolith.md)                                                                                                          | Accepted               | 2026-10-06 |
| ADR-0002 | [License code under AGPLv3 and keep the brand separate](0002-license-and-brand.md)                                                                                 | Accepted               | 2026-10-06 |
| ADR-0003 | [Layered projects with module namespaces and executable dependency rules](0003-layered-modules-and-executable-rules.md)                                            | Superseded by ADR-0018 | 2026-10-12 |
| ADR-0004 | [Use PostgreSQL with EF Core and enforce append-only financial tables in the database](0004-postgresql-ef-core-append-only.md)                                     | Accepted               | 2026-10-06 |
| ADR-0005 | [Store issued invoices as a canonical representation with a SHA-256 fingerprint](0005-issued-invoice-representation.md)                                            | Accepted               | 2026-10-06 |
| ADR-0006 | [Round money once per line and compute VAT per rate on rounded line amounts](0006-money-rounding-and-vat.md)                                                       | Accepted               | 2026-10-06 |
| ADR-0007 | [Allocate gapless numbers per series and year in the issuing transaction](0007-gapless-numbering.md)                                                               | Accepted               | 2026-10-06 |
| ADR-0008 | [Render invoice PDFs with an in-house deterministic writer behind a port](0008-deterministic-pdf-renderer.md)                                                      | Accepted               | 2026-10-06 |
| ADR-0009 | [Store documents through a port with file-system and Azure Blob adapters](0009-document-storage.md)                                                                | Accepted               | 2026-10-06 |
| ADR-0010 | [Record compliance obligations through a seam before implementing VERI\*FACTU](0010-compliance-seam.md)                                                            | Accepted               | 2026-10-06 |
| ADR-0011 | [Build the frontend with React and Vite on OpenAPI-generated types](0011-frontend-stack.md)                                                                        | Accepted               | 2026-10-06 |
| ADR-0012 | [Serve one organization per deployment for the MVP](0012-single-organization-per-deployment.md)                                                                    | Accepted               | 2026-10-06 |
| ADR-0013 | [Ship one application image deployed through Compose, Aspire and Azure](0013-deployment-topology.md)                                                               | Accepted               | 2026-10-06 |
| ADR-0014 | [Test through public seams against real PostgreSQL without a mocking library](0014-testing-strategy.md)                                                            | Accepted               | 2026-10-06 |
| ADR-0015 | [Enforce build and quality gates in the build and in CI](0015-build-quality-gates.md)                                                                              | Accepted               | 2026-10-06 |
| ADR-0016 | [Add commit-time hooks, CI mirrors, security gates and spec tooling as guardrails](0016-guardrails.md)                                                             | Accepted               | 2026-10-10 |
| ADR-0017 | [Run end-to-end journeys against the real AppHost in a real browser with a pinned selection](0017-end-to-end-testing.md)                                           | Accepted               | 2026-10-10 |
| ADR-0018 | [Split modules into assemblies named after the product's bounded contexts](0018-module-assemblies-named-after-bounded-contexts.md)                                 | Accepted               | 2026-10-12 |
| ADR-0019 | [Give each module its own schema and DbContext, sharing one transaction](0019-module-owned-schemas-with-a-shared-transaction.md)                                   | Accepted               | 2026-10-12 |
| ADR-0020 | [Communicate between modules through synchronous contracts and defer events](0020-synchronous-module-contracts-with-deferred-events.md)                            | Accepted               | 2026-10-12 |
| ADR-0021 | [Authenticate with local Identity and cookie sessions, authorize with scoped role permissions](0021-local-identity-with-cookie-sessions-and-scoped-permissions.md) | Accepted               | 2026-10-12 |
| ADR-0022 | [Write each use case as a single-file slice returning result errors](0022-single-file-use-case-slices-with-result-errors.md)                                       | Accepted               | 2026-10-12 |
| ADR-0023 | [Organize the frontend as feature-owned vertical slices](0023-frontend-feature-slice-architecture.md)                                                              | Accepted               | 2026-10-12 |
| ADR-0024 | [Publish static engineering snapshots through GitHub Pages](0024-publish-static-engineering-snapshots-on-github-pages.md)                                          | Accepted               | 2026-10-13 |
| ADR-0025 | [Host the public site through Cloudflare Pages](0025-host-the-public-site-on-cloudflare-pages.md)                                                                  | Accepted               | 2026-10-16 |
| ADR-0026 | [Distribute versioned application images through Docker Compose](0026-publish-versioned-compose-releases.md)                                                       | Accepted               | 2026-10-16 |
| ADR-0027 | [Keep architecture diagram sources beside arc42 prose](0027-keep-architecture-diagram-sources-with-arc42.md)                                                       | Accepted               | 2026-10-16 |
| ADR-0028 | [Deliver application email through configured SMTP](0028-configure-smtp-email-delivery-through-mailkit.md)                                                         | Accepted               | 2026-10-18 |
| ADR-0029 | [Record self-hosted VERI\*FACTU responsibility boundary](0029-self-hosted-verifactu-responsibility-boundary.md)                                                    | Accepted               | 2026-10-20 |
| ADR-0030 | [Give Equipment ownership of its catalog and stock records](0030-equipment-module-boundary.md)                                                                     | Proposed               | 2026-10-20 |
| ADR-0031 | [Validate Equipment image uploads before storage](0031-validate-equipment-image-uploads-before-storage.md)                                                         | Proposed               | 2026-10-20 |
