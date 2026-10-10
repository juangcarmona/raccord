---
id: "SB-DOC-EMAIL-ACCEPTED-BUT-SPAM"
type: structured-behaviour
title: "Spam filing does not change the status"
status: draft
illustrates:
  - "BR-DOC-DELIVERY-STATUS-TRANSITIONS"
  - "UC-DOC-INVESTIGATE-DELIVERY-FAILURE"
given:
  - "An e-mail's status is sent"
when: "The recipient's mailbox files the accepted message as spam"
then:
  - "The status remains sent"
  - "The user must ask the recipient to check the spam folder or allow the sender"
uses-terms:
  - "TERM-EMAIL-SENT"
provenance:
  source: ".private/research/help-center/articles/21646575802258.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Explains a common support case.

## Boundaries

None.
