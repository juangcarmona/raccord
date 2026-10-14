---
title: Risks and technical debt
arc42-section: "11"
description: Records architectural risks, technical debt, and evidence-backed mitigations.
---
<!-- markdownlint-disable MD025 -->

# 11. Risks and technical debt

## Risks

| ID | Risk | Impact | Mitigation |
| --- | --- | --- | --- |
| R-1 | **No role-based authorization yet** | Any authenticated account can reach the current application API | Keep account creation and deployment access controlled; add scoped permissions before multi-role deployments. |
| R-2 | VERI*FACTU legal responsibility for open-source self-hosted software unresolved | Could block the Spanish market | Compliance seam records obligations now; legal review before implementation |
| R-3 | Tax rules and dates change (announced 2028 postponement not enacted) | Rework | Requirements dated and sourced; adapters isolated |
| R-4 | Rounding policy not yet confirmed by an accountant | Possible disagreement on VAT breakdown | ADR-0006 explicit and tested; easy to change in one place |
| R-5 | Scope creep toward full operations parity | Delays a trustworthy billing core | Roadmap orders billing before planning |
| R-6 | Self-hosting support burden | Upgrade failures | Explicit migration job, health checks, rehearsed restore |
| R-7 | Numbering contention with several replicas | 409 retries under load | One replica by default; load test before scaling |
| R-8 | Aspire's orchestrator (DCP) failed to open its local log socket on one Windows workstation | Dashboard/proxy degraded locally | Resources still ran; Compose remains the reference path; track upstream |

## Technical debt

| ID | Debt | Plan |
| --- | --- | --- |
| D-1 | No editing of customers or billable lines yet | MVP slice 3 |
| D-2 | Lists are unpaginated; draft list loads projects/customers one by one | Add pagination and projections before measuring QR-RESPONSE-TIME |
| D-3 | PDF renderer: WinAnsi only, no logo/QR/PDF-A | Revisit with VERI*FACTU QR (ADR-0008) |
| D-4 | Regime key / exemption cause not modelled per VAT group | Required by the compliance adapter |
| D-5 | Frontend has tests for formatting, draft and project pages only | Add tests as forms gain behavior |
| D-6 | No data export yet | MVP slice 8 |
| D-7 | Azure template uses PostgreSQL password auth over public network with "Azure services" firewall rule | Move to Entra auth and private networking for production |
| D-9 | Open technical questions: is one web replica enough for numbering under load; which role and permission model should ship for self-hosted deployments; should the PDF renderer move to a library once templates are configurable | Decide with MVP slices 2 and 5 |
| D-8 | Local Node 24.11 is below jsdom's declared minimum (24.15); tests pass, CI uses current Node 24 | Upgrade local toolchains |
