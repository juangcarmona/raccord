---
id: CHG-EQP-CATALOG-TYPES-306
type: product-change
title: Define rental and sale catalog items, serial units, and paged Equipment browsing
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
    add:
        - UC-EQP-BROWSE-CATALOG
        - BR-EQP-SERIAL-UNIT-STOCK-MATCHES
        - FR-EQP-019
        - TERM-SERIAL-NUMBERED-UNIT
    modify:
        - UC-EQP-CREATE-ITEM
        - UC-EQP-CREATE-SALE-ITEM
        - FR-EQP-001
    remove: []
---

## Problem

The Equipment definition distinguishes rental/sale usage and says the context includes serial-numbered units, but it does not define how those types and units are created or represented in the catalog. The existing model also lacks a paged catalog-browsing use case and requirement. The existing type immutability rule already protects structural equipment type after planning, but does not state whether rental/sale usage is covered.

## Intended Product Outcome

Equipment managers can create rental and sale catalog items, optionally identify each physical unit by a serial number while keeping item quantity consistent, and browse the catalog in pages. The existing structural-type immutability behavior remains governed by `UC-EQP-CHANGE-ITEM-TYPE` and `BR-EQP-TYPE-FIXED-ONCE-PLANNED`.

## Rationale

These catalog distinctions are required before later Equipment detail, kit and import capabilities can refer to stable item and unit records. Explicit serial-unit behavior is needed to prevent the item aggregate quantity from disagreeing with its individually tracked units. Paged browsing lets an operator work with catalogs larger than one response without changing the catalog's product scope.

## Affected Product Areas

- `BC-EQUIPMENT`: catalog item type and usage, serialized units and catalog browsing.
- `UC-EQP-CREATE-ITEM` and `UC-EQP-CREATE-SALE-ITEM`: creating rental and sale items with optional unit tracking.
- `UC-EQP-CHANGE-ITEM-TYPE` and `BR-EQP-TYPE-FIXED-ONCE-PLANNED`: existing structural type remains fixed after planning; usage immutability is an open question.
- `TERM-EQUIPMENT-TYPE`, `TERM-RENTAL-ITEM`, `TERM-SALE-ITEM`, `TERM-EQUIPMENT-ITEM`, `TERM-STOCK-QUANTITY`, and `TERM-PAGINATION-CURSOR` are neighboring definitions checked for consistency.

## Open Questions

- Does optional serial tracking apply to both rental and sale items, and can a unit be created without a manufacturer-assigned serial number by using an internal identifier?
- At what scope must serial identifiers be unique: within an item, across the full catalog, or across a company workspace?
- When serial tracking is enabled, is aggregate stock quantity exactly the count of registered units, and which unit states count as owned/available after booking, loss, sale or correction?
- Can an existing quantity-tracked item switch into serial tracking, and if so how must its existing stock be reconciled? Can serial tracking later be disabled?
- Does "item type cannot change once planned" include changing rental versus sale usage, or does the existing rule apply only to structural type (single item, physical kit, virtual kit)?
- What catalog pagination controls must users see (page size, previous/next or numbered pages), which paging style is intended (cursor or numbered pages), and which stable sort order must be preserved when navigating?

## Product Acceptance

- The definition distinguishes the item's structural type from its rental/sale usage.
- Item creation defines the behavior for rental and sale records and the optional serial-unit choice.
- The aggregate quantity cannot silently disagree with the individually registered serial units.
- A catalog browse flow returns a bounded page and a continuation path until no further records remain.
- Existing type-change behavior for planned structural types stays explicit and covered by its current use case and rule.
- All open questions have an Owner answer before the change is approved.

## Out of Scope

- Product Change approval, application or acceptance; implementation, API shape, UI presentation and database design.
- Catalog filters, import/export, images, accessories, alternatives, kits and kit content.
- Project planning, reservations, warehouse booking, lost/found lifecycle and serial-unit movement history.
- Changing structural type after planning or changing rental/sale usage after planning until the open question is resolved.
