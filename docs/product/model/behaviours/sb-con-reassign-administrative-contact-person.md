---
id: SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON
type: structured-behaviour
title: Reassign an administrative contact person atomically
status: active
illustrates:
  - BR-CON-CONTACT-PERSON-DESIGNATIONS
given:
  - Alex is the administrative person for a contact
  - Bea is the default person for the same contact
when: An authorized coordinator designates Bea as administrative
then:
  - Bea is the sole administrative person and remains the default person
  - Alex remains under the contact without the administrative designation
  - Concurrent successful writes cannot leave two administrative people
---

## Intent

Reassign an administrative contact person atomically.

## Boundaries

Recipient selection when sending an invoice remains governed by BR-PRJ-INVOICE-RECIPIENT; no invoice is sent here.
