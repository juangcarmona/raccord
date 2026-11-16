# Product reconciliation tasks

<!-- pdac-scope: cited -->

The following artifacts bind the approved product scope.

<!-- pdac:cite id="FR-WSP-030" digest="sha256:3106f257093a9439e1653c31c1876a6945d04ea9e4880be382388d0a9c41253d" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e" -->
<!-- pdac:cite id="BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION" digest="sha256:d6491901cbe82003e5ce323b7a979d0a2a6c33be2383d97e5db0f65920206371" -->
<!-- pdac:cite id="SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION" digest="sha256:1c6c3ce42e36bc172f3b6e66bc595e0223a0673b03bfe62dba9978176a0f21f7" -->

## 1. Reconcile and apply

- [x] 1.1 Recover the six immutable source artifacts and the existing owner decisions without restoring the historical model.
- [x] 1.2 Preserve current relationship typing and justify all four unchanged direct impacts with provider digests.
- [x] 1.3 Validate the overlay without diagnostics, review the dry-run and explicitly apply the approved change through ProductShape.

## 2. Qualify and publish

- [x] 2.1 Verify the applied model, all citations, strict OpenSpec change and repository Markdown; inspect the exact product diff.
- [ ] 2.2 Publish the operator recovery PR and inspect its actual current-head CI. Keep native blocked proof, main acceptance and runtime delivery separate.
