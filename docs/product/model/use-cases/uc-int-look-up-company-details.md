---
id: UC-INT-LOOK-UP-COMPANY-DETAILS
type: use-case
title: "Fill company details from a registry"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PARTIES"
governed-by: []
uses-terms:
  - "TERM-COMPANY-REGISTRY-LOOKUP"
provenance:
  source: ".private/research/help-center/articles/14042545177618.md, .private/research/help-center/articles/30759527977490.md, .private/research/help-center/articles/360015784860.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

A new contact gets correct legal name and address without typing.

## Trigger

A coordinator creates a contact for a company.

## Preconditions

A registry or search service is available for the country.

## Main Flow

1. The coordinator types the company name or registration number.
2. The product shows matching companies from the registry or search service.
3. The coordinator picks one; the product fills in name, address and registration data.

## Alternative Flows

No match: details are typed manually.

## Failure Conditions

Incomplete results (for example a missing name) leave fields empty for manual entry.

## Postconditions

The contact holds the proposed details.
