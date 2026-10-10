---
id: BR-CRW-ONE-DEFAULT-RATE
type: business-rule
title: "A crew member has at most one default rate"
status: draft
applies-to:
  - "UC-CRW-LINK-RATES-TO-CREW-MEMBER"
uses-terms:
  - "TERM-CREW-RATE"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/7191443180562.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Among the rates linked to a crew member, at most one is the default; making another rate default replaces the previous one.

## Rationale

Planning needs one unambiguous starting rate.

## Examples

Bulk-linking a global cost rate as default replaces the earlier default of each selected person.

## Exceptions

None.
