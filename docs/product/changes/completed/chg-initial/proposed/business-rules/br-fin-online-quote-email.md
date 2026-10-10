---
id: BR-FIN-ONLINE-QUOTE-EMAIL
type: business-rule
title: "Online quote e-mails carry link and password, not the PDF"
status: draft
applies-to:
  - "UC-FIN-PUBLISH-ONLINE-QUOTE"
uses-terms:
  - "TERM-ONLINE-QUOTE"
provenance:
  source: ".private/research/help-center/articles/30841616039570.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The default e-mail for a published quote contains its link and password, filled in when sent, and no quote PDF is attached unless the user attaches it.

## Rationale

Avoids redundant attachments and version confusion.

## Examples

Sending the default message delivers the link and password only.

## Exceptions

None.
