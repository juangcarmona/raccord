---
id: FR-CRW-CREW-DOCUMENTS
type: functional-requirement
title: "Keep crew documents"
status: draft
derived-from:
  - "UC-CRW-MANAGE-CREW-DOCUMENTS"
  - "UC-CRW-GENERATE-CREW-AGREEMENT"
  - "BR-CRW-DOCUMENT-NAME-REQUIRED"
  - "BR-CRW-SIGNATURE-STATUS-ASSIGNMENT"
  - "BR-CRW-DOCUMENT-ACCESS-PERMISSION"
verification:
  - scenario-ref: "SB-CRW-UPLOADED-DOCUMENT-NOT-REQUESTED"
  - scenario-ref: "SB-CRW-GENERATED-AGREEMENT-SIGNED"
uses-terms:
  - "TERM-CREW-DOCUMENT"
provenance:
  source: ".private/research/help-center/articles/26200585082258.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST store named, typed, dated multi-file crew documents, generate agreements with optional digital signing, track signature status and restrict access by permission.

## Rationale

Compliance and qualification checks.
