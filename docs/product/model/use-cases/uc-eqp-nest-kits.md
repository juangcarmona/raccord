---
id: UC-EQP-NEST-KITS
type: use-case
title: "Nest a kit inside another kit"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
  - "ACT-WAREHOUSE-TECHNICIAN"
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-EQP-NESTING-LIMIT"
uses-terms:
  - "TERM-NESTED-KIT"
  - "TERM-DEFAULT-CONTENT"
  - "TERM-PLANNED-CONTENT"
provenance:
  source: ".private/research/help-center/articles/19934846796306.md, .private/research/help-center/articles/360013086279.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Modular setups are expressed as kits made of kits, permanently, or for one project.

## Trigger

A setup includes another standard setup.

## Preconditions

- Both kits exist.

## Main Flow

1. The equipment manager adds a kit as default content of another kit in the catalog.
2. The product saves the nesting; every future planning of the outer kit includes the inner one.

## Alternative Flows

- 1a. Project only: the planner adds a kit inside a planned kit on one project; the catalog is unchanged.

## Failure Conditions

- Nesting would exceed 10 levels: refused.

## Postconditions

- The nesting is visible in the kit content at each level.
