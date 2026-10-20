---
id: FR-CON-002
type: functional-requirement
title: "Maintain contact persons and designations"
status: active
derived-from:
  - "UC-CON-MANAGE-CONTACT-PERSONS"
  - "BR-CON-CONTACT-PERSON-DESIGNATIONS"
  - "BR-PRJ-CONTACT-PERSON-NOT-MOVABLE"
verification:
  - scenario-ref: "SB-CON-CHANGE-DEFAULT-CONTACT-PERSON"
  - scenario: "A coordinator sets a new default contact person"
  - scenario: "A coordinator creates a person under another contact"
  - scenario: "The contact persons feature does not change an issued invoice"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-PERSON"
  - "TERM-DEFAULT-CONTACT-PERSON"
  - "TERM-ADMINISTRATIVE-CONTACT-PERSON"
---

## Requirement

The product MUST let an authorized user create and edit contact persons under a contact and record their supported default and administrative designations. A contact MUST have at most one default contact person and at most one administrative contact person; setting another person as default MUST clear the previous designation atomically. A contact person MUST remain associated with the contact under which it was created. These designations do not make the person a project or invoice recipient.

## Rationale

Maintaining a contact's people and designations gives operations teams a reliable directory while keeping project selection and document delivery in their owning workflows.
