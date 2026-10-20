---
id: UC-PRJ-EDIT-PROJECT-DETAILS
type: use-case
title: "Maintain project details"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-PROJECT-NUMBER-ASSIGNMENT"
  - "BR-PRJ-ACCOUNT-MANAGER-POWER-USER"
  - "BR-PRJ-EXTERNAL-REFERENCE-PRINTING"
  - "BR-PRJ-PARTIES-FROM-CONTACTS"
  - "BR-PRJ-CONTACT-SELECTION-EFFECTS"
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
  - "BR-PRJ-PROJECT-TYPE-DEFAULTS"
uses-terms:
  - "TERM-PROJECT-NUMBER"
  - "TERM-ACCOUNT-MANAGER"
  - "TERM-PROJECT-EXTERNAL-REFERENCE"
  - "TERM-PROJECT-LOCATION"
  - "TERM-PROJECT-TIME-ZONE"
  - "TERM-CUSTOMER"
  - "TERM-CONTACT-PERSON"
provenance:
  source: ".private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md, .private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360013677019.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project's identifying, commercial and logistic details are correct.

## Trigger

A project has been created or its circumstances change.

## Preconditions

The project exists.

## Main Flow

1. The coordinator sets the name, project type, number (or leaves it empty for automatic numbering), account manager, colour, external reference and (if enabled) time zone.
2. The coordinator chooses the client and contact person from the contact database; the product fills in the client's address, default contact person, warning and automatic project note.
3. The coordinator chooses the location; the product shows it on a map with distance and travel time from the warehouse.
4. The coordinator adds custom field values, notes, tasks and files as needed.
5. The product saves and records the change in the project history.

## Alternative Flows

- Client or location not yet in the database: the coordinator creates the contact inline and continues at step 2 or 3.
None.

## Failure Conditions

A number already used by another project is refused.

## Postconditions

The project's details are saved and appear on overviews and generated documents.
