---
id: BR-WHS-DELAY-EXTENDS-PLANNING-ONLY
type: business-rule
title: "Marking delayed extends planning, not usage"
status: draft
applies-to:
  - "UC-WHS-MARK-DELAYED"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-DELAYED-EQUIPMENT"
  - "TERM-EXPECTED-RETURN-DATE"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Marking equipment groups delayed sets the planning period end of their equipment still out to the expected return date, keeping it unavailable until then; it does not change usage periods, so the customer is not charged for the delay unless the planner changes usage or adds a cost.

## Rationale

Availability must be truthful; charging is a commercial decision.

## Examples

See SB-WHS-MARK-DELAYED-EXTENDS.

## Exceptions

None.
