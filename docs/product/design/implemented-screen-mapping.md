# Implemented screen mapping

This maps current frontend routes to the closest documented screen and trace. The portfolio is a design reference, not evidence that every depicted workflow exists. Keep rendered controls within current frontend/API behavior.

| Implemented route | Closest reference | Current implementation boundary / gap |
| --- | --- | --- |
| `/projects` | `project-detail` shared project/table patterns | Use existing project list data and actions only; no schedule, tabs, or template flows. |
| `/projects/new` | `project-creation` | Current API-backed name/customer/date fields only; omit template selection and preview. |
| `/projects/:id` | `project-detail` | Existing project and billable-work behavior only; no unsupported schedule or document tabs. |
| `/customers` | `contact-detail` shared identity/metadata patterns | No exact customer-list screen in the portfolio; retain the existing list and record this as a portfolio gap. |
| `/customers/new` | `contact-detail` form patterns | Current customer fields only; no extra commercial profile or contact workflows. |
| `/drafts` | `invoicing-queue` | Drafts supported by existing endpoints; omit unsupported filters and queue actions. |
| `/drafts/:id` | `invoice-draft-editor` | Existing editable draft fields and lines only; no payment or issue workflow beyond supported actions. |
| `/invoices` | `invoice-list` | Existing invoice list/status only; do not imply unsupported KPI, bulk-selection, or filtering behavior. |
| `/invoices/:id` | `invoice-list` detail/table/status patterns | Existing invoice detail and audit data; no new actions. |
| `/settings/issuer` | `issuer-profile` | Current issuer fields only; omit unsupported company profile and branding controls. |

Shared shell: follow `docs/design/visual-direction.md`, `color-tokens.md`, and `logo-usage.md`. For behavior provenance consult `docs/product/design/screen-traces.md`; for the larger route portfolio consult `screen-backlog.md` and `raccord-screens.pen`.
