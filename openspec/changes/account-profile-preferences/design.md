<!-- pdac-scope: cited -->

# Design

<!-- pdac:cite id="UC-WSP-EDIT-PROFILE" digest="sha256:110a8e418914abd5f8a156d5729ec9a0055972823b845b5d17c5e1873d6b99ae" -->
<!-- pdac:cite id="UC-WSP-SET-LANGUAGE" digest="sha256:4e1830ebc5acc09bdedf5900f8543f6d29ee5a13439f608dfb055a43bf972aed" -->
<!-- pdac:cite id="BR-WSP-PROFILE-OWNED-BY-PERSON" digest="sha256:cc05019851b877d398a0a749334a59b40bc905731e3744f87b67532ba493f7c6" -->
<!-- pdac:cite id="BR-WSP-LANGUAGE-SCOPES" digest="sha256:495045d198c600a95725b0796363ad8d29d8690f67b494c4b12be5275178b335" -->
<!-- pdac:cite id="TERM-PROFILE-LANGUAGE" digest="sha256:0782e7729b035f366560a1116c249189a7e896cf146614e2f44bf3078889acd8" -->

The authenticated identity is the source of ownership. Add `DisplayName` and `PreferredLanguage` to `RaccordUser`; keep both fields optional/defaulted for existing accounts, and default an absent language to Spanish. Add a migration that does not rewrite or link crew records.

Extend the existing `/api/auth/me` response with the current user's display name, preferred language, and permission-aware role-management capability. Add an authenticated `PUT /api/auth/me` that accepts only display name and supported language, loads the user from the current principal, validates both values, persists them through `UserManager`, and returns the updated representation. The sign-in response should use the same current-user representation. Never accept an email or user id as the profile target.

Add an `/account` page behind the existing auth route. Move the current header's email, language selector, and sign-out into a compact account entry plus sign-out action; use the display name with email fallback in the entry. The account form shows the email read-only and saves display name/language together. On session restoration, initialize the UI language from the API response; after a successful save, update the current UI immediately. Keep local storage only as the anonymous/loading fallback.

The API reports whether the authenticated user has either role-read or role-edit permission based on the user's current role claims. The account page uses that value only to show the existing role-management link; it does not replace endpoint authorization.

No persistent application dependency is added. Change semantics remain within the existing profile ownership and profile-language rules. The structural-impact review also checked account-email uniqueness/change behavior, crew membership, profile sections and workspace settings; these are not changed because this slice does not edit login identity, qualify or link crew records, or change workspace-wide preferences.

<!-- pdac:cite id="FR-WSP-016" digest="sha256:a6318f734164595fecbe906b6be4b7e17830d40f1e32d1db2fc161f5a3ad63d5" -->
<!-- pdac:cite id="FR-WSP-018" digest="sha256:38cd7c21cde0b0d0037f1475e74e69dd454a153b247d804ff2f862bfa1d0db6d" -->
<!-- pdac:cite id="TERM-PERSONAL-PROFILE" digest="sha256:9e904c99957e8115e03adbe906f242aae13fa65eb1dea0e1a4f7bf1667d9675c" -->
<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->
