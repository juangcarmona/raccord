---
id: BR-FIN-ONLINE-QUOTE-VIEWS
type: business-rule
title: "Only visitors who are not logged-in users are counted as views"
status: draft
applies-to:
  - "UC-FIN-RESPOND-TO-ONLINE-QUOTE"
uses-terms:
  - "TERM-ONLINE-QUOTE"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The view counter of an online quote increases only when it is opened by someone not logged in as a user of the company in that browser.

## Rationale

Engagement figures must reflect the customer, not staff.

## Examples

See SB-FIN-STAFF-VIEW-NOT-COUNTED.

## Exceptions

None.
