<!-- pdac-scope: cited -->

## Context

The published SDLC 0.24.4 fingerprint uses one authenticated API call per model file. See proposal.md for the failed native execution and unchanged approved product scope.

## Goals / Non-Goals

Reduce read requests while preserving every native admission, exact-head approval, model digest, provider dry-run, apply and publication check. The cache never handles mutations, other repositories, unknown object IDs or non-model files.

## Decisions

Check out the manually selected workflow revision without persisted credentials. For an exact model-blob GET, read the immutable object from this checkout, enforce the provider's 64 KiB bound and recompute its Git SHA-1 with the blob header before returning the normal API shape. The native API tree still supplies the authoritative object ID, so cached data cannot substitute another revision's bytes. All unmatched requests use the original network function unchanged.

Load the adapter only for router/apply verification and publication commands through a Node preload. Keep the published harness and its provider packages unchanged. Regenerate the workflow lock using installed gh-aw v0.93.2 and manually replay the original approval comment from this recovery branch, with no merge.

Waiting and replaying without changing reads risks repeating the same per-job budget failure. Raising permissions, issuing credentials and manually applying or manufacturing provider receipts are excluded.

## Risks / Trade-offs

The cache is bounded to this repository and its existing model path; a changed or missing object falls back to normal authenticated reads. Git checkout availability is now a verification prerequisite. Fail closed on malformed or oversized local objects and record native workflow results separately from local tests.

FR-WSP-013 remains unchanged on this branch; only trusted ProductShape apply on PR #450 may apply its approved change.

<!-- pdac:cite id="FR-WSP-013" digest="sha256:71858127bfa1e12d421b7f6568afb1b982b6ee763d53bed2feb94e3104f6e5bc" -->
