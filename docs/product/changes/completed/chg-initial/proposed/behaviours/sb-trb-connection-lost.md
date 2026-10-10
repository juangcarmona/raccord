---
id: "SB-TRB-CONNECTION-LOST"
type: structured-behaviour
title: "Blocked connection triggers the lost-connection warning"
status: draft
illustrates:
  - "BR-TRB-CONNECTION-LOST-WARNING"
  - "CON-TRB-LIVE-CONNECTION-NETWORK"
given:
  - "The user's company VPN blocks the live-update endpoint"
when: "The live update connection cannot be maintained"
then:
  - "The user is warned that the connection was lost and updates may have been missed"
  - "A refresh is recommended"
  - "Colleagues' changes are not shown until the user refreshes"
uses-terms:
  - "TERM-LIVE-UPDATE-CONNECTION"
provenance:
  source: ".private/research/help-center/articles/360013988420.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Visible degradation instead of silent staleness.

## Boundaries

None.
