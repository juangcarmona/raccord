---
id: BR-CRW-RECURRING-EDIT-SCOPE
type: business-rule
title: "Changes to a recurring item ask which occurrences they affect"
status: draft
applies-to:
  - "UC-CRW-EDIT-APPOINTMENT"
  - "UC-CRW-EDIT-SHIFT"
uses-terms:
  - "TERM-RECURRENCE-PATTERN"
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/23976860498834.md, .private/research/help-center/articles/17297296257426.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Editing or deleting an occurrence of a recurring appointment or shift asks which occurrences the change applies to; start and end times cannot be changed for several occurrences at once; deleting all occurrences of a shift removes past ones too.

## Rationale

Series often change from a point onwards.

## Examples

See SB-CRW-DELETE-ALL-RECURRING-SHIFTS.

## Exceptions

None.
