---
id: "FR-DOC-003"
type: functional-requirement
title: "Configure template settings including language and country"
status: draft
derived-from:
  - "UC-DOC-CONFIGURE-TEMPLATE-SETTINGS"
  - "BR-DOC-COUNTRY-FORMATTING"
verification:
  - scenario-ref: "SB-DOC-COUNTRY-DATE-FORMAT"
uses-terms:
  - "TERM-TEMPLATE-LOCALE"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/360014232559.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let each document type's template carry a language, country, subject, file name, header, footer, signing options and default letterhead, and MUST format generated content by the template country.

## Rationale

Documents must suit their audience and region.
