---
id: JRN-INT-BUILD-API-INTEGRATION
type: journey
title: "Build and run a custom integration"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
steps:
  - use-case: UC-INT-ENABLE-API-ACCESS
  - use-case: UC-INT-READ-RECORDS-THROUGH-API
  - use-case: UC-INT-WRITE-RECORDS-THROUGH-API
  - use-case: UC-INT-ADAPT-TO-API-CHANGE
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/15274709111826.md, .private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---
## Intended Outcome
An external system stays in step with the company's data through the public interface.
## Entry Conditions
The company has a developer and an administrator with configuration access.
## Journey Narrative
The administrator enables interface access and hands the token to the developer. The external system reads records through the interface and writes back its own changes. When the interface evolves, the integration follows the version notes during the transition period.
## Variants and Branches
The token leaks: it is regenerated (UC-INT-REGENERATE-API-TOKEN).
## Completion Conditions
The external system reads and writes the data it needs and survives interface versions.
