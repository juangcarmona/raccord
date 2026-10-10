---
id: JRN-PRJ-DELIVER-PRODUCTION-WORK
type: journey
title: "Plan, deliver and charge pre- and post-production work"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
steps:
  - use-case: "UC-PRJ-CONFIGURE-TASK-STATUSES"
  - use-case: "UC-PRJ-MANAGE-TEAMS"
  - use-case: "UC-PRJ-MANAGE-SERVICES"
  - use-case: "UC-PRJ-ADD-TASK"
  - use-case: "UC-PRJ-PROGRESS-TASK"
  - use-case: "UC-PRJ-TRACK-TASK-TIME"
  - use-case: "UC-PRJ-REPORT-TASK-TIME"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md, .private/research/help-center/articles/38421818533522.md, .private/research/help-center/articles/38423504883858.md, .private/research/help-center/articles/38423178380050.md, .private/research/help-center/articles/38423389508242.md"
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

Production work is sold as a service, executed as tasks, measured in hours and compared with the estimate.

## Entry Conditions

Task workflow and quoting capability are available.

## Journey Narrative

The administrator sets up task statuses and teams. The coordinator adds a service (often a default service that creates its tasks) to the project, links or creates tasks with planned time, deadline and assignee, and shares the project board. Team members move tasks through statuses and track their time; the service breakdown and exports compare planned with actual hours, informing the next quote.

## Variants and Branches

- Work not sold: plain tasks without service, hours stay internal cost.
- External collaborator: invited to the project board.

## Completion Conditions

Tasks completed, hours approved and the service invoiced with known margin.
