---
id: SB-INT-FINALIZED-NOT-DELETABLE
type: structured-behaviour
title: "Finalized invoice cannot be deleted"
status: draft
illustrates:
  - "BR-ISSUED-INVOICE-IMMUTABLE"
given:
  - "reporting is connected"
when: "a user tries to delete or edit a finalized invoice"
then:
  - "deletion and editing are not offered"
  - "the user corrects it with a credit note"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/27697046471314.md, .private/research/help-center/articles/21998749265938.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Confirms immutability once finalized.

## Boundaries

The source applies this only when reporting is enabled; the model applies it always (see conflict report).
