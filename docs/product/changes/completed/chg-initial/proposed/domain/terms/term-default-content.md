---
id: TERM-DEFAULT-CONTENT
type: domain-term
title: "Default content"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "standard content"
  - "kit content"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-VIRTUAL-KIT"
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-PLANNED-CONTENT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/19886981689362.md, .private/research/help-center/articles/19895983469330.md, .private/research/help-center/articles/20641311900690.md, .private/research/help-center/articles/20641508946450.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The catalog definition of what a physical or virtual kit consists of: a list of content lines, each naming an equipment item (single item, sale item or another kit) and a quantity per kit. Default content is maintained only in the catalog (never by scanning) or by content import.

Per content line of a physical kit:

- **Stock binding**: reserved from stock (fixed to the kit) or available outside the kit.
- **Show on quotes** and **show on packing list**: whether the line prints on customer documents and warehouse documents.

Sale items may be default content of physical kits and are never bound to the kit's stock.

## Distinguish From

- Planned content: the content of a kit as planned on one project, which may differ.
- Accessory: linked but planned as a separate line.

## Usage

Drives stock deduction for physical kits, stock derivation for virtual kits, kit weight, and the content added when a kit is planned.
