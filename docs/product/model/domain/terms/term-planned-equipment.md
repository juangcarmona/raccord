---
id: TERM-PLANNED-EQUIPMENT
type: domain-term
title: "Planned equipment line"
status: draft
defined-in: BC-EQUIPMENT
synonyms:
  - "planned equipment"
  - "planned item"
uses-terms:
  - "TERM-PLANNED-QUANTITY"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
  - "TERM-EQUIPMENT-GROUP"
  - "TERM-RESERVATION"
  - "TERM-EQUIPMENT-ITEM"
provenance:
  source: ".private/research/help-center/articles/23230026077586.md, .private/research/help-center/articles/360013730520.md, .private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A line on a project or project that plans a quantity of one catalog article (single item, physical or virtual combination, accessory) or of a temporary article. When planned it takes a snapshot of the catalog data (name, prices, price factor, remarks and similar); from then on the snapshot and the catalog evolve independently until someone explicitly refreshes the line.

Significant attributes: planned quantity, equipment group, planning period, usage period, optional flag, unit price and factor, remark, reserved quantity, shortage indication. It refers to one catalog article and belongs to exactly one equipment group of one project.

## Distinguish From

Not the catalog article itself: editing a planned line never changes the catalog and editing the catalog never changes existing planned lines. Not a billable line: the line's commercial projection is handled by Commercial/Projects.

## Usage

Created when equipment is planned, read by availability calculation, shortage detection, packing lists and quotations.
