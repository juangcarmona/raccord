---
id: UC-DEFINE-ISSUER-PROFILE
type: use-case
title: "Define the issuer profile"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
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

The issuer's legal details are ready to appear on invoices.

## Trigger

First use of the installation, or the issuer's legal details change.

## Preconditions

None.

## Main Flow

1. The administrator enters legal name, tax identifier and complete postal address.
2. The product normalizes the tax identifier and saves the profile.
3. The change is audited.

## Alternative Flows

The profile already exists: the same flow updates it; issued invoices keep their snapshot.

## Failure Conditions

A missing legal name, tax identifier or incomplete address is refused with the reason.

## Postconditions

New invoices use the profile.
