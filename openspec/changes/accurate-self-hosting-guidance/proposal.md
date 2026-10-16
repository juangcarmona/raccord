<!-- pdac-scope: cited -->

## Why

The self-hosting guide says authentication is not implemented, although sign-in and authorization now exist. It also omits the one-time administrator setup needed before a new installation can be used.

## What Changes

- Explain that the first administrator is created through a one-time unauthenticated setup endpoint and that sign-in is required afterward.
- Document the current Docker Compose startup, health check, first-user setup and issuing-company steps.
- Keep the initial setup and database on a trusted network, state that there is no public registration, and retain the TLS and exposure guidance.
- Keep the public homepage link pointed at the canonical self-hosting guide and the repository contribution path.

## Capabilities

### Modified Capabilities

- `self-hosting`: the maintained installation path describes the current first-user and access-control behavior.

## Product definition impact

This updates operating instructions for the existing installation administrator and sign-in use case. It adds no application behavior.

<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
