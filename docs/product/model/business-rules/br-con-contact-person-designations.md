---
id: BR-CON-CONTACT-PERSON-DESIGNATIONS
type: business-rule
title: "A contact has at most one default and one administrative contact person"
status: active
applies-to:
  - "UC-CON-MANAGE-CONTACT-PERSONS"
uses-terms:
  - "TERM-CONTACT-PERSON"
  - "TERM-DEFAULT-CONTACT-PERSON"
  - "TERM-ADMINISTRATIVE-CONTACT-PERSON"
---

## Rule

A contact may have no default contact person or one default contact person, and may have no administrative contact person or one administrative contact person. When another person is designated as default, the previous designation is cleared as part of the same successful change. A rejected default change leaves the previous default intact.

## Rationale

Project workflows that later use these designations need an unambiguous person for each designation on a contact.

## Examples

Changing the default from Alex to Bea leaves Bea as the sole default for the contact; Alex remains associated with the contact without the default designation. A contact cannot have two people with the administrative designation.

## Exceptions

None.
