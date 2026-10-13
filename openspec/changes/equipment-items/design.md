<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-SELL-RENTAL-EQUIPMENT" digest="sha256:290f17bd7cceaef4612f4b4c72bb6380b7e06cb62813e1748f2561a2f038b0f7" -->
<!-- pdac:cite id="UC-EQP-CREATE-SALE-ITEM" digest="sha256:15cfbde339e90beb583ceb1a124de9b487ca5a7b9aea3a7b1be9282615f43f62" -->

# Design

Equipment item identity and stock are owned by the Equipment schema. The API stores serial numbers as a JSON collection and returns a deterministic, one-based page envelope. Planned state is persisted so later planning slices can enforce the immutable item type rule.
