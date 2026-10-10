# Spanish invoicing compliance requirements (SIF, VERI*FACTU, B2B e-invoicing)

Research date: **2026-10-06**. All sources are official Spanish primary sources (BOE, AEAT electronic office and developer portal, Ministry of Hacienda), accessed on 2026-10-06 unless stated otherwise. This note paraphrases them. It is an engineering input for Raccord and **not legal advice**. Anything marked *unverified* or listed in [Open questions](#11-open-questions-requiring-legal-review) needs Spanish tax/legal review before Raccord relies on it.

It complements the [audiovisual operations domain note](audiovisual-operations-domain.md) with record-level, protocol-level and design-level detail on Spanish invoicing.

## Sources

| Short name | Document | Version / status checked |
|---|---|---|
| RRSIF | [Real Decreto 1007/2023, Reglamento de requisitos de los sistemas informáticos de facturación (consolidated)](https://www.boe.es/buscar/act.php?id=BOE-A-2023-24840) | Consolidated text last updated 2025-12-03 (in force 2025-12-04) |
| RD 254/2025 | [Real Decreto 254/2025 (amends RRSIF)](https://www.boe.es/buscar/act.php?id=BOE-A-2025-6600) | In force 2025-04-03 |
| RDL 15/2025 | [Real Decreto-ley 15/2025, final provision 1](https://www.boe.es/buscar/doc.php?id=BOE-A-2025-24446) | In force 2025-12-04; ratified by Congress (Resolution of 2025-12-11, [BOE-A-2025-25695](https://www.boe.es/buscar/doc.php?id=BOE-A-2025-25695)) |
| OM SIF | [Orden HAC/1177/2024 (consolidated)](https://www.boe.es/buscar/act.php?id=BOE-A-2024-22138) | Includes the correction published in BOE of 2024-11-08 |
| ROF | [Real Decreto 1619/2012, Reglamento de facturación (consolidated)](https://www.boe.es/buscar/act.php?id=BOE-A-2012-14696) | Includes the RD 238/2026 amendments (in force 2026-04-20) |
| Ley 18/2022 | [Ley 18/2022, art. 12 and final provision 8](https://www.boe.es/buscar/act.php?id=BOE-A-2022-15818) | Consolidated |
| RD 238/2026 | [Real Decreto 238/2026, mandatory B2B e-invoicing (consolidated)](https://www.boe.es/buscar/act.php?id=BOE-A-2026-7295) | Published 2026-03-31, in force 2026-04-20 |
| OM e-invoice | [Orden HAC/1028/2026, public e-invoicing solution](https://www.boe.es/buscar/doc.php?id=BOE-A-2026-20587) | Published BOE 2026-10-05, in force 2026-10-06 |
| AEAT SIF hub | [AEAT: SIF and VERI*FACTU](https://sede.agenciatributaria.gob.es/Sede/iva/sistemas-informaticos-facturacion-verifactu.html) and [technical information index](https://sede.agenciatributaria.gob.es/Sede/iva/sistemas-informaticos-facturacion-verifactu/informacion-tecnica.html) | Index page updated 2026-03-26 |
| AEAT WS | [Web service description "Sistemas Informáticos de Facturación"](https://www.agenciatributaria.es/static_files/AEAT_Desarrolladores/EEDD/IVA/VERI-FACTU/Veri-Factu_Descripcion_SWeb.pdf) | v1.0.3, 2025-07-28 |
| AEAT hash | [Hash (huella) specification](https://www.agenciatributaria.es/static_files/AEAT_Desarrolladores/EEDD/IVA/VERI-FACTU/Veri-Factu_especificaciones_huella_hash_registros.pdf) | v0.1.2, 2024-08-27 |
| AEAT QR | [QR code and verification URL specification](https://www.agenciatributaria.es/static_files/AEAT_Desarrolladores/EEDD/IVA/VERI-FACTU/DetalleEspecificacTecnCodigoQRfactura.pdf) | v0.5.0, 2025-12-10 |
| AEAT FAQ-Dev | [Clarifications for developers](https://www.agenciatributaria.es/static_files/AEAT_Desarrolladores/EEDD/IVA/VERI-FACTU/FAQs-Desarrolladores.pdf) | v1.3, 2025-12-04 |
| AEAT XSD/WSDL | [SistemaFacturacion.wsdl](https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tikeV1.0/cont/ws/SistemaFacturacion.wsdl), [SuministroLR.xsd](https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tikeV1.0/cont/ws/SuministroLR.xsd), [SuministroInformacion.xsd](https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/tikeV1.0/cont/ws/SuministroInformacion.xsd) | Production schemas, version 1.0 |
| AEAT signature | [Signature specification page (developer portal)](https://sede.agenciatributaria.gob.es/Sede/iva/sistemas-informaticos-facturacion-verifactu/informacion-tecnica/especificaciones-tecnicas-firma-electronica-registros-evento.html) | Links spec v0.1.5; **PDF not reviewed** for this note |
| Hacienda note | [Ministry press note, 2026-10-05](https://www.hacienda.gob.es/sgt/gabsehacienda/nota-informativa-verifactu.pdf) | Announcement only, not law |

---

## 1. Scope

### What a SIF is

The RRSIF (art. 1.2) defines a *sistema informático de facturación* (SIF) as the hardware and software used to issue invoices by doing any of the following:

- accepting billing data by any method;
- keeping billing data, whether stored locally, exported to another medium, or sent to another system;
- processing billing data to produce derived results, wherever that processing happens.

"Invoice" covers both full and simplified invoices as defined in the ROF. The definition is functional. A system that drafts, numbers and issues invoices for a business is a SIF even when it also does other things. AEAT FAQ-Dev no. 10 separates the SIF, which generates and keeps its own billing records, from a wider invoice-management system that may also hold invoices imported from other SIFs.

### Who is obligated

- **Taxpayers** (RRSIF art. 3.1): corporate income tax (IS) taxpayers; personal income tax (IRPF) taxpayers with economic activities; non-resident income tax taxpayers with a permanent establishment; and income-attribution entities that carry on economic activities. These taxpayers are in scope even if they use a billing system for only part of their activity.
- **Producers and resellers** of SIF software (art. 3.2), for their production and marketing activities.
- **Excluded:**
  - taxpayers who keep VAT books through the immediate information supply system (SII, RIVA art. 62.6) (art. 3.3);
  - certain operations listed in art. 4: ROF additional provisions 3 and 6, invoices for foreign permanent establishments, and some self-billing/third-party cases where the issuer uses the SII;
  - exempt entities under IS law art. 9.1.
- **Territory** (art. 1.3): the RRSIF applies across Spain, except taxpayers domiciled for tax purposes in the Basque Country or Navarre, which have their own foral regimes. Canary Islands, Ceuta and Melilla follow the equivalent IGIC/IPSI rules.
- **Opt-out** (art. 5): the AEAT may grant a resolution that the regulation does not apply to a sector or taxpayer.

### Two modes

The RRSIF arts. 15–16 and the OM SIF set out two modes, which AEAT calls modalities:

| | VERI*FACTU ("sistema de emisión de facturas verificables") | NO VERI*FACTU |
|---|---|---|
| Sending to AEAT | Every billing record is sent automatically and immediately to the AEAT | Records are kept locally and sent only when the AEAT requests them (*requerimiento*) |
| Signature | Not required; the hash is enough (RRSIF art. 16.3) | XAdES electronic signature on every billing and event record (OM SIF art. 14) |
| Event log | Not required. FAQ-Dev no. 15 says a VERI*FACTU-only product need not implement one, but a dual-mode product must | Mandatory (OM SIF art. 9) |
| Presumption | Presumed compliant by design with RRSIF art. 8; OM SIF art. 3 waives OM arts. 6.b–f, 7.f, 7.h–j, 8 and 9 | Must meet every requirement itself |
| Invoice legend | QR code plus the legend "VERI*FACTU" or "Factura verificable en la sede electrónica de la AEAT" | QR code only, pointing to a different AEAT URL |
| Lock-in | Once chosen, the mode must be kept at least until 31 December of that year; opting out is signalled in the submission header before year end (OM SIF art. 17, field `FechaFinVeriFactu`) | — |

Every SIF must be *capable* of sending its records to the AEAT, whatever the mode (RRSIF art. 8.1, second paragraph; OM SIF art. 4).

### Deadlines: enacted vs announced (status at 2026-10-06)

| Item | Date | Status |
|---|---|---|
| RRSIF in force | 2023-12-07 | Enacted |
| OM SIF published / in force | 2024-10-28 / 2024-10-29 | Enacted |
| Producers must offer adapted products | Nine months after the OM SIF entered into force (late July 2025) | Enacted (RRSIF final provision 4) |
| AEAT VERI*FACTU services in production | 2025-04-23 | Fact, per AEAT FAQ-Dev |
| Adaptation deadline, IS taxpayers | **Before 2027-01-01** | **Enacted** by RDL 15/2025 (moved from 2026-01-01 set by RD 254/2025); the RDL was ratified on 2025-12-11 |
| Adaptation deadline, all other obligated taxpayers | **Before 2027-07-01** | **Enacted** by RDL 15/2025 (moved from 2026-07-01) |
| Move remaining SIF obligations for SMEs and self-employed to **October 2028**, aligned with B2B e-invoicing for businesses with turnover up to €8m, followed by a later reform to converge scope and technical aspects with the EU "VAT in the Digital Age" (ViDA) package | — | **Announced only** (Hacienda note of 2026-10-05, titled "previsión"). No BOE text exists as of 2026-10-06, and the consolidated RRSIF still shows the 2027 dates. The note says the integrity, conservation, accessibility, legibility, traceability and inalterability guarantees would stay substantially the same. |

Raccord must plan against the enacted 2027 dates until a BOE text changes them. The note does not say clearly whether the IS-taxpayer deadline of 2027-01-01 would also move (see open questions).

---

## 2. Invoice content (ROF, RD 1619/2012)

### Full invoice (ROF art. 6.1)

Mandatory fields:

- **Number and, where used, series.** Numbering is correlative within each series.
- **Issue date.**
- **Full legal name** of both issuer and recipient.
- **Issuer's NIF**: the Spanish tax ID, or the VAT ID of another EU member state under which the operation was carried out.
- **Recipient's NIF**, required when:
  - the supply is an exempt intra-EU delivery;
  - the recipient is liable for the tax (reverse charge);
  - the operation is located in Spain and the issuer is established in Spain.
- **Address** of issuer and recipient. Where either party has several fixed places of business, state the relevant establishment if it affects the tax treatment.
- **Description of the operations**, with every figure needed to determine the taxable base: unit price excluding tax, and any discount not already in the unit price.
- **Tax rate(s)** applied.
- **Tax amount (cuota)**, shown separately.
- **Operation date**, or advance-payment date, when it differs from the issue date.
- **Exemption reference**: the legal provision (Spanish VAT law or Directive 2006/112/EC) or a statement that the operation is exempt.
- **Special mentions** when they apply:
  - "facturación por el destinatario" (self-billing);
  - **"inversión del sujeto pasivo"** (reverse charge);
  - the travel agencies, used goods/art/antiques and cash-accounting special-regime mentions;
  - the new-means-of-transport data.
- **Separate taxable bases** (art. 6.2) when one invoice mixes exempt and non-exempt operations, reverse-charge and normal operations, or different VAT rates.
- **For invoices issued with a SIF** (art. 6.5, added by the RRSIF): the tax **QR code** and, in VERI*FACTU mode only, the legend. In a structured e-invoice the QR content (the URL) may replace the image.

### Simplified invoice (ROF arts. 4 and 7)

Allowed when:

- the total is at most €400 including VAT;
- a rectifying invoice is being issued;
- the total is at most €3,000 for listed retail-type activities.

Minimum content:

- number and series, correlative within the series;
- issue date, and operation date if different;
- issuer's NIF and name;
- type of goods or services;
- tax rate, optionally with the phrase "IVA incluido", plus a split base per rate when several rates apply;
- total consideration;
- for rectifying invoices, an unambiguous reference to the invoice corrected;
- the special mentions of art. 6.1.j–p.

When a business recipient asks, the invoice must also show the recipient's NIF and address and the tax amount shown separately. This is a "qualified" simplified invoice (art. 7.2). The art. 6.5 QR and legend requirement also applies to simplified invoices (art. 7.5).

### Numbering rules

- Numbering is correlative within each series (arts. 6.1.a and 7.1.a).
- Separate series are allowed when justified, for example several establishments or operations of different kinds.
- **Separate series are mandatory** for:
  - **rectifying invoices**;
  - invoices issued by the recipient or by third parties (one series for each);
  - certain special VAT cases (art. 6.1.a, points 3–5).
- A business that issues both full and simplified invoices in the same calendar year must use separate series for each (art. 7.1.a).
- The SIF must never produce two billing records for the same invoice identifier. AEAT FAQ-Dev no. 6 covers this, and the AEAT rejects duplicates (section 6).

### Issue deadline (ROF art. 11)

- In general, the invoice is issued when the operation takes place.
- When the recipient is a business acting as such, the invoice must be issued before the 16th day of the month after VAT became chargeable.

### Rectifying invoices (ROF art. 15)

- **When mandatory:**
  - the original fails art. 6 or 7 requirements;
  - the VAT charged was wrong;
  - the taxable base changes under VAT law art. 80 (discounts after supply, returns, cancellation, unpaid debts in insolvency or after collection attempts, and similar).
- **Time limit:** issue as soon as the cause is known, and no later than four years after VAT became chargeable or the art. 80 circumstance arose.
- **Form:** a new invoice that identifies the rectified invoice or invoices. One rectifying document may cover several invoices. Volume discounts, or other cases the AEAT authorises, may refer to a period instead of listing invoices.
- **Content:** the rectifying invoice meets art. 6 or 7. Its base and tax figures show either the amount of the rectification or the corrected figures together with the rectification amount.
- **Not rectifying invoices:** an invoice issued to replace earlier valid simplified invoices (art. 15.6). It is coded F3 (see below).
- **The two record-level methods** (OM SIF list L3, field `TipoRectificativa`):
  - **S – by substitution**: the corrected invoice fully replaces the original. The record carries the original's base and tax (`ImporteRectificacion`).
  - **I – by differences**: the rectifying invoice carries only the delta.

### Rectifying invoice codes (OM SIF list L2, `TipoFactura`)

| Code | Meaning (paraphrased) |
|---|---|
| R1 | Error grounded in law, and VAT law art. 80 One, Two and Six (for example price changes, returned goods, cancelled operations) |
| R2 | VAT law art. 80.3: recipient in insolvency proceedings |
| R3 | VAT law art. 80.4: uncollectable debts |
| R4 | All other causes |
| R5 | Rectification of simplified invoices |

---

## 3. Billing records

### Generation rule

A *registro de facturación de alta* must be generated automatically at the same time as, or immediately before, the issue of each invoice (RRSIF art. 9). A *registro de facturación de anulación* is generated only when an invoice was issued by mistake and its alta record must be cancelled (art. 11.1). It is not a credit note. Corrections to real operations go through rectifying invoices, which produce new alta records.

### Format

Records use XML in UTF-8, structured as in the OM SIF annex (arts. 10–11). For legibility, records kept or exported must keep that same structure (OM SIF art. 8.5). In the AEAT schemas the blocks are `RegistroAlta`, `RegistroAnulacion`, `RegistroEvento` and `SistemaInformatico`. All amounts are expressed in **euros**, converting foreign-currency invoices (RRSIF art. 10.2).

### IDFactura

`IDFactura` is the unique key of a billing record (AEAT WS §3). It has three parts:

- `IDEmisorFactura` (issuer NIF);
- `NumSerieFactura` (series and number combined in one string of at most 60 characters);
- `FechaExpedicionFactura` (format `dd-mm-yyyy`).

In an anulación record the elements are `IDEmisorFacturaAnulada`, `NumSerieFacturaAnulada` and `FechaExpedicionFacturaAnulada`.

### Alta record content

Sources: RRSIF art. 10; element names from the AEAT XSD and WS spec examples.

- **Identification and header data:**
  - `IDVersion` (schema version 1.0);
  - `IDFactura`;
  - `NombreRazonEmisor`;
  - `RefExterna` (optional, free text up to 60 characters; useful as Raccord's correlation key).
- **Correction flags:** `Subsanacion` (S/N) and `RechazoPrevio` (N/S/X, list L17).
- **Invoice type and rectification:**
  - `TipoFactura` (F1, F2, F3, R1–R5);
  - `TipoRectificativa` (S/I);
  - `FacturasRectificadas`;
  - `FacturasSustituidas` (for F3);
  - `ImporteRectificacion`.
- **Dates:** `FechaOperacion`, when it differs from the issue date.
- **Description:** `DescripcionOperacion`.
- **Special-case flags:**
  - `FacturaSimplificadaArt7273` (qualified simplified invoice);
  - `FacturaSinIdentifDestinatarioArt61d`;
  - `Macrodato` (very large amount flag).
- **Third parties and recipients:**
  - `EmitidaPorTerceroODestinatario` with `Tercero` (self-billing or third-party issue, list L6);
  - `Destinatarios`/`IDDestinatario`: name plus `NIF`, or `IDOtro` with country, ID type (list L7) and ID.
- **Coupon flag:** `Cupon`.
- **Tax breakdown** (`Desglose`/`DetalleDesglose`, one line per regime/qualification/rate):
  - `Impuesto` (01 VAT, 02 IPSI, 03 IGIC, 05 other);
  - `ClaveRegimen`;
  - `CalificacionOperacion` or `OperacionExenta`;
  - `TipoImpositivo`;
  - `BaseImponibleOimporteNoSujeto`;
  - `BaseImponibleACoste`;
  - `CuotaRepercutida`;
  - `TipoRecargoEquivalencia` and `CuotaRecargoEquivalencia`.
- **Totals:** `CuotaTotal` and `ImporteTotal`.
- **Chaining:** `Encadenamiento`, containing either `PrimerRegistro`=S or `RegistroAnterior`, which holds the previous record's IDFactura data and its `Huella`.
- **System identification:** `SistemaInformatico` (see below).
- **Generation timestamp:** `FechaHoraHusoGenRegistro`.
- **Agreements:** `NumRegistroAcuerdoFacturacion` and `IdAcuerdoSistemaInformatico`, used when an authorisation or non-application resolution exists (OM SIF art. 12).
- **Hash:** `TipoHuella` (01 = SHA-256) and `Huella`.
- **Signature:** the XAdES signature, NO VERI*FACTU only.

`SistemaInformatico` identifies the system and its producer:

- producer name and NIF (or other ID);
- `NombreSistemaInformatico`;
- `IdSistemaInformatico`: a short code chosen by the producer, unique among that producer's systems;
- `Version`;
- `NumeroInstalacion`;
- `TipoUsoPosibleSoloVerifactu`;
- `TipoUsoPosibleMultiOT`;
- `IndicadorMultiplesOT`.

### Anulación record content (RRSIF art. 11)

- `IDFactura` of the cancelled invoice (issuer NIF, series and number, issue date);
- `SinRegistroPrevio`: set when the alta never reached the AEAT;
- `RechazoPrevio`;
- `GeneradoPor` and `Generador` (issuer, recipient or third party, list L16);
- `Encadenamiento`;
- `SistemaInformatico`;
- `FechaHoraHusoGenRegistro`;
- `TipoHuella` and `Huella`;
- the signature, in NO VERI*FACTU mode.

### Code lists

Code lists come from the OM SIF annex, section 6, cross-checked against the production XSD.

**`TipoFactura` (L2)**

| Code | Meaning |
|---|---|
| F1 | Full invoice (ROF arts. 6, 7.2, 7.3) |
| F2 | Simplified invoice, or invoice without recipient identification (ROF art. 6.1.d) |
| F3 | Invoice issued to replace simplified invoices already issued and declared |
| R1–R5 | Rectifying invoices (see section 2) |

**`ClaveRegimen` for VAT (L8A)**

| Key | Regime |
|---|---|
| 01 | General regime |
| 02 | Export |
| 03 | Used goods, art and antiques |
| 04 | Investment gold |
| 05 | Travel agencies |
| 06 | VAT group (advanced level) |
| 07 | Cash accounting |
| 08 | Operations subject to IPSI/IGIC |
| 09 | Travel agencies acting as intermediaries |
| 10 | Collections on behalf of third parties (professional fees and IP rights) |
| 11 | Business-premises leases |
| 14 | VAT pending accrual: public-sector construction certifications |
| 15 | VAT pending accrual: successive supplies |
| 17 | OSS/IOSS |
| 18 | Equivalence surcharge |
| 19 | Agriculture, livestock and fishing special regime (REAGYP) |
| 20 | Simplified regime |

L8B is the parallel list for IGIC. The production XSD also accepts key **21**, which the BOE annex does not list. Its meaning is *unverified* here; check the AEAT record-design workbook.

**`CalificacionOperacion` (L9)**

| Code | Meaning |
|---|---|
| S1 | Subject and not exempt, no reverse charge |
| S2 | Subject and not exempt, **with reverse charge** |
| N1 | Not subject under VAT law arts. 7, 14 and others |
| N2 | Not subject because of place-of-supply rules |

**`OperacionExenta` (L10)**

| Code | Exempt under VAT law |
|---|---|
| E1 | Art. 20 |
| E2 | Art. 21 |
| E3 | Art. 22 |
| E4 | Arts. 23 and 24 |
| E5 | Art. 25 |
| E6 | Other |

The production XSD also accepts **E7** and **E8**, which the BOE annex does not list. Their meanings are *unverified* here.

**Event types (L2E, NO VERI*FACTU)**

| Code | Event |
|---|---|
| 01 | Start of operation as NO VERI*FACTU |
| 02 | End of operation as NO VERI*FACTU |
| 03 | Billing-record anomaly check launched |
| 04 | Anomaly detected in billing records |
| 05 | Event-record anomaly check launched |
| 06 | Anomaly detected in event records |
| 07 | Backup restored |
| 08 | Billing records exported |
| 09 | Event records exported |
| 10 | Event summary |
| 90 | Other, producer-defined |

**Anomaly types (L1E)**

Codes 01–15 and 90 cover hash, signature, chain-link and date-order anomalies.

---

## 4. Hash chaining (huella)

Sources: OM SIF art. 13; AEAT hash specification v0.1.2.

- **Algorithm:** SHA-256 (list L12, value `01`). It is the only algorithm allowed at the date of the AEAT document.
- **Input:** the fields below are concatenated in the order given, as `name=value` pairs joined by `&`. Field names are the literal XML element names. Values match the XML content after these rules:
  - leading and trailing spaces are trimmed;
  - numeric values with one or two decimals, or trailing zeros, are equivalent (`123.1` = `123.10`).
- **Alta record order:**
  `IDEmisorFactura`, `NumSerieFactura`, `FechaExpedicionFactura`, `TipoFactura`, `CuotaTotal`, `ImporteTotal`, `Huella` (the previous record's hash), `FechaHoraHusoGenRegistro`.
- **Anulación record order:**
  `IDEmisorFacturaAnulada`, `NumSerieFacturaAnulada`, `FechaExpedicionFacturaAnulada`, `Huella` (previous), `FechaHoraHusoGenRegistro`.
- **Event record order (NO VERI*FACTU):** producer `NIF`, producer `ID` (only one of the two is filled; the other appears as `ID=`), `IdSistemaInformatico`, `Version`, `NumeroInstalacion`, obligated party `NIF`, `TipoEvento`, `HuellaEvento` (previous), `FechaHoraHusoGenEvento`.
- **Missing or empty fields** still appear, with nothing after the `=`.
- **First record:** the previous-hash value is empty (`...&Huella=&...`). The record carries `PrimerRegistro`=S and no `RegistroAnterior` block, but **its own hash must still be computed and stored**.
- **Encoding and output:**
  - the input string is encoded as UTF-8 bytes;
  - the output is the hexadecimal digest in **uppercase, 64 characters**;
  - the value goes in `RegistroAlta/Huella`, `RegistroAnulacion/Huella` or `RegistroEvento/Evento/HuellaEvento`.
- **Chain scope:**
  - one chain per obligated taxpayer per SIF (OM SIF art. 7.c);
  - the chain mixes alta and anulación records in order of generation time (art. 7.d);
  - event records form their own separate chain.
- **Formats inside the hash input:**
  - dates are `dd-mm-yyyy`;
  - the generation timestamp is ISO 8601 with a time-zone offset, for example `2024-01-01T19:20:30+01:00`.
- **Validation:** if a VERI*FACTU submission's hash does not match AEAT's own calculation, the record is marked *AceptadoConErrores* (accepted with errors).
- **Verified for this note:** AEAT example case 1 hashes to `3C464DAF61ACB827C65FDA19F352A4E3BDC2C640E9E9FC4CC058073F38F12F60`, and a plain `sha256` over the documented string reproduces it. Raccord should use the three AEAT examples as golden test vectors.

Related traceability rules (OM SIF art. 7):

- Before generating a record, check that the last record is correctly chained, and that its timestamp is not more than one minute later than the timestamp about to be used. FAQ-Dev no. 15 explains that the second check stops time going backwards; long gaps between records are normal.
- The record timestamp must reflect the time in the territory the invoice is issued from and include the time-zone offset (art. 7.e, 7.g).
- The user must ensure the clock is accurate to within one minute (art. 7.f). Art. 3 waives art. 7.f for VERI*FACTU, but accurate time is still good practice.
- Users must be able to navigate the chain forwards and backwards, seeing whether each link and its time order are valid (art. 7.h; waived for VERI*FACTU).
- If a check fails, the system raises an alarm and logs an event, but **billing must never be blocked** (FAQ-Dev no. 15). This applies to NO VERI*FACTU.

---

## 5. QR code

Sources: OM SIF arts. 20–21; ROF art. 6.5; AEAT QR specification v0.5.0.

### Technical requirements

- **Standard:** ISO/IEC 18004 (the AEAT document cites the 2015 edition).
- **Error correction:** level **M**.
- **Size:** between **30×30 mm and 40×40 mm**.
- **Contrast:** enough for reliable reading.
- **Quiet zone:** at least 2 mm of white space on every side; 6 mm recommended.

### Content: a URL

The QR encodes a URL to the AEAT verification service. The parameters are, in this order:

| Parameter | Content |
|---|---|
| `nif` | Issuer NIF, 9 characters |
| `numserie` | Series and number, at most 60 characters, printable ASCII 32–126 only |
| `fecha` | Issue date, `DD-MM-AAAA` |
| `importe` | Invoice total, dot as decimal separator, at most 12 integer digits and 2 decimals |

Encoding rules:

- Parameter values must be **URL-encoded as UTF-8**. For example, `&` inside `numserie` becomes `%26`.
- Two optional parameters exist: `idioma` (es, ca, gl, eu, va, en) and `formato=json`. The `formato` parameter must **never** be placed in the printed QR; it is for machine verification only.

Base URLs:

| Mode | Production | Test (Portal de Pruebas Externas) |
|---|---|---|
| VERI*FACTU | `https://www2.agenciatributaria.gob.es/wlpl/TIKE-CONT/ValidarQR?...` | `https://prewww2.aeat.es/wlpl/TIKE-CONT/ValidarQR?...` |
| NO VERI*FACTU | `https://www2.agenciatributaria.gob.es/wlpl/TIKE-CONT/ValidarQRNoVerifactu?...` | `https://prewww2.aeat.es/wlpl/TIKE-CONT/ValidarQRNoVerifactu?...` |

### Placement and labels

- The QR appears **once, on the first page**, at the start of the invoice before the system-generated content. It must be the first QR on the invoice.
- Portrait layout: near the top margin, preferably centred, otherwise top-left. Landscape layout: on the left, preferably top-left.
- Where there is a justified obstacle, the QR may sit elsewhere but must stay prominent and clearly separated from other content.
- The label **"QR tributario:"** always goes directly above the QR.
- In **VERI*FACTU** mode, the legend **"Factura verificable en la sede electrónica de la AEAT"** or **"VERI*FACTU"** goes directly below the QR, preferably centred. It may wrap over several lines. NO VERI*FACTU invoices do not carry this legend.
- Label and legend fonts must be legible and no smaller than the rest of the invoice data.

### Structured e-invoices

In a structured e-invoice, include the QR URL as a separate field instead of the image (OM SIF art. 20.2).

### Restrictions on customisation

FAQ-Dev no. 9: letting users move the QR within the top area is a legitimate customisation. Letting users turn the mandatory QR off is not.

---

## 6. Submission (VERI*FACTU and responses to requests)

Sources: OM SIF arts. 4, 5, 16–18; AEAT WS v1.0.3; WSDL.

### Transport

- SOAP 1.1 over HTTPS, document/literal style, UTF-8.
- Responses are synchronous.
- The client authenticates with a **qualified electronic certificate** accepted by the AEAT.
- Endpoints (from the production WSDL):
  - production voluntary sending: `https://www1.agenciatributaria.gob.es/wlpl/TIKE-CONT/ws/SistemaFacturacion/VerifactuSOAP`;
  - production alternative host: `www10.agenciatributaria.gob.es` (the WSDL also lists it, presumably the seal-certificate variant; *unverified*);
  - responses to requests: `.../RequerimientoSOAP`;
  - test: `prewww1.aeat.es` and `prewww10.aeat.es`.
- One XSD serves both voluntary sending and responses to requests, but the two services are separate AEAT systems that do not share records.

### Message structure

- A `Cabecera` header holds the obligated issuer, optional `Representante`, and either `RemisionVoluntaria` (with optional `FechaFinVeriFactu` and `Incidencia`) or `RemisionRequerimiento` (with `RefRequerimiento` and optional `FinRequerimiento`).
- It is followed by **1 to 1,000** `RegistroFactura` items. Alta and anulación records may be mixed in one message (OM SIF annex §2.2 and §4; XSD `maxOccurs="1000"`).

### Flow control (tiempo de espera, OM SIF art. 16.2)

- The initial wait between submissions is **60 seconds**.
- Every response returns `TiempoEsperaEnvio` (seconds), which applies to the next submission.
- The next submission may go when **either** `t` seconds have passed since the previous one **or** 1,000 records are waiting, whichever comes first.

### Validation and responses

- **Rejection of the whole message:** a schema or header error returns a SOAP `Fault`. A `Client` fault means the message was malformed or wrong; fix it before resending. A `Server` fault, or no valid response at all, means resend.
- **Global status (`EstadoEnvio`):**
  - `Correcto`: every record is correct;
  - `ParcialmenteCorrecto`: some records are `Incorrecto` or `AceptadoConErrores`;
  - `Incorrecto`: every record was rejected.
- **Per-record status (`EstadoRegistro`):**
  - `Correcto`: registered;
  - `AceptadoConErrores`: registered despite an "admissible" error;
  - `Incorrecto`: rejected for a "non-admissible" error and **not registered**.
- Each record line comes with `CodigoErrorRegistro` and `DescripcionErrorRegistro`. The full catalogue of error codes is in AEAT's separate document of validations and errors.
- **Duplicates:** a rejected duplicate returns details of the record already stored: `IdPeticionRegistroDuplicado` and `EstadoRegistroDuplicado` (`Correcta`, `AceptadaConErrores` or `Anulada`).
- **CSV:** a 16-character secure verification code (*código seguro de verificación*) is returned whenever at least one record is accepted. The AEAT warns that the system **must store the CSV when it receives it, because it cannot be retrieved later**. The response also returns `TimestampPresentacion` (AEAT server time zone) and `NIFPresentador`.
- **Acceptance is not final:** an accepted record can still be reviewed later (OM SIF art. 16.3).

### Error handling and corrections

AEAT WS §9 and FAQ-Dev no. 17 describe the cases below. For each one, the original record stays unchanged and a **new** chained record is generated.

| Case | Action |
|---|---|
| Error that the ROF says requires rectification (amounts, rates, parties and so on) | Issue a rectifying invoice, which produces new alta records (R1–R5). Do **not** change the original record, even if the AEAT rejected it or accepted it with errors. |
| Error only in record-level "internal" data (for example a tax code) with no ROF rectification needed; original accepted, or accepted with errors | New alta record with `Subsanacion`=S |
| Same, but the original alta was **rejected** and never existed at the AEAT | New alta record with `Subsanacion`=S and `RechazoPrevio`=X ("alta por rechazo") |
| Invoice issued by mistake (no real operation behind it, for example a test invoice) | Anulación record. Add `SinRegistroPrevio`=S if the alta never reached the AEAT, or `RechazoPrevio`=S if an earlier anulación was rejected. |
| Error affecting neither the ROF nor any record field | Correct the invoice document; no new record |

Additional rules:

- For **voluntary sending**, rejected records and records accepted with errors must be corrected and resent, unless a rectifying invoice or an anulación applies instead.
- For **responses to a request**, business-validation errors must **not** be corrected. The records must be sent exactly as kept.

### AEAT unavailable or connectivity incident (OM SIF art. 16.4)

- Keep generating and chaining records locally.
- Send pending records as soon as possible, **in order of generation**, flagged with `Incidencia`=S.
- **Retry at least once an hour.**
- Show a persistent warning, with the number of records still pending, until all have been sent.
- Be ready to justify the incident if the AEAT asks.

### Certificates and representation

Sources: OM SIF art. 5; AEAT WS §4.1; FAQ-Dev no. 16.

The sender may be:

- the taxpayer, with its own qualified certificate;
- a proxy (apoderado);
- a **social collaborator** (colaborador social) under RD 1065/2007 arts. 79–81 and Orden HAC/1398/2003.

Software companies can sign collaboration agreement **Convenio 17**, which allows direct and indirect representation, in addition to the existing 001 and 002 agreements. Standard representation forms exist (AEAT Resolution of 2024-12-18), and FAQ-Dev no. 16 accepts electronic consent flows as long as the grant of representation is unambiguous. The header's `Representante` element records an adviser who sits between the taxpayer and the sending platform.

### Query service (VERI*FACTU only)

- Issuers and recipients can query the records already submitted, filtered by year and period.
- Results come in pages of up to 10,000 records.
- `RefExterna` can be used as a filter.

---

## 7. NO VERI*FACTU extra requirements

- **Electronic signature** (RRSIF art. 12; OM SIF art. 14):
  - every billing record and event record is signed with **XAdES Enveloped Signature** (ETSI EN 319 132);
  - the key belongs to a **qualified signature certificate** that is valid at signing time, issued by a qualified trust service provider on the EU/EEA Trusted List (eIDAS);
  - the system must be able to verify any record's signature on demand (OM SIF art. 6.d);
  - AEAT publishes a separate technical signature profile (v0.1.5, with example annexes). It was **not reviewed** for this note, so the canonicalisation, policy and algorithm details are *unverified*.
- **Event log** (OM SIF art. 9):
  - logs the events in list L2E (section 3);
  - adds at least one **summary event every 6 hours of operation**, even when nothing happened, and one before shutdown;
  - events are XML in UTF-8, SHA-256-chained and signed;
  - events must meet the same integrity, inalterability, traceability, conservation, accessibility and legibility requirements as billing records.
- **Anomaly detection:** on-demand checks of hashes, signatures and chains (whole or partial) for billing records and events. They need not run continuously (FAQ-Dev no. 15).
- **Conservation and export** (OM SIF art. 8):
  - keep every record and allow export of all records for a period to external storage in a readable format;
  - records may be removed from the system only after a correct export, and only as consecutive records starting from the oldest, with a visible warning;
  - export is separate from backups;
  - the taxpayer keeps billing records (and event records) for the same period as invoice copies (ROF art. 19, which points to the General Tax Law, LGT), **even after changing system**.
- **Responding to requests:** send the kept records through the request web service (OM SIF art. 18), or deliver them on physical media (RRSIF art. 14.2).
- **Inspection:** the tax administration may require on-site access to records and logs, including any credentials needed (RRSIF art. 14.1).

---

## 8. System requirements and producer obligations

- **Six guarantees** (RRSIF art. 8): every SIF guarantees the integrity, conservation, accessibility, legibility, traceability and inalterability of billing records.
- **Alteration** (art. 8.2.a) means:
  - hiding or deleting any record that was generated;
  - hiding or changing any of its data, in whole or in part;
  - adding simulated or false records.
- **Corrections** happen only through at least one later additional record, so the original stays unchanged.
- **Traceability** (art. 8.2.b): **any feature that lets someone alter or hide the trail breaks this requirement**, and every entry must be correctly dated.
- **Data separation** (art. 8.4): access to tax-relevant data must be kept separate from non-tax confidential data, so the tax administration can access billing records and logs directly.
- **Several taxpayers in one SIF** (RRSIF art. 7; OM SIF art. 2):
  - keep each taxpayer's records apart;
  - allow VERI*FACTU independently per taxpayer;
  - always show which taxpayer is currently being operated on;
  - show clearly when the system serves more than one taxpayer.
- **Declaración responsable** (RRSIF art. 13; OM SIF art. 15):
  - the **producer** certifies compliance in a written statement titled "DECLARACIÓN RESPONSABLE DEL SISTEMA INFORMÁTICO DE FACTURACIÓN", visible inside the system for every version, and given to resellers and customers;
  - it contains, in this order: system name; `IdSistemaInformatico`; full version; hardware/software components and functions; whether the system is VERI*FACTU-only; whether it supports several taxpayers; signature types used in NO VERI*FACTU; producer's name, NIF or foreign ID, and postal address; the compliance statement; and the date and place of signature;
  - extensions or components from other producers need their own statements;
  - FAQ-Dev nos. 8–9: customisations that change how a requirement is implemented need their own statement; cosmetic ones do not;
  - producers keep the statements for every version.
- **Drafts, proformas and test invoices** (FAQ-Dev no. 11):
  - drafts are legitimate, but AEAT reads LGT art. 29.2.j as requiring that business-system records, drafts included, are kept unaltered or are changed only through later recorded entries;
  - a SIF running in production must not issue "fake" invoices. Test or training invoices are real invoices and must be cancelled afterwards (anulación), ideally in a dedicated, clearly labelled series.
- **Imported invoices from another SIF** (FAQ-Dev no. 10) are kept with their own records and are not re-registered, re-QR'd or re-sent.

---

## 9. B2B e-invoicing (Ley 18/2022 art. 12, RD 238/2026, Orden HAC/1028/2026)

This regime is **separate from VERI*FACTU**. VERI*FACTU registers invoice records with the AEAT. B2B e-invoicing mandates the structured exchange of the invoice itself between businesses, plus reporting of the invoice's status.

### Legal basis

Ley 18/2022 art. 12 rewrote art. 2 bis of Ley 56/2007:

- every business or professional must issue, send and receive e-invoices with other businesses, and both sides must report invoice status;
- platforms must interconnect and interoperate free of charge;
- recipients may request copies free of charge for four years;
- a recipient cannot force a supplier to use a given platform.

Final provision 8 makes art. 12 conditional on obtaining the EU derogation from Directive 2006/112/EC arts. 218 and 232. This note does **not** confirm whether that condition is satisfied, for example through the ViDA amendments (see open questions).

### Scope (RD 238/2026 arts. 3–4)

- Applies to invoices from Spanish businesses or professionals to business or professional recipients established in Spain, including self-billing and third-party issue.
- **Excluded:** simplified invoices under ROF art. 4, unless they are qualified simplified invoices (art. 7.2); sector exclusions made by order; and the electricity/gas market operators and IATA clearing systems (additional provision 2).

### Architecture

The system is called the "sistema español de factura electrónica" (art. 5). It consists of:

- **private exchange platforms**: they need ISO/IEC 27001, AS2 or AS4 transport, advanced signature and seal capability, conversion between all permitted syntaxes, business continuity, 24×7 support, data governance, and free interconnection (art. 13);
- the AEAT **public solution**, which is also the universal repository.

Further rules:

- A recipient that has not agreed a private platform is deemed to use the public solution.
- Recipients publish their entry point (art. 6).
- Interconnection between platforms must work within one month of a request (art. 9).

### Formats (art. 7)

- Data model: **EN 16931**, in one of these syntaxes: **CII**, **UBL**, **EDIFACT**, or **Facturae**.
- Private platforms must be able to convert between these.
- Invoices sent through private platforms carry an advanced electronic signature.
- Every e-invoice has a unique code built from issuer NIF + series and number + issue date. The public solution defines it as that concatenation (OM e-invoice art. 6).
- **Public solution:** UBL only, EN 16931, with the minimum elements in OM e-invoice annex I. No embedded or attached files are allowed, although interconnected invoices may carry their signature.

### Fiel copy (*copia fiel*)

- A business **not** using the public solution must send the public solution a fiel copy of each invoice in UBL, at the moment of issue (RD 238/2026 art. 6.2; OM e-invoice art. 4).
- The copy is marked with a copy indicator. Duplicate copies are rejected.
- A rejected copy must be fixed and resent while staying consistent with the original. If the original itself is wrong in a way that does not need rectification, issue a corrected original.
- A copy (or an original sent through the public solution) may be withdrawn only if the operation never existed, and the withdrawal stays traceable.
- If the public solution is down for more than 24 hours for reasons on its side, submissions may be made during the four working days after it is restored (OM e-invoice art. 5).

### Invoice status reporting

- **Between parties** (RD 238/2026 art. 10):
  - the recipient reports to the issuer the **commercial acceptance or rejection** (with date) and the **full payment** (with date);
  - optional statuses: partial acceptance or rejection, partial payment, and assignment of the invoice to a third party;
  - the deadline is **four calendar days, excluding Saturdays, Sundays and national holidays**.
- **To the public solution** (RD 238/2026 art. 12; OM e-invoice arts. 7–8):
  - the recipient reports **rejection** or **full payment with the payment date**, plus the **payment due date**; dates of receipt of goods or invoice are optional;
  - same four-day deadline;
  - in UBL, through web services or a form;
  - with no rejection and no later rectifying invoice, the invoice is presumed accepted;
  - the issuer may voluntarily report collection or non-payment;
  - an invoice already showing a payment date on or before its issue date counts as paid.
- **Access:** qualified certificate, plus Cl@ve for the forms. Third parties act through proxies registered with the AEAT or through social collaboration (OM e-invoice art. 11).
- **Public solution features:** retrieval of received invoices, consultation and download of invoices and statuses, and a free invoice form (additional provision 1). Invoices produced with the form count as verifiable under ROF art. 6.5.b.

### Interaction with the SIF rules

- E-invoices must be produced by systems compliant with LGT art. 29.2.j and the RRSIF (RD 238/2026 art. 2.d; Ley 56/2007 art. 2 bis.6).
- The VERI*FACTU QR URL travels as a field inside structured invoices (OM SIF art. 20.2).

### Deadlines

The clocks started when the OM e-invoice entered into force on **2026-10-06** (OM e-invoice final provision; RD 238/2026 final provision 4).

| Obligation | Nominal date | Status |
|---|---|---|
| Businesses with turnover above €8m | ~**2027-10-06** (12 months) | Enacted |
| All other businesses and professionals | ~**2028-10-06** (24 months) | Enacted |
| Private platforms: fiel copy, interconnection and operating requirements | ~2027-10-06 (12 months) | Enacted |
| Status reporting by natural persons and income-attribution entities with turnover up to €8m | Voluntary until 12 months after their own start date (~2029-10-06) | Enacted |
| Businesses above €8m must attach a legibility PDF (sent to the recipient, not to the public solution) unless the recipient agrees otherwise | First 12 months of their obligation | Enacted |
| Public sector subcontractors | 24 months | Enacted |
| Public solution available | At least two months before first effective application | Enacted |
| SIF dates aligned with the B2B October 2028 date | — | **Announced only** (see section 1) |

The exact calendar day (for example 2027-10-06 vs 2027-10-07) depends on how the period is counted. Confirm with counsel. AEAT technical specifications, volumetrics and web-service definitions for the public solution are to be published on the AEAT electronic office. They are **not reviewed** here.

---

## 10. Implications for Raccord's design

Raccord's architecture already separates the invoice domain, the compliance adapter, and the accounting and e-invoicing adapters (`docs/operations/claude-execution-brief.md`). The requirements below describe the **compliance seam** between the immutable invoice domain and any SIF or e-invoicing adapter.

### Inputs, outputs and evidence

1. **Domain event into the seam.** "Invoice issued" carries an immutable snapshot with everything the alta record needs:
   - issuer identity;
   - series, number and issue date;
   - type F1, F2, F3 or R1–R5, with rectification method S or I and the rectified or replaced invoice IDs;
   - recipient(s) with NIF or other ID;
   - operation date;
   - description;
   - breakdown lines per tax, regime key, qualification or exemption, rate, base, tax and surcharge;
   - totals in EUR;
   - a correlation ID that maps to `RefExterna`.

   Raccord's domain uses its own names. The XSD vocabulary appears only inside the adapter.
2. **The adapter produces a billing record** containing the canonical XML, `FechaHoraHusoGenRegistro`, the previous-record reference, the `Huella`, the QR URL, and (NO VERI*FACTU only) the signature. The record is persisted **before** the invoice document is released (RRSIF art. 9). The rendered PDF must use the same QR URL and legend.
3. **Evidence store, append-only.** For every submission attempt, store:
   - the exact request XML and the raw response;
   - the global and per-record status;
   - error codes and descriptions;
   - the **CSV**, saved on receipt;
   - `TimestampPresentacion` and `TiempoEsperaEnvio`;
   - the certificate or representation used;
   - the endpoint.
4. **Status model per record:**
   - `PendingSubmission` → `Accepted` | `AcceptedWithErrors` | `Rejected` | `SubmissionIncident`;
   - plus the derived states `SupersededBySubsanacion` and `Cancelled` (anulación), each linked to the new record that caused it;
   - the model must make clear when a correction or rectifying invoice is required.

### Chain, time and numbering

5. **One chain per (taxpayer, SIF installation).** Generating a record is a serialised, single-writer critical section per chain. It reads the last record, checks its link and time order, computes the hash, and appends. Use a database-level uniqueness or ordering guarantee, not only application locks.
6. **Golden tests.** The hash function is a pure function tested against the three AEAT examples (section 4), including the empty-previous-hash case, whitespace trimming and decimal equivalence. Add a chain verifier usable on demand, for the whole chain or part of it.
7. **Timestamps:**
   - come from a single trusted clock source (NTP-synchronised, with alerting if it drifts by 1 minute or more);
   - are stored as an instant **plus** the issuing territory's offset (Europe/Madrid or Atlantic/Canary, depending on where the invoice is issued);
   - never go backwards within a chain;
   - are generated by the server, never taken from client input.

   Issue dates (`dd-mm-yyyy`) are derived in the issuer's local time zone, not UTC.
8. **Numbering:**
   - correlative and gap-free per series;
   - assigned only at finalisation;
   - mandatory separate series for rectifying invoices, for self-billed or third-party invoices, and for simplified versus full invoices in the same year;
   - (issuer NIF, series and number, issue date) is unique;
   - a dedicated, clearly labelled series for test or training invoices that are always cancelled afterwards.

### Submission behaviour

9. **Queue and transport:**
   - an outbox queue per taxpayer;
   - batches of at most 1,000 records;
   - respects `TiempoEsperaEnvio` (60 seconds at first), or sends early once 1,000 records are waiting;
   - sends in order of generation;
   - per-taxpayer certificate or representation selection.
10. **Idempotency:**
    - a retry resends the **same** record XML, with the same hash and timestamp; it never generates a new record;
    - a duplicate rejection whose stored record matches ours (check `EstadoRegistroDuplicado`) counts as already registered;
    - a transport timeout counts as "unknown" and is reconciled through the AEAT query service before any resend, if applicable.
11. **Outages:**
    - keep issuing invoices;
    - set `Incidencia`=S on delayed records;
    - retry at least every hour;
    - show a persistent banner with the number of pending records;
    - log the incident for later justification.
12. **Error routing:**
    - SOAP `Client` faults and per-record `Incorrecto` go to a human work queue that suggests a subsanación, an "alta por rechazo" or a rectifying invoice;
    - Raccord never edits the original record in place;
    - `AceptadoConErrores` also needs a decision, because the AEAT keeps that record permanently.

### Immutability and corrections

13. **Corrections:**
    - an issued invoice is corrected only by a rectifying invoice (new number in a rectifying series, R-code, method S or I, reference to the original);
    - record-only data errors are corrected by a subsanación alta;
    - an invoice issued in error is cancelled by an anulación record;
    - each path produces a new chained record and an audit event.
14. **Draft history:** drafts may be edited, but their history should be append-only (FAQ-Dev no. 11). Finalisation is the single point where the number, record, hash and QR are produced.
15. **Conservation and export:**
    - records, events and evidence are kept for the legal retention period, including across migrations;
    - export of all records for a period produces readable XML matching the OM SIF annex structure;
    - restore operations are logged.
16. **Multi-tenant self-hosting:**
    - each tenant is a separate taxpayer with its own chain, series and certificate;
    - the active taxpayer is always visible;
    - the system indicates when several taxpayers are served;
    - the identity fields (`IdSistemaInformatico`, `Version`, `NumeroInstalacion`, `IndicadorMultiplesOT`) are configured per installation.
17. **Mode as configuration:** choose VERI*FACTU-only (simplest; no signature or event log) or dual-mode (NO VERI*FACTU requires XAdES signing, the event log with 6-hourly summaries, and anomaly checks). A per-taxpayer switch must respect the rule that VERI*FACTU continues until the end of the calendar year, and the opt-out must be signalled.
18. **Declaración responsable:** ship one per released version, readable inside the product. Any fork or customisation that changes how a requirement is implemented needs its own.
19. **B2B adapter boundary:** keep it separate from the SIF seam. It handles:
    - EN 16931/UBL mapping;
    - the unique invoice ID;
    - platform or public-solution delivery, plus the fiel copy when using a private platform;
    - inbound invoices;
    - status messages for acceptance, rejection and payment with dates;
    - the four-business-day status clock.

### Raccord must not

- Edit, delete, renumber or reorder an issued invoice, billing record, event record or submission evidence. This includes database-admin "fix" scripts and soft-delete flags that hide records.
- Offer any feature that disables the QR or legend, hides records, resets or forks a chain, backdates timestamps, or deletes records except through the art. 8 export-then-purge process (NO VERI*FACTU).
- Generate a new record when retrying a transport failure.
- Treat a PDF, an XML export, or a successful HTTP call as proof of AEAT registration. Only a stored per-record `Correcto`/`AceptadoConErrores` and the CSV count.
- Issue "fake" test invoices from a production installation without cancelling them.
- Correct records sent in response to an AEAT request.
- Block invoicing because a chain check failed. Alert and log instead.
- Put the QR `formato=json` parameter in printed QR codes.

---

## 11. Open questions requiring legal review

1. **Producer identity for open-source self-hosting.** Who is the "productor" signing the declaración responsable when Raccord is open source and self-hosted: the project maintainer, each distributor, or each operator who modifies it? What about NIF, postal address and liability?
2. **Mode choice.** VERI*FACTU-only, dual-mode, or delegation to a certified external SIF? If Raccord delegates, is Raccord itself still a SIF because it "admits, keeps or processes" billing data (RRSIF art. 1.2)?
3. **The October 2028 announcement.** Will it cover IS taxpayers (currently 2027-01-01) or only SMEs and self-employed? Which instrument will enact it, and what technical changes will the planned ViDA convergence bring? Until BOE publication, the 2027 dates stand.
4. **Retention periods.** Confirm the exact retention period for billing and event records and submission evidence: the LGT limitation period versus longer commercial or accounting duties.
5. **Undocumented code values.** What do `ClaveRegimen` 21 and `OperacionExenta` E7/E8 mean (present in the production XSD, absent from the BOE annex)? What exact list of AEAT validation and error codes applies today?
6. **XAdES profile.** The signature profile details (AEAT spec v0.1.5) were not reviewed. Confirm before implementing dual mode.
7. **Representation model.** Does a self-hosted operator send with the client's own certificate, as a proxy, or through Convenio 17 collaboration? How should consent be captured electronically to satisfy FAQ-Dev no. 16?
8. **Audiovisual specifics.** Which regime keys, exemptions or not-subject keys and reverse-charge cases apply to typical audiovisual production services? Examples: cross-border services (N2), rental of equipment, crew services, IP rights (key 10 collections), Canary Islands productions (IGIC).
9. **EU derogation for B2B.** Is the Ley 18/2022 final provision 8 condition (EU derogation for arts. 218/232) satisfied, or superseded by the ViDA Directive? Does it affect the B2B start dates?
10. **Deadline arithmetic and turnover test.** How exactly are the 12- and 24-month B2B periods computed from 2026-10-06, and how is the €8m threshold measured in a given year?
11. **Drafts and proformas.** Does Raccord's draft/proforma flow need append-only history under LGT art. 29.2.j (FAQ-Dev no. 11)? Do quotes or delivery notes ("albaranes") linked to invoices fall within that scope?
12. **Foral territories.** Clients domiciled in the Basque Country or Navarre are outside the RRSIF but subject to foral systems. Should Raccord exclude them initially?

## Verification performed

- Downloaded and read the AEAT hash, QR and web-service specifications and the developer FAQ (v1.3) as PDFs. Extracted the production WSDL and XSD enumerations directly.
- Recomputed the AEAT hash example case 1 with SHA-256 and confirmed the published digest.
- Read the consolidated BOE texts of RD 1007/2023, Orden HAC/1177/2024, RD 1619/2012, Ley 18/2022 and RD 238/2026, and the published text of Orden HAC/1028/2026. Confirmed that RDL 15/2025 is ratified and that no BOE text enacts the 2026-10-05 Ministry announcement as of 2026-10-06.
- Not reviewed:
  - the AEAT XAdES signature PDF;
  - the validations-and-errors document;
  - the record-design workbook;
  - the public e-invoicing solution's technical specifications.
