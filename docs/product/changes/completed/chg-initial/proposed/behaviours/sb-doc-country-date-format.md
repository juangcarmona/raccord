---
id: "SB-DOC-COUNTRY-DATE-FORMAT"
type: structured-behaviour
title: "Dates follow the template country"
status: draft
illustrates:
  - "BR-DOC-COUNTRY-FORMATTING"
given:
  - "The quotation template is set to country Spain in one case and to Germany in another"
when: "A quotation for a project starting on 23 June 2018 is generated in each case"
then:
  - "With country Spain the date is printed as 23/06/2018"
  - "With country Germany the date is printed as 23.06.2018"
uses-terms:
  - "TERM-TEMPLATE-LOCALE"
provenance:
  source: ".private/research/help-center/articles/360013503420.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Regional conventions come from the template country.

## Boundaries

None.
