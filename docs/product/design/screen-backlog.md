# Screen backlog — journeys that deserve screens

Triage of all 27 journeys in `docs/product/model/journeys/` into screens,
driven by the mobile-delivery decision (CHG-INITIAL: one responsive PWA with
field mode, management mode, and a read-only large-screen board).

Built screens live in `raccord-screens.pen`; per-screen PDaC citations live in
[screen-traces.md](screen-traces.md).

## Already built (3 + 37 + 2)

| Screen | Mode | Journey |
|---|---|---|
| Invoice list | management | JRN-BILL-FINISHED-PROJECT |
| Field check-in | field | JRN-RECORD-WORKING-DAY |
| Operations board | board | JRN-WHS-DISPATCH-EQUIPMENT / JRN-WHS-RECEIVE-RETURNS |
| Invoice draft editor | management | JRN-BILL-FINISHED-PROJECT |
| Invoicing queue | management | JRN-FIN-STAGED-BILLING |
| Payments & dunning | management | JRN-FIN-STAGED-BILLING |
| Quote review & generation | management | JRN-FIN-QUOTE-TO-SIGNATURE |
| Online quote page | management | JRN-FIN-QUOTE-TO-SIGNATURE |
| Financial settings hub | management | JRN-FIN-PREPARE-FINANCE |
| Issuer profile settings | management | JRN-SET-UP-INVOICING |
| Project creation | management | JRN-PRJ-RUN-RENTAL-PROJECT |
| Project detail & schedule | management | JRN-PRJ-RUN-RENTAL-PROJECT |
| Project task board | management | JRN-PRJ-DELIVER-PRODUCTION-WORK |
| Service planned-vs-actual | management | JRN-PRJ-DELIVER-PRODUCTION-WORK |
| Task workflow settings | management | JRN-PRJ-DELIVER-PRODUCTION-WORK |
| Project equipment plan & shortages | management | JRN-WHS-SECURE-EQUIPMENT |
| Packing list detail | management | JRN-WHS-DISPATCH-EQUIPMENT |
| Return handling | management | JRN-WHS-RECEIVE-RETURNS |
| Stock count session | management | JRN-EQP-KEEP-STOCK-ACCURATE |
| Stock movements & disposition | management | JRN-EQP-KEEP-STOCK-ACCURATE |
| Equipment import | management | JRN-EQP-SET-UP-CATALOG |
| Item detail | management | JRN-EQP-SET-UP-CATALOG |
| Kit builder | management | JRN-EQP-SET-UP-CATALOG |
| Contact import | management | JRN-PRJ-SET-UP-CONTACT-DATABASE |
| Contact detail & commercial defaults | management | JRN-PRJ-SET-UP-CONTACT-DATABASE |
| Crew availability | management | JRN-CRW-STAFF-A-PROJECT |
| Function staffing | management | JRN-CRW-STAFF-A-PROJECT |
| Job board postings | management | JRN-CRW-FILL-VIA-JOB-BOARD |
| Shift planner | management | JRN-CRW-PLAN-INTERNAL-WORK |
| Time approvals & reconciliation | management | JRN-CRW-HOURS-TO-PAYROLL |
| Payroll export | management | JRN-CRW-HOURS-TO-PAYROLL |
| Crew import | management | JRN-CRW-ONBOARD-CREW |
| Crew member profile | management | JRN-CRW-ONBOARD-CREW |
| Personal schedule & calendar feeds | management | JRN-CRW-STAFF-A-PROJECT / JRN-INT-SYNC-PERSONAL-CALENDAR |
| Workday record corrections & approval | management | JRN-RECORD-WORKING-DAY (management spill) |
| Document send & communications log | management | JRN-DOC-SEND-DOCUMENT-AND-CONFIRM-RECEIPT |
| Letterhead & template branding | management | JRN-DOC-SET-UP-BRANDED-DOCUMENTS |
| Equipment labels | management | JRN-DOC-LABEL-EQUIPMENT |
| Workspace settings & roles | management | JRN-WSP-SET-UP-WORKSPACE |
| API access & tokens | management | JRN-INT-BUILD-API-INTEGRATION |

All 37 management screens plus the two field candidates have been generated
and committed on `trial/pen-dev-design-canvas`; exports live alongside this
file, citations in [screen-traces.md](screen-traces.md).

## Generated — management (37, all landed)

Low-visual-interest setup screens are marked *(setup)*.

