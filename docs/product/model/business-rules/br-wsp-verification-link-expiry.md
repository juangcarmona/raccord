---
id: BR-WSP-VERIFICATION-LINK-EXPIRY
type: business-rule
title: "Verification links expire after 24 hours"
status: draft
applies-to: []
uses-terms:
  - "TERM-VERIFICATION-LINK"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36291020702098.md"
  confidence: high
  recovered-from: documentation
---

## Rule

An email verification link is valid for 24 hours after it is sent; afterwards it is refused and the person must restart verification to receive a new link.

## Rationale

Limits the window in which an intercepted link can be abused.

## Examples

- A link opened 30 hours after sending is refused; starting the sign-in again sends a fresh one.

## Exceptions

None.
