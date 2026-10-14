---
title: Solution strategy
arc42-section: "04"
description: Summarizes the architectural strategies used to meet the system's goals and constraints.
---
<!-- markdownlint-disable MD025 -->

# 4. Solution strategy

| Goal | Approach | Decision |
| --- | --- | --- |
| Correct money and numbering | Rich domain model with value objects (`Money`, `VatRate`, `LinePricing`, `InvoiceTotals`, `InvoiceNumberSequence`); one rounding policy; TDD | ADR-0006, ADR-0007 |
| Integrity | Canonical issued representation + SHA-256; append-only tables via triggers; optimistic concurrency | ADR-0004, ADR-0005 |
| Maintainable boundaries | Modular monolith, layered projects, module namespaces, executable rules | ADR-0001, ADR-0003 |
| Replaceable integrations | Ports in Application, adapters in Infrastructure: persistence, document storage, rendering, compliance, clock | ADR-0008–0010 |
| Contract-first UI | OpenAPI generated from the API; typed client; no domain logic in the UI | ADR-0011 |
| Simple operations | One container image; migrations as an explicit job; health endpoints; OpenTelemetry via Aspire service defaults | ADR-0013 |
| Compliance without guessing | A seam that records every obligation now; implementation after legal review | ADR-0010 |
| Self-hosting first | One organization per deployment; file-system storage default | ADR-0012, ADR-0009 |

Further strategies (planned): outbox for outbound messages (e-mail, AEAT), idempotency keys on commands that create external effects, inbox for inbound messages (B2B status), snapshots of financial figures at issue time for reporting.
