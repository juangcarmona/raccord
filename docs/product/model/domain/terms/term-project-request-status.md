---
id: TERM-PROJECT-REQUEST-STATUS
type: domain-term
title: "Project request status"
status: draft
defined-in: "BC-PROJECTS"
synonyms: []
uses-terms:
  - "TERM-PROJECT-REQUEST"
provenance:
  source: ".private/research/help-center/articles/4403455031186.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

The lifecycle of a project request.

| State | Meaning |
| --- | --- |
| Received | The request has arrived, notifications have been sent and it waits for processing |
| Declined | Staff rejected it; its creator was e-mailed |
| Converted | A project was created from it |

Transitions: Received to Declined (allowed at any point before a project is created); Received to Converted (when a project is created). Declined and Converted are final.

## Distinguish From

Project status: lifecycle of the resulting project.

## Usage

Request list and notifications.
