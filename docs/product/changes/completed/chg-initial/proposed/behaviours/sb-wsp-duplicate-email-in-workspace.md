---
id: SB-WSP-DUPLICATE-EMAIL-IN-WORKSPACE
type: structured-behaviour
title: "Shared email address blocks verification"
status: draft
illustrates:
  - "BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE"
  - "BR-WSP-ONE-PROFILE-PER-EMAIL"
given:
  - "Two crew members are registered with the same email address"
when: "One of them tries to verify that shared address"
then:
  - "The product refuses because the address is used more than once"
  - "Administrators see both crew members flagged"
uses-terms:
  - "TERM-USER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36290993235730.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Each verified login needs its own address.

## Boundaries

Plus-addressed variants of one mailbox are distinct addresses and are accepted.
