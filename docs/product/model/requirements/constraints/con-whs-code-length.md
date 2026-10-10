---
id: CON-WHS-CODE-LENGTH
type: constraint
title: "Scan codes are limited to 25 characters"
status: draft
applies-to:
  - "UC-WHS-ASSIGN-SCAN-CODES"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/360013101260.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Scan codes hold at most 25 characters, spaces included; longer codes are truncated. Printed QR labels can be as small as 7 by 7 millimetres.

## Rationale

Fixed product limit on code storage; QR format allows small labels.

## Consequences

Labels must be chosen within 25 characters; truncated codes may collide.
