---
id: CHG-CONTACT-PERSONS-299
type: product-change
title: Accept contact persons and their designations in Contacts
status: applied
base-revision: "769edf87e11dec97b23de000741c2f745545256b"
operations:
  add:
    - UC-CON-MANAGE-CONTACT-PERSONS
    - FR-CON-002
    - BR-CON-CONTACT-PERSON-DESIGNATIONS
    - SB-CON-CHANGE-DEFAULT-CONTACT-PERSON
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

None. The Product Owner approved this proposal as drafted on 2026-10-20, including the recommendations: at most one administrative person per contact; changing the default atomically clears the former default; deletion, custom fields, and files stay out of #299; and project selection and invoice/message dispatch remain outside this change.

## Product Acceptance

- The Product Owner approved the scoped behavior: contact persons are maintained under one contact and cannot be moved to another.
- Each contact has at most one default and at most one administrative contact person; changing the default atomically clears the previous default.
- Contact-person deletion, custom fields and files are excluded from #299 pending later product definition.
- Project selection and invoice/message dispatch remain outside #299.

## Out of Scope

Selecting contact persons on projects, choosing invoice recipients, sending email, editing contact data, deleting or merging contacts, custom fields, file attachments, import, and implementation, technical design, release or deployment. Contact-person deletion remains outside the proposed scope unless the Product Owner resolves the open question by expanding this change.
