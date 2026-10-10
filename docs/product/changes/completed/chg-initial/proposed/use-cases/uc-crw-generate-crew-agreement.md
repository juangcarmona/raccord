---
id: UC-CRW-GENERATE-CREW-AGREEMENT
type: use-case
title: "Generate a crew agreement for digital signing"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DOCUMENT-NAME-REQUIRED"
  - "BR-CRW-SIGNATURE-STATUS-ASSIGNMENT"
  - "BR-CRW-DOCUMENT-ACCESS-PERMISSION"
uses-terms:
  - "TERM-CREW-DOCUMENT"
  - "TERM-SIGNATURE-STATUS"
provenance:
  source: ".private/research/help-center/articles/26200585082258.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

An agreement is produced from crew data and signed remotely.

## Trigger

A contract must be created or renewed.

## Preconditions

The user has crew document access; the crew member's data is complete enough.

## Main Flow

1. The user generates an agreement document for the crew member with a name.
2. The user enables digital signing.
3. The product stores the document as requested and sends it to the crew member.
4. The crew member signs it; the status becomes signed.

## Alternative Flows

Generated without signing: stored as a document only.

## Failure Conditions

Not signed: stays requested.

## Postconditions

A contract document exists with its signature state.
