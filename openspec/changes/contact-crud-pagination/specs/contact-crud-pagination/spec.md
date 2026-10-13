<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-ADD-CONTACT" digest="sha256:2e1a5370a1272d63a080dfaf62c97c487748110e038add310a553fce4a787881" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:e0f754e26dd900c146a21376b0fa9bd32e7da5e4849f3ab61fff86178c785227" -->

# contact-crud-pagination Specification

## ADDED Requirements

### Requirement: Contacts SHALL support create, read, and update

The Contacts module SHALL expose create, read, and update operations for customer, supplier, and venue contacts.

#### Scenario: a contact is edited

- **WHEN** a valid contact update is submitted
- **THEN** subsequent reads return the updated identity data and existing invoice snapshots remain unchanged

### Requirement: Contact browsing SHALL be paginated

The Contacts list SHALL return a bounded page envelope with one-based page numbering and a server-enforced maximum page size.

#### Scenario: a page is requested

- **WHEN** a contact list request supplies page and pageSize
- **THEN** the response contains only that page, the normalized pagination values, and the total contact count
