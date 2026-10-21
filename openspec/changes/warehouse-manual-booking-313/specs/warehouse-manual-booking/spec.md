<!-- pdac-scope: cited -->
# Manual warehouse booking

## Purpose

Make the accepted manual warehouse booking flow reachable and verifiable from the authenticated project workspace.

## ADDED Requirements

### Requirement: Operators can move project equipment manually

The system MUST show each project packing line and expose only its next valid packing transition. Manual book-out MUST send no scan code, and every successful transition MUST reload the authoritative line state.

#### Scenario: Book out and return equipment manually

- **WHEN** an operator prepares a planned line, books it out manually and records its return
- **THEN** the line progresses through prepared, booked-out and received states and the server records the corresponding movement

#### Scenario: Keep invalid transitions server-authoritative

- **WHEN** an operator attempts an unavailable transition or the stock boundary rejects the movement
- **THEN** the UI reports the failure and does not present the line as advanced

<!-- pdac:cite id="UC-WHS-BOOK-MANUALLY" digest="sha256:c7a2b856f5d53f1ef57161e3ea1bdd30dec94a5e89a823c15d39bc192fc8290a" -->
<!-- pdac:cite id="FR-WHS-027" digest="sha256:0a30f95748288e9062e692866a7aa7e268f1c8f1752264ec9ca794d5f52906cd" -->
<!-- pdac:cite id="BR-WHS-DEFAULT-BOOKING-TARGET" digest="sha256:8044bc8dee95732d4ebe787017b831f49b5d71b147b6b003345b32c3aae24d33" -->
