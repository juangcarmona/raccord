---
id: SB-CON-CHANGE-DEFAULT-CONTACT-PERSON
type: structured-behaviour
title: Reassign a default contact person atomically
status: active
illustrates:
  - BR-CON-CONTACT-PERSON-DESIGNATIONS
given:
  - A contact has Alex as its default person and Bea as another person
when: An authorized coordinator designates Bea as default
then:
  - Bea is the sole default person
  - Alex remains under the same contact without the default designation
  - A rejected or conflicting change cannot leave a partial reassignment
---

## Intent

Reassign a default contact person atomically.

## Boundaries

This changes contact-person metadata, not project associations or issued-invoice snapshots.
