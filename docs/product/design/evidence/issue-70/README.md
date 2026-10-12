# Issue 70 runtime evidence

Captured from the Vite-served React app using Chromium. The Vite API proxy was pointed at a temporary local fixture server returning representative project, customer, draft, invoice, and issuer data. This records the actual React routes/layouts but does not verify backend integration or business behavior.

- Desktop (1440 × 1000) and narrow (390 × 844) captures exist for every in-scope route: projects list/create/detail, customers list/create, drafts list/detail, invoices list/detail, and issuer settings.
- `invoices-desktop-es.png` and `invoices-narrow-es.png` show the Spanish invoice-list locale.

The static fixture uses synthetic data only. The captures were opened and visually reviewed at both sizes; they do not replace API-backed end-to-end verification. Owner approval of visual acceptance against the mapped references remains pending.
