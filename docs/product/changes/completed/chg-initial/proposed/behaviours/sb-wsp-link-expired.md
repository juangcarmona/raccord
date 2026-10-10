---
id: SB-WSP-LINK-EXPIRED
type: structured-behaviour
title: "Expired verification link is refused"
status: draft
illustrates:
  - "BR-WSP-VERIFICATION-LINK-EXPIRY"
given:
  - "A crew member started verification and received a link"
when: "The person opens the verification link 25 hours after it was sent"
then:
  - "The product refuses the link as expired"
  - "The account stays unverified"
  - "The person can restart verification to receive a new link"
uses-terms:
  - "TERM-VERIFICATION-LINK"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36291020702098.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Fixes the 24-hour validity window of verification links.

## Boundaries

None.
