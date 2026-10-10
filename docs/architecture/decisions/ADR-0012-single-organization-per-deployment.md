# ADR-0012: One organization per deployment for the MVP

- Status: accepted
- Date: 2026-10-06

## Context

The product definition lists tenant isolation. Raccord is self-host first: each company runs its own installation. Multi-tenant data partitioning adds a filter to every query and a class of severe bugs.

## Decision

- An installation serves one issuing organization (one issuer profile, its series and data). Isolation between companies is achieved by separate deployments and databases.
- The data model does not carry a tenant column yet.

## Consequences

- A hosted multi-tenant offering or multiple legal entities would require a migration (tenant/entity key, row-level security or query filters). That is recorded as assumption A-1 and roadmap wave 5.
- Authentication and roles (MVP slice 2) are still required inside the single organization.
