---
id: SB-WSP-ACCOUNT-CREW-AMBIGUOUS
type: structured-behaviour
title: "Ambiguous or conflicting crew matches are not linked"
status: draft
illustrates:
  - "BR-WSP-ACCOUNT-CREW-ASSOCIATION"
  - "UC-WSP-LINK-EXISTING-CREW-RECORD"
given:
  - "A planner attempts to associate an authenticated profile with an existing crew record"
  - "The workspace has duplicate email addresses, no plausible record, multiple plausible records, or a record already linked to another profile"
when: "The product evaluates the requested association"
then:
  - "The product refuses to guess and creates no link"
  - "Neither profile nor crew record is merged or deleted"
  - "The planner is shown that manual resolution is required without exposing another person's profile data"
  - "The refusal is audited with the actor, time, workspace, affected record references and reason"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
  - "TERM-CREW-MEMBER"
---

## Intent

Ambiguous identity and conflicting associations require explicit resolution before access changes.

## Boundaries

Email equality is not accepted as sufficient proof, and the product does not select among multiple possible records.
