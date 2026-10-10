---
id: "SB-DOC-QUOTE-SIGNED-CONFIRMS-PROJECT"
type: structured-behaviour
title: "Signing a quotation stores it and confirms the project"
status: draft
illustrates:
  - "BR-DOC-SIGNED-COPY-STORED"
  - "UC-DOC-CONFIGURE-DIGITAL-SIGNING"
given:
  - "A project is pending"
  - "Its quotation was sent with a signing element"
when: "The customer signs the quotation on the signing page"
then:
  - "The signed quotation is stored in the project's files"
  - "The project status changes from pending to confirmed"
uses-terms:
  - "TERM-DIGITAL-SIGNING-ELEMENT"
provenance:
  source: ".private/research/help-center/articles/360014193840.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Signature closes the commercial loop.

## Boundaries

None.
