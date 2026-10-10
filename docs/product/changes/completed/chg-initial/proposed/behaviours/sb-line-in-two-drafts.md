---
id: SB-LINE-IN-TWO-DRAFTS
type: structured-behaviour
title: "A drafted line cannot be drafted again"
status: draft
illustrates:
  - "BR-SINGLE-BILLING"
given:
  - "A billable line is in an open draft"
when: "The billing administrator tries to create another draft with it"
then:
  - "No draft is created"
  - "The line stays in the first draft"
uses-terms: []
---

## Intent

Prevents double billing, including when both drafts are requested at the same moment.

## Boundaries

None.
