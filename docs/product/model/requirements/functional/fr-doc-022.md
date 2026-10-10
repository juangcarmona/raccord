---
id: "FR-DOC-022"
type: functional-requirement
title: "Configure digital signing of documents"
status: draft
derived-from:
  - "UC-DOC-CONFIGURE-DIGITAL-SIGNING"
  - "BR-DOC-SIGNING-REQUIRES-ENABLEMENT"
  - "BR-DOC-SIGNING-COMMENT"
  - "BR-DOC-SIGNED-COPY-STORED"
verification:
  - scenario-ref: "SB-DOC-QUOTE-SIGNED-CONFIRMS-PROJECT"
  - scenario-ref: "SB-DOC-SIGNING-COMMENT-DISABLED"
uses-terms:
  - "TERM-DIGITAL-SIGNING-ELEMENT"
provenance:
  source: ".private/research/help-center/articles/360014193840.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST provide a signing element for signable documents, governed by company settings for enabling, logo, comments and signing after expiry, and MUST store signed copies with the project.

## Rationale

Customers approve without printing.
