---
pdac-scope: cited
---

## ADDED Requirements

### Requirement: Import contacts with row validation and audit

The system MUST let an authorized contact administrator import new contact records from CSV or XLSX after mapping columns and reviewing a preview of the first 10 data rows. Files MUST contain one worksheet, no more than 64 columns, plain text values and no embedded images, and MUST not exceed 10 MiB. The system MUST validate every row before confirmation, create each valid row as one complete contact, leave invalid rows unsaved, and report every rejected row number and reason. Each row MUST be atomic; valid rows can be imported when other rows fail. The audit history MUST record the actor, time, and outcome counts without retaining the uploaded file or raw contact values.

<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:28c6c81be85f3cb0be1ea8156cc46c0b0ac712bed0dd3a5313ff693a6d501a75" -->
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:b762dc5819168c4d0bcc6fe2fd7b4826b8a6e0efa5f0afec7b6f7abbd018926b" -->
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:ed358fb85c081e36b5dd42afa11d1dc3e3a268f9687d736d920a967c24de162b" -->
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:42f4cbb77ecbc40f73e0ab222bf28dd104d22e7ad6b2f47d9a497cfae0773a49" -->
<!-- pdac:cite id="FR-CON-003" digest="sha256:4d08fb6282312db4988c5987bc65d9677fe56edf51b8e63547711008185ffd00" -->

#### Scenario: Create contacts from a valid spreadsheet

- **WHEN** an authorized user uploads a supported spreadsheet, maps required contact fields, reviews the preview, and confirms import
- **THEN** each valid row creates one complete contact with a unique contact number and the selected roles

#### Scenario: Preview mapped spreadsheet rows

- **WHEN** an authorized user uploads a supported spreadsheet and maps its columns
- **THEN** the system shows the first 10 data rows and the validation result for every row before import is confirmed

#### Scenario: Reject unsupported spreadsheet structure

- **WHEN** a user uploads an unsupported file, a workbook with more than one worksheet, more than 64 columns, or a file larger than 10 MiB
- **THEN** the system refuses the upload before creating contacts and explains the supported format

#### Scenario: Import valid rows and report rejected rows

- **WHEN** a confirmed import contains valid rows and rows with invalid required values
- **THEN** each valid row is persisted as a complete contact, each invalid row remains unsaved, and the result lists every rejected row number and reason

#### Scenario: Leave all contacts unchanged when every row is invalid

- **WHEN** an authorized user confirms an import whose rows are all invalid
- **THEN** no contacts are created and the result lists every rejected row number and reason

#### Scenario: Record a minimal import audit outcome

- **WHEN** an authorized user completes a spreadsheet import
- **THEN** the audit history records the actor, time, and accepted and rejected counts without retaining the uploaded file or raw contact values

#### Scenario: Deny spreadsheet import without contact-create permission

- **WHEN** an actor without permission to create contacts attempts an import
- **THEN** the system rejects the request and creates no contacts
