<!-- pdac-scope: none reason="Implementation tasks concern CI maintenance and do not depend on product requirements; this exemption is approved by the Owner for issue #247." -->

## Tasks

- [x] Add and test a changed-path classifier that treats only non-empty OpenSpec-only diffs as documentation-only.
- [x] Route the main CI workflow so OpenSpec-only changes keep Markdown, OpenSpec, ProductShape and security validation while skipping unrelated application jobs.
- [x] Prevent OpenSpec-only pushes to `main` from starting the post-merge runtime probe.
- [ ] Obtain a green hosted CI run for workflow routing, ProductShape citation verification, and Markdown/OpenSpec validation.
