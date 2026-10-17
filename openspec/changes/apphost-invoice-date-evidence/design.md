<!-- pdac-scope: cited -->

## Context

The invoice finalization journey already creates synthetic customer and project records, finalizes an invoice through the browser, and reads the issued representation through the API. The frontend sends a nullable `issueDate`; the server applies the organization-time-zone date when it is null. The AppHost uses the default organization zone, `Europe/Madrid`.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="FR-INVOICE-006" digest="sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUE-DATE-ORDER" digest="sha256:330d7738dac277884a78466f76c771e7a88c63c2cd3211a5318c33d418792aca" -->

## Goals / Non-Goals

**Goals:** prove through Playwright and the real AppHost that the browser sends a null date when untouched, the server's organization date is recorded, and an explicit date survives finalization; run both journeys in the bounded per-PR suite.

**Non-Goals:** change date defaults, server validation, invoice numbering, application behavior, or external integrations.

## Decisions

- Extend the existing invoice-finalization E2E class so the journeys reuse its authenticated browser context and synthetic customer/project setup.
- Freeze only the browser clock at `2099-01-01T12:00:00Z` for the blank-date journey. This makes browser and server dates observably different without changing server time or introducing a test-only production seam.
- Assert the issue-date input is empty and the finalize request carries JSON `null`; then read the issued invoice and compare its date with `IssuedAt` converted to `Europe/Madrid`.
- In the explicit-date journey, select the organization's current date, assert the serialized request value, and assert the saved invoice has the same date.
- Keep the journeys under the existing Invoicing trait and add their fully qualified names to `per-pr-selection.txt`.
- No test framework or production dependency is added.

## Risks / Trade-offs

The time-zone expectation intentionally names the AppHost's configured default, `Europe/Madrid`; if the fixture later changes its organization zone, this assertion must follow that fixture configuration. The frozen browser date is deliberately far from the server date so accidental browser defaults fail clearly.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="FR-INVOICE-006" digest="sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUE-DATE-ORDER" digest="sha256:330d7738dac277884a78466f76c771e7a88c63c2cd3211a5318c33d418792aca" -->
