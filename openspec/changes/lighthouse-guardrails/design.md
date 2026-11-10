<!-- pdac-scope: none reason="Repository agent harness and verification tooling only; no product behavior changes." -->

# Design

Lighthouse owns its generated manifest projection, commit-message hook, pre-push hook, and verification workflow. Raccord keeps its existing analyzer and architecture-test enforcement; the Lighthouse .NET profile adds guidance without installing overlapping analyzer code. The React profile requires visual-theme, so both are selected for the existing browser application.

The pinned SDLC package installs its default triage and refine workflows through `install-workflows.mjs` in package mode. `gh aw v0.93.2` compiles their lockfiles. The router requires `LIGHTHOUSE_AUTOMATION_ENABLED=true` before dispatch, and that repository variable is not set by this change.

The Claude Code PreToolUse command invokes a dependency-free Node script. The script reads `tool_input.command` from stdin, exits 2 with a repair message for hook bypasses, and exits 0 for ordinary calls or malformed input. A shell wrapper also treats script launch failures as nonblocking. The guard does not change Git or Husky behavior for human users.

The repository already mirrors its prior commitlint policy in the CI lint job. Lighthouse's generated commit hook remains byte-for-byte managed so `lighthouse verify` can check it.
