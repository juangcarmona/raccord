<!-- pdac-scope: none reason="This design covers HTTP response hardening and its engineering tests without defining product behavior." -->

# Design: Tighten browser Content Security Policy

## Context

The production container serves the built SPA from `Raccord.Web`'s web root, and the response middleware in `src/Raccord.Web/Program.cs` adds CSP to web responses. Aspire E2E runs the API and Vite frontend as separate resources, so navigating only to Vite cannot verify the production CSP.

## Decisions

- Keep the policy at the existing response middleware boundary, where it applies consistently to static files, SPA routes, API responses, and health responses.
- Set `style-src` and `connect-src` to `'self'`; the frontend uses external stylesheets, and its production API calls are same-origin.
- Disable Vite production source maps so the published bundle has no `sourceMappingURL` comment or companion `.map` file.
- Expose the Aspire web endpoint URL from `DistributedAppFixture` and assert CSP on `/health/ready` with Playwright's request context before continuing the existing seeded-project browser journey.
- Assert the complete CSP string in the web hosting test so future policy broadening is reviewed explicitly.

## Alternatives considered

- Testing only Vite would miss the response middleware used by the production container.
- Adding `Sec-Fetch-*` response headers would misread scanner messages about incoming request metadata as missing response headers.
- Adding a nonce or hash for inline styles is unnecessary because the current frontend does not require inline style attributes.

## Product model review

No ProductShape artifacts are impacted because the change governs browser transport policy and does not alter product behavior, terminology, or domain rules.
