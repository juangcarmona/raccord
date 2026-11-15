---
id: UC-FIN-RESTRICT-FINANCIAL-VISIBILITY
type: use-case
title: "Restrict financial visibility for a role"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-FIN-FINANCIAL-VISIBILITY"
uses-terms:
  - "TERM-FINANCIAL-DETAILS"
provenance:
  source: ".private/research/help-center/articles/4409044768658.md, .private/research/help-center/articles/360017645140.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Staff see only the financial information they need.

## Trigger

A role (for example warehouse staff) should not see prices and margins.

## Preconditions

Roles exist.

## Main Flow

1. The administrator edits a role and switches financial visibility off.
2. The product hides financial details for all users of that role.

## Alternative Flows

Switching it on again restores visibility.

## Failure Conditions

- An actor without permission to edit roles is refused and nothing changes.
- Switching visibility off for a role the administrator belongs to is shown with its consequence before it takes effect, since financial details then disappear for that actor too.
- Users of the restricted role cannot add additional costs; such an attempt is refused (BR-FIN-FINANCIAL-VISIBILITY).
- If another user changed the same role in the meantime, the change is refused and the administrator must reload and retry.

## Postconditions

Users of the role see no financial details but can still plan.
