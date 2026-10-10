---
id: FR-WSP-011
type: functional-requirement
title: "Consistent enforcement of permissions"
status: draft
derived-from:
  - "BR-WSP-ONE-ROLE-PER-USER"
  - "BR-WSP-DELETE-NEEDS-EDIT"
  - "BR-WSP-EXTRA-ITEMS-OVERRIDE"
  - "BR-WSP-DASHBOARD-RESPECTS-PERMISSIONS"
verification:
  - scenario-ref: "SB-WSP-FREELANCER-APP-SCOPE"
  - scenario-ref: "SB-WSP-DELETE-ONLY-EDITABLE"
  - scenario-ref: "SB-WSP-EXTRA-ITEM-WINS"
  - scenario-ref: "SB-WSP-NO-FINANCE-NO-REVENUE-WIDGET"
uses-terms:
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST apply a user's single role identically on the web, in the mobile web app, in calendar feeds and in dashboards, including the documented dependencies and precedences between permissions.

## Rationale

Access must not leak through secondary channels.
