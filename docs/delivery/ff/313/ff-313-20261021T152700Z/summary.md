# Fast Forward run ff-313-20261021T152700Z

- Work item: #313 — Book equipment out and back by manual selection in the Warehouse UI.
- Implementation branch and PR: `codex/313-warehouse-manual-booking`, [PR #423](https://github.com/juangcarmona/raccord/pull/423), merged into `codex/epic-376-warehouse`.
- The project detail page now exposes the next valid warehouse transition for each packing line: pack, manual book-out and return. Manual book-out sends `scanCode: null` through the existing API boundary and reloads authoritative project state.
- Frontend tests passed (5/5), the focused Playwright journey passed (1/1), solution build passed with zero warnings/errors, and all required remote CI jobs passed including the E2E battery.
- The OpenSpec change `warehouse-manual-booking-313` is valid and cites the existing manual and scanned booking product rules; no canonical Product Definition files were edited.
- The Owner explicitly waived the independent-reviewer gate for this unattended cycle; deterministic checks and forge policy remained enforced.
