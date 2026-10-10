---
id: UC-INT-CREATE-PROJECT-THROUGH-API
type: use-case
title: "Create a project through the public interface"
status: draft
primary-actor: "ACT-EXTERNAL-APPLICATION"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by: []
uses-terms:
  - "TERM-PROJECT"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---
## Goal
A project is set up from an external system with its function groups and functions.
## Trigger
A booking is confirmed in an external sales or event system.
## Preconditions
Valid token with project creation rights; the customer exists in the contact database.
## Main Flow
1. The application creates the project for the customer with the supported subset of fields.
2. The product creates the project.
3. The application adds function groups and functions to the project.
## Alternative Flows
None.
## Failure Conditions
Fields outside the supported subset are rejected or ignored; the capability may change between interface versions.
## Postconditions
The project exists with its customer.
