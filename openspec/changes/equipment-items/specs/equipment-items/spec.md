<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-SELL-RENTAL-EQUIPMENT" digest="sha256:290f17bd7cceaef4612f4b4c72bb6380b7e06cb62813e1748f2561a2f038b0f7" -->
<!-- pdac:cite id="UC-EQP-CREATE-SALE-ITEM" digest="sha256:15cfbde339e90beb583ceb1a124de9b487ca5a7b9aea3a7b1be9282615f43f62" -->

# equipment-items Specification

## ADDED Requirements

### Requirement: Equipment SHALL support rental and sale items

The Equipment module SHALL create items with a rental or sale type, stock quantity, and optional serial numbers.

#### Scenario: an item is created

- **WHEN** a valid item request is submitted
- **THEN** the item is persisted with its type, quantity, and serial numbers

### Requirement: The equipment catalogue SHALL be paginated

The catalogue SHALL return a one-based page envelope and clamp the requested page size to a safe server maximum.

#### Scenario: a catalogue page is requested

- **WHEN** a list request supplies page and pageSize
- **THEN** only that page is returned with normalized pagination metadata and the total count
