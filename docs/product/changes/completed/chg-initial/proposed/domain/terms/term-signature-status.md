---
id: TERM-SIGNATURE-STATUS
type: domain-term
title: "Crew document signature status"
status: draft
defined-in: "BC-CREW"
synonyms: []
uses-terms:
  - "TERM-CREW-DOCUMENT"
provenance:
  source: ".private/research/help-center/articles/26200585082258.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

Lifecycle of the digital signature of a crew document.

States: not requested (uploaded documents), requested (generated with digital signing and awaiting signature), signed (signature received).

Transitions: generated with signing -> requested; requested -> signed when the crew member signs. Uploaded documents start and stay at not requested.

## Distinguish From

Contract validity (start and end date): independent from signature.

## Usage

Following up agreements sent for signature.
