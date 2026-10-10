---
id: BR-WORKDAY-OPEN-DAY
type: business-rule
title: "An unclosed day must be corrected"
status: draft
applies-to:
  - "UC-CLOCK-WORKDAY"
  - "UC-CORRECT-WORKDAY-RECORD"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Rule

A day that is still Working or On break at the end of the calendar day becomes Open; the employee and their manager are notified, and the day only becomes Finished through a correction.

## Rationale

Forgotten clock-outs are the most common error and must not silently inflate hours.

## Examples

See SB-WORKDAY-FORGOTTEN-CLOCK-OUT.

## Exceptions

None.
