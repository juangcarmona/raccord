---
id: BR-INT-INTEGRATIONS-NEED-CONFIGURATION-ACCESS
type: business-rule
title: "Only configuration users manage integrations"
status: draft
applies-to:
  - "UC-INT-ENABLE-API-ACCESS"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-INTEGRATION"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/15274709111826.md, .private/research/help-center/articles/35485155508370.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

Connecting, configuring, revealing credentials of and disconnecting integrations is reserved to users whose role gives access to the workspace configuration.

## Rationale

Credentials give broad access to company data.

## Examples

A warehouse technician cannot see the API token.

## Exceptions

None.
