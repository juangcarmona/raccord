<!-- pdac-scope: cited -->

## Why

The signed-in header currently exposes the login email, sign-out, and a workspace-local language selector as unrelated controls. Users need a clear account area for their own display name and preferred language, with workspace-wide settings remaining separate.

<!-- pdac:cite id="UC-WSP-EDIT-PROFILE" digest="sha256:110a8e418914abd5f8a156d5729ec9a0055972823b845b5d17c5e1873d6b99ae" -->
<!-- pdac:cite id="UC-WSP-SET-LANGUAGE" digest="sha256:4e1830ebc5acc09bdedf5900f8543f6d29ee5a13439f608dfb055a43bf972aed" -->
<!-- pdac:cite id="FR-WSP-016" digest="sha256:a6318f734164595fecbe906b6be4b7e17830d40f1e32d1db2fc161f5a3ad63d5" -->
<!-- pdac:cite id="FR-WSP-018" digest="sha256:38cd7c21cde0b0d0037f1475e74e69dd454a153b247d804ff2f862bfa1d0db6d" -->

## What Changes

- Add a signed-in account page reachable from the application header.
- Let a user maintain their own display name and supported personal language preference.
- Show the verified sign-in email as read-only account information.
- Replace the loose header controls with an account entry point and a clear sign-out action.
- Provide a permission-aware route from the account page to the existing role administration page.

This does not change sign-in email, merge a user account with a crew record, or alter workspace language, company data, or role permissions.

## Capabilities

### New Capabilities

- `personal-account`: signed-in users can inspect and maintain their own basic profile and personal language preference.

### Modified Capabilities

None.

## Impact

- Frontend: authenticated shell, routes, account page, and Spanish/English message catalogs.
- API: authenticated `/api/auth/me` response and a self-service profile update operation.
- Persistence: optional display name and preferred language on the local identity user, with an EF Core migration.
- Generated OpenAPI and TypeScript client contract.
- Tests: Workspace web/API tests, frontend tests, and an AppHost Playwright journey.

<!-- pdac:cite id="BR-WSP-PROFILE-OWNED-BY-PERSON" digest="sha256:cc05019851b877d398a0a749334a59b40bc905731e3744f87b67532ba493f7c6" -->
<!-- pdac:cite id="BR-WSP-LANGUAGE-SCOPES" digest="sha256:495045d198c600a95725b0796363ad8d29d8690f67b494c4b12be5275178b335" -->
<!-- pdac:cite id="TERM-PERSONAL-PROFILE" digest="sha256:9e904c99957e8115e03adbe906f242aae13fa65eb1dea0e1a4f7bf1667d9675c" -->
<!-- pdac:cite id="TERM-PROFILE-LANGUAGE" digest="sha256:0782e7729b035f366560a1116c249189a7e896cf146614e2f44bf3078889acd8" -->
<!-- pdac:cite id="ACT-CREW-MEMBER" digest="sha256:7e8350f158cc4ac6c30857753bcf74655a043878f28b0d5561343b7092ecfdbe" -->
<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->
