<!-- pdac-scope: cited -->

## 1. Record and index the boundary

The tasks preserve the current operator-responsibility boundary and the separate no-compliance-claim constraint; they do not change either product rule.

<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

- [x] 1.1 Add ADR-0029 describing the product/deployment responsibility boundary, linking the two ProductShape constraints, PR #99 and research question 1; explicitly avoid asserting statutory liability or external legal advice. Verify its number, references and wording against `docs/adr/README.md` and the cited sources.
- [x] 1.2 Add ADR-0029 to `docs/adr/README.md` and arc42 section 9 while preserving existing ADR-0023. Verify both links resolve to the new record.

## 2. Reconcile architecture and follow-ups

- [x] 2.1 Update risk R-2 and the compliance-seam overview to summarize and link the boundary, retain the no-compliance-claim/legal-review gate, and add current ProductShape citations. Verify the statements do not claim verified compliance.
- [x] 2.2 Add a process-ledger entry recording that a stacked PR merge did not integrate into `main`, with a default-branch ancestry and file-presence check. Verify the entry names an observable follow-up check.
- [x] 2.3 Re-read #325–#328 and update only dependency wording that incorrectly treats #59 as resolving detailed tax semantics; while ADR-0029 is pending, cite draft PR #390 as its source, then retain the permanent ADR reference after it reaches `main`. Preserve each issue's official-specification, legal-review and Product Change prerequisites.

## 3. Validate the documentation change

- [x] 3.1 Run Markdown lint on the changed Markdown files and `git diff --check`; verify ADR and relative links.
- [x] 3.2 Run `npx -y @prodshape/cli@0.22.0 citations verify --provider openspec` and `npx -y @fission-ai/openspec@1.12.0 validate --all --no-interactive`; resolve diagnostics for this change without editing ProductShape or hiding unrelated failures.
- [x] 3.3 Review the final diff against the acceptance criteria and confirm no product behavior, API, database, credentials or production configuration changed.
