<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-GENERATE-QUOTE" digest="sha256:56ae41de1ef841ad1d5e386acfe32ab75cde55b2e0f416d519cee98138be69a1" -->
<!-- pdac:cite id="FR-FIN-QUOTES" digest="sha256:19bbedb0836b7f972239790fff9ee80f056cd86c79e44409932aa79021a2b0fa" -->

# Design

Extend the quote state machine with `SignatureRecorded`. `POST /api/quotes/{id}/sign` validates the accepted state, stores `SignedBy` and obtains `SignedAt` from `IClock`.
