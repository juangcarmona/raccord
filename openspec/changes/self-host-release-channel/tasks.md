<!-- pdac-scope: cited -->

## 1. Record the release policy

- [x] 1.1 Write ADR-0026 with the supported Compose and official-image policy, rejected alternatives, migration recovery limits, and support window.
- [x] 1.2 Owner accepted ADR-0026 on 2026-10-16 before implementation began.

## 2. Deliver the operator path

- [ ] 2.1 Publish versioned application images for tagged releases and pin image references in Compose.
- [x] 2.2 Update the install and upgrade instructions to use the supported release, verified backup, migration, health, and recovery steps.
- [ ] 2.3 Verify clean install, prior-release upgrade, migration failure, backup/restore, and preservation of invoice, audit, and document data.

## 3. Validate

- [x] 3.1 Run the relevant CI and container checks for the proposed policy documentation.
- [x] 3.2 Validate this OpenSpec change and verify ProductShape citations.

<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
