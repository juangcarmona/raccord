---
id: FR-CRW-TIME-REGISTRATION
type: functional-requirement
title: "Register worked time"
status: draft
derived-from:
  - "UC-CRW-REGISTER-HOURS-MANUALLY"
  - "UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES"
  - "UC-CRW-CLOCK-IN-AND-OUT"
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
  - "BR-CRW-DAYS-BACK-LIMIT"
  - "BR-CRW-BREAK-AND-TRAVEL-EXCLUDED"
  - "BR-CRW-HOUR-TYPE-TOTALS"
verification:
  - scenario-ref: "SB-CRW-WORKED-HOURS-NET-OF-BREAK-AND-TRAVEL"
  - scenario-ref: "SB-CRW-COMPENSATION-NOT-WORKED"
  - scenario-ref: "SB-CRW-HOURS-WITHOUT-ACTIVITIES"
uses-terms:
  - "TERM-TIME-REGISTRATION"
provenance:
  source: ".private/research/help-center/articles/360015498439.md, .private/research/help-center/articles/360015498839.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let authorised users register time manually, from planned activities or by clocking in, with break, travel, hour type, distance, lunch and activities linked to functions, shifts or free text, within the role's rights and days-back limit.

## Rationale

Hours drive pay and actual cost.
