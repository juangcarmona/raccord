<!-- pdac-scope: cited -->

## 1. Canonical tokens and drift guard

- [x] 1.1 Define the canonical CSS tokens for color, typography, spacing, shape, focus, and semantic status; update `docs/design/color-tokens.md`, `docs/design/visual-direction.md`, and `docs/design/README.md` with approved values, usage, and contrast evidence, then verify the design guidance no longer labels the active system exploratory.
- [x] 1.2 Add a dependency-free design-system check and focused tests for missing imports, redefined shared tokens, mismatched values, contrast regressions, and documented surface-specific exceptions; verify the check passes on the three current consumers and fails on each drift fixture.
- [x] 1.3 Invoke the same check from Husky pre-commit and add a dedicated CI job that runs on every pull request and push to main without depending on changed-path classification; verify the local hook rejects a drift fixture and the CI job runs for a public-site-only change.

## 2. App and public site

- [x] 2.1 Make the React app consume the canonical token file and map its existing shared colors, typography, spacing, shape, focus, and status aliases to it; verify frontend lint, typecheck, unit tests, build, and format checks pass and existing routes retain their behavior.
- [x] 2.2 Update the public site styles and generated root locale handoff to use the canonical tokens, retain both full locale pages, and provide functional Spanish and English links without JavaScript; verify its build/test output includes shared tokens and root/locale route assertions pass.

## 3. Engineering site

- [x] 3.1 Map the engineering report shell, generated architecture snapshot, and diagram theme to canonical tokens while keeping static output self-contained; verify the existing static-site and architecture snapshot checks pass and generated output contains no external style dependency.

## 4. Cross-surface verification

- [x] 4.1 Run the CI-equivalent checks for the touched Node and frontend projects, both design-system check entry points, OpenSpec validation, and ProductShape citation verification; verify every command passes on the final plan and implementation branch and confirm whether repository required-check settings make the CI result a merge gate.
- [x] 4.2 Capture representative app, public-site, and engineering-site pages at desktop and narrow widths, in both available locales; verify keyboard focus, readable semantic statuses, and attach the evidence for Owner review.
- [x] 4.3 Review the Definition of Done evidence with the Owner and record the accepted visual direction and any surface-specific exceptions in the pull request before integration.
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
<!-- pdac:cite id="QR-MOBILE-WEB-APP" digest="sha256:ba2d4a17bef47a1512bec2d45e4c9f8fdd6e2fab7a438280959a62ba70bdb18b" -->
