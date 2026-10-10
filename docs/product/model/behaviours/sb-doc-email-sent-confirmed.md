---
id: "SB-DOC-EMAIL-SENT-CONFIRMED"
type: structured-behaviour
title: "Accepted e-mail becomes sent"
status: draft
illustrates:
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
  - "UC-DOC-RECORD-DELIVERY-OUTCOME"
given:
  - "An invoice e-mail is pending"
when: "The company's mail account confirms acceptance"
then:
  - "The e-mail's status becomes sent"
  - "The sent time is visible on the customer's communication list"
uses-terms:
  - "TERM-EMAIL-SENT"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/29923797058194.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Users can tell customers when a document arrived.

## Boundaries

None.
