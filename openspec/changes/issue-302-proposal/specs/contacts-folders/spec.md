# Contacts folders

<!-- pdac-scope: cited -->

## Purpose

Let authorized users organize Contacts in an optional hierarchy while preserving safe folder operations.

## ADDED Requirements

### Requirement: Optional contact filing

The system SHALL allow authorized users to create roots and subfolders, rename, reorder and move folders, and assign or unfile a contact, which belongs to at most one folder.
<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:ed07fa1bacf8f84b6e6e255289a94b5cfe2ac70d4da2893823b086ab7fc57e79" -->

#### Scenario: Manage hierarchy and filing

- **WHEN** an authorized user creates and renames a child folder, reorders or moves it, assigns a contact and then unfiles it
- **THEN** each change persists and the contact remains available with no folder

### Requirement: Protect folder hierarchy

The system SHALL refuse deletion of folders containing contacts or child folders and moves into self or descendants, including under concurrent operations. Conflicting operations SHALL return controlled errors.
<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:ed07fa1bacf8f84b6e6e255289a94b5cfe2ac70d4da2893823b086ab7fc57e79" -->

#### Scenario: Nonempty folder

- **WHEN** a user deletes a folder containing a contact or a child folder
- **THEN** deletion is refused and the hierarchy and contact remain unchanged

#### Scenario: Cycle and concurrent moves

- **WHEN** a user moves a folder into itself or a descendant, or opposing concurrent moves would form a cycle
- **THEN** the invalid outcome cannot commit and a rejected operation returns a controlled error

#### Scenario: Concurrent deletion and filing

- **WHEN** folder deletion races with assigning a contact or creating a child
- **THEN** no committed state contains an invalid reference and an unsuccessful operation returns a controlled conflict
