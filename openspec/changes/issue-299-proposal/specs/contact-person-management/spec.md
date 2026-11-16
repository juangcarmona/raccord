# Spec Delta

## Purpose

Provides authorized operations users with an installation-isolated directory of contacts and their affiliated people. It maintains supported designations without changing project associations or issued-invoice party snapshots.

## ADDED Requirements

### Requirement: Maintain contact persons under a contact

The system MUST let an authorized user list, create, read, and edit contact persons under an existing contact, including a required name and supported email and phone details. Each person MUST remain attached to its original contact.

#### Scenario: Create and edit a person
- **WHEN** an authorized user creates a person with valid details under an existing contact and later edits that person
- **THEN** the person is listed and readable under that contact with the saved details

#### Scenario: Reject invalid details
- **WHEN** a user submits a missing required name or invalid contact detail
- **THEN** the request is rejected and no invalid record or partial update is saved

#### Scenario: Do not move a person between contacts
- **WHEN** an update attempts to change a person's parent contact
- **THEN** the request is rejected and the person remains attached to its original contact

#### Scenario: Enforce authorization and installation ownership
- **WHEN** a user without Contacts permission attempts to read or change a person's record, or a record is accessed outside its owning installation
- **THEN** access is denied and no data changes

### Requirement: Enforce unique default and administrative designations

A contact MUST have at most one default person and at most one administrative person. The same person MAY hold both designations. Reassigning either designation MUST clear its previous holder atomically, and a rejected or conflicting change MUST NOT leave a partial reassignment.

#### Scenario: Reassign the default designation
- **WHEN** an authorized user designates a different person as default
- **THEN** the new person is the sole default and the former default remains under the contact without that designation

#### Scenario: Reassign the administrative designation independently
- **WHEN** an authorized user designates a different person as administrative
- **THEN** the new person is the sole administrative person and any default designation remains unchanged

#### Scenario: Preserve designation integrity on conflict
- **WHEN** concurrent or invalid designation changes conflict
- **THEN** no contact is left with duplicate designations or a partially completed reassignment

### Requirement: Preserve project and issued-invoice history

Contact-person writes MUST NOT change project contact associations or immutable issued-invoice party snapshots. A person's designations MUST NOT select a project contact or determine invoice delivery.

#### Scenario: Edit contact people with existing project and invoice history
- **WHEN** an authorized user creates or edits a person or reassigns a designation for a contact with existing project and issued-invoice records
- **THEN** those project associations and invoice party snapshots remain unchanged
