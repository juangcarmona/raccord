---
id: BR-WSP-FILE-DOWNLOAD-LEVELS
type: business-rule
title: "File downloads follow the role's download level"
status: draft
applies-to:
  - "UC-WSP-MANAGE-ROLE"
uses-terms:
  - "TERM-FILE-DOWNLOAD-LEVEL"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A user can download no files, only non-confidential files, or all files, according to the download level of their role.

## Rationale

Confidential documents must not leave the product through the wrong people.

## Examples

- A role limited to non-confidential files cannot download a contract marked confidential.

## Exceptions

None.
