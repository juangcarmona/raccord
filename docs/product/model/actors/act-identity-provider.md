---
id: ACT-IDENTITY-PROVIDER
type: actor
title: "Identity provider"
status: draft
actor-kind: external-system
provenance:
  source: ".private/research/help-center/articles/17043308502290.md, .private/research/help-center/articles/360018767240.md, .private/research/help-center/articles/360014204180.md"
  confidence: medium
  recovered-from: documentation
---

## Purpose

An external sign-in service (for example a consumer account provider or the organization's own directory) that authenticates a person on the product's behalf.

## Goals

- Authenticate people according to the organization's own policies.

## Responsibilities

- Confirms the identity of a person signing in with an external account.
- Applies its own additional factors, password rotation and similar policies when sign-in is enforced through it.

## Boundaries

Does not create, remove or authorize users inside a workspace; user records and roles stay managed in the product.
