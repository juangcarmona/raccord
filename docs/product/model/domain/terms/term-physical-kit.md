---
id: TERM-PHYSICAL-KIT
type: domain-term
title: "Physical kit"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "case"
  - "fixed kit"
  - "physical combination"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
  - "TERM-DEFAULT-CONTENT"
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-EMPTY-WEIGHT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/19886981689362.md, .private/research/help-center/articles/360018672100.md, .private/research/help-center/articles/19895983469330.md"
  confidence: high
  recovered-from: documentation
---

## Definition

An equipment item whose content is physically kept together with it and moves as one unit, for example a flight case holding a speaker and cables, or an amplifier rack.

Characteristics:

- Its default content is fixed per catalog item; content lines bound to the kit's stock are taken out of the free stock of their own items (kit quantity multiplied by content quantity).
- It has its own price, independent of the price of its content; the price can be changed on a project.
- Its weight is its empty weight plus the weight of its default content; its volume comes from its own dimensions.
- Its stock is entered manually.
- Its availability is computed like that of a single item; content is not re-checked because it is already allocated.
- It can carry a scan code.
- On a project only the kit itself is listed; its content may not be edited on a project.

## Distinguish From

- Virtual kit: content is flexible, stock is derived from the content, no scan code.
- Accessory: a separately planned item that is merely suggested together with another one.

## Usage

Used for cases, racks and any assembly that is always rented together; the kit can be scanned and booked as one item.
