---
name: self-hosted-verifactu-responsibility
description: Self-hosted operators own the responsibility for the installation they operate and Raccord makes no compliance claim without independent verification.
status: Accepted
date: 2026-10-13
deciders: ["Juan G. Carmona"]
tags: ["compliance", "verifactu", "self-hosting", "legal"]
---

# ADR-0023: Make the self-hosted operator responsible for its installation

## Context

Raccord is open-source and self-hostable. Each installation may be configured, modified, and operated by a different company, while tax obligations and certificate custody belong to the company issuing invoices. The product model already requires a compliance record per invoice and prohibits legal compliance claims before verification. VERI*FACTU submission must not encode an unverified assertion about who signs a responsible declaration.

## Decision

We will define the company operating a Raccord installation as responsible for that installation's configuration, invoices, responsible declaration, and electronic certificate. Raccord will provide implementation, declaration guidance, and release information, but will not claim that an installation is legally compliant. Modified forks and customisations require their operator to reassess the declaration and responsibility model. Tax and legal counsel must verify this boundary before any compliance claim or production submission is enabled.

## Consequences

- **Positive:** Responsibility is explicit at the deployment boundary, certificate custody remains with the operator, and future compliance work can be enabled per installation without representing Raccord as the operator.
- **Negative:** Operators must obtain their own advice and complete their own declaration; Raccord cannot promise compliance merely because the software is released.
- **Neutral:** The compliance seam remains useful, but submission, declaration tooling, and authority integration remain gated by official-specification and expert review.

## Alternatives considered

- **Raccord is always the responsible producer:** rejected because Raccord does not operate or control self-hosted installations and cannot sign for another company's invoices.
- **The open-source maintainer is always the responsible producer:** rejected because the maintainer neither controls each deployment nor necessarily issues the deployment's invoices.
- **Make no product boundary decision:** rejected because the ambiguity would block implementation planning and encourage unsupported compliance claims.

## References

- [VERI*FACTU compliance constraint](../product/model/requirements/constraints/con-sif-regulation.md)
- [No legal compliance claim without verification](../product/model/requirements/constraints/con-no-compliance-claim.md)
- [Spanish invoicing compliance research, open question 1](../research/spanish-invoicing-compliance-requirements.md)
- [ADR-0010: compliance seam](0010-compliance-seam.md)
- Related issue: [#59](https://github.com/juangcarmona/raccord/issues/59)
