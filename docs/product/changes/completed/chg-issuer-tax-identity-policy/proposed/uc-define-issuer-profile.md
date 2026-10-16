---
id: UC-DEFINE-ISSUER-PROFILE
type: use-case
title: "Define the issuer profile"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-WORKSPACE-ADMINISTRATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-TAX-ID-NORMALIZATION"
  - "BR-PARTY-SNAPSHOT"
  - "BR-AUDITED-ACTIONS"
uses-terms:
  - "TERM-ISSUER"
  - "TERM-TAX-ID"
---

## Goal

The issuer's current legal and contact details are ready to appear on future invoices, and changes to legal identity receive deliberate review after invoicing has begun.

## Trigger

First use of the installation, or a change to the issuer's address, contact details or legal identity.

## Preconditions

The actor has permission to maintain issuer settings. A second distinct administrator with issuer-settings permission is available to review a legal-identity change after an invoice has been issued.

## Main Flow

1. The administrator enters or updates the issuer's legal name, tax identifier, postal address or contact details.
2. The product normalizes the tax identifier and identifies whether the request changes legal identity or only address/contact details.
3. If no invoice has yet been issued, the product saves and audits the change.
4. If an invoice has been issued and legal identity changes, the product records the requested values and reason as pending review; a second distinct authorized administrator reviews and confirms or rejects the request.
5. The product audits the request and decision. An accepted change becomes the profile for future invoices.

## Alternative Flows

- An address or contact-detail change is saved by one authorized administrator and affects future invoices only.
- A legal-identity change before the first issued invoice is saved by one authorized administrator and affects future invoices.
- A requested legal-identity change represents a different legal entity: the administrator uses a separate installation for that issuer.

## Failure Conditions

- A missing legal name, tax identifier or incomplete address is refused with the reason.
- A legal-identity change after invoice issuance lacks a reason, second-person confirmation or required permission: it remains unapplied and the product records the rejected or incomplete review state.
- The proposed values represent a different issuing legal entity for this installation: the change is refused for this installation.

## Postconditions

Future invoices use the accepted issuer profile. All previously issued invoices retain the issuer snapshot recorded at their issue time. Every request and decision is audited.
