---
id: "BR-DOC-COMMUNICATION-ACCESS"
type: business-rule
title: "Communication history is always present but permission-filtered"
status: draft
applies-to:
  - "UC-DOC-REVIEW-COMMUNICATION-LOG"
  - "UC-DOC-REVIEW-SENT-EMAILS"
  - "UC-DOC-REVIEW-RECIPIENT-COMMUNICATIONS"
uses-terms:
  - "TERM-COMMUNICATION-LOG"
  - "TERM-USER-ROLE"
  - "TERM-ROLE-PERMISSION"
provenance:
  source: ".private/research/help-center/articles/29923797058194.md, .private/research/help-center/articles/360014199640.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The communication history is always kept by the product; which records a user may see depends on the user's role permissions, and by default users see the messages they sent themselves.

## Rationale

Messages can contain personal and commercial information.

## Examples

See SB-DOC-LOG-SHOWS-OTHER-USERS-MESSAGES and SB-DOC-OWN-MESSAGES-VISIBLE.

## Exceptions

None.
