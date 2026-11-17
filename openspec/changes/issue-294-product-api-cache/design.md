<!-- pdac-scope: cited -->

## Context

The published SDLC 0.24.4 fingerprint uses one authenticated API call per model file. See proposal.md for the failed native execution and unchanged approved product scope.

## Goals / Non-Goals

Reduce read requests while preserving every native admission, exact-head approval, model digest, provider dry-run, apply and publication check. The cache never handles mutations, other repositories, unknown object IDs or non-model files.

## Decisions

Check out the manually selected workflow revision without persisted credentials. For an exact model-blob GET, read the immutable object from this checkout, enforce the provider's 64 KiB bound and recompute its Git SHA-1 with the blob header before returning the normal API shape. The native API tree still supplies the authoritative object ID, so cached data cannot substitute another revision's bytes. All unmatched requests use the original network function unchanged.

Load the adapter only for router/apply verification and publication commands through a Node preload. Keep the published harness and its provider packages unchanged. Regenerate the workflow lock using installed gh-aw v0.93.2 and manually replay the original approval comment from this recovery branch, with no merge.

Waiting and replaying without changing reads risks repeating the same per-job budget failure. Raising permissions, issuing credentials and manually applying or manufacturing provider receipts are excluded.

Use the supported `LIGHTHOUSE_MODEL_PROFILE: plan` task selection in the recovery branch's Product Change workflow. The operator already provisions this profile with review capabilities. Native run44039134527 still selected the default profile after a host-side atomic config replacement: the running container retained its read-only bind mount of the old file inode. Restore that temporary host assignment and select the task profile directly; do not restart a runner or change another worker. Regenerate the lock and check `runtime.json` before claiming the selection took effect. The selected model must still perform the complete integrity review and the unchanged trusted host must independently validate its candidate.

## Risks / Trade-offs

Recovery router44045324725 received403 on its first approval-comment read, with zero cache hits. Record only allowlisted numeric rate-limit and retry headers on denied repository GETs to distinguish exhausted request allowance from other denials. Never log request headers, response bodies or credentials; return the original response unchanged and preserve every authorization check.

The cache is bounded to this repository and its existing model path; a changed or missing object falls back to normal authenticated reads. Git checkout availability is now a verification prerequisite. Fail closed on malformed or oversized local objects and record native workflow results separately from local tests.

FR-WSP-013 remains unchanged on this branch; only trusted ProductShape apply on PR #450 may apply its approved change.

<!-- pdac:cite id="FR-WSP-013" digest="sha256:71858127bfa1e12d421b7f6568afb1b982b6ee763d53bed2feb94e3104f6e5bc" -->
