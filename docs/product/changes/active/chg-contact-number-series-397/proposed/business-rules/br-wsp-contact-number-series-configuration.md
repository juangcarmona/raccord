---
id: BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION
type: business-rule
title: Contact-number series settings apply to future allocations
status: active
applies-to:
  - "UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES"
uses-terms:
  - "TERM-CONTACT-NUMBER"
---

## Rule

A workspace has one contact-number series with an optional static prefix and an unpadded decimal current number. The current number is the latest sequence value considered used; the next automatic number is the next unused decimal value after it. New workspaces start with an empty prefix and current number zero. Updating the prefix or current number affects future assignments only and never rewrites existing contact numbers. The allocator must avoid numbers already assigned manually or automatically.

## Rationale

A single predictable series gives a workspace consistent references while protecting historical contact identifiers.

## Examples

- With an empty prefix and current number zero, the first automatically assigned number is 1.
- With prefix `C-` and current number 104, the next unused automatically assigned number is `C-105`.
- If the next candidate is already assigned manually, allocation advances until it finds an unused value.

## Exceptions

A user may supply a custom contact number instead of using the series, but it must remain unique. Series allocation does not alter contact numbers already assigned.
