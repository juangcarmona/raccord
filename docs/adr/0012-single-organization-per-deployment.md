---
name: single-organization-per-deployment
description: For the MVP, each Raccord installation serves one issuing organization, and isolation between companies comes from separate deployments and databases.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["tenancy", "self-hosting", "data-model", "mvp"]
---

# ADR-0012: Serve one organization per deployment for the MVP

## Context

The product definition lists tenant isolation. Raccord is self-host first: each company runs its own installation. Multi-tenant data partitioning adds a filter to every query and a class of severe bugs.

## Decision

We will serve one issuing organization per deployment for the MVP.

- An installation serves one issuing organization (one issuer profile, its series and data). Isolation between companies is achieved by separate deployments and databases.
- The data model does not carry a tenant column yet.

## Consequences

- **Positive:**
  - Queries need no tenant filter, avoiding that class of severe bugs.
- **Negative:**
  - A hosted multi-tenant offering or multiple legal entities would require a migration (tenant/entity key, row-level security or query filters). That is recorded as assumption A-1 and roadmap wave 5.
- **Neutral:**
  - Authentication and roles (MVP slice 2) are still required inside the single organization.

## Alternatives considered

- **Multi-tenant data partitioning in one installation:** rejected for the MVP because it adds a filter to every query and a class of severe bugs, while each company runs its own installation.

## References

- Assumption A-1 and roadmap wave 5
- Related: ADR-0021
