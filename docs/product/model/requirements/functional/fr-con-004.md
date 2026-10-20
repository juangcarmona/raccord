---
id: FR-CON-004
type: functional-requirement
title: "Tag and filter contacts"
status: active
derived-from:
    - "UC-CON-TAG-CONTACTS"
    - "UC-CON-FILTER-CONTACT-DIRECTORY"
    - "BR-CON-CONTACT-TAG-SCOPE"
    - "BR-CON-TAG-FILTER-MODES"
verification:
    - scenario-ref: "SB-CON-TAG-FILTER-ANY-AND-ALL"
    - scenario-ref: "SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE"
uses-terms:
    - "TERM-CONTACT"
    - "TERM-CONTACT-TAG"
---

## Requirement

The product MUST let authorized users create and remove Contacts tags and assign or remove them on contacts. The contact directory MUST filter by one or more tags, with both any-match and all-match behavior. Contact tags and filter results MUST remain scoped to Contacts in the active workspace.

## Rationale

Teams can group contacts for operational use without affecting unrelated record types or exposing another workspace's contacts.
