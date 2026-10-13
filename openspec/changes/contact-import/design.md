<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-IMPORT-CONTACTS" digest="sha256:4af29a38f956f50cc0595520b37addfd90cc559dd74db74be9d76c380fe69105" -->

# Design

The import use case parses and validates the complete upload before staging any party rows. A single unit-of-work save commits all rows, while rejected input returns row-level errors without persistence.