| # | Screen | Job | Journey |
|---|---|---|---|
| 1 | Invoice draft editor | select unbilled work, review totals, remove lines, fix blockers, issue | JRN-BILL-FINISHED-PROJECT |
| 2 | Invoicing queue | work items due per invoice moment, generate partial/final invoices | JRN-FIN-STAGED-BILLING |
| 3 | Payments & dunning | record payments, chase overdue, accounting export | JRN-FIN-STAGED-BILLING |
| 4 | Quote review & generation | costs/margins, customer defaults, discounts, lock price | JRN-FIN-QUOTE-TO-SIGNATURE |
| 5 | Online quote page | customer-facing read-only quote with signature | JRN-FIN-QUOTE-TO-SIGNATURE |
| 6 | Financial settings hub | validity, price factors, ledgers, VAT, invoice moments, payment terms *(setup)* | JRN-FIN-PREPARE-FINANCE |
| 7 | Issuer profile settings | issuer legal details entered once *(setup)* | JRN-SET-UP-INVOICING |
| 8 | Project creation | from template or blank | JRN-PRJ-RUN-RENTAL-PROJECT |
| 9 | Project detail & schedule | client/venue, planning + usage periods, status | JRN-PRJ-RUN-RENTAL-PROJECT |
| 10 | Project task board | shareable board, shift tasks, track time | JRN-PRJ-DELIVER-PRODUCTION-WORK |
| 11 | Service planned-vs-actual | service breakdown, hour exports | JRN-PRJ-DELIVER-PRODUCTION-WORK |
| 12 | Task workflow settings | statuses, teams *(setup)* | JRN-PRJ-DELIVER-PRODUCTION-WORK |
| 13 | Project equipment plan & shortages | organise groups, shortages, alternatives, reserve | JRN-WHS-SECURE-EQUIPMENT |
| 14 | Packing list detail | scan items in/out, shortages, print slip, extras | JRN-WHS-DISPATCH-EQUIPMENT |
| 15 | Return handling | scan returns, mark lost/delayed, count consumables | JRN-WHS-RECEIVE-RETURNS |
| 16 | Stock count session | count items, compare vs expected, correct | JRN-EQP-KEEP-STOCK-ACCURATE |
| 17 | Stock movements & disposition | scheduled movements, sale, archive | JRN-EQP-KEEP-STOCK-ACCURATE |
| 18 | Equipment import | file import, bulk updates, undo by import tag | JRN-EQP-SET-UP-CATALOG |
| 19 | Item detail | type, usage, stock, prices, accessories, images | JRN-EQP-SET-UP-CATALOG |
| 20 | Kit builder | physical/virtual kits, content attach/import | JRN-EQP-SET-UP-CATALOG |
| 21 | Contact import | contacts + persons with folder paths *(setup)* | JRN-PRJ-SET-UP-CONTACT-DATABASE |
| 22 | Contact detail & commercial defaults | role tags, special terms, tax detail | JRN-PRJ-SET-UP-CONTACT-DATABASE |
| 23 | Crew availability | availability across crew/vehicles | JRN-CRW-STAFF-A-PROJECT |
| 24 | Function staffing | invite, responses, plan resources, plan status | JRN-CRW-STAFF-A-PROJECT |
| 25 | Job board postings | post/remove functions, review sign-ups | JRN-CRW-FILL-VIA-JOB-BOARD |
| 26 | Shift planner | recurring shifts, invite/plan crew and vehicles | JRN-CRW-PLAN-INTERNAL-WORK |
| 27 | Time approvals & reconciliation | approve/revert, plan vs reality | JRN-CRW-HOURS-TO-PAYROLL |
| 28 | Payroll export | export runs, freelancer invoice references | JRN-CRW-HOURS-TO-PAYROLL |
| 29 | Crew import | import with login off, enable + invite *(setup)* | JRN-CRW-ONBOARD-CREW |
| 30 | Crew member profile | rates, contracts, certificates | JRN-CRW-ONBOARD-CREW |
| 31 | Personal schedule & calendar feeds | share schedules, feed URLs | JRN-CRW-STAFF-A-PROJECT |
| 32 | Workday record corrections & approval | correct open clock-outs with reason, approve, export | JRN-RECORD-WORKING-DAY (management spill) |
| 33 | Document send & communications log | generate doc, email, delivery outcomes | JRN-DOC-SEND-DOCUMENT-AND-CONFIRM-RECEIPT |
| 34 | Letterhead & template branding | stationery, logo, colours, defaults *(setup)* | JRN-DOC-SET-UP-BRANDED-DOCUMENTS |
| 35 | Equipment labels | label design, generate & print code labels | JRN-DOC-LABEL-EQUIPMENT |
| 36 | Workspace settings & roles | company details, number series, roles *(setup)* | JRN-WSP-SET-UP-WORKSPACE |
| 37 | API access & tokens | enable access, copy/regenerate token *(setup)* | JRN-INT-BUILD-API-INTEGRATION |

## Field candidates (2) — built

Field mode is defined as check-in/out plus read-only assignments; these two
are interactive on the crew side and were triaged as candidates that need an
explicit scope decision before they count:

| Screen | Job | Governing artifacts |
|---|---|---|
| My assignments & responses | view posted functions/invitations, respond/sign up | UC-CRW-SIGN-UP-FOR-POSTED-JOB, UC-CRW-RESPOND-TO-INVITATION |
| Hours registration | log hours against planned activities/shifts | UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES |

## No UI (4 journeys)

JRN-OPERATE-INSTALLATION (infrastructure), JRN-INT-SYNC-PERSONAL-CALENDAR
(feed URLs, served inside screen 31), JRN-DOC-SERVE-FOREIGN-CUSTOMER
(language/country rides screen 34 + reuse), JRN-INT-BUILD-API-INTEGRATION
(seam; settings surface counted as screen 37).
