<!-- pdac-scope: cited -->
# Design

The Project detail page renders a Warehouse section when packing lines are present. It resolves equipment names from the equipment catalog and exposes only the next valid transition for each line: Pack, Book out manually, then Record return. The book-out request deliberately sends a null scan code so the existing server transition uses the manual path.

The UI reloads the project after each successful transition. API and domain validation remain unchanged, so invalid order, quantity and stock conditions continue to fail at the existing write boundary without partial client state.

The frontend test covers the manual request payload and persisted state. A selected Playwright journey creates synthetic project equipment, drives all three UI transitions and verifies the received state through the API.

<!-- pdac:cite id="UC-WHS-BOOK-MANUALLY" digest="sha256:c7a2b856f5d53f1ef57161e3ea1bdd30dec94a5e89a823c15d39bc192fc8290a" -->
<!-- pdac:cite id="FR-WHS-027" digest="sha256:0a30f95748288e9062e692866a7aa7e268f1c8f1752264ec9ca794d5f52906cd" -->
