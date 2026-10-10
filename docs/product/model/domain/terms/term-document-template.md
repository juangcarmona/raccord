---
id: "TERM-DOCUMENT-TEMPLATE"
type: domain-term
title: "Document template"
status: draft
defined-in: "BC-DOCUMENTS"
synonyms:
  - "Document layout"
uses-terms:
  - "TERM-DOCUMENT-TYPE"
  - "TERM-TEMPLATE-LOCALE"
  - "TERM-LETTERHEAD"
  - "TERM-DOCUMENT-STYLE"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/360014232559.md, .private/research/help-center/articles/17707866444178.md, .private/research/help-center/articles/23413689614226.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The built-in definition of what a generated document of one document type contains and how it looks. Attributes:
- document type (TERM-DOCUMENT-TYPE);
- language and country (TERM-TEMPLATE-LOCALE);
- e-mail subject and file name;
- header text and footer text;
- digital signing options;
- default letterhead (TERM-LETTERHEAD);
- the company's branding (TERM-DOCUMENT-STYLE).

Each document type has one template, delivered with the product and branded by the company; its layout is fixed.

## Distinguish From

Generated document (TERM-GENERATED-DOCUMENT): the output of applying a template to a record. Letterhead: the page background, kept separately and combined with the template at generation. Project template (TERM-PROJECT-TEMPLATE): a starting point for projects, unrelated to documents.

## Usage

Branded and configured by administrators; selected (or proposed by project type defaults) whenever a document is generated.
