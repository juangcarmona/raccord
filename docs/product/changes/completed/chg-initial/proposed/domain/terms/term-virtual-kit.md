---
id: TERM-VIRTUAL-KIT
type: domain-term
title: "Virtual kit"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "flexible kit"
  - "virtual combination"
  - "set"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-DEFAULT-CONTENT"
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360017696339.md, .private/research/help-center/articles/360013086279.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A grouping of separate catalog items that are usually rented together but may also be rented individually, such as a set of tops, subwoofers and full-range cabinets.

Characteristics:

- It has default content; planning the kit adds that content to the project, and the content can then be changed per project.
- Its price is either a fixed amount or computed from its content, and can be recalculated after its content is changed on a project.
- Its stock and availability are derived from its rental content: the number of kits possible is the minimum, over content lines, of the content item's quantity divided by the quantity needed per kit (rounded down). Sale items inside a virtual kit do not limit its availability.
- It has no weight or volume of its own; the content's weight and volume are used.
- It cannot have a scan code.
- It is excluded from inventory counts.

## Distinguish From

- Physical kit: content is fixed and consumed from stock; the kit itself is a physical, scannable unit.
- Equipment group on a project: an organisational heading in a project's equipment list, not a catalog item.

## Usage

Used to speed up planning of frequently combined items; shows a shortage when some of its content is unavailable.
