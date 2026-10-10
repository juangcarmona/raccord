---
id: BR-CRW-MINIMUM-PAID-HOURS-CHECK
type: business-rule
title: "Registrations below minimum paid hours are flagged"
status: draft
applies-to:
  - "UC-CRW-APPROVE-TIME-REGISTRATIONS"
  - "UC-CRW-CONFIGURE-TIME-REGISTRATION"
uses-terms:
  - "TERM-MINIMUM-PAID-HOURS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When minimum paid hours are configured, a registration with fewer hours is flagged in the approval check; without configuration this signal is absent.

## Rationale

Some jurisdictions require minimum payment per job.

## Examples

Minimum 4, registered 3: flagged.

## Exceptions

None.
