---
id: BR-EQP-AUTO-SCAN-CODE
type: business-rule
title: "Scan codes generated when not supplied"
status: draft
applies-to:
  - "UC-EQP-IMPORT-EQUIPMENT"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/24338169105298.md, .private/research/help-center/articles/24320695034642.md, .private/research/help-center/articles/360013086279.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When an item is created (manually or by import) without a scan code, the product generates one; codes supplied by the user, such as existing barcodes, are kept.

## Rationale

Every scannable record needs a code; companies with labels already in use keep them.

## Examples

- Importing 30 cables without a code column gives the cable item a generated code.

## Exceptions

None.
