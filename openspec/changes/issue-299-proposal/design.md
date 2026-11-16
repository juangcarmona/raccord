# Design

## Context

The accepted Product Changes define Contacts directory and contact-person behavior, including tenant isolation, authorization, auditability, single-designation cardinality, immutable parent ownership, and preservation of issued-invoice party snapshots. The current runtime has customer-only adapters and no Contacts routes or editor. See the approved model artifacts and the issue for behavior.

## Goals / Non-Goals

**Goals:** Restore the #298 directory prerequisite, then implement contact people using existing module, persistence, authorization, audit, and frontend patterns. Keep designation changes transactional and enforce uniqueness in persistence as well as application validation.

**Non-Goals:** Contact-person deletion/retention, custom fields, attachments, project selection, invoice delivery, and changes to issued-invoice snapshots.

## Decisions

- Recover and requalify the reviewed epic-branch implementation if its source is available; otherwise implement the smallest change against current Contacts module conventions. This avoids recreating existing approved work unnecessarily.
- Model people as children of a contact with a non-mutable parent identifier. Scope every lookup and write through the authenticated installation/database and contact boundary; do not add a tenant discriminator or client-selected workspace identifier.
- Use a database transaction and tenant/contact-scoped uniqueness constraints for each designation. Setting one designation must clear the prior holder and set the new holder in the same successful transaction; the other designation remains independent.
- Keep contact-person operations separate from project and invoicing write paths. Verify snapshots and associations remain unchanged in regression tests.
- Reuse established Contacts authorization and audit mechanisms; do not introduce new roles or permission semantics.

## Risks / Trade-offs

- [The directory prerequisite or reviewed epic implementation is absent from the candidate] → Confirm the exact missing files against the approved source before rebuilding; do not substitute behavior from newer #301/#302 work.
- [Concurrent designation changes could violate cardinality] → Enforce uniqueness in persistence and handle transaction conflicts without partial reassignment.
- [Installation boundary omissions could expose another installation's records] → Preserve the existing database-per-installation boundary and cover unauthorized access and contact ownership in API/persistence tests; do not invent cross-installation access paths.
- [Contacts editor work could accidentally alter downstream records] → Keep writes confined to Contacts and assert existing project/invoice data remains unchanged.

## Migration Plan

Add a forward-only migration for the approved directory/person records and designation constraints. Deploy the schema before the application version that uses it. Roll back by rolling back the application first; retain data and use the repository's normal migration rollback policy rather than deleting contact history.

## Open Questions

None about product behavior. The implementation source/reference availability is an operational recovery constraint, not a product decision.
