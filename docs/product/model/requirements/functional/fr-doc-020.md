---
id: "FR-DOC-020"
type: functional-requirement
title: "Keep a permission-filtered communication history"
status: draft
derived-from:
  - "UC-DOC-REVIEW-COMMUNICATION-LOG"
  - "UC-DOC-REVIEW-SENT-EMAILS"
  - "UC-DOC-REVIEW-RECIPIENT-COMMUNICATIONS"
  - "BR-DOC-COMMUNICATION-ACCESS"
  - "BR-DOC-LOG-SCOPE"
  - "BR-DOC-STATUS-EMAIL-ONLY"
verification:
  - scenario-ref: "SB-DOC-LOG-SHOWS-OTHER-USERS-MESSAGES"
  - scenario-ref: "SB-DOC-OWN-MESSAGES-VISIBLE"
  - scenario-ref: "SB-DOC-NOTIFICATION-HAS-NO-STATUS"
uses-terms:
  - "TERM-COMMUNICATION-LOG"
  - "TERM-COMMUNICATION-RECORD"
provenance:
  source: ".private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/360014199640.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST log every outgoing communication with recipient, type, channel, subject, content, date and (for e-mail) status, filterable by recipient, type, channel and period, viewable per recipient, and limited by permissions.

## Rationale

Answering 'was it sent?' instantly.
