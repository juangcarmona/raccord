# Process ledger

This ledger records repeatable improvements discovered during delivery reviews. Each entry names the observation, the durable action, and the check that prevents regression.

## 2026-10-12 · Epic #16 module architecture

- Observation: A final integration PR exposed inherited commit-message failures that child PRs did not see against their stacked feature branch.
- Action: Run the complete CI-equivalent gate against the eventual target branch before opening or merging the final integration PR, including commitlint over the full target range.
- Check: The final PR's lint job must pass over `main` to head, not only over the immediately preceding stacked branch.

- Observation: Moving the Aspire host changed both endpoint discovery and the relative Vite path.
- Action: Treat host relocation as a runtime change and exercise the AppHost E2E journey before integration.
- Check: The E2E job must boot the AppHost, pass health checks, and complete the selected journeys.

- Observation: Implemented child issues remained open because merged PRs did not consistently carry closing references.
- Action: Reconcile every child issue during the integration review and close stale implementation issues with links to the merged PRs.
- Check: The review records the final state of the epic and every delivered child issue.

- Observation: The local merge command could not check out `main` because another worktree owned it.
- Action: Use a worktree-safe merge path and never disturb another agent's checkout.
- Check: Verify the remote merge result and worktree status after integration.

## Review rule

Every completed FF loop must review this ledger, add new observations and actions when useful, and verify that existing actions were followed. The ledger is process memory for future agents; it is not a substitute for CI or product documentation.
