---
id: FR-FIN-103
type: functional-requirement
title: "Set the conditions printed on a project's documents"
status: draft
derived-from:
  - "UC-FIN-SET-PROJECT-CONDITIONS"
  - "BR-FIN-DEFAULTS-BY-PROJECT-TYPE"
verification:
  - id: "PROJECT-CONDITIONS"
    scenario: "Given a project type with default additional conditions, when a project of that type is created, those conditions and the general terms are pre-selected; after the coordinator adds another saved condition and free text and saves, documents generated afterwards print them, unless the template lacks the additional-conditions section, in which case the text is not printed."
uses-terms:
  - "TERM-ADDITIONAL-CONDITIONS"
  - "TERM-TERMS-AND-CONDITIONS"
provenance:
  source: ".private/research/help-center/articles/360013938099.md, .private/research/help-center/articles/360013264720.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST pre-select, for a new project, the default additional conditions of its project type and include the general terms, and MUST let the coordinator choose other saved additional conditions or write free additional text and save them. The product MUST print the saved conditions on documents generated afterwards, and MUST NOT print additional text when the document template has no section for it.

## Rationale

Specific jobs call for specific conditions, and quotes and invoices must carry the ones agreed.
