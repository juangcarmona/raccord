---
id: FR-CON-001
type: functional-requirement
title: "Maintain and browse the Contacts directory"
status: active
derived-from:
  - "UC-CON-MANAGE-CONTACT-DIRECTORY"
  - "UC-REGISTER-CUSTOMER"
  - "BR-PRJ-CONTACT-NUMBER-ASSIGNMENT"
  - "BR-PRJ-CONTACT-ROLES-BY-TAG"
  - "BR-PARTY-SNAPSHOT"
verification:
  - scenario: "A user creates a company contact as both customer and supplier"
  - scenario: "A user creates a private-person venue and finds it on a later contact-list page"
  - scenario: "A user edits a customer and the details of an already issued invoice remain unchanged"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-TYPE"
  - "TERM-CUSTOMER"
  - "TERM-SUPPLIER-CONTACT"
  - "TERM-VENUE"
  - "TERM-CONTACT-NUMBER"
---

## Requirement

The product MUST let an authorized user create, view, edit and browse paginated company and private-person contacts with one or more customer, supplier and venue roles. It MUST assign a unique contact number when omitted, and MUST preserve every issued invoice's party snapshot when a contact is edited.

## Rationale

A shared, maintainable contact directory keeps project and billing workflows consistent without rewriting historical invoices.
