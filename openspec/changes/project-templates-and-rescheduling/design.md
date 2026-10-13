<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-CREATE-PROJECT-TEMPLATE" digest="sha256:a22829e84511d226d7863a063a20ddaa3332df9282fef5a203816d4bee27da38" -->
<!-- pdac:cite id="UC-PRJ-CREATE-FROM-TEMPLATE" digest="sha256:16823e8d642041d00c4820d07e05bcd675ecb4c23a4402fc20547ba6dca3f1b3" -->
<!-- pdac:cite id="UC-PRJ-SHIFT-PROJECT-TIMES" digest="sha256:3fa803cf5608b13a2a019e4ef443ea1d76ec1fac969d94c1ad96f9b9a930cba9" -->

# Design

Templates are a separate aggregate containing only reusable project identity data in this MVP. Project creation delegates to the existing customer lookup and project factory, preserving discount and payment-term snapshots. Rescheduling is an aggregate operation: the project period and every project-owned planned equipment line receive the same offset. Availability is recalculated by the existing read model from the persisted dates; no duplicated reservation state is introduced.

Lifecycle status remains event-driven through the existing project status operation. Date shifting does not infer a quote or warehouse event, avoiding a false status transition until those event sources exist.
