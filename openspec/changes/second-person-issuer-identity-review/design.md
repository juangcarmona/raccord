---
pdac-scope: cited
---

# Design

<!-- pdac:cite id="FR-ISSUER-001" digest="sha256:1d443dcade4a5af9dce47d75a383a243a298dd37de5a2d1be22bc80d5dfe1712" -->
<!-- pdac:cite id="UC-DEFINE-ISSUER-PROFILE" digest="sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:f278c6a11a99fe63feadf0105b0ce359e74c71580b9b070a4fbf95c3b34b1604" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="CON-SINGLE-ISSUER" digest="sha256:f41d4ba8da941e762b3ee568fa15b478f1a60443cb2ea905d4826b13cdb1e1cd" -->

## Approach

Keep the workflow in Workspace because issuer profile ownership and settings authorization already live there. Add a persisted `IssuerIdentityChangeRequest` record in the Workspace schema, including initiator, reason, old/proposed legal name and tax ID, status, reviewer, decision time, and creation time. Expose create/list/review through Workspace application services and organization endpoints. Use the existing append-only audit writer for request and decision events.

Workspace must not query Invoicing tables directly. Store `issued_invoice_count` on the issuer profile and increment it from invoice finalization in the same unit of work as the issued invoice. Backfill the count from existing issued invoices in the migration. This gives Workspace the issuance state without a cross-schema read and provides one optimistic-concurrency row shared by issuer updates and every invoice issuance. A stale operation fails with the existing concurrency conflict response and can be retried against current state. The approval path applies only the requested legal fields and leaves address/contact values current.

The existing issuer update endpoint keeps setup and address/contact edits. A separate `POST /api/organization/issuer-identity-changes` requires a reason and creates a pending request. `GET` lists retained requests and `PUT /{id}` decides one. A reviewer decision updates request state and, on approval, the issuer profile and audit event in one unit of work. Rejection records the decision and leaves the profile untouched. Requests are never deleted. Use `xmin` optimistic concurrency and a partial unique index to allow at most one pending request per issuer, so two reviewers cannot decide the same request and conflicting proposals cannot be queued.

Persist request and issuer updates through the same Workspace unit of work; audit appending enlists in the shared transaction as required by ADR-0019. Every invoice issuance increments the issuer row's concurrency counter in that same transaction, so concurrent first issuance and legal identity edits cannot commit against stale state.

## API and UI

Add endpoints under `/api/organization/issuer-identity-changes` to create, list pending/history, and decide an identified request. Require `workspace:organization.edit` for mutations and `workspace:organization.read` for reads. The issuer response reports whether an invoice has been issued so the UI can request a reason and send legal identity changes through the review endpoint. Return conflict for a stale/concurrent decision and forbidden for missing permission; reject self-review as a business rule.

Update the issuer settings page to request review when legal identity changes after issuance, display the reason field and pending status, and show request details/decision controls to authorized reviewers. The requester cannot approve their own change. Preserve localized copy and current setup behavior. Run the mutating two-user browser journey in a dedicated E2E collection with its own ephemeral AppHost database so it cannot alter state observed by the rest of the suite.

## Alternatives considered

- Query issued invoices from Workspace persistence: rejected because it crosses the module-owned schema boundary.
- Put the workflow in Invoicing: rejected because the issuer profile and its settings authorization are owned by Workspace.
- Apply the identity immediately and audit afterward: rejected because audit does not enforce second-person approval.
- Treat address edits as requiring two people: rejected because approved policy explicitly keeps these available to one administrator.

## Compatibility and architecture

The existing issuer GET and PUT remain compatible except that the issuer response gains an `hasIssuedInvoice` field. Add create/list/decision OpenAPI contracts and generated TypeScript types. Add a Workspace schema migration that backfills invoice counts and stores requests, without modifying issued invoice rows or issuer identity values. No new production dependency. No deployment config changes.

## Product definition drift

None. The implementation follows the applied Product Change and existing product model.
