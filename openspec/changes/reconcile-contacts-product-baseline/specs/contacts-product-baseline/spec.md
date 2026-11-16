<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Apply the approved Contacts baseline through its provider

The repository SHALL represent the accepted Contacts slices through FR-CON-001, FR-CON-002, FR-CON-003, FR-CON-004 and FR-CON-005, applied by ProductShape with the owner approval and source-bound historical decisions recorded.

#### Scenario: Historical Contacts intent is reconciled

- **GIVEN** the historical decisions and current accepted baseline are available
- **WHEN** CHG-CONTACTS-RECOVERY is validated and applied
- **THEN** the complete accepted model validates without errors or warnings
- **AND** current OpenSpec citations resolve to reviewed provider-generated digests
- **AND** implementation recovery remains separately recorded until code is delivered

### Requirement: Preserve existing dependent workflow boundaries

The reconciliation SHALL preserve BR-PRJ-INVOICE-RECIPIENT and the current BR-PARTY-SNAPSHOT semantics while keeping the bounded Contacts profiles distinct from their extended counterparts.

#### Scenario: Product baseline is ready for native delivery

- **GIVEN** the reconciled baseline is integrated
- **WHEN** the first pending Contacts item is refined
- **THEN** its evidence can resolve the accepted Contacts IDs
- **AND** it cannot infer historical runtime code has already reached main

<!-- pdac:cite id="FR-CON-001" digest="sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
<!-- pdac:cite id="FR-CON-003" digest="sha256:133563562f019c506fdf1f12725247c3798b2d0ef0e3d013a2557b7cbbc86e2c" -->
<!-- pdac:cite id="FR-CON-004" digest="sha256:ff9158b9c1402055feb909003cde0c68f9044e7859241ad37ba665901edcdae2" -->
<!-- pdac:cite id="FR-CON-005" digest="sha256:ed722021b10ab3fe829bc5482ad2f687a3b76ef6ddcdbe5c01c2423848d4d9f7" -->
<!-- pdac:cite id="BR-PRJ-INVOICE-RECIPIENT" digest="sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5" -->
