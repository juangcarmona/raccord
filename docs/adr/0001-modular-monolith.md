---
name: modular-monolith
description: Raccord starts as one deployable application organized into explicit domain modules, extracting services only when a measured need justifies it.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["architecture", "modular-monolith", "deployment"]
---

# ADR-0001: Start with a modular monolith

## Context

Raccord needs an initial application structure. It is deployed locally and self-hosted, its end-to-end workflows must be cheap to test, and its domain areas must stay separable as the product grows.

## Decision

We will start with one deployable application organized into explicit domain modules. Extract services only when a measured operational or ownership need justifies it.

## Consequences

- **Positive:**
  - Local and self-hosted deployment stay simple.
  - End-to-end workflows remain cheap to test.
- **Negative:**
  - Module boundaries must be enforced in code review and tests.
- **Neutral:**
  - Service extraction stays possible, but only with a measured operational or ownership need.

## Alternatives considered

- **Start with separately deployed services:** rejected because it complicates local and self-hosted deployment and makes end-to-end workflows costlier to test, without a measured operational or ownership need.

## References

- Related: ADR-0003, ADR-0018
