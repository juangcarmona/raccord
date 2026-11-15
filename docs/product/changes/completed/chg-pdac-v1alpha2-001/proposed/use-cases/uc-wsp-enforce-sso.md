---
id: UC-WSP-ENFORCE-SSO
type: use-case
title: "Enforce single sign-on for roles"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-IDENTITY-PROVIDER"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-SSO-PER-ROLE"
  - "BR-WSP-SSO-COMPLIANCE"
  - "BR-WSP-SSO-CONFIG-ACCESS"
  - "BR-WSP-SSO-NO-PROVISIONING"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
  - "TERM-LOGIN-DOMAIN-RESTRICTION"
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Internal staff can only sign in through the organization's identity provider.

## Trigger

The organization decides to centralize authentication for some roles.

## Preconditions

- User roles separating internal staff from external crew exist.
- The administrator's role grants configuration access.

## Main Flow

1. The administrator opens the security settings and chooses an identity provider.
2. The administrator enters the allowed email domains, separated by commas.
3. The administrator selects the roles to enforce and saves.
4. The product checks every user of those roles and, if all comply, activates enforcement.

## Alternative Flows

- Some users do not comply: the product reports it per role and lists the users; continue with resolving non-compliance.

## Failure Conditions

- The administrator would lock themselves out by enforcing a role they do not comply with; the product warns that non-compliant users cannot sign in.

## Postconditions

- Users of enforced roles sign in only through the provider and are subject to its policies.
