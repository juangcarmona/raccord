---
id: "CON-DOC-EXTERNAL-MAIL-DELIVERY"
type: constraint
title: "E-mail delivery depends on the mail account and recipients' servers"
status: draft
applies-to:
  - "UC-DOC-EMAIL-DOCUMENT"
  - "UC-DOC-RECORD-DELIVERY-OUTCOME"
  - "UC-DOC-INVESTIGATE-DELIVERY-FAILURE"
uses-terms:
  - "TERM-OUTGOING-EMAIL"
provenance:
  source: ".private/research/help-center/articles/21646575802258.md, .private/research/help-center/articles/360014199640.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

Outgoing e-mail is sent through the company's own mail account and accepted or refused by recipients' mail servers, whose availability, sending limits and spam scoring the product does not control.

## Rationale

Internet mail is a federated system.

## Consequences

Delays, throttling, outages and spam filing must be surfaced as status and reasons, not prevented; users resolve recipient-side issues with the recipient.
