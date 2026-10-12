---
name: license-and-brand
description: Raccord's code and documentation are licensed under AGPLv3, while the code license grants no trademark license to its name, logo or official product identity.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["licensing", "brand", "open-source"]
---

# ADR-0002: License code under AGPLv3 and keep the brand separate

## Context

Raccord is self-hostable and accepts community contributions. Modified versions can be hosted by others, and official releases must be distinguishable from forks.

## Decision

We will license code and documentation under AGPLv3. Raccord's name, logo, and official product identity are not granted as a trademark license by the code license.

Rationale: this supports self-hosting and community contributions while keeping hosted modifications available under the reciprocal license and distinguishing official releases from forks.

## Consequences

- **Positive:**
    - Self-hosting and community contributions are supported.
    - Hosted modifications stay available under the reciprocal license.
    - Official releases are distinguishable from forks.
- **Negative:**
    - Anyone hosting a modified version must make the modifications available under the reciprocal license.
- **Neutral:**
    - The code license grants no trademark license to Raccord's name, logo or official product identity.

## Alternatives considered

- **A permissive code license:** rejected because hosted modifications would not have to stay available under a reciprocal license.

## References

- Related: ADR-0016 (its SBOM and license policy is load-bearing for this decision)
