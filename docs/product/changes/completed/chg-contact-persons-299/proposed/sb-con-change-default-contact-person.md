---
id: SB-CON-CHANGE-DEFAULT-CONTACT-PERSON
type: structured-behaviour
title: "Change contact-person designations without duplicate defaults"
status: active
illustrates:
  - "BR-CON-CONTACT-PERSON-DESIGNATIONS"
given:
  - "A contact has one contact person, who is its current default"
when: "An authorized coordinator creates a second contact person, then marks the new person as default and administrative"
then:
  - "The new contact person is saved under the selected contact"
  - "The second contact person has both selected designations"
  - "The first contact person is no longer the default"
  - "The contact has exactly one default contact person"
  - "Neither designation changes project contacts or invoice recipients"
uses-terms:
  - "TERM-CONTACT-PERSON"
  - "TERM-DEFAULT-CONTACT-PERSON"
  - "TERM-ADMINISTRATIVE-CONTACT-PERSON"
---

## Intent

Proves that changing contact-person roles preserves the one-default invariant and stays within the Contacts workflow.

## Boundaries

The scenario does not select a person on a project or use designations for invoice or email delivery.
