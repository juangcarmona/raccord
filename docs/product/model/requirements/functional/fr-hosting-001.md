---
id: FR-HOSTING-001
type: functional-requirement
title: "Install from documented steps"
status: draft
derived-from:
  - "UC-INSTALL-RACCORD"
  - "CON-SELF-HOSTABLE"
verification:
  - scenario: "Following the documented steps on a server with a container runtime yields a healthy installation that serves the application"
uses-terms: []
---

## Requirement

The product MUST be installable on a single server and on Azure from documented steps, MUST prepare its schema before accepting requests, and MUST expose its liveness and readiness.

## Rationale

Self-hosting is a first-class path.
