<!-- pdac-scope: cited -->

# Contacts tags and filters

## ADDED Requirements

### Requirement: Manage contact tags

The system MUST let a user with permission to manage contacts create and remove Contacts tags and assign or remove those tags on contacts in the active deployment. A tag MUST classify Contacts records only. The deployment's database is the isolation boundary; the API MUST NOT accept a client-selected workspace id.
<!-- pdac:cite id="FR-CON-004" digest="sha256:a492f73718d6ae6fffae47c5aaa562c092ab8a86e167c5a1f167b667ad55cfe6" -->
<!-- pdac:cite id="BR-CON-CONTACT-TAG-SCOPE" digest="sha256:08680cad6deb2e72201f7a7d3585a8197131d694897c9b0c56e824b0b909f739" -->

#### Scenario: Assign an existing tag

- **WHEN** an authorized user assigns a tag from the active workspace to a contact in that workspace
- **THEN** the contact has that tag in the directory

#### Scenario: Reject an unknown tag id

- **WHEN** a user attempts to assign a tag id that does not exist in the active deployment database
- **THEN** the operation is rejected and no assignment is created

### Requirement: Filter by any or all selected tags

The system MUST filter the active deployment's Contacts directory by one or more selected tags and MUST support both any-match and all-match modes.
<!-- pdac:cite id="UC-CON-FILTER-CONTACT-DIRECTORY" digest="sha256:8c11b6106c10adc68b9e495e57ed5bdd3e1763b98b1033c7cc4c9614770413a9" -->
<!-- pdac:cite id="BR-CON-TAG-FILTER-MODES" digest="sha256:fbdc5c466fa3c20260000a51e7105b01cab9002409efcae7bd795d2ba83f1966" -->
<!-- pdac:cite id="SB-CON-TAG-FILTER-ANY-AND-ALL" digest="sha256:ee82a9793807a2a39857f77f49c6fb2ee0c21de3831a710d782d2db96c6e940f" -->

#### Scenario: Filter tags using any mode

- **WHEN** a contact has only one of several selected tags and the user chooses any mode
- **THEN** that contact is included in the results

#### Scenario: Filter tags using all mode

- **WHEN** a contact does not have every selected tag and the user chooses all mode
- **THEN** that contact is excluded from the results

#### Scenario: Keep tag results inside the active deployment

- **WHEN** a user filters contacts by a tag in the active deployment database
- **THEN** only contacts from that database are returned
