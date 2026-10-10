---
id: BR-WSP-ONE-ROLE-PER-USER
type: business-rule
title: "Each user has exactly one role, applied everywhere"
status: draft
applies-to:
  - "BC-WORKSPACE"
  - "UC-WSP-ASSIGN-ROLE"
uses-terms:
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/360014360700.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every user account holds exactly one user role, and the same permissions apply in the web product, in the mobile web app and in calendar feeds.

## Rationale

One consistent access model avoids leaks through secondary channels.

## Examples

- A crew-role user sees in the mobile web app only the projects they are planned on, as on the web.

## Exceptions

None.
