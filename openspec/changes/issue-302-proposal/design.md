# Design

<!-- pdac-scope: cited -->

## Context

See proposal.md. The source-bound #396 packet precedes the eventual #299/#301 runtime base; its tests and reported CI do not establish current correctness. Contacts are isolated by deployment and database under ADR-0012.
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:5eb794eff7858450263824556fd9756d03d96ea9d32b5db2df4b672778539165" -->

## Goals / Non-Goals

**Goals:** Integrate with existing Contacts permissions, transaction boundaries, generated client and established UI patterns; preserve import, designation and directory behavior.

**Non-Goals:** Shared multi-tenant schema, new cross-module folder abstraction, bulk or import-time organization, production action. No architecture-rule deviation is intended.

## Decisions

1. Adapt historical organization service, module port, DTOs, typed endpoints, repository predicates, UI and tests to the delivered base. Do not replace entire host, repository, routes, translations, generated files or product model. Rebuilding from scratch wastes reviewed work; restoring wholesale risks overwriting newer behavior.
2. Use Contacts-owned tag and assignment tables, nullable folder reference and folder parent/position with restrictive foreign keys. Preserve unique assignments and deterministic contact-number/id pagination; normalize tag IDs before any/all predicates. Filtering remains server-side and ANDs role, folder and tag categories. Alternative client-side filtering breaks pagination and isolation.
3. Serialize all hierarchy-changing operations, folder deletion and contact filing with one transaction-scoped database lock per installation, acquired before reading hierarchy or checking emptiness and held through commit. Retain restrictive foreign keys as a backstop; map known constraint/serialization conflicts to controlled errors without masking cancellation or infrastructure failures. This trades concurrency for correctness and avoids the historical read-then-write cycle race; row-only locks or sequential tests do not protect opposing moves. Tag creation, assignment and deletion use the same lock. The historical assigned-tag deletion refusal is retained: accepted removal remains available after explicit unassignment, and no contact or assignment is silently deleted. FR-CON-004 and the tag use case require removal but do not require automatic removal of assignments; the approved delivery scenario removes assignments before deleting the tag.
4. Expose feature-local authenticated controls and regenerate OpenAPI and EF metadata from the actual base. Compare historical assigned-tag deletion refusal with the accepted tag use case before reuse; retain it only if compatible rather than asserting a new product rule.

## Risks / Trade-offs

- [Prerequisite base changes] → Reconcile the actual delta after #299/#301; preserve host links, designation transactions, fixtures, imports and directory journeys.
- [Historical hierarchy race] → Shared lock plus real-database opposing-move, reorder, delete and assignment race tests; risk is review-derived, not an observed failure.
- [Coarse locking contention] → Keep transactions short and monitor during implementation; avoid unbounded retry.

## Migration Plan

Generate an additive migration and snapshot/designer on the actual base; existing contacts start unfiled. Test upgrade with existing contacts and imports and test rollback on a disposable database, verifying preexisting data survives. Rollback removes new organization data, so export or otherwise preserve it before a real rollback; no production operation is authorized here.

## Verification and definition of done

Use API/domain/repository tests for filters, permission boundaries, independent installation databases, races and migration, and feature tests for keyboard, localization, errors and empty states. Add the authenticated synthetic organization journey to the per-PR manifest and rerun directory and import journeys. Capture desktop and narrow viewport evidence in supported appearances, including focus and errors, and apply the accepted UI contract and Taste fallback with recorded operator visual review under the owner’s explicit delivery and merge authorization. Done means all scoped tasks and CI-equivalent checks pass on the actual revision, citations are current, evidence is recorded and the recorded owner delivery and merge authority applies; proposal planning itself is not acceptance.
