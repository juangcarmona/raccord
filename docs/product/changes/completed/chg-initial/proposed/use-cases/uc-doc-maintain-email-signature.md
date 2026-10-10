---
id: "UC-DOC-MAINTAIN-EMAIL-SIGNATURE"
type: use-case
title: "Maintain a personal e-mail signature"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-SIGNATURE-APPENDED"
uses-terms:
  - "TERM-EMAIL-SIGNATURE"
provenance:
  source: ".private/research/help-center/articles/360014259799.md, .private/research/help-center/articles/360014391919.md, .private/research/help-center/articles/360014405639.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user's e-mails end with personal and company details.

## Trigger

The user joins or details change.

## Preconditions

The user is signed in.

## Main Flow

1. The user opens the communication settings.
2. The user writes the signature with text and images.
3. The user saves.

## Alternative Flows

None.

## Failure Conditions

- Invalid content, such as an unsupported image, is refused with the reason and the previous signature remains.
- A user cannot change another user's signature; the attempt is refused and nothing changes.
- If the signature was changed elsewhere in the meantime, the save is refused and the user must reload and retry.

## Postconditions

Subsequent e-mails of the user carry the signature.
