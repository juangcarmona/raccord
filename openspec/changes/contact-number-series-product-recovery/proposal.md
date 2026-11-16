# Reconcile the approved contact-number Product Change

<!-- pdac-scope: cited -->

## Why

Issue #397 depends on the existing CHG-CONTACT-NUMBER-SERIES-397 from PR #399. Its historical proposal has not reached the current main product baseline. Native preparation43818486954 reports a host evidence serialization limit, so its blocked result is preserved while this operator recovery uses the complete installed ProductShape provider directly.

## What Changes

Reconcile and explicitly apply the same approved Product Change against b9fec25c8cf85a4ab00326d7be59303e0b3cee48. Preserve current v1alpha2 relationship typing and account for the four unchanged direct consumers of the modified assignment rule. The referenced artifacts define the product scope; this proposal adds no product decision.

<!-- pdac:cite id="FR-WSP-030" digest="sha256:3106f257093a9439e1653c31c1876a6945d04ea9e4880be382388d0a9c41253d" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e" -->
<!-- pdac:cite id="BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION" digest="sha256:d6491901cbe82003e5ce323b7a979d0a2a6c33be2383d97e5db0f65920206371" -->
<!-- pdac:cite id="SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION" digest="sha256:1c6c3ce42e36bc172f3b6e66bc595e0223a0673b03bfe62dba9978176a0f21f7" -->

## Capabilities

### New Capabilities

- `contact-number-series-product-recovery`: Validate and materialize the approved Product Change through ProductShape on the review branch.

### Modified Capabilities

None. Runtime delivery remains a later slice in #437.

## Impact

Product artifacts and their retained decision record only, plus this bound recovery plan and a process lesson. No application, dependency, workflow or infrastructure change. Original PR #399, source commits, complete provider report and the native blocked assessment remain preserved. Application on a branch does not merge the definition into main, implement the settings or supply a native review binding.
