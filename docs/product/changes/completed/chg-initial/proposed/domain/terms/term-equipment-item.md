---
id: TERM-EQUIPMENT-ITEM
type: domain-term
title: "Equipment item"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "equipment"
  - "catalog item"
  - "article"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-EQUIPMENT-USAGE"
  - "TERM-EQUIPMENT-CODE"
  - "TERM-SCAN-CODE"
  - "TERM-EQUIPMENT-FOLDER"
  - "TERM-EQUIPMENT-RECORD-STATE"
  - "TERM-VAT-CLASS"
  - "TERM-LEDGER-ACCOUNT"
  - "TERM-DISCOUNT-GROUP"
  - "TERM-FACTOR-GROUP"
  - "TERM-DEFAULT-CONTENT"
  - "TERM-ACCESSORY"
  - "TERM-EQUIPMENT-ALTERNATIVE"
  - "TERM-CUSTOM-FIELD"
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/31926136198546.md, .private/research/help-center/articles/13141562920338.md, .private/research/help-center/articles/360013580119.md, .private/research/help-center/articles/360017649020.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A catalog entry describing one kind of thing the company rents out or sells: a microphone model, a light fixture, a flight case with contents, a roll of tape. It is the master record that planning, warehouse handling, documents and pricing all refer to.

Significant attributes:

- **Name** shown in planning, in the warehouse and on documents.
- **Equipment code** (generated when left empty) and **scan code**.
- **Equipment type**: single item, physical kit or virtual kit.
- **Usage**: rental or sale.
- **Stock quantity**: the number owned; for sale items, a stock management mode.
- **Folder** in the catalog hierarchy, tags, and an optional **default equipment group** into which the item is placed when it is planned.
- **Prices**: rental price or sale price, list price or purchase price, margin price, and financial defaults (VAT class, ledger account, discount group, factor group).
- **Physical properties**: length, height, width, weight (empty weight for physical kits), transport volume, packed-per quantity, surface pricing, measuring unit.
- **Planner visibility** and, for virtual kits, whether content may be edited per project.
- **Remarks** (external and internal), images, attached files, tasks, notes and custom field values.
- **Location in warehouse**: where the item sits in the warehouse.
- **Record state**: active or archived.

Relationships: has a stock quantity; may have default content (if a kit); may have accessories and alternatives; may itself be content of kits or an accessory of other items.

## Distinguish From

- Planned line: the occurrence of the item inside a project's equipment list; editing the line does not change the catalog item.
- Temporary item: a one-project line that does not exist in the catalog.

## Usage

Created and maintained by the equipment manager; selected when planning projects; scanned in the warehouse through its identifiers; counted in inventory counts.
