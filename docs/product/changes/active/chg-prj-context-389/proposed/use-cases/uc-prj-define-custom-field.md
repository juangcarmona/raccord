---
id: UC-PRJ-DEFINE-CUSTOM-FIELD
type: use-case
title: "Define a custom field"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-PRJ-CUSTOM-FIELD-FROZEN"
  - "BR-PRJ-CONFIDENTIAL-CUSTOM-FIELD"
uses-terms:
  - "TERM-CUSTOM-FIELD"
provenance:
  source: ".private/research/help-center/articles/360013637220.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Company-specific data is captured as part of core records.

## Trigger

A fact such as 'lunch provided' or 'project manager' must be recorded and printed.

## Preconditions

Administrator rights.

## Main Flow

1. The administrator names the field, chooses the record kind, entry type (including linked crew member or contact), default value, group, searchable flag and confidentiality.
2. The product adds the field to that record kind; values can be filtered, shown as columns, printed on documents, imported and exported.

## Alternative Flows

- Confidential: only roles allowed to see confidential fields see it.

## Failure Conditions

After creation only drop-down options can be changed.

## Postconditions

The field is available.
