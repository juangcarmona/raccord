# Process ledger

This ledger records repeatable delivery improvements for future agents.

## 2026-10-13 · Contact-person slice

- Observation: A feature branch can be a moving integration target when another agent owns the prerequisite child.
- Action: Stack the next child on the prerequisite branch in a separate worktree, and keep the prerequisite checkout untouched.
- Check: The PR base is the prerequisite feature branch and the source worktree remains agent-owned.
