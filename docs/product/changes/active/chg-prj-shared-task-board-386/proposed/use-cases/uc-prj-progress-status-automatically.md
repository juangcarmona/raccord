---
id: UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY
type: use-case
title: "Advance project status automatically"
status: active
primary-actor: "ACT-RACCORD-AUTOMATION"
supporting-actors:
  - "ACT-CUSTOMER"
  - "ACT-WAREHOUSE-TECHNICIAN"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-QUOTE-SENT-PENDING"
  - "BR-PRJ-SIGNED-QUOTE-CONFIRMED"
  - "BR-PRJ-WAREHOUSE-STATUS-PROMPT"
  - "BR-PRJ-INITIAL-STATUS"
  - "BR-INT-PROJECT-MANAGEMENT-LIFECYCLE"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project status follows business events without manual updates.

## Trigger

A quotation is sent, a customer signs a quotation digitally, or warehouse handling of the project's equipment completes a stage.

## Preconditions

The project exists.

## Main Flow

1. A quotation for the project is sent: the product sets the status to Pending.
2. The customer digitally signs a quotation: the product sets Confirmed.
3. The first equipment of the project is booked in the warehouse: the product sets In progress.
4. All equipment of every packing list has returned: the product sets Execution done.

## Alternative Flows

- The project was created from a project request: it starts as Inquiry.

## Failure Conditions

Equipment still out: the project stays In progress; a user may change status only under the project lifecycle rules. Complete requires explicit reopening before operational changes or new planning.

## Postconditions

The status reflects the latest event.
