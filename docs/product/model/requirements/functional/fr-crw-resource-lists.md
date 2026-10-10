---
id: FR-CRW-RESOURCE-LISTS
type: functional-requirement
title: "Crew and vehicle lists"
status: draft
derived-from:
  - "UC-CRW-FIND-CREW-WITH-LISTS"
  - "UC-CRW-MANAGE-CUSTOM-RESOURCE-LISTS"
  - "BR-CRW-TAG-MATCH-EXACT"
  - "BR-CRW-AVAILABLE-LIST-ORDER"
  - "BR-CRW-CUSTOM-LISTS-SHARED"
  - "BR-CRW-DYNAMIC-LIST-MEMBERSHIP"
verification:
  - scenario-ref: "SB-CRW-TAG-MATCH"
  - scenario-ref: "SB-CRW-AVAILABLE-LIST-ORDER"
  - scenario-ref: "SB-CRW-FOLDER-LIST-AUTO-UPDATE"
  - scenario-ref: "SB-CRW-HIDE-LIST-PERSONAL"
uses-terms:
  - "TERM-CREW-LIST"
provenance:
  source: ".private/research/help-center/articles/12866138319506.md, .private/research/help-center/articles/14924701052050.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST offer planned-in-project, available and matching-tags lists for a selected function and shared custom lists by pick, folder or tags that each user can hide.

## Rationale

Find the right person quickly.
