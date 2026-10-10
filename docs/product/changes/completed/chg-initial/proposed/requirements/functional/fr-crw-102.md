---
id: FR-CRW-102
type: functional-requirement
title: "Configure time registration policy"
status: draft
derived-from:
  - "UC-CRW-CONFIGURE-TIME-REGISTRATION"
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
  - "BR-CRW-MINIMUM-PAID-HOURS-CHECK"
verification:
  - id: "TIME-REGISTRATION-POLICY"
    scenario: "Given a role that may register only its own hours and a default break of 30 minutes, when a member of that role registers hours for a colleague the product refuses, and when the member registers own hours the default break is applied."
  - id: "MINIMUM-PAID-HOURS"
    scenario: "Given a minimum number of paid hours and approval enabled on the worked hour type, when registered hours fall below the minimum the product flags them and requires approval before they count."
uses-terms:
  - "TERM-TIME-REGISTRATION-RIGHTS"
  - "TERM-MINIMUM-PAID-HOURS"
provenance:
  source: ".private/research/help-center/articles/360015498839.md, .private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let an administrator define which roles may register their own and other people's hours, pre-register hours, use activities and clock in from the mobile web app, and MUST enforce those rights. The product MUST let the administrator set how many days back hours may be registered and a default break, and MUST allow an optional minimum of paid hours and approval on the worked hour type, applying all settings to subsequent registrations.

## Rationale

Each company has its own policy on who records time and how, and the product must follow it rather than impose one.
