---
id: TERM-RESERVATION-STATE
type: domain-term
title: "Reservation state"
status: draft
defined-in: BC-EQUIPMENT
synonyms: []
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-SOFT-HOLD"
  - "TERM-INVALID-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360013836939.md, .private/research/help-center/articles/360012699859.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

The lifecycle of the reservation of a planned line (and, aggregated, of a project):

| State | Meaning |
| --- | --- |
| Unreserved (soft hold) | Planned, counted in the forecast, no stock secured. Default in inquiry, draft and pending projects. |
| Partially reserved | Some of the planned quantity is reserved; the rest is short or not yet reserved. |
| Fully reserved | The whole planned quantity is reserved. |
| Invalid | Reserved quantities are no longer backed by stock (loss, delay, booking out for another project). |
| Released | Reservation removed (project cancelled, early return, manual reallocation). |

Allowed transitions: Unreserved to Partially or Fully reserved (confirmation, manual reservation, booking out); Partially to Fully reserved (re-save after stock becomes free, manual reservation); Fully or Partially reserved to Invalid (stock loss, delayed returns elsewhere, booking out elsewhere); Invalid to Fully reserved (stock restored or reallocated) or to Released; any state to Released (cancellation, early return, reallocation); Released back to Unreserved when still planned.

## Distinguish From

Not the project status, which only sets the default (automatic reservation or not).

## Usage

Shown on the timeline as pending, not fully reserved and fully reserved bars, and in shortage explanations.
