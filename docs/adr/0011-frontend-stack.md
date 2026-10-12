---
name: frontend-stack
description: The frontend is a React and TypeScript single-page app built with Vite that talks to the API through a typed client generated from the OpenAPI contract.
status: Accepted
date: 2026-10-06
deciders: ["Juan G. Carmona"]
tags: ["frontend", "react", "openapi", "accessibility", "localization"]
---

# ADR-0011: Build the frontend with React and Vite on OpenAPI-generated types

## Context

Raccord needs a web UI that the API serves in production. Business rules live in the backend, so the UI must consume the API's contract without a hand-maintained copy of it. The UI is localized (Spanish and English) and must be accessible.

## Decision

We will build the frontend with React, TypeScript and Vite, and type the API client from the generated OpenAPI contract.

- React 19 + TypeScript (strict, `noUncheckedIndexedAccess`, `exactOptionalPropertyTypes`), Vite, React Router (data router). Feature folders under `src/features/*`, shared building blocks under `src/shared/*`.
- **Contract-first client:** the Web project generates `openapi/raccord.json` at build time (`Microsoft.Extensions.ApiDescription.Server`); `openapi-typescript` turns it into `schema.d.ts`; `openapi-fetch` provides a typed client. CI fails if the committed contract drifts. The frontend imports nothing else from the backend.
- JSON number handling is strict so decimals are typed as `number`.
- No UI component library: semantic HTML and a small CSS file with light/dark tokens. Accessibility is linted with `eslint-plugin-jsx-a11y` (strict) and tested with Testing Library queries by role and label.
- Localization: a typed message catalog (es, en) and `Intl` formatters. Server rule codes (e.g. `recipient.tax-id-missing`) are localized by code; the server's English message is the fallback. Date-only values are formatted in UTC so they never shift a day.
- Tooling versions: TypeScript 5.9 and ESLint 9 (typescript-eslint and jsx-a11y do not yet support TypeScript 7 / ESLint 10).
- Production: the built `dist` is copied into the web container's `wwwroot`; the API serves it with an SPA fallback that excludes `/api`, `/health`, `/openapi` and file paths. Development: Vite dev server proxies to the API (Aspire injects the address).

## Consequences

- **Positive:**
  - Business rules live only in the backend; the UI disables actions based on server-reported issues.
- **Negative:**
  - Tooling stays on TypeScript 5.9 and ESLint 9 until typescript-eslint and jsx-a11y support TypeScript 7 / ESLint 10.
- **Neutral:**
  - New dependencies: `react`, `react-dom`, `react-router`, `openapi-fetch` (runtime); test/lint/build tools (dev).

## Alternatives considered

- **Hand-written API types or imports from the backend:** rejected; the client is typed from the generated OpenAPI contract, CI fails if the committed contract drifts, and the frontend imports nothing else from the backend.
- **A UI component library:** rejected in favour of semantic HTML and a small CSS file with light/dark tokens, keeping the runtime dependencies to those listed above.
- **TypeScript 7 / ESLint 10:** rejected for now because typescript-eslint and jsx-a11y do not yet support them.

## References

- Related: ADR-0013, ADR-0015
