<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Raccord mark identity

## ADDED Requirements

### Requirement: Use the authored mark across web identity surfaces

The web application MUST use the authored R mark in the authenticated shell, sign-in surface and browser favicon without stretching or arbitrarily recoloring it.

#### Scenario: Shell retains accessible product naming

- **WHEN** the authenticated shell is rendered
- **THEN** the authored mark is shown beside the product name and is hidden from duplicate assistive announcements

#### Scenario: Sign-in identifies the product

- **WHEN** the sign-in page is rendered
- **THEN** the authored mark is shown with an informative accessible name

#### Scenario: Browser identity is available

- **WHEN** the browser loads the frontend document
- **THEN** the authored mark is declared as the favicon
