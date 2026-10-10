---
id: TERM-ROLE-PERMISSION
type: domain-term
title: "Role permission"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-USER-ROLE"
  - "TERM-ACCESS-LEVEL"
  - "TERM-OWN-RECORD"
  - "TERM-FILE-DOWNLOAD-LEVEL"
  - "TERM-LOCATION-CONTACT-VISIBILITY"
  - "TERM-BOOKED-EQUIPMENT-EDIT-RIGHT"
  - "TERM-AVAILABILITY-VISIBILITY-LEVEL"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md, .private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/35770035545362.md"
  confidence: high
  recovered-from: documentation
---

## Definition

One configurable right inside a user role, grouped by area:
- General: file download level, scope for editing email and text templates, scope for creating and deleting saved views, editing own data, adding notes and uploading files to records one may not edit, printing packing slips, creating and deleting tags, seeing confidential custom fields.
- Dashboard access.
- Schedule: which other people's planning is visible (none, functions of projects one is planned on, everything), planned equipment in the mobile web app, own appointments, others' appointments, all public appointments, own availability, others' availability, location contact visibility, job board access.
- Warehouse: which projects (planned or assigned to, or all), changing the warehouse status, adding extra or other items, invoicing extra items, changing stock on damage or loss, invoicing damage or loss, packing without scanning, assigning crew to projects, emptying kits.
- Projects: visible projects (own or all), changing projects (none, own, all), equipment availability visibility, editing booked equipment, deleting projects, editing default functions, creating invoices, creating quotations and contracts, changing the schedule.
- Crew planner: changing the schedule, sending planning emails.
- Financial visibility.
- Invoices: own invoices and all other invoices, by access level.
- Equipment, contacts, crew members and vehicles databases by access level; crew documents; login information; last seen; changing user roles; crew and vehicle timelines.
- Tasks of others; time registration (own hours, others' hours, pre-registration, activities, crew rates in activities, clocking in from the mobile web app); maintenance; statistics.
- Configuration: access and creating and editing roles.
- Communication: the full communication log, emails sent by others, incoming notes.
## Distinguish From

Not a single sign-on policy; not record-level sharing such as task board visibility.

## Usage

Edited per role in configuration.
