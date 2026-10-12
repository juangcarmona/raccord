---
name: synchronous-module-contracts-with-deferred-events
description: Modules talk to each other only through synchronous in-process interfaces in their contracts assemblies; domain events and an outbox are deferred until a real asynchronous consumer exists.
status: Proposed
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["modules", "integration", "events", "outbox"]
---

# ADR-0020: Communicate between modules through synchronous contracts and defer events

## Context

Today `docs/architecture/architecture-rules.md` says only that "cross-module writes go through use cases in one unit of work". Arc42 §4 lists an outbox as planned. There are no domain events.

With module assemblies (ADR-0018) and module-owned schemas (ADR-0019), modules need a defined way to read each other's data and to take part in each other's writes. Examples:

- Invoicing reads billable lines from Projects and party identities from Contacts.
- Invoicing marks lines as invoiced.
- Every module appends audit events.

The sibling projects show both ends of the spectrum:

- One uses only synchronous contract interfaces and has no events.
- The other dispatches domain events in process before commit, plus integration events through a third-party outbox library. Its own ADR lists eventual consistency, test polling and a heavy dependency as the costs.

Raccord's first asynchronous needs are known but not built yet: e-mail sending (BC-DOCUMENTS) and submission to the tax authority (runtime scenario R-5).

## Decision

We will make modules communicate only through synchronous, in-process interfaces published in their `.Contracts` assemblies, inside the caller's unit of work. Events come later, when a real consumer exists.

- **Contracts are queries and commands, not entities.** A `.Contracts` assembly exposes interfaces and immutable records:
    - `Projects.Contracts`: `IBillableWork`, to reserve, release and mark lines as invoiced.
    - `Contacts.Contracts`: `IPartyDirectory`, which returns legal-identity snapshots.
    - `Compliance.Contracts`: `IComplianceRecorder`.

    The owning module implements each interface internally. Nothing in a contracts assembly references a domain or persistence type.

- **Writes through contracts join the caller's transaction.** The shared connection and unit of work (ADR-0019) make them atomic, so finalizing an invoice remains one commit.
- **Audit is a shared-kernel port.** `IAuditAppender` is declared in `Raccord.SharedKernel` and implemented by Compliance, because every module audits. Audit events are written in the business transaction (ADR-0004).
- **No domain-event dispatcher, outbox or messaging library for now.** When the first asynchronous consumer is implemented (e-mail or tax-authority submission), a new ADR will introduce:
    - a transactional outbox table owned by the producing module and written in the same transaction;
    - a background dispatcher in the web host.

    Third-party messaging frameworks need their own justification under AGENTS.md's dependency rule.

- **No cycles.** Contract dependencies between modules must form an acyclic graph, checked by an architecture test.

## Consequences

- **Positive:**
    - Every cross-module interaction can be found by searching the contracts.
    - Behaviour is strongly consistent and testable without polling.
    - No new infrastructure or dependency.
- **Negative:**
    - The caller depends on the callee being available in process, which is always true in a monolith but makes later extraction harder.
    - Fan-out ("when an invoice is issued, notify X, Y and Z") must be coded explicitly in the caller until events exist.
    - The acyclic rule may force a contract to move to the lower module.
- **Neutral:**
    - The outbox stays on the roadmap of arc42 §4 and is tied to the first asynchronous consumer.
    - `architecture-rules.md` must state the contracts-only rule.

## Alternatives considered

- **Domain events dispatched in process before commit:** rejected for now because it adds indirection with no consumer that needs it. It can be added without changing the contracts.
- **Integration events through an outbox library from day one:** rejected because it brings eventual consistency and a heavy dependency before any asynchronous need exists, and finalization must stay synchronous anyway.
- **Shared read access to other modules' tables:** rejected because it breaks module data ownership (ADR-0019).

## References

- Related: ADR-0004, ADR-0010, ADR-0018, ADR-0019
- Runtime scenarios R-2 and R-5 in `docs/architecture/arc42/06-runtime-view.md`
