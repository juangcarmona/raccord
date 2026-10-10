---
id: TERM-RESOURCE-IMPORT
type: domain-term
title: "Crew and vehicle import"
status: draft
defined-in: "BC-CREW"
synonyms: []
uses-terms:
  - "TERM-CREW-MEMBER"
  - "TERM-VEHICLE"
  - "TERM-RECORD-FOLDER"
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A bulk load of crew members or vehicles from a single-sheet spreadsheet or delimited text file. Attributes: import mode (create every row; update existing records; add new and skip existing), optional header row, column mapping to record fields (including custom fields), match columns for update and skip modes, a root folder under which folder paths from the file are created, and a result listing imported and failed rows with reasons, downloadable as a file. Every import tags the created records with its date and time.

## Distinguish From

Not a way to change roles, rates or passwords.

## Usage

Setting up the company, bulk updates.
