---
id: TERM-PROJECT-STATUS
type: domain-term
title: "Project status"
status: draft
defined-in: "BC-PROJECTS"
synonyms: []
uses-terms:
  - "TERM-PROJECT-STATUS-GROUP"
  - "TERM-PROJECT-STATUS-INQUIRY"
  - "TERM-PROJECT-STATUS-DRAFT"
  - "TERM-PROJECT-STATUS-PENDING"
  - "TERM-PROJECT-STATUS-CONFIRMED"
  - "TERM-PROJECT-STATUS-IN-PROGRESS"
  - "TERM-PROJECT-STATUS-EXECUTION-DONE"
  - "TERM-PROJECT-STATUS-COMPLETE"
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-PROJECT-STATUS-CANCELLED"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013668139.md, .private/research/help-center/articles/29017857097618.md, .private/research/help-center/articles/23361953107346.md, .private/research/help-center/articles/28083930405778.md, .private/research/help-center/articles/360013677019.md, .private/research/help-center/articles/37629880908050.md, .private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/38542583895186.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

The commercial and project-management lifecycle stage of a project. It decides which areas show the project, whether its equipment can be handled in the warehouse, whether it is eligible for invoicing, how it affects availability, and how it is reported in revenue figures. The set of statuses is fixed (no custom project statuses). Physical progress of equipment is not a project status: it is the warehouse status of each packing list (TERM-WAREHOUSE-STATUS).

States:

| State | Meaning | Group | Set by |
| --- | --- | --- | --- |
| Inquiry | Customer wishes recorded, not yet worked out | Unconfirmed | Creation from a project request, or a user |
| Draft | Being planned | Unconfirmed | Creation of a blank project, or a user |
| Pending | Quote sent, awaiting the customer's agreement | Unconfirmed | Sending a quotation, or a user |
| Confirmed | The customer agreed | Confirmed | Digital signature of a quotation, or a user |
| In progress | Execution has started | Confirmed | The first warehouse booking of the project's equipment, or a user |
| Execution done | All equipment of every packing list is back | Confirmed | The last return, or a user |
| Complete | Closed by the team | Confirmed | A user only |
| Cancelled | The project will not go ahead | Cancelled | A user only |

Entry: a blank project starts in Draft; a project created from a project request starts in Inquiry; a project created from a template starts in the template's status.

Automatic transitions: an unconfirmed state to Pending when a quotation is sent; to Confirmed when the customer digitally signs a quotation; Confirmed to In progress at the first warehouse booking; In progress to Execution done when every packing list's equipment has returned. Complete and Cancelled are only set by users.

Manual transitions: a user may set any state from any state; the order above is the normal forward order.

There is no Invoiced state: being fully invoiced is derived from the completed invoice moments.

## Distinguish From

Warehouse status: progress of a packing list's equipment (to pack, prepped, custom stages, on location, returned). Task status: progress of a single task. Project request status: the lifecycle of an incoming request before a project exists.

## Usage

Filtering, presets, warehouse eligibility, invoicing eligibility, availability effect, revenue charts, cancellation workflow.
