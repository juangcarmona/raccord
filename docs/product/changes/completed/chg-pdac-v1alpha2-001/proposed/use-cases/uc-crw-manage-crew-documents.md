---
id: UC-CRW-MANAGE-CREW-DOCUMENTS
type: use-case
title: "Upload and organise crew documents"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DOCUMENT-NAME-REQUIRED"
  - "BR-CRW-SIGNATURE-STATUS-ASSIGNMENT"
  - "BR-CRW-DOCUMENT-ACCESS-PERMISSION"
  - "BR-CRW-ACTIVE-CONTRACT-DERIVATION"
uses-terms:
  - "TERM-CREW-DOCUMENT"
  - "TERM-CREW-DOCUMENT-TYPE"
  - "TERM-ACTIVE-CONTRACT"
provenance:
  source: ".private/research/help-center/articles/26200585082258.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Personnel documents are stored per crew member by type with validity dates.

## Trigger

A contract, licence, certificate, insurance or ID must be kept.

## Preconditions

The user's role grants crew document access.

## Main Flow

1. The user selects a crew member.
2. The user uploads one or more files forming one document.
3. The user gives name, type, start and end date.
4. The product saves the document with signature status not requested.

## Alternative Flows

- Existing files of the crew member are copied into crew documents with name and type.
- The crew overview shows whether each person has an active contract and its end date.

## Failure Conditions

No name: refused. No permission: documents are not accessible.

## Postconditions

The document is listed under its type for the crew member.
