---
id: UC-INT-READ-RECORDS-THROUGH-API
type: use-case
title: "Read records through the public interface"
status: draft
primary-actor: "ACT-EXTERNAL-APPLICATION"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
  - "BR-INT-PAGE-SIZE-LIMITS"
  - "BR-INT-CURSOR-PAGING-DEFAULT"
  - "BR-INT-OFFSET-PAGING-FOR-OTHER-SORTS"
  - "BR-INT-GENERATED-FIELDS-ON-REQUEST"
  - "BR-INT-CUSTOM-FIELDS-QUERYABLE"
  - "BR-INT-RATE-LIMIT-REFUSAL"
  - "BR-TRB-OFFSET-PAGING-RETIRED"
uses-terms:
  - "TERM-PUBLIC-API"
  - "TERM-COLLECTION-PAGE"
  - "TERM-PAGINATION-CURSOR"
  - "TERM-API-FILTER"
  - "TERM-FIELD-SELECTION"
  - "TERM-RELATION-EXPANSION"
  - "TERM-GENERATED-FIELD"
  - "TERM-API-ITEM-REFERENCE"
  - "TERM-CUSTOM-FIELD"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/15274709111826.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An external application obtains exactly the workspace records and fields it needs.

## Trigger

The application needs data, for example on a schedule.

## Preconditions

The application holds a valid token whose owner may see the records.

## Main Flow

1. The application requests a collection, optionally with filters (including on custom fields), sorting, a page size up to 1500, selected fields and relations to expand.
2. The product returns the first page with the matching records and, in cursor mode, the address of the next page.
3. The application follows next-page addresses until the address is empty.
4. For a single record, the application requests it by its item reference.

## Alternative Flows

Sorting on another column than the identifier: the product pages by offset and gives no next-page address. Generated fields are returned only when selected.

## Failure Conditions

Invalid or revoked token: refused. Rate limit exceeded: refused; the application waits and retries. Unknown field or relation: the request is refused with an explanation.

## Postconditions

The application has the records it asked for.
