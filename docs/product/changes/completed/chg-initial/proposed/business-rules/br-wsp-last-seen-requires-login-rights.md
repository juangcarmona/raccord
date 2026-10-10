---
id: BR-WSP-LAST-SEEN-REQUIRES-LOGIN-RIGHTS
type: business-rule
title: "Last seen requires the right to manage login information"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-LAST-SEEN"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md, .private/research/help-center/articles/36290993235730.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The last-seen information about crew members can only be enabled for roles that may change login information (and edit and delete crew member data).

## Rationale

Activity data about colleagues is sensitive.

## Examples

- A planner who cannot change passwords does not see when colleagues were last active.

## Exceptions

None.
