---
id: UC-WHS-ASSIGN-SCAN-CODES
type: use-case
title: "Assign scan codes to equipment"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-SCAN-CODE-UNIQUE"
uses-terms:
  - "TERM-SCAN-CODE"
provenance:
  source: ".private/research/help-center/articles/360013101260.md, .private/research/help-center/articles/5843706612370.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Every article and case can be scanned.

## Trigger

New equipment is labelled.

## Preconditions

None.

## Main Flow

1. The technician either scans a pre-printed label onto an article or case, or types a unique code.
2. The product stores the code; generated codes exist by default.

## Alternative Flows

- Several codes per article.

## Failure Conditions

- Duplicate code: refused (inferred).

## Postconditions

Code linked.
