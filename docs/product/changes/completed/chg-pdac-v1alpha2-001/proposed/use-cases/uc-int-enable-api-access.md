---
id: UC-INT-ENABLE-API-ACCESS
type: use-case
title: "Enable public-interface access and obtain a token"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-EXTERNAL-APPLICATION"
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
  - "BR-INT-TOKEN-VALIDITY-PERIOD"
  - "BR-INT-INTEGRATIONS-NEED-CONFIGURATION-ACCESS"
uses-terms:
  - "TERM-API-TOKEN"
  - "TERM-API-TOKEN-VALIDITY"
  - "TERM-PUBLIC-API"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An external application can authenticate against the company's public interface.

## Trigger

The company wants to build or connect an integration.

## Preconditions

The administrator's role gives configuration access.

## Main Flow

1. The administrator opens the integrations configuration and connects the public interface if it is switched off.
2. The product creates an API token owned by the administrator, valid for ten years.
3. The administrator asks to see the token and copies it into the external application.
4. The external application calls the public interface with the token.

## Alternative Flows

Access already connected: the administrator only reveals the existing token.

## Failure Conditions

The administrator lacks configuration access: the integrations configuration is not available. The token is later used beyond its owner's permissions: those calls are refused.

## Postconditions

A valid token exists and the integration is active.
