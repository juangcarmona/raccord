---
id: BR-CON-TAG-FILTER-MODES
type: business-rule
title: "Contact tag filters match any or all"
status: active
applies-to:
    - "UC-CON-TAG-CONTACTS"
    - "UC-CON-FILTER-CONTACT-DIRECTORY"
uses-terms:
    - "TERM-CONTACT-TAG"
---

## Rule

When filtering contacts by several tags, the actor can choose whether results include contacts with any selected tag or only contacts with all selected tags.

## Rationale

Both broad grouping and intersection filtering are useful when organizing contacts.

## Examples

Filtering for `venue` and `customer` in any mode includes either group; all mode includes contacts carrying both tags.

## Exceptions

None.
