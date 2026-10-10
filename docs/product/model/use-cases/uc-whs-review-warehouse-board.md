---
id: UC-WHS-REVIEW-WAREHOUSE-BOARD
type: use-case
title: "Review the warehouse board for a day"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-BOARD-DUE-DATE"
  - "BR-WHS-PACKING-LIST-PER-DATE"
  - "BR-WHS-READY-TO-PACK-VISIBILITY"
  - "BR-WHS-DELAYED-AUTOMATIC"
  - "BR-WHS-STATUS-INDEPENDENCE"
uses-terms:
  - "TERM-PACKING-LIST"
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-WAREHOUSE-DUE-DATE"
  - "TERM-WAREHOUSE-PROGRESS"
  - "TERM-DELAYED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360014458519.md, .private/research/help-center/articles/15125682136850.md, .private/research/help-center/articles/15125679219858.md, .private/research/help-center/articles/15239125718546.md, .private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The crew sees what to pack, what is out, what comes back and what is late.

## Trigger

Start of a warehouse shift.

## Preconditions

None.

## Main Flow

1. The technician opens the board for today or another date, optionally filtered by saved filters and search.
2. The product shows packing lists in columns to pack, prepped (and custom statuses), on location or expected back, and delayed, as cards or a list.
3. Each card shows name, number, date, a scannable code, shortage indicator, sync colour, notes and actions (book, change status, print documents, assign crew).
4. Alongside, the product shows that day's departing and returning vehicles and scheduled crew.

## Alternative Flows

- List view shows counts per equipment status for each list.

## Failure Conditions

- Packing lists of a project not marked ready to pack do not appear, except for users whose role grants visibility of all projects (BR-WHS-READY-TO-PACK-VISIBILITY).
- A user cannot set a list as delayed manually; the request is refused (BR-WHS-DELAYED-AUTOMATIC).
- A date with no lists shows an empty board.
- Changing a status that conflicts with a concurrent change by another user is refused and the technician must reload and retry (BR-WHS-STATUS-INDEPENDENCE).

## Postconditions

Nothing changes until a booking starts.
