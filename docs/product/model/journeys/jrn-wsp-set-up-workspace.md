---
id: JRN-WSP-SET-UP-WORKSPACE
type: journey
title: "Set up the installation from scratch to go-live"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
steps:
  - use-case: "UC-WSP-CREATE-WORKSPACE"
  - use-case: "UC-WSP-EDIT-COMPANY-DETAILS"
  - use-case: "UC-WSP-SET-TIME-ZONE"
  - use-case: "UC-WSP-SET-LANGUAGE"
  - use-case: "UC-WSP-CONFIGURE-NUMBER-SERIES"
  - use-case: "UC-WSP-EMPTY-SAMPLE-DATA"
  - use-case: "UC-WSP-UPDATE-BY-IMPORT"
  - use-case: "UC-WSP-MANAGE-ROLE"
  - use-case: "UC-WSP-ASSIGN-ROLE"
provenance:
  source: ".private/research/help-center/articles/35600301773970.md, .private/research/help-center/articles/15426355918226.md, .private/research/help-center/articles/360015603179.md"
  confidence: medium
  recovered-from: documentation
---

## Intended Outcome

The company runs its daily operations in its own correctly configured installation.

## Entry Conditions

A company decides to adopt the product and names someone to lead the set-up.

## Journey Narrative

The lead sets up the installation, explores it with sample data, maps the company's current way of working and decides which modules matter. The company details, logo, time zone and language are set, then numbering and, where relevant, invoicing and tax defaults. Sample data is removed and the master data is built: individual equipment first, then kits, then customers and crew, preferably by import after cleaning the source data. Document templates and the quoting process are configured. Roles are designed and assigned so each colleague gets suitable access. A complete test workflow from quote to invoice is run, live projects are entered and the team takes a go/no-go decision.

## Variants and Branches

- Going live quickly: only core inventory, project creation and quoting, basic templates and company details are required; advanced reports, integrations and historical data follow later.

## Completion Conditions

The team decides go, works in the workspace daily, and the core inventory, company details and basic templates are in place.
