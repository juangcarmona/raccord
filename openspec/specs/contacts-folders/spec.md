<!-- pdac-scope: cited -->

# Contacts folders Specification

## Purpose

Authorized Contacts users organize contact records in an optional hierarchical folder structure scoped to the active deployment.

## Requirements

### Requirement: Manage workspace-specific contact folders

The system MUST let an authorized Contacts user create root folders and subfolders, rename, reorder, move and delete folders in the active deployment. A folder containing a contact or child folder MUST NOT be deleted, and a folder MUST NOT be moved into itself or a descendant. The deployment database is the isolation boundary.
<!-- pdac:cite id="FR-CON-005" digest="sha256:351710f9bb90a6e303b5eb4a6a6a2d09df952aeb3b6d69d620030a507ac18bff" -->
<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-FOLDERS" digest="sha256:64bf01e23b42803074aa40b1c666ecfc1c2a1fbd0b453c4d15c2c11c4572c082" -->
<!-- pdac:cite id="BR-CON-FOLDER-HIERARCHY" digest="sha256:9607b972d82930e5ca57988dc52ec26241b8b9104bc7cd3a6eeada76251dd0c7" -->
<!-- pdac:cite id="BR-CON-FOLDER-DELETE-NONEMPTY" digest="sha256:3a1f1955d9faa33b90388cd7c56418adf2c2c5dd468b44878edf731d5ff074bd" -->

#### Scenario: Create a child folder

- **WHEN** an authorized user creates a folder under a parent in the active workspace
- **THEN** the folder appears beneath that parent in the same workspace hierarchy

#### Scenario: Refuse deletion of a nonempty folder

- **WHEN** an authorized user deletes a folder containing a contact or child folder
- **THEN** the request is refused and the folder and its contents remain unchanged

#### Scenario: Refuse a recursive move

- **WHEN** an authorized user moves a folder into itself or one of its descendants
- **THEN** the request is refused and the hierarchy remains unchanged

### Requirement: Assign contacts to zero or one folder

The system MUST let an authorized user assign a contact to one folder or leave it unfiled. Folder assignments MUST reference folders in the active deployment database, and folder filters MUST return only matching contacts from that database.
<!-- pdac:cite id="BR-CON-FOLDER-HIERARCHY" digest="sha256:9607b972d82930e5ca57988dc52ec26241b8b9104bc7cd3a6eeada76251dd0c7" -->
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:8f28e980e4bafd4fa79db9083f468bdd258d208c6d30d4d091a8886c65e95960" -->

#### Scenario: Assign a contact to a folder

- **WHEN** a contact is assigned to a folder in its workspace
- **THEN** the contact appears in that folder's filtered directory

#### Scenario: Reject an unknown folder id

- **WHEN** a user assigns a contact to a folder id that does not exist in the active deployment database
- **THEN** the operation is rejected and the contact's existing folder assignment is unchanged
