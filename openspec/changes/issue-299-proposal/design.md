<!-- pdac-scope: cited -->

# Design

## Context

The accepted Product Changes define Contacts directory and contact-person behavior, including tenant isolation, authorization, auditability, single-designation cardinality, immutable parent ownership, and preservation of issued-invoice party snapshots. The current runtime has customer-only adapters and no Contacts routes or editor. See the approved model artifacts and the issue for behavior.

## Goals / Non-Goals

**Goals:** Restore the #298 directory prerequisite, then implement contact people using existing module, persistence, authorization, audit, and frontend patterns. Keep designation changes transactional and enforce uniqueness in persistence as well as application validation.

**Non-Goals:** Contact-person deletion/retention, custom fields, attachments, project selection, invoice delivery, and changes to issued-invoice snapshots.

## Decisions

- Recover and requalify the reviewed epic-branch implementation if its source is available; otherwise implement the smallest change against current Contacts module conventions. This avoids recreating existing approved work unnecessarily.
- Model people as children of a contact with a non-mutable parent identifier. Scope every lookup and write through the authenticated installation/database and contact boundary; do not add a tenant discriminator or client-selected workspace identifier.
- Use a database transaction and contact-scoped uniqueness constraints for each designation. Setting one designation must clear the prior holder and set the new holder in the same successful transaction; the other designation remains independent.
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

<!-- pdac:cite id="FR-CON-001" digest="sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:b80ca1deda21cf11609d89248e6aa165275057b80e02519feb6ea2cbb967c033" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:de5fef396fa13aa8f696c139c356dc719be7dde97995a7db49bf9c8d15111c8b" -->
<!-- pdac:cite id="BR-PRJ-INVOICE-RECIPIENT" digest="sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:5cd239965aa4275900b2daef3a6f6d520dbe10391c8f83e0aa8ae3c517d2d8ea" -->
<!-- pdac:cite id="SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON" digest="sha256:0513919df0cd30d0d16fc75d98b69f1c76d8824d19e38a292620a9c0c661abb4" -->
<!-- pdac:cite id="SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT" digest="sha256:93235b2bbd6b5add5a7f27ceed023329ab7ca3af5b2c4a2e4f3178d997095124" -->
