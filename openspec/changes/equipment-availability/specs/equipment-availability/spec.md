<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-CHECK-AVAILABILITY" digest="sha256:3ea0d591977f59dc60f5c0ec835cc07dfa11b9d36ab62afabc68ed1864c2efed" -->

# Equipment availability

## ADDED Requirements

### Requirement: Availability separates soft holds and reservations

Availability MUST exclude cancelled projects, report draft/planned quantities as soft holds, report active/completed quantities as reservations, and calculate remaining availability from stock.

#### Scenario: Check a date window

- **WHEN** a client requests availability for an equipment item and date range
- **THEN** overlapping project lines are classified and remaining availability is returned

#### Scenario: Invalid date window or item

- **WHEN** the end date precedes the start date or the item does not exist
- **THEN** the API returns not found without calculating availability
