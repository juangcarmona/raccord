---
id: BR-PRJ-REMEMBER-LAST-TEMPLATE
type: business-rule
title: "A type may remember the last used document template"
status: active
applies-to:
  - "UC-PRJ-MANAGE-PROJECT-TYPES"
  - "UC-DOC-GENERATE-DOCUMENT"
uses-terms:
  - "TERM-PROJECT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013631800.md"
  confidence: high
  recovered-from: documentation
---

## Rule

For each document template slot of a project type, the option 'remember last used' makes the product propose the template last used instead of the configured default.

## Rationale

Users often reuse the same template.

## Examples

After using a particular quotation template once, it is proposed next time.

## Exceptions

None.
