# Contacts tags and filters

<!-- pdac-scope: cited -->

## Purpose

Allow authorized users to classify Contacts and find matching records within the active installation.

## ADDED Requirements

### Requirement: Manage Contacts tags
The system SHALL allow authorized users to create and remove Contacts-only tags and assign or remove them on contacts, rejecting tag names containing whitespace.
<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:ed07fa1bacf8f84b6e6e255289a94b5cfe2ac70d4da2893823b086ab7fc57e79" -->

#### Scenario: Tag assignment and removal
- **WHEN** an authorized user creates a valid tag, assigns it to a contact and then removes the assignment
- **THEN** the assignment is saved and removed without deleting the contact, and an unassigned tag can be removed

#### Scenario: Invalid or unauthorized change
- **WHEN** a user submits a tag containing whitespace or lacks Contacts permission
- **THEN** the operation is refused and no unauthorized change is saved

### Requirement: Filter by tags
The directory SHALL support any or all selected tags, combined with folder and existing role filters, with stable ordering and pagination. Repeated IDs SHALL be normalized and unsupported modes rejected.
<!-- pdac:cite id="SB-CON-TAG-FILTER-ANY-AND-ALL" digest="sha256:412dd0ad25bed41cac3ab569508e183c82ce5a1b554877b95ac841ad17d251af" -->

#### Scenario: Any and all
- **WHEN** contact A has tags X and Y, contact B has X, and the user selects X and Y
- **THEN** any returns A and B and all returns only A

#### Scenario: Combined paged filter
- **WHEN** a user selects a folder, role and repeated tag IDs while paging results
- **THEN** only contacts matching all selected filter categories appear in stable order and repeated IDs do not alter results

#### Scenario: Unsupported matching mode
- **WHEN** a request specifies an unsupported matching mode
- **THEN** the request is rejected with a controlled validation response

### Requirement: Installation isolation and permissions
The system SHALL enforce existing Contacts permissions and restrict organization reads, writes and filters to the active installation database without accepting a client-selected workspace identifier.
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:5eb794eff7858450263824556fd9756d03d96ea9d32b5db2df4b672778539165" -->

#### Scenario: Separate installations
- **WHEN** an authorized actor organizes or filters contacts in one of two independent installations with identical names
- **THEN** only the first database is read or changed and the other is neither queried nor modified

#### Scenario: Unauthorized reads and writes
- **WHEN** a caller lacks the relevant Contacts read or manage permission
- **THEN** the corresponding operation is refused and no data is disclosed or changed
