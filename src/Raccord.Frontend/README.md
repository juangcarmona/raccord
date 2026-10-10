# Raccord frontend

React 19 + TypeScript + Vite. It talks to the backend only through the generated OpenAPI contract (ADR-0011).

```text
src/app        application shell, routes, not-found page
src/features   projects | customers | invoicing | organization (one folder per workflow area)
src/shared     api (generated schema.d.ts + typed client) | i18n (es, en) | format (Intl) | hooks | ui
openapi/       OpenAPI document generated from Raccord.Web
```

## Scripts

| Script | Purpose |
|---|---|
| `npm run dev` | Vite dev server; proxies `/api` and `/health` to `RACCORD_API_URL` (Aspire injects it) |
| `npm run build` | Type check and production build into `dist/` (copied into the web image's `wwwroot`) |
| `npm run lint` | ESLint: strict type-checked rules, React hooks, jsx-a11y strict |
| `npm run typecheck` | TypeScript for app and config files |
| `npm test` | Vitest + Testing Library |
| `npm run api:generate` | Regenerate `openapi/raccord.json` from the backend and `src/shared/api/schema.d.ts` |

## Rules

- No business calculations: amounts, VAT and totals come from the API; the UI only formats them.
- Server rule codes are localized through `describe('issue' | 'error', code)`; add Spanish and English messages for new codes.
- Calendar dates (`yyyy-mm-dd`) are formatted with `format.date` so they never shift across time zones.
- Every control has a visible or accessible label; pages start with a focusable `h1` via `PageHeader`.
