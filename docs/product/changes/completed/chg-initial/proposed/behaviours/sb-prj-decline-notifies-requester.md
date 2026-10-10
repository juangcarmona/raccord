---
id: SB-PRJ-DECLINE-NOTIFIES-REQUESTER
type: structured-behaviour
title: "Declining a request notifies the requester"
status: draft
illustrates:
  - "BR-PRJ-REQUEST-DECLINE-NOTICE"
  - "UC-PRJ-DECLINE-PROJECT-REQUEST"
given:
  - "A request is Received"
when: "The coordinator declines it"
then:
  - "The request becomes Declined"
  - "The staff member who created the request receives an e-mail"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/4403455031186.md"
  confidence: high
  recovered-from: documentation
---

## Intent

The person who raised the request learns the outcome by e-mail.

## Boundaries

None.
