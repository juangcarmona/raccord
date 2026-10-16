<!-- pdac-scope: none reason="These tasks deliver architecture documentation and static-site tooling only; they do not change product semantics." -->

# Tasks

## 1. Visual contract and evidence base

- [x] 1.1 Add the arc42 diagram selection, visual language, accessibility and source-evidence guide with a PR checklist; verify Markdown lint and that the static-site renderer can load the supporting link.
- [x] 1.2 Verify each planned diagram against host/module composition, API/runtime source and Compose/Azure deployment files; record any corrected claims in the owning arc42 section.

## 2. Mermaid migration and renderer

- [x] 2.1 Replace ASCII diagrams in sections 03, 05, 06, 07 and 10 with focused Mermaid diagrams that follow the guide; verify every node and edge against implementation or explicitly labeled future scope.
- [x] 2.2 Add the shared theme, accessible diagram title and bounded overflow behavior to `tools/arc42-snapshot`; add focused sanitizer, naming, theme and sizing tests and run `npm test --prefix tools/arc42-snapshot`.

## 3. Static-site validation and evidence

- [x] 3.1 Add source-contract validation for Mermaid accessibility titles and run it against all arc42 Markdown; verify a missing title fails with the file and diagram index.
- [x] 3.2 Extend the focused pull-request validation job to install the pinned renderer, run renderer tests, generate the complete snapshot, capture sections 03/05/06/07/10, and upload the HTML and PNGs; verify unrelated application E2E/build jobs are not added.
- [x] 3.3 Update affected arc42 navigation and process documentation; record the chosen structural-plus-human-visual regression strategy and limitations in the process ledger.

## 4. Tool evaluations and decisions

- [x] 4.1 Evaluate one representative view using a model-based authoring option and compare model ownership, source links, export and maintenance costs with Markdown; save dated source-backed findings privately and record the decision in an ADR. (Documented evaluation; local CLI execution was unavailable because Java was absent and Docker access was denied.)
- [x] 4.2 Evaluate an agent-assisted discovery workflow against the same view; verify every generated claim against repository evidence and save dated source-backed findings privately without promoting generated files to canonical sources.
- [x] 4.3 Attempt the alternate renderer only after styling Mermaid; compare the same difficult deployment/runtime facts for readability, accessibility, layout, export and toolchain cost, then record adopt/defer/reject in the private research and ADR. (The condition was not met: styled Mermaid passed visual review, so D2 was not installed.)

## 5. Full verification

- [x] 5.1 Run Markdown lint, OpenSpec validation, ProductShape citation verification, renderer tests, source-contract validation and a final local snapshot generation; record exact outcomes and any unrunnable CI-only checks.
- [x] 5.2 Review the final diff against issues #253–#260 and the actual evidence; keep the task list aligned with delivered behavior and document unresolved limitations.
