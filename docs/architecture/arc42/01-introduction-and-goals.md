# 1. Introduction and goals

Raccord is an independent, open-source, self-hostable operations and invoicing system for audiovisual production and rental companies. It is its own system of record. See the [product definition](../../product/README.md) (PDaC model, proposed as `CHG-INITIAL`).

## Essential requirements

- Turn project work into correct, numbered, immutable invoices without re-typing.
- Make every financial action auditable and every issued document reproducible.
- Prepare for Spanish VERI*FACTU and B2B e-invoicing without claiming compliance prematurely.
- Install and operate cheaply: Docker Compose, .NET Aspire for development, Azure via Bicep.

## Quality goals

| Priority | Goal | Scenario (see §10) | Measure |
|---|---|---|---|
| 1 | Correctness | Q-1, Q-2 | Money, VAT, rounding and numbering are deterministic and covered by tests |
| 2 | Integrity & auditability | Q-3, Q-4 | Issued records cannot be altered; tampering is detected; actions are audited |
| 3 | Operability | Q-6, Q-7 | Fresh install < 1 h; health checks; rehearsed backup/restore |
| 4 | Maintainability | Q-8 | Executable dependency rules; analyzers as errors; contract-first frontend |
| 5 | Portability | Q-9 | Linux containers; local or Azure storage; full data export (MVP) |
| 6 | Security | Q-10 | No secrets in Git; managed identity; non-root containers; authentication (MVP) |

## Stakeholders

| Role | Expectation |
|---|---|
| Production/rental companies (actors ACT-OPERATIONS-COORDINATOR, ACT-BILLING-ADMINISTRATOR) | Correct invoices, less re-typing, low cost |
| External accountants and tax authority | Consistent numbering, verifiable records, exports |
| Self-hosting administrators | Simple installation, upgrades, backups |
| Contributors | Clear boundaries, fast tests, documented decisions |
| Maintainers | Brand protection (ADR-0002), legal caution on compliance |
