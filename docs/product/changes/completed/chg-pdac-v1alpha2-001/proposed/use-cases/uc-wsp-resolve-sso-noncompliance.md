---
id: UC-WSP-RESOLVE-SSO-NONCOMPLIANCE
type: use-case
title: "Resolve users who do not meet single sign-on requirements"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-SSO-COMPLIANCE"
uses-terms:
  - "TERM-SSO-ENFORCEMENT"
  - "TERM-LOGIN-DOMAIN-RESTRICTION"
  - "TERM-LOGIN-STATUS"
provenance:
  source: ".private/research/help-center/articles/17043308502290.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Every user of an enforced role can sign in again.

## Trigger

The product reports that users in a role do not meet the domain restrictions.

## Preconditions

- Single sign-on enforcement is configured.

## Main Flow

1. The administrator opens the list of non-compliant users of the role.
2. For each user the administrator assigns a role without enforcement, or the user changes and verifies an email address in an allowed domain, or a user whose address is unverified verifies it.
3. The product re-evaluates compliance.

## Alternative Flows

- None.

## Failure Conditions

- Unresolved users remain unable to sign in.

## Postconditions

- All users of the enforced roles comply.
