# ADR-0001: Start with a modular monolith

- Status: accepted
- Date: 2026-10-06

## Decision

Start with one deployable application organized into explicit domain modules. Extract services only when a measured operational or ownership need justifies it.

## Consequences

Local and self-hosted deployment stay simple, and end-to-end workflows remain cheap to test. Module boundaries must be enforced in code review and tests.
