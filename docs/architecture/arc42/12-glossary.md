---
title: Glossary
arc42-section: "12"
description: Defines architecture terms used consistently across the Raccord documentation.
---
<!-- markdownlint-disable MD025 -->

# 12. Glossary

| Term | Meaning in Raccord |
| --- | --- |
| **Organization / issuer** | The legal entity using the installation and issuing invoices (one per deployment, ADR-0012). |
| **Issuer profile** | The issuer's legal name, tax ID and address used on new invoices. |
| **Customer** | A company or person Raccord bills. *Invoiceable* when tax ID and address are complete. |
| **Project** | An audiovisual job or rental engagement for one customer, with a period. |
| **Billable line** | A priced piece of project work (equipment, labour, transport, consumable, service, other) with a position and a billing state. |
| **Billing state** | *Unbilled*, *in draft* (reserved by one draft) or *invoiced* (never billable again). |
| **Planning period / usage period** | Time the equipment is away (availability) vs. time it is charged (price). Post-MVP. |
| **Invoice draft** | Editable, unnumbered invoice built from billable lines. |
| **Validation issue** | A coded reason a draft cannot be issued yet. |
| **Finalization / issuing** | Turning a valid draft into an issued invoice with a number. |
| **Invoice series** | A numbering family identified by a code (`F` ordinary, `R` rectifying). |
| **Number sequence** | The gapless counter of a series in one calendar year. |
| **Issued invoice** | Immutable invoice: snapshot of parties, lines, totals, number and dates. |
| **Issued representation** | Canonical JSON of an issued invoice plus its SHA-256 *fingerprint*. |
| **Operation date** | Date the work was delivered (project end), printed when different from the issue date. |
| **Rectifying invoice** | A new invoice correcting an issued one (Spanish *factura rectificativa*). MVP. |
| **Compliance record** | The obligation to report an issued invoice to the tax system, created at issue time. |
| **Billing record** | VERI*FACTU *registro de facturación* (alta/anulación), chained by hash. Future adapter. |
| **SIF** | *Sistema informático de facturación* — invoicing software regulated by RD 1007/2023. |
| **VERI*FACTU** | SIF mode that sends billing records to AEAT immediately. |
| **Audit event** | Append-only record of who did what, when, with facts. |
| **Migration adapter / importer** | Optional, isolated tool that reads customer-provided files into Raccord. |
