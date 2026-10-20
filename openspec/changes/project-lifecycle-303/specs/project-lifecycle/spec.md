<!-- pdac-scope: cited -->
# Project details and lifecycle

## ADDED Requirements

### Requirement: Project details can be maintained

The system MUST let an operator edit the accepted project detail fields and persist the update to the project history. A duplicate manually assigned project number MUST be rejected.

#### Scenario: Update project details

- **WHEN** an operator saves valid project details
- **THEN** the project overview and detail show the saved values

### Requirement: Project schedules have planning and usage periods

The system MUST create default planning and usage periods, validate usage within planning, allow named periods to be edited and ordered, and preserve links when period dates change.

#### Scenario: Change a linked period

- **WHEN** an operator changes the dates of a period linked to planning records
- **THEN** the linked records use the new dates and their project association remains intact

### Requirement: Project status is a fixed manually controlled lifecycle

The system MUST expose the canonical project statuses and allow manual status changes independently of warehouse item status. Completing a project MUST prevent operational edits until explicitly reopened.

#### Scenario: Change project status

- **WHEN** an operator changes a project's lifecycle status
- **THEN** the project overview reflects it without changing the warehouse status of its equipment

### Requirement: Cancellation confirms only applicable actions

The system MUST show cancellation actions only when relevant, execute exactly the confirmed actions, and persist a warning if crew planning or tasks remain open. Cancelling a project MUST preserve issued invoices and invoiced work.

#### Scenario: Cancel with open work and preserve invoices

- **WHEN** an operator cancels a project that has open operational items and issued invoices
- **THEN** the project is cancelled with a persistent open-items warning, and every issued invoice and invoiced work line remains unchanged and linked to the project

<!-- pdac:cite id="UC-PRJ-EDIT-PROJECT-DETAILS" digest="sha256:cc87eb90988fdbd32fb2e626a02aa7154783ffc7b161702a3c2bf4ace9c2bdb7" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-TIME-SCHEDULE" digest="sha256:63f5e75776fe129ece805663868a322547da608fd04023944189c071b9c04bae" -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:1a5c43dfd663a08a833301fa25ccad7dc02057bfbc8c444f4fb18b3347905acc" -->
<!-- pdac:cite id="UC-PRJ-CANCEL-PROJECT" digest="sha256:80e15e9d11dba906cb7d753fd78b7ce8b29c30651bfe9cdb9ac1d7cdc3118d96" -->
<!-- pdac:cite id="BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK" digest="sha256:383aadaf0babcb79c58e36c9c9559532ceb4589ad3b8bc92f409874c012ae258" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->
<!-- pdac:cite id="BR-PRJ-FIXED-STATUS-SET" digest="sha256:f411f75ad65570a3f229398208c89ec530518c016468ff13a4717e89240ab485" -->
<!-- pdac:cite id="BR-PRJ-DEFAULT-PERIODS" digest="sha256:bb6592f3d1d4b8a0d8a746a819b75d60c73b45b2261e3f79eb471bd468f64edd" -->
<!-- pdac:cite id="BR-PRJ-USAGE-WITHIN-PLANNING" digest="sha256:2a82bc0ebab249536508bad9a156a6bb869de6653e0d5f797b3d8d06f6a123b9" -->
<!-- pdac:cite id="BR-PRJ-LINKED-PERIOD-PROPAGATION" digest="sha256:3b497e850f50437dcda2ec98fc24cd238ea9c52bd3f8de34aedf208407907d66" -->
