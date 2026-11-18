<!-- pdac-scope: cited -->

# Design

## Context

See proposal.md for motivation. ADR-0021 authorizes existing local Identity. The setup API uses a transaction but has no cross-request serialization. The shared AppHost seeds demo users, so it cannot prove fresh bootstrap. The recovered UI and mocked journey are partial source, not passing evidence.

## Goals / Non-Goals

Goals: authoritative server status, accessible recoverable states, one initial administrator even under concurrent requests, privacy-preserving registration warning. Non-goals: change subsequent Crew assignment, extend sign-in/password workflows, or complete later company onboarding.

## Decisions

- Recover existing UI source rather than replace its visual language. Load status alongside identity; unknown status never enables setup. Handle signed-in visitors, stale setup tabs, server errors and created-account/sign-in failure explicitly.
- Let authentication state drive the existing route guards after sign-in, sign-out and first-account continuation. Form/shell handlers update authentication without also navigating; competing imperative/declarative navigations otherwise start the same route twice and can leave logout stuck on protected content.
- Keep server creation authoritative. Use a PostgreSQL transaction advisory lock before checking users, rather than a process-local lock, to cover multiple application instances. Retain all existing role/catalog initialization in the same transaction.
- Check public password ranges on the server using only five SHA-1 hex characters and padded responses; compare suffixes locally, never log/store the raw password or full hash. A typed client with bounded timeout and no HTTP logging returns matched, clear or unavailable. The response includes warning status; show successful setup and warning before an explicit sign-in continuation so it cannot disappear during navigation. No external request is needed in deterministic tests.
- Use a separate fresh fixture/AppHost with demo seeding disabled only by explicit Testing configuration; leave normal journeys unchanged. Run the fresh collection non-parallel because both AppHosts install into the same frontend directory. Assert real API/database state and concurrent refusal. Disable trace/video for this credential-handling journey; masked screenshots use repository artifact paths.
- Reuse native labels, alert/status regions and shared tokens. Inspect narrow/desktop, themes, keyboard and reduced motion. Human visual acceptance remains explicit where required by the UI contract.

## Risks / Trade-offs

- [Public breach service unavailable] → bounded timeout, honest unavailable message and non-blocking sign-in continuation.
- [Another initializer wins] → locked server check returns conflict; stale tab loses its create action and offers sign-in.
- [Creation succeeds, sign-in fails] → retain completed state and offer sign-in without repeating setup.
- [Evidence contains credentials] → no trace/video, mask fields, no request-body logging; synthetic accounts only.

## Migration Plan

No schema migration. Deploy API and frontend together; response additions are backward-compatible. Roll back the bundle/API without deleting initialized accounts.

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe" -->
<!-- pdac:cite id="BR-WSP-BREACHED-PASSWORD-CHECK" digest="sha256:8593dbb34d2881761fd788ab1ac34905d59270419aa1c918b0b84603658d6a34" -->
