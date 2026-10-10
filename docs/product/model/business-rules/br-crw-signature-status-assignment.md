---
id: BR-CRW-SIGNATURE-STATUS-ASSIGNMENT
type: business-rule
title: "Signature status follows how a document was created"
status: draft
applies-to:
  - "UC-CRW-MANAGE-CREW-DOCUMENTS"
  - "UC-CRW-GENERATE-CREW-AGREEMENT"
uses-terms:
  - "TERM-SIGNATURE-STATUS"
provenance:
  source: ".private/research/help-center/articles/26200585082258.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

An uploaded crew document gets signature status not requested; a document generated with digital signing gets requested and becomes signed when the crew member signs it digitally.

## Rationale

Shows which agreements are still awaiting signature.

## Examples

A generated freelance agreement sent for signing shows requested until signed.

## Exceptions

None.
