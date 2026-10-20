---
id: UC-PRJ-ADD-NOTE
type: use-case
title: "Add a note to a project or record"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-NOTE-DEFAULT-VISIBILITY"
  - "BR-PRJ-NOTE-DISPLAY"
  - "BR-PRJ-NOTE-WITHOUT-EDIT-RIGHTS"
uses-terms:
  - "TERM-PROJECT-NOTE"
  - "TERM-NOTE-VISIBILITY"
  - "TERM-NOTE-DISPLAY-TARGET"
provenance:
  source: ".private/research/help-center/articles/360013763199.md, .private/research/help-center/articles/360013628759.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Information is kept with the record and shown where it is needed.

## Trigger

Instructions, dress codes, customer feedback or e-mail content must be recorded.

## Preconditions

The user can edit the record, or the user's role allows adding notes without edit rights.

## Main Flow

1. The user adds a note with title and text.
2. The user chooses visibility (everyone or account managers only) and display targets.
3. The product stores the note and shows it at the chosen targets.

## Alternative Flows

- The note comes from a template or from the client's automatic project note.

## Failure Conditions

- A note without text is refused with the reason and nothing is stored.
- A user without edit rights who is not allowed to add notes is refused (BR-PRJ-NOTE-WITHOUT-EDIT-RIGHTS).
- A note with no chosen visibility stays visible only to account managers (BR-PRJ-NOTE-DEFAULT-VISIBILITY).
- If another user changed the record in the meantime, the note is refused and the user must reload and retry.

## Postconditions

The note is attached.
