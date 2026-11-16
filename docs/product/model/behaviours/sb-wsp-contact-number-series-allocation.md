---
id: SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION
type: structured-behaviour
title: Contact allocation uses the configured sequence without changing existing numbers
status: active
illustrates:
  - "UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES"
  - "BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION"
  - "BR-PRJ-CONTACT-NUMBER-ASSIGNMENT"
given:
  - "The contact-number prefix is `C-` and the current number is 104."
  - "A contact already has number `C-105`."
  - "Another existing contact has number `legacy-42`."
when: "Two contacts are created concurrently without supplied contact numbers after the series is saved."
then:
  - "Both new contacts receive different numbers from the configured sequence."
  - "The number `C-105` is not assigned again."
  - "The existing numbers `C-105` and `legacy-42` remain unchanged."
---

## Intent

Concurrent contact creation uses the configured sequence without duplicating or rewriting contact numbers.

## Boundaries

This scenario does not define project or financial-document series, bulk renumbering, or recovery after a failed contact save.
