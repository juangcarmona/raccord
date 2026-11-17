# Contacts organization verification

<!-- pdac-scope: cited -->

## Delivery scope

The owner explicitly authorized Codex to deliver and merge #299, #301 and #302 in that order. Actual prerequisite merges are `166fd329cc8f312137ecc0abb81052a1ee8793dc` and `5d5052b8f599d5b2e0481764cd52275083a40908`; only after both merges was #302 activated. Existing PR #446 is reused. No production operation, credential change, shared history rewrite, new production dependency or canonical product-model edit is included.

Historical source `fc2a21670ec3f72e3d146932a45ea633a16a1bc6`, based on `8add3aa44e5fdef6cb7d5ca93ab887eb0a6bf673`, supplied 33 selectively adapted source/test paths. Historical product files, planning documents, generated contracts and migrations were excluded. The additive migration `20261117032821_ContactOrganization` and contracts were generated against the delivered base.

<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:ed07fa1bacf8f84b6e6e255289a94b5cfe2ac70d4da2893823b086ab7fc57e79" -->

## Reviewed behavior

All organization mutations acquire database-local transaction advisory lock `(302, 1)` before validation and hold it through commit or rollback. Deterministic real-PostgreSQL tests queue competing requests behind a held lock and prove opposing folder moves, reordering, folder deletion versus filing or child creation, tag deletion versus assignment and duplicate tag creation. Restrictive foreign keys remain. Cancellation and unexpected infrastructure failures propagate; a synthetic trigger proves rollback rather than a disguised validation response. Tag names containing whitespace are rejected, including leading and trailing whitespace.

Any/all filters normalize repeated IDs and AND with role and folder before stable contact-number/id pagination. API tests cover unfiling, rename, empty deletion, nonempty deletion refusal, unknown targets and separate installation databases with identical names. Every organization endpoint enforces existing read/manage permissions. Upgrade, rollback and re-upgrade retain existing ordinary and imported contacts; rolled-back organization data is intentionally removed. No production rollback was executed.

Tag removal retains the historical safe refusal while assignments exist: explicit unassignment then removal is supported, matching the approved delivery scenario. No contact or assignment is silently removed. Contact import transactions, administrative designation changes and snapshots remain on their integrated implementations.

## Local checks

- Backend: 17 architecture, 7 analyzer, 4 building-block, 132 domain and 11 infrastructure tests passed. The complete Web run passed 92 tests; the subsequently added rename/unfile/delete regression passed separately, giving 93 distinct Web tests and 264 distinct backend tests. These are separate terminal runs, not a single invented result.
- Frontend: all 56 tests, lint and production build passed, including server error/disabled controls during pending organization saves. The first new error fixture used `message` instead of the actual RFC problem `detail`; its failure remains preserved and the corrected fixture passes.
- Browser: the focused directory/import/organization selection passed all four cases. The complete selected suite passed 42 cases, zero failed/skipped, approximately 142 seconds. The manifest contains 40 journeys plus two metadata cases. The full browser run preceded only the extra API rejection of boundary whitespace; the final-head CI must independently qualify the published source.
- Four synthetic organization error-state captures at widths 1280/390 and light/dark were inspected by Codex against the design contract and adopter CSS. Keyboard focus proceeds from New tag to Create tag, no horizontal page overflow occurs, and reduced-motion mode is exercised. The captures use actual OS color scheme and `data-appearance`. This is operator review under existing owner delivery authority; no separate human visual signoff is claimed.
- Strict OpenSpec, the unchanged 1,918-artifact ProductShape model with zero diagnostics, all 597 current citations, the exact 40-journey manifest and Markdown lint passed before commit. Commit hooks and final-head CI are recorded separately when terminal.

## Retained receipts

Local terminal logs are `/tmp/raccord-302-backend-final.log`, `/tmp/raccord-302-unfile-final.log`, `/tmp/raccord-302-ui-tests-final-green.log`, `/tmp/raccord-302-ui-lint-final-green.log`, `/tmp/raccord-302-ui-build-final.log`, `/tmp/raccord-302-browser-feature.log`, `/tmp/raccord-302-browser-full.log`, `/tmp/raccord-302-contract-final.log`, `/tmp/raccord-302-product.log`, `/tmp/raccord-302-citations.log`, `/tmp/raccord-302-selection.log` and `/tmp/raccord-302-format-final.log`. Rendered evidence and SHA-256 hashes are outside Git at `/home/juan/.local/share/raccord-delivery-watch/qualification/organization-recovery/302-rendered/receipt.json`.

Preserved failures include the initial compatibility-path migration command, the pre-lock opposing-move regression, analyzer corrections and the two boundary-whitespace rejections that initially returned 201. They are not passing evidence. Required current-head CI and the owner-authorized expected-head merge remain pending.
