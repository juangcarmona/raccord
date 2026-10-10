---
id: UC-INT-WRITE-RECORDS-THROUGH-API
type: use-case
title: "Create, update or delete records through the public interface"
status: draft
primary-actor: "ACT-EXTERNAL-APPLICATION"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
  - "BR-INT-API-COVERAGE-BOUNDARY"
  - "BR-INT-WAREHOUSE-STATUS-NOT-WRITABLE"
uses-terms:
  - "TERM-PUBLIC-API"
  - "TERM-API-RESOURCE"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/35485155508370.md, .private/research/help-center/articles/30759527977490.md, .private/research/help-center/articles/14042545177618.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An external application keeps workspace data up to date without a person typing it.

## Trigger

Something changed in the external system, such as a new lead or a supplier update.

## Preconditions

Valid token; the record kind offers the operation; the token owner's role allows it.

## Main Flow

1. The application sends the new or changed record, or a delete, for an offered resource.
2. The product validates it with the same rules as the user interface.
3. The product stores the change and records it in history.

## Alternative Flows

Child records are created under their parent, for example a task under a project.

## Failure Conditions

Operation not offered for that kind (for example creating a quotation or an invoice): refused. Validation fails: refused with the field and reason. Writing a warehouse status into a project status: refused.

## Postconditions

The change is stored as if made by the token owner.
