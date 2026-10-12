# Issue 70 runtime evidence

Captured from the Vite-served React app at `/projects` using Chromium. The Vite API proxy was pointed at a temporary local stub returning an empty list, so this records the real application shell, projects empty state, and responsive layout—not backend integration or populated workflows.

- `projects-desktop.png` — 1440 × 1000
- `projects-narrow.png` — 390 × 844
- `project-creation-desktop.png` — 1440 × 1000
- `project-creation-narrow.png` — 390 × 844

The new-project screenshot shows the existing “register a customer first” empty state because the stub returns no customers; it does not show the project form. Other route-specific screenshots and data-backed workflows were not captured in this environment. The issue’s full visual acceptance criterion still requires Owner review against the mapped references.
