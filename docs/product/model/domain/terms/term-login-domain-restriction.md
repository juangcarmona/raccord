---
id: TERM-LOGIN-DOMAIN-RESTRICTION
type: domain-term
title: "Login domain restriction"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The set of email domains accepted for users of an SSO-enforced role; a user complies only when the domain of their verified email address is in the set. Several domains can be listed.

## Distinguish From

Not the mail account the company sends email from.

## Usage

Part of single sign-on enforcement; listed non-compliant users must be fixed before they can sign in.
