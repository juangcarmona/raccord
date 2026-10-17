---
pdac-scope: cited
---

# Tasks

<!-- pdac:cite id="FR-ISSUER-001" digest="sha256:1d443dcade4a5af9dce47d75a383a243a298dd37de5a2d1be22bc80d5dfe1712" -->
<!-- pdac:cite id="UC-DEFINE-ISSUER-PROFILE" digest="sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:f278c6a11a99fe63feadf0105b0ce359e74c71580b9b070a4fbf95c3b34b1604" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="CON-SINGLE-ISSUER" digest="sha256:f41d4ba8da941e762b3ee568fa15b478f1a60443cb2ea905d4826b13cdb1e1cd" -->

- [x] Add failing Workspace/API tests for setup behavior, reason requirement, pending request, self-review rejection, authorization, approval/rejection, immutable prior invoice snapshots, and supported input lengths.
- [x] Add an issuer invoice counter, increment it in the invoice unit of work, backfill it from historical issued invoices, and prove first-invoice races fail safely through optimistic concurrency.
- [x] Add request domain model, Workspace persistence mapping, migration, query and command operations with one-pending-request and single-decision concurrency protection.
- [x] Add request/review endpoints with scoped permissions and audit events in the same unit of work.
- [x] Update generated OpenAPI contracts and frontend request, pending-review list, reviewer actions, and localized accessible messages; add component tests.
- [x] Add a two-user AppHost E2E journey for post-issuance request, reviewer inspection, approval and future-invoice identity; isolate its database and add it to per-PR selection.
- [x] Update affected architecture/operations docs and the process ledger.
- [ ] Run focused tests, full .NET/frontend checks, AppHost E2E, OpenSpec validation, ProductShape validation/citations, and CI-equivalent checks; attach runtime evidence.
- [ ] Self-review final diff, verify all Done evidence and final-head CI, archive this OpenSpec change, then merge and close #341.
