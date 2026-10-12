# Issue 70 runtime evidence

Captured from the Vite-served React app at `/projects` using Chromium. The Vite API proxy was pointed at a temporary local stub returning an empty list, so this records the real application shell, projects empty state, and responsive layout—not backend integration or populated workflows.

- `projects-desktop.png` — 1440 × 1000
- `projects-narrow.png` — 390 × 844
- `project-creation-desktop.png` — 1440 × 1000
- `project-creation-narrow.png` — 390 × 844
- `customers-desktop.png` / `customers-narrow.png`
- `customer-creation-desktop.png` / `customer-creation-narrow.png`
- `drafts-desktop.png` / `drafts-narrow.png`
- `invoices-desktop.png` / `invoices-narrow.png`
- `issuer-settings-desktop.png` / `issuer-settings-narrow.png`

List routes show their existing empty states because the stub returns empty collections. The new-project screenshot shows the existing “register a customer first” prerequisite; the customer-creation screenshot shows its form. Project, draft, and invoice detail routes and populated data states were not captured. The captures use the English UI only. Full visual acceptance still requires Owner review against the mapped references and all route states.
