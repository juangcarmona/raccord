---
id: FR-INT-005
type: functional-requirement
title: "Accept projects from integrations"
status: draft
derived-from:
  - "UC-INT-CREATE-PROJECT-THROUGH-API"
verification:
  - id: "PROJECT-CREATED-THROUGH-API"
    scenario: "Given a token whose owner may create projects, when an application creates a project for an existing customer with the supported fields, the project exists with that customer; fields outside the supported subset are rejected or ignored."
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/360013699120.md, .private/research/help-center/articles/9723268717202.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let integrations holding a valid token create projects for an existing customer through the public interface, with the supported subset of project fields.

## Rationale

Sales and event systems can create the project when a booking is confirmed.
