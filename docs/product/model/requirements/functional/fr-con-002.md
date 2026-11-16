---
id: FR-CON-002
type: functional-requirement
title: Maintain contact persons and designations
status: active
derived-from:
  - UC-CON-MANAGE-CONTACT-PERSONS
  - BR-CON-CONTACT-PERSON-DESIGNATIONS
  - BR-PRJ-CONTACT-PERSON-NOT-MOVABLE
verification:
  - scenario-ref: SB-CON-CHANGE-DEFAULT-CONTACT-PERSON
  - scenario-ref: SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON
  - scenario-ref: SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT
uses-terms:
  - TERM-CONTACT
  - TERM-CONTACT-PERSON
  - TERM-DEFAULT-CONTACT-PERSON
  - TERM-ADMINISTRATIVE-CONTACT-PERSON
---

## Requirement

The product MUST let an authorized user list, create, read and edit a contact's people with their names, email/phone details and optional default and administrative designations. BR-CON-CONTACT-PERSON-DESIGNATIONS governs both designation cardinalities and atomic reassignment; BR-PRJ-CONTACT-PERSON-NOT-MOVABLE fixes parent ownership. These writes MUST preserve project contact associations and issued-invoice party snapshots. This requirement does not select a person for a project or send an invoice or communication.

## Rationale

Maintaining a contact's people and designations gives operations teams a reliable directory while keeping project selection and document delivery in their owning workflows.
