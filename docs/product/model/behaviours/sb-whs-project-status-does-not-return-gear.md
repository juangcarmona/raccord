---
id: SB-WHS-PROJECT-STATUS-DOES-NOT-RETURN-GEAR
type: structured-behaviour
title: "Project status changes leave gear where it is"
status: draft
illustrates:
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "UC-WHS-MARK-DELAYED"
given:
  - "Equipment is on location"
when: "The project status is changed to a finished status"
then:
  - "The equipment is still on location"
  - "It stays unavailable until booked back"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Independence of statuses.

## Boundaries

None.
