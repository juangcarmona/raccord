---
id: FR-WSP-029
type: functional-requirement
title: "Configuration of company details and numbering"
status: draft
derived-from:
  - "UC-WSP-EDIT-COMPANY-DETAILS"
  - "UC-WSP-CONFIGURE-NUMBER-SERIES"
  - "BR-WSP-SERIES-CURRENT-NUMBER"
verification:
  - scenario: "After setting a series' current number to 99, the next record of that kind receives number 100"
  - scenario: "An uploaded workspace image appears behind the sign-in form"
uses-terms:
  - "TERM-COMPANY-DETAILS"
  - "TERM-NUMBER-SERIES"
provenance:
  source: ".private/research/help-center/articles/15426355918226.md, .private/research/help-center/articles/15744586750866.md, .private/research/help-center/articles/29749329716754.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST let administrators maintain company details and workspace image, and adjust number series.

## Rationale

Basic company configuration.
