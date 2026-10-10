---
id: JRN-PRJ-RUN-RENTAL-PROJECT
type: journey
title: "Run a rental project from first contact to return"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
steps:
  - use-case: "UC-PRJ-CREATE-FROM-TEMPLATE"
  - use-case: "UC-PRJ-EDIT-PROJECT-DETAILS"
  - use-case: "UC-PRJ-MANAGE-TIME-SCHEDULE"
  - use-case: "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
  - use-case: "UC-DOC-GENERATE-DOCUMENT"
  - use-case: "UC-PRJ-CHANGE-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/14829964454546.md, .private/research/help-center/articles/13724369155218.md, .private/research/help-center/articles/13793720770194.md, .private/research/help-center/articles/29017857097618.md, .private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013590720.md"
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

A customer's event or rental is planned, agreed, delivered and brought back, with the project status reflecting each stage.

## Entry Conditions

A customer asks for equipment, crew or both; the customer and venue are (or can be) in the contact database.

## Journey Narrative

The coordinator creates the project (from a template or blank), fills in client, contact person, location, type and account manager, and sets up the time schedule with planning and usage periods and further phases. Equipment groups and crew functions are planned against the periods (other areas). A quotation is sent, making the project Pending; the customer signs it digitally, making it Confirmed. The warehouse prepares, sends out and receives the equipment per packing list; the first booking makes the project In progress and the last return makes it Execution done, after which the team marks it Complete. Packing slips and call sheets are produced along the way. Invoicing follows its own invoice moments.

## Variants and Branches

- Crew-only project: no equipment groups; functions are created from periods.
- Equipment-only (dry hire) project: no crew; customer picks up and returns.
- Dates change: UC-PRJ-SHIFT-PROJECT-TIMES.
- Customer withdraws: UC-PRJ-CANCEL-PROJECT.
- Quote agreed by phone: status set manually.

## Completion Conditions

The project is Complete and its invoice moments are complete; the project remains in statistics.
