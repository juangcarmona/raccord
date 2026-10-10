---
id: BR-CRW-ACCOUNT-INVITATION-ON-CREATION
type: business-rule
title: "New crew members with login permission receive an account invitation"
status: draft
applies-to:
  - "UC-CRW-ADD-CREW-MEMBER"
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-MANAGE-CREW-LOGIN-ACCESS"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014366299.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Adding or importing a crew member whose login is allowed automatically sends them an email inviting them to set up their account; an import row whose login permission is off sends nothing.

## Rationale

Crew members must be able to see their own schedule; suppressing emails lets a company set up its crew records before contacting anyone.

## Examples

An import of 40 people with login permission off sends no email; enabling login later in bulk sends the invitations then.

## Exceptions

An agency crew record without email login is not invited.
