---
id: BC-CONTACTS
type: bounded-context
title: Contacts
status: active
---

## Responsibility

The organizations and people Raccord deals with: the issuing organization, customers and other contacts (companies and private persons), their contact persons, venues and suppliers, with legal identities, addresses and commercial defaults. Contact persons are maintained as records under their parent contact.

## Language

Legal names, tax identifiers and postal addresses as printed on invoices.

## Boundaries

Crew members and users are defined in the crew and workspace contexts; commercial documents use party data but belong to Commercial and Invoicing. Maintaining a person's designations here does not select a project contact or determine invoice delivery.

## External Relationships

Invoicing takes a snapshot of legal party details at issue time under BR-PARTY-SNAPSHOT; later Contacts edits do not reach issued invoices. Project selection and document delivery remain governed by their own use cases. The active installation's database is the Contacts isolation boundary; this change introduces no organization selector or tenant partition inside a deployment.
