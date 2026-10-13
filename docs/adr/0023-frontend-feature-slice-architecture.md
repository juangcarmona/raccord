---
name: frontend-feature-slice-architecture
description: The React frontend uses thin application routes, feature-owned vertical slices, and a small shared kernel with explicit dependency direction.
status: Accepted
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["frontend", "architecture", "enforced"]
---

# ADR-0023: Organize the frontend as feature-owned vertical slices

## Context

ADR-0011 selects the frontend technology and already places feature code under `src/features/`, shared building blocks under `src/shared/`, and route composition under `src/app/`. It does not define the responsibilities or dependency rules of those areas. The backend now has an explicit feature-slice convention (ADR-0022), but the frontend equivalent has not been recorded.

Raccord is a browser SPA with React Router. Its existing `app/`, `features/`, and `shared/` structure already supports these principles; the missing piece is explicit ownership and dependency guidance, not a new framework or folder taxonomy.

## Decision

The frontend grows as feature-owned vertical slices, composed by a thin application layer, with a small shared kernel.

- **`src/app/` owns application composition:** route definitions, the application shell, providers, and not-found behavior. Route declarations select and compose feature screens; product workflows and data-fetching behavior stay out of route configuration and the global shell.
- **`src/features/<capability>/` owns user-facing capability slices:** screens, feature-specific components, API orchestration, and feature-local types/tests. A feature may depend on `src/shared/`, but not on another feature's implementation. Cross-feature reuse moves to `shared/` only when it is genuinely generic and has more than one real consumer.
- **`src/shared/` owns stable cross-cutting mechanisms:** the generated API contract/client, localization, formatting, reusable UI primitives, and generic hooks. Shared code does not import from `app/` or `features/`, and does not contain product workflows or domain policy.
- **Dependency direction is inward:** `app → features → shared`; `app` may also use `shared`. No reverse dependency is allowed, and features do not import peer features. Backend business rules remain authoritative; frontend code presents and orchestrates API behavior rather than reimplementing those rules.
- **Keep boundaries proportional.** Do not create domain/application/infrastructure layers, wrapper abstractions, or empty feature folders without a concrete need. Split files or introduce stricter sublayers only when a capability's complexity makes the boundary useful.
- **Verification:** keep TypeScript strictness, feature tests, and route-level accessibility tests. `npm run architecture:check` parses relative imports under `src/` and fails on reverse-layer or peer-feature dependencies; its fixture suite proves each forbidden direction and the allowed compositions.

This records the architecture for the existing SPA; it does not require a folder migration or a rewrite of current screens.

## Consequences

- **Positive:** The current layout has an explicit ownership and dependency model; route files remain composition points; frontend work follows the same capability-oriented, explicit-boundary foundations used elsewhere in the codebase.
- **Negative:** The checker intentionally covers relative imports under the three architectural roots; introducing a new import mechanism requires extending the checker and its fixtures.
- **Neutral:** ADR-0011 remains authoritative for stack, API typing, styling, localization, and deployment. This decision does not adopt full Feature-Sliced Design terminology or tooling.

## Alternatives considered

- **Adopt a larger, framework-specific frontend layer taxonomy immediately:** rejected for now. Raccord already has a simpler `app/features/shared` layout; adding layers and tooling without a demonstrated need would add structure rather than clarify a real boundary.
- **Leave the existing folders undocumented:** rejected because ADR-0011 specifies locations but not ownership or allowed dependencies, leaving contributors and agents to infer boundaries.
- **Mirror backend Clean Architecture layers in the browser:** rejected. The backend's domain/application/infrastructure boundaries protect business rules and persistence. The frontend consumes those rules through HTTP; duplicating the same layers would add indirection without creating a meaningful trust boundary.

## References

- ADR-0011 — frontend stack and technology choices
- ADR-0022 — backend use-case slice conventions
- Established house foundations: feature-owned vertical slices, thin composition, explicit dependency direction, and a small shared kernel
- `src/Raccord.Frontend/src/app/`, `features/`, and `shared/` — current realization
