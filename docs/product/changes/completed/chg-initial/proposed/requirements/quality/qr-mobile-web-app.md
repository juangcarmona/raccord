---
id: QR-MOBILE-WEB-APP
type: quality-requirement
title: "Usable from a phone browser"
status: draft
quality-attribute: "usability"
applies-to:
  - "JRN-RECORD-WORKING-DAY"
  - "BC-WAREHOUSE"
verification:
  - scenario: "On a current iPhone and Android phone, an employee installs Raccord to the home screen, clocks in, and a warehouse technician scans an equipment QR code with the camera, without any app store"
uses-terms: []
---

## Requirement

Raccord MUST be fully usable from current phone and tablet browsers as an installable web app, without native apps. The first mobile delivery prioritises a field experience for workday check-in/out and read-only personal assignments, plus a management experience for operational control on desktop and tablet. A read-only large-screen operational board is supported for office display. Camera scanning and equipment booking remain the next field slice. Employees see only assignments and projects they are involved in, with no project edits in the first field slice.

## Measurement

Manual test on the two latest major iOS and Android versions before each release. Check-in/out works online by default; companies may explicitly enable controlled offline check-in/out. Location rules are optional per company or project. Personal accounts are the default, with optional managed kiosk mode. Offline assignment data is read-only and limited to current and upcoming assignments.
