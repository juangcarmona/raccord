---
id: "UC-DOC-REVIEW-RECIPIENT-COMMUNICATIONS"
type: use-case
title: "Review communications sent to a contact or crew member"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-STATUS-EMAIL-ONLY"
  - "BR-DOC-COMMUNICATION-ACCESS"
uses-terms:
  - "TERM-COMMUNICATION-RECORD"
provenance:
  source: ".private/research/help-center/articles/29923797058194.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user sees all communications sent to one person.

## Trigger

The user needs to confirm a crew member got the planning or a customer got an invoice.

## Preconditions

The contact or crew member exists.

## Main Flow

1. The user opens the person's record.
2. The product lists communications sent to the person with subject, status (e-mails only), message start and date.
3. The user views the full message.

## Alternative Flows

None.

## Failure Conditions

- A user whose role does not permit seeing the records sees only the messages they sent themselves, so the list may be shorter than the recipient's full history (BR-DOC-COMMUNICATION-ACCESS).
- A person with no communications shows an empty list.
- In-app notifications show no delivery status (BR-DOC-STATUS-EMAIL-ONLY).

## Postconditions

Nothing changes.
