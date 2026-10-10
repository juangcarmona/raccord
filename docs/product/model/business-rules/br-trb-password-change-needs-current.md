---
id: "BR-TRB-PASSWORD-CHANGE-NEEDS-CURRENT"
type: business-rule
title: "Changing a known password requires the current one"
status: draft
applies-to:
  - "UC-WSP-MANAGE-PASSWORD"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014048499.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

A signed-in person changes their password by entering the current password together with the new one.

## Rationale

Prevents someone using an unattended session from taking over the account.

## Examples

See SB-TRB-PASSWORD-CHANGE-WRONG-CURRENT.

## Exceptions

Forgotten passwords are replaced through the reset link instead.
