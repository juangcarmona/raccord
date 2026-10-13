<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-IMPORT-EQUIPMENT" digest="sha256:04045c947021522a7381e7289dc2ef128763188b8e4179969c4bcb4a7335324c" -->

# Design

The Equipment catalogue owns a small CSV import boundary. Each row is validated before it is added to the pending unit of work; valid rows are saved together and rejected rows return their row number and reason.
