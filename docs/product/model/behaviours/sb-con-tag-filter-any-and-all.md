---
id: SB-CON-TAG-FILTER-ANY-AND-ALL
type: structured-behaviour
title: "Contact filters support any and all selected tags"
status: active
illustrates:
    - "BR-CON-TAG-FILTER-MODES"
given:
    - "Contact A has tags venue and customer"
    - "Contact B has tag venue"
when: "The user filters by venue and customer"
then:
    - "Any mode shows Contact A and Contact B"
    - "All mode shows only Contact A"
uses-terms:
    - "TERM-CONTACT-TAG"
---

## Intent

The user can choose between broad tag matching and intersection matching.

## Boundaries

Only contacts in the active workspace are considered.
