---
id: UC-WHS-MARK-DELAYED
type: use-case
title: "Mark equipment groups as delayed"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-DELAY-EXTENDS-PLANNING-ONLY"
  - "BR-WHS-DELAYED-AUTOMATIC"
  - "BR-WHS-INVALIDATION-ON-STOCK-CHANGE"
  - "BR-WHS-STATUS-INDEPENDENCE"
uses-terms:
  - "TERM-DELAYED-EQUIPMENT"
  - "TERM-EXPECTED-RETURN-DATE"
provenance:
  source: ".private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Availability reflects that gear will come back later.

## Trigger

A list appears delayed or the crew knows of a late return.

## Preconditions

Equipment is still out.

## Main Flow

1. The technician opens the delayed list and asks to mark groups delayed.
2. The technician enters the expected return date and time, selects groups and optionally a remark.
3. The product extends the planning period of their equipment still out to that moment.

## Alternative Flows

- Only some items late: mark the group and return the items that came back.
- Repeated when the new date passes.
- Entire project late: shift the project instead.

## Failure Conditions

- The extension creates shortages on later projects: shown there.

## Postconditions

Gear stays unavailable until the expected date; charges unchanged.
