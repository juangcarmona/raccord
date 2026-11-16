<!-- pdac-scope: cited -->

## Context

Recover the five source-bound historical decisions listed in `docs/operations/contacts-product-recovery.json`; retain immutable IDs and the current v1alpha2 relationship ownership. Canonical artifacts are written by the ProductShape apply operation.

## Decisions

The accepted product semantics are FR-CON-001 through FR-CON-005. The create-only import and extended import have explicit separate scopes. Contacts folder membership is optional, while deletion safety and other modules retain their rules. The existing single-installation boundary, BR-PRJ-INVOICE-RECIPIENT and later issuer-review/snapshot decisions are preserved. Review stale consumers before regenerating citations through the provider.

## Verification

Validate the complete overlay and impact ledger before applying, then validate the accepted baseline, current OpenSpec citations and the new change. Retain decision records without duplicate proposed artifacts. A product apply receipt does not establish recovered runtime implementation.

<!-- pdac:cite id="FR-CON-001" digest="sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
<!-- pdac:cite id="FR-CON-003" digest="sha256:133563562f019c506fdf1f12725247c3798b2d0ef0e3d013a2557b7cbbc86e2c" -->
<!-- pdac:cite id="FR-CON-004" digest="sha256:ff9158b9c1402055feb909003cde0c68f9044e7859241ad37ba665901edcdae2" -->
<!-- pdac:cite id="FR-CON-005" digest="sha256:ed722021b10ab3fe829bc5482ad2f687a3b76ef6ddcdbe5c01c2423848d4d9f7" -->
<!-- pdac:cite id="BR-PRJ-INVOICE-RECIPIENT" digest="sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5" -->
