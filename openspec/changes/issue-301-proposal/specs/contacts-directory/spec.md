<!-- pdac-scope: cited -->

## Purpose

Enable authorized administrators to create contacts from spreadsheets with reviewable validation and safe outcomes.

## ADDED Requirements

### Requirement: Inspect supported contact spreadsheets

The system SHALL support authorized CSV or single-sheet XLSX inspection and mapping with optional headers, 10 MiB and 64 columns, and text-only cells.
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:9a964862aa671bf1c5f9648286ecf92c5ab60631ed7b30c02c3de8be38a3d587" -->
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:f33969383814d4ba04d00eadd559a0dd470c67958d33cfc42ff6a8aded62f9fb" -->

#### Scenario: Inspect and preview

- **WHEN** an authorized actor maps a supported file with or without headers
- **THEN** the first 10 data rows and validation results for all rows are available before confirmation and no contacts are created

#### Scenario: Reject unsupported input

- **WHEN** a file exceeds 10 MiB or 64 columns, is malformed, has multiple worksheets, images, formulas or unsupported cells, or has an invalid mapping
- **THEN** it is rejected before contact writes with an explanation

### Requirement: Recheck a preview before writing

The system SHALL verify the confirmed file digest and validate every row again before contact writes. A changed file or invalid mapping SHALL create no contacts.
<!-- pdac:cite id="SB-CON-IMPORT-RECHECKS-PREVIEW" digest="sha256:e206d5bdd26ce6f69423f7fd81fe5b2ac2c3faae55d21e0abad07407f53ba122" -->
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:9a964862aa671bf1c5f9648286ecf92c5ab60631ed7b30c02c3de8be38a3d587" -->

#### Scenario: Confirmation after a change

- **WHEN** confirmation supplies a file whose digest differs from the preview, or an invalid mapping
- **THEN** the system refuses the import and creates no contacts

#### Scenario: Row validity changes after preview

- **WHEN** a previously valid row is invalid at confirmation
- **THEN** it is rejected without a partial contact and other valid rows can succeed

#### Scenario: Preview inputs change

- **WHEN** the user changes the file, header setting or mapping
- **THEN** the previous preview is invalidated and confirmation requires a new preview; choosing ignore removes that field mapping

### Requirement: Create complete rows and report rejections

The system SHALL create each valid row as one complete new contact independently, reject invalid or expected conflicting rows atomically, and report each rejected row number and reason. It SHALL not update existing contacts.
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:f6c233e920161353d0bdac7a81156a1fc58dca933546a8da314593dc4b61b2b8" -->
<!-- pdac:cite id="SB-CON-IMPORT-VALID-AND-INVALID-ROWS" digest="sha256:433ee6a37d8cfa638c74c8066aabcad2980c9b7908d2714c01cf1f24ad1c8454" -->

#### Scenario: Mixed rows

- **WHEN** an authorized actor confirms three valid rows and one invalid row
- **THEN** three complete contacts are created and the rejected row and its reason are reported without a partial fourth contact

#### Scenario: All rows invalid

- **WHEN** every confirmed row is invalid
- **THEN** no contacts are created and all rejected row numbers and reasons are reported

#### Scenario: Late expected conflict

- **WHEN** an expected row conflict occurs during persistence after validation
- **THEN** that row leaves no partial contact or row audit residue, is reported as rejected, and other valid rows can succeed

### Requirement: Protect imports and record outcomes

The system SHALL enforce contact-create permission and installation isolation for import, and record actor, server time, import id, file type and outcome counts without uploaded bytes or values in audit or logs.
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:bfa968766a4f0c22c180c9c6bad1742062be10f37b375f15c6e5058aded4327f" -->
<!-- pdac:cite id="FR-CON-003" digest="sha256:133563562f019c506fdf1f12725247c3798b2d0ef0e3d013a2557b7cbbc86e2c" -->

#### Scenario: Unauthorized actor

- **WHEN** an actor without contact-create permission attempts inspection, preview or confirmation
- **THEN** access is refused and no contacts are created

#### Scenario: Successful import audit

- **WHEN** an authorized actor completes an import
- **THEN** the audit records actor, server time, import id, file type and accepted and rejected counts, without uploaded bytes or values in audit or logs

#### Scenario: Separate installations

- **WHEN** a contact import is performed in one installation
- **THEN** another installation's contacts and audit are unaffected and inaccessible through that import
