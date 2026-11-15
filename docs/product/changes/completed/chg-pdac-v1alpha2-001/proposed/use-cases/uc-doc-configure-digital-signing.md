---
id: "UC-DOC-CONFIGURE-DIGITAL-SIGNING"
type: use-case
title: "Configure digital signing of documents"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-SIGNING-REQUIRES-ENABLEMENT"
  - "BR-DOC-SIGNING-COMMENT"
  - "BR-DOC-SIGNED-COPY-STORED"
  - "BR-FIN-SIGN-AFTER-EXPIRY"
  - "BR-PRJ-SIGNED-QUOTE-CONFIRMED"
uses-terms:
  - "TERM-DIGITAL-SIGNING-ELEMENT"
provenance:
  source: ".private/research/help-center/articles/360014193840.md, .private/research/help-center/articles/360014232559.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Recipients can sign documents digitally as the company intends.

## Trigger

The company wants signed quotations or slips.

## Preconditions

None.

## Main Flow

1. The administrator allows customers to sign digitally.
2. The administrator uploads the signing page logo and decides whether signers may comment and whether expired documents may be signed.
3. The administrator enables the signing element for the document types that customers may sign.

## Alternative Flows

None.

## Failure Conditions

With signing disallowed, signing elements are not usable.

## Postconditions

Documents generated with those templates can be signed; signed copies are stored with the project and signing a quotation confirms the project.
