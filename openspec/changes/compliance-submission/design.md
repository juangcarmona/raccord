<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-INT-ENABLE-FISCAL-REPORTING" digest="sha256:a4a91b99a567486e5665c9c0a3f0f7ebc4fe06525c7ac9eccbbc10e20424e04e" -->

# Design

The existing transactional compliance row is the outbox. A hosted worker submits rows using the record hash as the idempotency key, then stores accepted/rejected status, authority reference, attempt count and safe error text. HTTP failures remain pending for a later retry.
