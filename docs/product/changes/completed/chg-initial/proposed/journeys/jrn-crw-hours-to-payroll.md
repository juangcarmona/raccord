---
id: JRN-CRW-HOURS-TO-PAYROLL
type: journey
title: "From registered hours to payroll"
status: draft
primary-actor: "ACT-CREW-PLANNER"
steps:
  - use-case: "UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES"
  - use-case: "UC-CRW-APPROVE-TIME-REGISTRATIONS"
  - use-case: "UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME"
  - use-case: "UC-CRW-EXPORT-PAYROLL-DATA"
provenance:
  source: ".private/research/help-center/articles/360015498439.md, .private/research/help-center/articles/27336532785298.md, .private/research/help-center/articles/23814779870610.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intended Outcome

Approved, reconciled hours are exported to payroll.

## Entry Conditions

Crew worked on functions or shifts; time registration is configured.

## Journey Narrative

Crew register hours against planned activities, the planner reviews approval signals and approves, compares plan and reality and exports for payroll.

## Variants and Branches

- Manual or clocked registrations.
- An approval is reverted to correct hours.
- Freelancer invoice references recorded.

## Completion Conditions

Export produced with no pending registrations.
