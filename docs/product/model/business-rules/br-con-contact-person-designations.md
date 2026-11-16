---
id: BR-CON-CONTACT-PERSON-DESIGNATIONS
type: business-rule
title: A contact has at most one default and one administrative contact person
status: active
applies-to: []
uses-terms:
  - TERM-CONTACT-PERSON
  - TERM-DEFAULT-CONTACT-PERSON
  - TERM-ADMINISTRATIVE-CONTACT-PERSON
---

## Rule

A contact may have zero or one default contact person and zero or one administrative contact person. The same person may hold both designations. Assigning either designation to another person clears that designation on the previous person in the same successful change. Rejected or concurrent conflicting changes cannot leave duplicate designations or partially reassign them.

## Rationale

Project workflows that later use these designations need an unambiguous person for each designation on a contact.

## Examples

Changing the default from Alex to Bea leaves Bea as the sole default for the contact; Alex remains associated with the contact without the default designation. A contact cannot have two people with the administrative designation.

## Exceptions

None.
