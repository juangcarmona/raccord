<!-- pdac-scope: cited -->

## Why

Trusted product apply for #294 stopped at a GitHub blob read with HTTP 403 before the provider ran. Its accepted-model fingerprint reads 1,928 model files separately; the failed blob is entry 989, consistent with an exhausted API budget rather than a product decision failure.

## What Changes

Add a branch-scoped host read cache for immutable product-model Git blobs. Verify each cached byte sequence against its requested Git object ID, and leave authorization, current tree reads, digest computation, provider validation and publication with the unchanged pinned Lighthouse harness.

## Capabilities

No product or specification behavior changes. This tooling-only recovery sets `skip_specs: true` and supports the already-approved Product Change on PR #450.

## Impact

Only the recovery branch's router and trusted-apply workflows use the cache. The existing approval comment is replayed through the supported owner dispatch; no new approval, credential, accepted-model write, merge, release or production operation is introduced by this change.

FR-WSP-013 is reviewed only as context for the existing product decision. Its change remains owned by CHG-WSP-FIRST-ADMIN-BROWSER on PR #450.

<!-- pdac:cite id="FR-WSP-013" digest="sha256:71858127bfa1e12d421b7f6568afb1b982b6ee763d53bed2feb94e3104f6e5bc" -->
