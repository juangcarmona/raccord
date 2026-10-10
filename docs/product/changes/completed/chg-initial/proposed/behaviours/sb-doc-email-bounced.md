---
id: "SB-DOC-EMAIL-BOUNCED"
type: structured-behaviour
title: "Rejected e-mail becomes delivery failed"
status: draft
illustrates:
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
  - "UC-DOC-RECORD-DELIVERY-OUTCOME"
given:
  - "A quotation e-mail is pending"
when: "The recipient's server rejects the message because the address does not exist"
then:
  - "The e-mail's status becomes delivery failed"
  - "The rejection reason is shown with the message"
uses-terms:
  - "TERM-EMAIL-DELIVERY-FAILED"
provenance:
  source: ".private/research/help-center/articles/360014199640.md, .private/research/help-center/articles/21646575802258.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Failures are visible with their cause.

## Boundaries

None.
