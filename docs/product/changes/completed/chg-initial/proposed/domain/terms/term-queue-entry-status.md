---
id: TERM-QUEUE-ENTRY-STATUS
type: domain-term
title: "Invoicing queue entry status"
status: draft
defined-in: "BC-INVOICING"
synonyms: []
uses-terms:
  - "TERM-EXTERNALLY-INVOICED"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017700359.md, .private/research/help-center/articles/25948227210258.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

Lifecycle of one invoice moment part of a project.

| State | Meaning |
| --- | --- |
| Not yet due | The project is not confirmed or its trigger date lies outside the selected period. |
| Due | Confirmed and the trigger date lies in the selected period. |
| Overdue | The trigger date has passed and no invoice exists; highlighted. |
| Invoiced | An invoice has been generated for the part. |
| Invoiced externally | Marked as billed outside the product; no further reminder. |

Transitions: not yet due -> due (confirmation or date enters period); due -> overdue (date passes); due or overdue -> invoiced; due or overdue -> invoiced externally.

## Distinguish From

Invoice status; payment status.

## Usage

Drives highlighting and membership of the invoicing queue.
