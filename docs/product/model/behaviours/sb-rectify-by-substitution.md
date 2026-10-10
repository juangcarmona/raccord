---
id: SB-RECTIFY-BY-SUBSTITUTION
type: structured-behaviour
title: "Correcting an invoice by substitution"
status: draft
illustrates:
  - "BR-RECTIFYING-INVOICE"
  - "UC-CORRECT-ISSUED-INVOICE"
given:
  - "Invoice F2026-00007 has been issued"
when: "The billing administrator corrects it by substitution, because the recipient's tax identifier was wrong"
then:
  - "The rectifying invoice references F2026-00007 and states the corrected recipient and all corrected amounts"
  - "It is numbered in the rectifying series"
  - "F2026-00007 is unchanged"
uses-terms:
  - "TERM-RECTIFYING-INVOICE"
---

## Intent

Corrections leave the original intact and are traceable.

## Boundaries

None.
