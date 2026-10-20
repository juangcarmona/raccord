---
id: CHG-CONTACTS-DIRECTORY-001
type: product-change
title: Accept the Contacts directory workflows for customers, suppliers and venues
status: applied
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add:
    - UC-CON-MANAGE-CONTACT-DIRECTORY
    - FR-CON-001
  modify:
    - BC-CONTACTS
    - UC-REGISTER-CUSTOMER
    - FR-CUSTOMER-001
    - BR-PARTY-SNAPSHOT
    - BR-PRJ-PARTIES-FROM-CONTACTS
    - TERM-CONTACT
    - TERM-CONTACT-TYPE
    - TERM-CUSTOMER
    - TERM-SUPPLIER-CONTACT
    - TERM-VENUE
    - TERM-CONTACT-NUMBER
    - BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
    - BR-PRJ-CONTACT-ROLES-BY-TAG
  remove: []
---

## Problem

The Contacts model describes customers, suppliers and venues, but its related artifacts are still drafts and there is no accepted requirement for editing and paginated browsing. The current implementation only supports registering and listing customers, so teams cannot maintain the shared contact directory required by production workflows.

## Intended Product Outcome

Users with access to Contacts can create, view, edit and browse company or private-person records as customers, suppliers and venues. A contact may have more than one role; roles remain distinct from company/private-person type. Contacts have a stable unique number, and the list supports pagination. Project and invoice workflows use the shared contact record, while issued invoices retain the party details captured at issue time.

## Rationale

The five Contacts child issues build on one source of truth for organizations and people. Accepting the directory behavior first gives #298 a product-defined foundation while leaving contact persons, commercial defaults, imports, generic tags and folder management to their own stories.

## Affected Product Areas

Contacts, customer registration and invoice readiness, project client/location selection, contact numbering, customer/supplier/venue roles, and issued-invoice party snapshots.

## Open Questions

None. The accepted roles, contact type, number assignment and invoice snapshot behavior are grounded in the cited existing artifacts. Pagination has no user-visible page-size requirement; implementation may choose a suitable page size.

## Product Acceptance

- The Product Owner confirms that customers, suppliers and venues are roles of a company or private-person contact, and that a contact can have multiple roles.
- The Product Owner confirms that users can create, read, edit and paginate the contact directory.
- The Product Owner confirms that customer edits affect future invoices only and cannot mutate issued party snapshots.
- The Product Owner confirms contact-number uniqueness and automatic assignment when the number is omitted.

## Out of Scope

Contact persons and designations (#299), commercial defaults and project-time snapshots (#300), spreadsheet import (#301), generic tags and folders (#302), registry lookup, bulk edit, application implementation, technical design, release and deployment.
