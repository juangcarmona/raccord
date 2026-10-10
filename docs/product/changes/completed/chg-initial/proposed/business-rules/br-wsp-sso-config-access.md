---
id: BR-WSP-SSO-CONFIG-ACCESS
type: business-rule
title: "Only configuration users may enforce single sign-on"
status: draft
applies-to:
  - "UC-WSP-ENFORCE-SSO"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Only users whose role grants access to configuration can enable, change or disable single sign-on enforcement.

## Rationale

A security policy affecting everyone's access must be controlled by administrators.

## Examples

- A planner without configuration access cannot see the security settings.

## Exceptions

None.
