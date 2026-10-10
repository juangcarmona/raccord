---
id: TERM-CONTENT-STOCK-BINDING
type: domain-term
title: "Content stock binding"
status: draft
defined-in: "BC-EQUIPMENT"
synonyms:
  - "reserved from stock setting"
uses-terms:
  - "TERM-DEFAULT-CONTENT"
  - "TERM-PHYSICAL-KIT"
provenance:
  source: ".private/research/help-center/articles/19895983469330.md, .private/research/help-center/articles/20641257383954.md, .private/research/help-center/articles/19886981689362.md, .private/research/help-center/articles/20641568054546.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A per-line setting on the default content of a physical kit that decides how that content relates to stock:

| Value | Meaning |
| --- | --- |
| Reserved from stock | The content is fixed to the kit: kit quantity times line quantity is taken out of the content item's free stock and cannot be planned elsewhere. Default for new lines and for all pre-existing content. |
| Available outside the kit | The content behaves like virtual kit content: it remains in free stock, can be rented separately, and its availability is calculated on its own. |

A physical kit can mix both values, making some content fixed and the rest flexible.

## Distinguish From

- Reservation of planned equipment for a project: a commitment of quantities to one project, not a catalog setting.

## Usage

Decides how content is taken out of free stock.
