---
id: BR-EQP-AVAILABLE-OUTSIDE-BEHAVES-VIRTUAL
type: business-rule
title: "Content available outside a kit behaves like virtual content"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-DEFAULT-CONTENT"
uses-terms:
  - "TERM-CONTENT-STOCK-BINDING"
  - "TERM-VIRTUAL-KIT"
provenance:
  source: ".private/research/help-center/articles/19895983469330.md, .private/research/help-center/articles/20641257383954.md, .private/research/help-center/articles/19886981689362.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A physical kit content line marked available outside the kit is not taken out of the content item's free stock; the item can be rented separately and its availability is computed on its own, so a physical kit may combine fixed and flexible content.

## Rationale

Some accessories travel in a case but are also rented alone.

## Examples

- A case with a fixed amplifier and a flexible microphone: the microphone remains in the microphone's free stock.

## Exceptions

None.
