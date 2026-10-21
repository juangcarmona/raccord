---
id: SB-CONTACT-DEFAULTS-SNAPSHOT-ON-CREATE
type: structured-behaviour
title: "Contact commercial defaults are snapshotted when a project is created"
status: active
illustrates:
  - "UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS"
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
given:
  - "A contact has a 14-day payment term and an 8 percent rental discount"
when: "A coordinator creates Project A for that contact, changes the contact defaults to a 30-day term and 5 percent discount, then creates Project B for the same contact"
then:
  - "Project A retains the 14-day term and 8 percent discount"
  - "Project B receives the 30-day term and 5 percent discount"
  - "Updating the contact does not rewrite Project A"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
---

## Intent

Demonstrates that each new project receives the contact defaults that apply at its creation time.

## Boundaries

This scenario covers project creation and snapshot behavior only; it does not apply defaults to quotes or retroactively update existing projects.
