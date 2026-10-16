# Issue 166 evidence: inbound structured e-invoice status tracking

The Playwright journey receives a UBL invoice through the authenticated intake boundary, preserves its supplier identity and original XML hash, reports acceptance, records full payment, lists the resulting status, and captures the rendered lifecycle.

Product references:

- [UC-EXCHANGE-B2B-E-INVOICE](../../../model/use-cases/uc-exchange-b2b-e-invoice.md)
- [FR-B2B-E-INVOICING](../../../model/requirements/functional/fr-b2b-e-invoicing.md)
- [CON-B2B-E-INVOICE-REGULATION](../../../model/requirements/constraints/con-b2b-e-invoice-regulation.md)
- [BR-B2B-STATUS-REPORTING](../../../changes/completed/chg-initial/proposed/business-rules/br-b2b-status-reporting.md)

This increment deliberately implements the local intake and status-tracking boundary. It does not claim connectivity to a public exchange, certificate handling, protocol conformance, or legal compliance until those external requirements are selected and reviewed.

The hosted CI artifact contains the screenshot and trace from the successful run.
