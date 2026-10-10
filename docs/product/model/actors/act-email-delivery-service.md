---
id: "ACT-EMAIL-DELIVERY-SERVICE"
type: actor
title: "Company mail server"
status: draft
actor-kind: external-system
provenance:
  source: ".private/research/help-center/articles/21646575802258.md, .private/research/help-center/articles/360014199640.md"
  confidence: medium
  recovered-from: documentation
---

## Purpose

The company's own mail account or mail server that Raccord is configured to send e-mail through.

## Goals

Accept outgoing messages from Raccord and report whether they were accepted.

## Responsibilities

Delivers messages to recipients and authenticates the company's sending domain under its own configuration.

## Boundaries

Raccord does not manage sending domains; delivery beyond the mail server is outside the product.
