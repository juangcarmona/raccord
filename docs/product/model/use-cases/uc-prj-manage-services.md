---
id: UC-PRJ-MANAGE-SERVICES
type: use-case
title: "Sell production work as a service"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-BILLING-ADMINISTRATOR"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-DEFAULT-SERVICE-COPY"
  - "BR-PRJ-TASK-SINGLE-SERVICE"
  - "BR-PRJ-SERVICE-BREAKDOWN"
  - "BR-PRJ-SERVICE-EDIT-RIGHTS"
  - "BR-PRJ-SERVICE-DOCUMENT-SECTION"
  - "BR-PRJ-TASK-NOT-INVOICEABLE"
uses-terms:
  - "TERM-PRODUCTION-SERVICE"
  - "TERM-DEFAULT-SERVICE"
provenance:
  source: ".private/research/help-center/articles/38423504883858.md, .private/research/help-center/articles/38421818533522.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Pre- and post-production work is quoted and invoiced with margin insight.

## Trigger

A customer buys design, editing or similar work.

## Preconditions

Quoting and invoicing and structured task work are available.

## Main Flow

1. The coordinator adds a service to a project within a service group, or adds a default service, setting name, estimated time, price, cost per hour, VAT class, ledgers, discount and details.
2. Adding a default service creates its tasks.
3. The coordinator links existing tasks or creates tasks from the service.
4. The product shows the service breakdown and lists services in their own section of quotes and invoices.

## Alternative Flows

- A default service is created once and reused across projects.
- Price and description adjusted on the project only.

## Failure Conditions

Own document templates without a services section do not show services.

## Postconditions

The service is part of the project's financials.
