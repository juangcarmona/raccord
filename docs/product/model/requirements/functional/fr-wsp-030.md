---
id: FR-WSP-030
type: functional-requirement
title: Configure the contact-number series
status: active
derived-from:
  - "UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES"
  - "BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION"
  - "BR-PRJ-CONTACT-NUMBER-ASSIGNMENT"
verification:
  - scenario-ref: "SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION"
uses-terms:
  - "TERM-CONTACT-NUMBER"
---

## Requirement

The product MUST let Workspace administrators configure the optional prefix and current number used for future automatic contact-number allocation. A new workspace defaults to an empty prefix and current number zero. Allocation advances by one, skips any value already used by a contact, and MUST remain unique when contacts are created concurrently. Saving configuration MUST NOT change existing contact numbers. Users may supply a custom contact number, which is subject to the same uniqueness constraint.

## Rationale

Teams can continue their existing contact references without changing the identifiers already present on contacts and documents.
