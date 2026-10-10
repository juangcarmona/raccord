---
id: BR-INT-TOKEN-INHERITS-OWNER-ROLE
type: business-rule
title: "A token can do what its owner's role can do"
status: draft
applies-to:
  - "UC-INT-READ-RECORDS-THROUGH-API"
  - "UC-INT-WRITE-RECORDS-THROUGH-API"
  - "UC-INT-ENABLE-API-ACCESS"
  - "UC-INT-REPORT-PAYMENT-THROUGH-API"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-API-TOKEN"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/35485155508370.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every public-interface call is authorised with the permissions of the role of the user who created the token, evaluated at the time of the call.

## Rationale

Integrations must never bypass the access model of the workspace.

## Examples

A token created by a user without financial rights cannot read invoices; changing that user's role changes what the token can do.

## Exceptions

None.
