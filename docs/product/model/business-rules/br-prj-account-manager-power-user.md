---
id: BR-PRJ-ACCOUNT-MANAGER-POWER-USER
type: business-rule
title: "Account managers hold the Admin or Office role"
status: draft
applies-to:
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
uses-terms:
  - "TERM-ACCOUNT-MANAGER"
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Only a user with the Admin or Office role can be chosen as a project's account manager.

## Rationale

Account managers need full project access.

## Examples

A user with the Crew role cannot be account manager.

## Exceptions

None.
