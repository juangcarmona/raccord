# Process ledger

This ledger records repeatable delivery improvements for future agents.

## 2026-10-13 · Contact-person slice

- Observation: A feature branch can be a moving integration target when another agent owns the prerequisite child.
- Action: Stack the next child on the prerequisite branch in a separate worktree, and keep the prerequisite checkout untouched.
- Check: The PR base is the prerequisite feature branch and the source worktree remains agent-owned.

## 2026-10-13 · Contact import

- Observation: Bulk writes must validate the complete input before staging rows, otherwise a failed import can leave partial data.
- Action: Parse and validate all rows first, then save the complete valid batch once.
- Check: The import contract reports rejected rows and the web test suite exercises the invalid-row atomicity path.

## 2026-10-13 · Contact commercial defaults

- Observation: Contact defaults must not remain a live read through project views, or later edits silently change an existing project.
- Action: Copy defaults at project creation and persist them as project-owned values.
- Check: The project contract exposes copied values and migration-backed tests cover creation after a contact default is set.
