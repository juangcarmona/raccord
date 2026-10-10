---
id: SB-DRAFT-FOLLOWS-PROJECT-ORDER
type: structured-behaviour
title: "Draft lines follow project order"
status: draft
illustrates:
  - "BR-LINE-ORDER"
given:
  - "A project has, in this order, a camera kit, a gaffer and a van"
when: "A draft is created selecting the van, the camera kit and the gaffer in that order"
then:
  - "The draft lists the camera kit, the gaffer and the van in that order"
uses-terms: []
---

## Intent

Selection order never changes invoice order.

## Boundaries

None.
