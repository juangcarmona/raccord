---
title: Architecture decisions
arc42-section: "09"
description: Indexes the accepted architectural decisions and the views they affect.
---

<!-- markdownlint-disable MD025 -->

# 9. Architecture decisions

Decision records live in [`../../adr/`](../../adr/). This section lists the accepted decisions that shape the current architecture; status, rationale, alternatives and history stay in each ADR.

| Decision                                                                                     | ADR                                                                                      | Affected architecture                                             |
| -------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------- | ----------------------------------------------------------------- |
| Start with a modular monolith                                                                | [ADR-0001](../../adr/0001-modular-monolith.md)                                           | §4 strategy, §5 building blocks, §7 deployment                    |
| License code under AGPLv3 and keep the brand separate                                        | [ADR-0002](../../adr/0002-license-and-brand.md)                                          | §2 constraints                                                    |
| Use PostgreSQL with EF Core and enforce append-only financial tables in the database         | [ADR-0004](../../adr/0004-postgresql-ef-core-append-only.md)                             | §4 strategy, §5 building blocks, §8 persistence and immutability  |
| Store issued invoices as a canonical representation with a SHA-256 fingerprint               | [ADR-0005](../../adr/0005-issued-invoice-representation.md)                              | §6 runtime (finalization), §8 integrity                           |
| Round money once per line and compute VAT per rate on rounded line amounts                   | [ADR-0006](../../adr/0006-money-rounding-and-vat.md)                                     | §8 money and tax calculation                                      |
| Allocate gapless numbers per series and year in the issuing transaction                      | [ADR-0007](../../adr/0007-gapless-numbering.md)                                          | §6 runtime (finalization), §8 concurrency                         |
| Render invoice PDFs with an in-house deterministic writer behind a port                      | [ADR-0008](../../adr/0008-deterministic-pdf-renderer.md)                                 | §5 building blocks, §8 documents                                  |
| Store documents through a port with file-system and Azure Blob adapters                      | [ADR-0009](../../adr/0009-document-storage.md)                                           | §5 building blocks, §7 deployment                                 |
| Record compliance obligations through a seam before implementing VERI\*FACTU                 | [ADR-0010](../../adr/0010-compliance-seam.md)                                            | §3 context, §5 building blocks, §6 runtime                        |
| Build the frontend with React and Vite on OpenAPI-generated types                            | [ADR-0011](../../adr/0011-frontend-stack.md)                                             | §4 strategy, §5 building blocks, §8 API contract and localization |
| Serve one organization per deployment for the MVP                                            | [ADR-0012](../../adr/0012-single-organization-per-deployment.md)                         | §2 constraints, §7 deployment, §11 risks                          |
| Ship one application image deployed through Compose, Aspire and Azure                        | [ADR-0013](../../adr/0013-deployment-topology.md)                                        | §7 deployment                                                     |
| Test through public seams against real PostgreSQL without a mocking library                  | [ADR-0014](../../adr/0014-testing-strategy.md)                                           | §8 testing, §10 quality                                           |
| Enforce build and quality gates in the build and in CI                                       | [ADR-0015](../../adr/0015-build-quality-gates.md)                                        | §8 build and quality gates, §10 quality                           |
| Add commit-time hooks, CI mirrors, security gates and spec tooling as guardrails             | [ADR-0016](../../adr/0016-guardrails.md)                                                 | §8 build, security and quality gates, §10 quality                 |
| Run end-to-end journeys against the real AppHost in a real browser with a pinned selection   | [ADR-0017](../../adr/0017-end-to-end-testing.md)                                         | §8 testing, §10 quality                                           |
| Split modules into assemblies named after the product's bounded contexts                     | [ADR-0018](../../adr/0018-module-assemblies-named-after-bounded-contexts.md)             | §4 strategy, §5 building blocks, §8 architecture rules            |
| Give each module its own schema and DbContext, sharing one transaction                       | [ADR-0019](../../adr/0019-module-owned-schemas-with-a-shared-transaction.md)             | §5 building blocks, §6 runtime (R-2, R-4), §8 persistence         |
| Communicate between modules through synchronous contracts and defer events                   | [ADR-0020](../../adr/0020-synchronous-module-contracts-with-deferred-events.md)          | §4 strategy, §5 building blocks, §6 runtime (R-2, R-5)            |
| Authenticate with local Identity and cookie sessions, authorize with scoped role permissions | [ADR-0021](../../adr/0021-local-identity-with-cookie-sessions-and-scoped-permissions.md) | §8 security, §11 risks (R-1, D-9)                                 |
| Write each use case as a single-file slice returning result errors                           | [ADR-0022](../../adr/0022-single-file-use-case-slices-with-result-errors.md)             | §5 building blocks, §8 error handling and API contract            |
| Organize the frontend as feature-owned vertical slices                                       | [ADR-0023](../../adr/0023-frontend-feature-slice-architecture.md)                        | §5 building blocks, §8 frontend quality                           |
| Publish static engineering snapshots through GitHub Pages                                    | [ADR-0024](../../adr/0024-publish-static-engineering-snapshots-on-github-pages.md)       | §7 deployment, §8 testing evidence                                |
| Host the public site through Cloudflare Pages                                                | [ADR-0025](../../adr/0025-host-the-public-site-on-cloudflare-pages.md)                   | §7 deployment, §8 build and quality gates                         |
| Keep architecture diagram sources beside arc42 prose                                         | [ADR-0027](../../adr/0027-keep-architecture-diagram-sources-with-arc42.md)               | §3, §5–7, §10                                                     |
| Deliver application email through configured SMTP                                            | [ADR-0028](../../adr/0028-configure-smtp-email-delivery-through-mailkit.md)              | §5 building blocks, §7 deployment, §8 security and configuration  |
| Record the self-hosted VERI*FACTU responsibility boundary                                    | [ADR-0029](../../adr/0029-self-hosted-verifactu-responsibility-boundary.md)              | §2 constraints, §7 deployment, §11 risks                          |
