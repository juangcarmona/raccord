---
id: FR-PROJECT-001
type: functional-requirement
title: "Create projects for customers and periods"
status: draft
derived-from:
  - "UC-CREATE-PROJECT"
verification:
  - scenario: "A project whose usage period starts before its planning period is refused"
uses-terms:
  - "TERM-PROJECT"
---

## Requirement

The product MUST let the coordinator create a project with a name, an existing customer, a planning period and a usage period that lies within the planning period.

## Rationale

Every billable action happens in the context of a job.
