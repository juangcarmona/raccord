---
id: BR-WHS-NO-EARLY-CHECK-OUT
type: business-rule
title: "Equipment cannot be booked before its planning period"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-BOOK-BY-SCANNING"
uses-terms:
  - "TERM-PLANNING-PERIOD"
  - "TERM-CHECK-OUT"
provenance:
  source: ".private/research/help-center/articles/360012699859.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment cannot be booked out before the start of its planning period; to send it earlier, the planning period must be changed in the project first.

## Rationale

Availability is computed over the planning period only.

## Examples

Packing on Friday for a Monday pickup requires moving the planning period to Friday.

## Exceptions

None.
