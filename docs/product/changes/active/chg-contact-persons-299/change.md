---
id: CHG-CONTACT-PERSONS-299
type: product-change
title: Accept contact persons and their designations in Contacts
status: proposed
base-revision: "769edf87e11dec97b23de000741c2f745545256b"
operations:
  add:
    - UC-CON-MANAGE-CONTACT-PERSONS
    - FR-CON-002
    - BR-CON-CONTACT-PERSON-DESIGNATIONS
  modify:
    - BC-CONTACTS
    - BR-PRJ-CONTACT-PERSON-NOT-MOVABLE
    - TERM-CONTACT-PERSON
    - TERM-DEFAULT-CONTACT-PERSON
    - TERM-ADMINISTRATIVE-CONTACT-PERSON
  remove: []
---

## Problem

The Contacts model has draft terms for people affiliated with a contact and their default or administrative designations, but the accepted product definition has no requirement for maintaining those people. The backlog issue #299 requires contact-person management and the single-default invariant, while project selection and invoice dispatch remain later behaviors.

## Intended Product Outcome

An authorized operations coordinator can maintain people affiliated with a company or private-person contact and record their supported designations. A contact person belongs to the contact under which it was created and is not moved to another contact. Each contact has zero or one default contact person. This change stores designations; it does not select a person on projects or send invoices to them.

## Rationale

Projects and later communication workflows need stable contact-person records with explicit roles. Accepting maintenance independently keeps #299 focused while project selection and invoice dispatch remain governed by their own product stories.

## Affected Product Areas

Contacts, contact-person records, the default and administrative designations, and the future project and communication workflows that may consume them.

## Open Questions

- Should a contact allow at most one administrative contact person, matching the singular term and future invoice-recipient use case, or may several people carry this designation? Recommendation: at most one.
- When a person is set as default while another person is already default, should the product atomically clear the previous designation or reject the change until the previous designation is cleared? Recommendation: atomically clear the previous designation.
- Should #299 allow deleting a contact person, and, if so, what happens to its default or administrative designation and any future project or communication references? Recommendation: leave deletion out of #299 until reference and audit behavior is defined.
- Should custom fields and files currently named in the draft `TERM-CONTACT-PERSON` be included in #299, or tracked as a separate Contacts/Documents issue? Recommendation: defer them; #299 asks for person records and designations, and its E2E does not require file handling.

## Product Acceptance

- The Product Owner confirms that contact persons are maintained independently under one contact and cannot be moved between contacts.
- The Product Owner confirms that each contact has at most one default contact person and that changing it clears the previous default.
- The Product Owner confirms whether #299 supports deleting a contact person or defers deletion for a later change.
- The Product Owner confirms which administrative designation cardinality to accept.
- The Product Owner confirms whether custom fields and files are excluded from #299 and need separate backlog coverage.
- The Product Owner confirms that project selection and invoice/email dispatch are not implemented by #299.

## Out of Scope

Selecting contact persons on projects, choosing invoice recipients, sending email, editing contact data, deleting or merging contacts, custom fields, file attachments, import, and implementation, technical design, release or deployment. Contact-person deletion remains outside the proposed scope unless the Product Owner resolves the open question by expanding this change.
