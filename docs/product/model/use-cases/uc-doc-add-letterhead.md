---
id: "UC-DOC-ADD-LETTERHEAD"
type: use-case
title: "Add a letterhead"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-LETTERHEAD-PAGE-SIZE"
  - "BR-DOC-LETTERHEAD-MARGINS"
  - "BR-DOC-LETTERHEAD-TYPE-AVAILABILITY"
  - "BR-DOC-LETTERHEAD-PAGE-MAPPING"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md, .private/research/help-center/articles/20257253618322.md, .private/research/help-center/articles/360017700539.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Documents print on the company's stationery or at label size.

## Trigger

The company introduces or changes its stationery, or needs a label format.

## Preconditions

The letterhead design is available as a PDF of the target page size.

## Main Flow

1. The administrator adds a letterhead and names it.
2. The administrator uploads the PDF.
3. The administrator chooses a standard page size or enters custom width and height matching the file.
4. The administrator sets margins that keep content clear of the design.
5. The administrator chooses the document types the letterhead is for.
6. The administrator confirms.

## Alternative Flows

A multi-page PDF provides different backgrounds for first and following pages.

## Failure Conditions

Dimensions that do not match the file misplace the background; margins that are too small let content overlap it.

## Postconditions

The letterhead can be chosen for its document types and set as default.
