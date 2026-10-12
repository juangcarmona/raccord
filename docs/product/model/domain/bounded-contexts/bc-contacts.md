---
id: BC-CONTACTS
type: bounded-context
title: "Contacts"
status: draft
---

## Responsibility

The organizations and people Raccord deals with: the issuing organization, customers and other contacts (companies and private persons), their contact persons, venues and suppliers, with legal identities, addresses and commercial defaults.

## Language

Legal names, tax identifiers and postal addresses as printed on invoices.

## Boundaries

Crew members and users are defined in the crew and workspace contexts; commercial documents use party data but belong to Commercial and Invoicing.

## External Relationships

Invoicing takes a snapshot of a party's legal identity at issue time; later changes in Parties do not reach issued invoices.
