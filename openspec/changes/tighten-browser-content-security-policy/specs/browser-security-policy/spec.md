<!-- pdac-scope: none reason="This specification defines an HTTP security response contract, not product behavior." -->

## ADDED Requirements

### Requirement: Content Security Policy limits browser resource sources

The web application SHALL return a Content-Security-Policy that restricts styles and connections to the same origin and does not permit inline styles or wildcard protocol sources.

#### Scenario: Browser policy on the web response

- **WHEN** a client requests a web application response
- **THEN** `style-src` and `connect-src` allow only `'self'`
- **AND** the policy does not contain `unsafe-inline`, `ws:`, or `wss:`

### Requirement: Production assets do not publish source maps

The frontend production build SHALL NOT publish source-map files or a JavaScript `sourceMappingURL` comment.

#### Scenario: Production JavaScript bundle

- **WHEN** the frontend production build completes
- **THEN** its output contains no `.map` files
- **AND** JavaScript assets contain no `sourceMappingURL` reference

#### Scenario: SPA remains usable with the restricted policy

- **WHEN** the hosted browser journey checks the web resource policy and opens the SPA
- **THEN** the seeded projects list renders successfully
