---
id: BR-PRJ-IMPORT-RESULT-REPORT
type: business-rule
title: Imports report failed rows with reasons
status: draft
applies-to: []
uses-terms: []
provenance:
  source: .private/research/help-center/articles/4413562661138.md, .private/research/help-center/articles/39225660180626.md, .private/research/help-center/articles/360013553000.md
  confidence: high
  recovered-from: documentation
---

## Rule

Every import ends with a result listing imported and failed rows and the reason for each failure, and offers the failed rows as a downloadable file for correction and re-import.

## Rationale

Partial failures must be fixable.

## Examples

See SB-PRJ-IMPORT-FAILED-ROWS-FILE.

## Exceptions

UC-CON-IMPORT-CONTACTS reports rejected row numbers and reasons in the product; downloadable correction files and re-import are outside that create-only profile.
