<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-TAG-RECORDS" digest="sha256:4848a8e7720a327080902073b520293c7f2635891f2ff92dcf7e64c7b5ef1986" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-RECORD-FOLDERS" digest="sha256:c6084e098fbeb011a1838333a9dec324dda5c224051ca0485c8e42d3c03549ac" -->

# contact-taxonomy Specification

## ADDED Requirements

### Requirement: Contacts SHALL support tags and an optional folder

The Contacts module SHALL persist normalized tags and an optional folder assignment for each contact.

#### Scenario: taxonomy is updated

- **WHEN** a contact taxonomy update contains duplicate tags or surrounding whitespace
- **THEN** the stored response contains distinct trimmed tags and a trimmed folder value

### Requirement: Contacts SHALL be filterable by taxonomy

The customer list SHALL accept optional tag and folder filters and return only matching contacts.

#### Scenario: contacts are filtered

- **WHEN** a list request includes a tag or folder filter
- **THEN** matching is case-insensitive and only contacts satisfying every supplied filter are returned
