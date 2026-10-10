---
id: FR-PRJ-002
type: functional-requirement
title: "Maintain project details and parties"
status: draft
derived-from:
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
  - "BR-PRJ-PROJECT-NUMBER-ASSIGNMENT"
  - "BR-PRJ-PARTIES-FROM-CONTACTS"
  - "BR-PRJ-CONTACT-SELECTION-EFFECTS"
verification:
  - scenario-ref: "SB-PRJ-CONTACT-WARNING-AND-NOTE"
  - scenario: "A project saved with an empty number receives the next project number"
uses-terms:
  - "TERM-PROJECT-NUMBER"
  - "TERM-PROJECT-LOCATION"
provenance:
  source: ".private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST record for each project its name, type, status, unique number (auto-assigned when empty), account manager, colour, external reference, time zone, client with contact person, and location, applying the selected contacts' stored warning, note, address and travel data.

## Rationale

Correct project data feeds documents and logistics.
