---
id: TERM-WAREHOUSE-STATUS
type: domain-term
title: "Warehouse status"
status: draft
defined-in: BC-WAREHOUSE
synonyms:
  - "equipment status"
uses-terms:
  - "TERM-PACKING-LIST"
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
  - "TERM-DELAYED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/15239125718546.md, .private/research/help-center/articles/15239120279442.md, .private/research/help-center/articles/14004547874834.md, .private/research/help-center/articles/15125682136850.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The physical stage of equipment (per booked quantity) and of packing lists. Ordered states:

| State | Kind | Meaning |
| --- | --- | --- |
| Confirmed (to pack) | locked | Planned for a confirmed project and still on the shelf. |
| Prepped | default, movable | Packed and ready. |
| Custom statuses | user-defined | E.g. loaded, in transit, cleaning; all count as out of the warehouse. |
| On location | locked | At the job. |
| Returned | locked | Back in stock. |

Delayed is a derived condition (not settable) for equipment still out after its planning period ends.

Transitions: forward to any later status, including skipping (default target on check-out is On location; on check-in Returned); backwards to correct mistakes; Confirmed and On location and Returned cannot be removed or reordered. A packing list's status follows its equipment (the product proposes the status when all equipment reached it).

## Distinguish From

Not the project status (commercial and management progress). Equipment and project statuses may differ.

## Usage

Organises the warehouse board and the booking of equipment.
