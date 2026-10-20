---
name: self-hosted-verifactu-responsibility-boundary
description: Raccord records a product boundary for self-hosted installations while leaving statutory responsibility and compliance claims to independent legal verification.
status: Accepted
date: 2026-10-20
deciders: ["Juan G. Carmona"]
tags: ["compliance", "verifactu", "self-hosting", "legal"]
---

# ADR-0029: Record the self-hosted VERI*FACTU responsibility boundary

## Context

Raccord is open-source and self-hostable. Each installation can be configured and operated by a different company. The product model currently assigns the installing company responsibility for the installation it operates, including its responsible declaration and electronic certificate, while requiring legal verification before any compliance claim. That model is draft and is not independent legal advice. Research question 1 remains open about the statutory identity and liability of the software producer for open-source self-hosted installations.

PR #99 recorded this product boundary as ADR-0023 on a feature branch, where that number was available. It merged into `feat/20-templates`, not `main`. On the current mainline ADR-0023 records frontend feature slices and ADR-0028 is the latest ADR, so this record uses ADR-0029.

## Decision

We will treat the installing company as the operator responsible, in Raccord's product and deployment design, for configuring and operating its installation, handling its declaration and certificate, and assessing customisations it deploys. Raccord may provide declaration guidance and release information. This decision records the current product boundary; it does not determine statutory producer identity or liability, establish that a declaration is legally sufficient, or substitute for independent tax/legal advice.

Raccord must not claim VERI*FACTU or other tax compliance until the implementation has been checked against current official specifications and reviewed by tax and legal experts. Submission or other production compliance functionality remains gated by those checks and the applicable approved Product Change.

## Consequences

- **Positive:** Product and architecture documents can describe the operator boundary consistently without asserting a legal conclusion.
- **Negative:** Operators must manage their installation configuration, credentials, declarations and customisations, while the legal sufficiency of this model remains unverified.
- **Neutral:** Detailed tax semantics, official protocol requirements, certificates and authority submission remain separate prerequisites; the compliance seam remains an engineering boundary and is not evidence of regulatory compliance.

## Alternatives considered

- **State that the maintainer is always the statutory producer:** rejected because the research does not resolve that legal question and Raccord does not operate each self-hosted installation.
- **State that the operator is always the statutory producer:** rejected because the available research does not establish that legal conclusion for every deployment.
- **Leave the product boundary undocumented:** rejected because it would leave product and architecture records inconsistent and invite unsupported claims.

## References

- [VERI*FACTU compliance constraint](../product/model/requirements/constraints/con-sif-regulation.md)
- [No legal compliance claim without verification](../product/model/requirements/constraints/con-no-compliance-claim.md)
- [Spanish invoicing compliance research, open question 1](../research/spanish-invoicing-compliance-requirements.md#11-open-questions-requiring-legal-review)
- [ADR-0010: compliance seam](0010-compliance-seam.md)
- [Integration plan](../../openspec/changes/integrate-self-hosted-responsibility-boundary/proposal.md)
- [PR #99, merged into a feature branch](https://github.com/juangcarmona/raccord/pull/99)
- Related integration issue: [#59](https://github.com/juangcarmona/raccord/issues/59)
