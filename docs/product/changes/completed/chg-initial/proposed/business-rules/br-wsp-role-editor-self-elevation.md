---
id: BR-WSP-ROLE-EDITOR-SELF-ELEVATION
type: business-rule
title: "Role editors can extend their own role"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-ROLE-PERMISSION"
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md, .private/research/help-center/articles/360012611160.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Users allowed to create and edit user roles can add rights to any role, including their own; users allowed to change user roles can change other users' and their own role.

## Rationale

Role administration is a trusted capability and should be granted sparingly.

## Examples

- An office manager with role editing adds invoice deletion to the office role they hold.

## Exceptions

None.
