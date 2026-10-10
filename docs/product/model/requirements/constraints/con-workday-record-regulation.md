---
id: CON-WORKDAY-RECORD-REGULATION
type: constraint
title: "Spanish daily working-time record"
status: draft
applies-to:
  - "BC-CREW"
uses-terms:
  - "TERM-WORKDAY-RECORD"
---

## Constraint

Every company in Spain must record, for each employee, the actual start and end of each working day (Workers' Statute art. 34.9, in force since 2019), keep the records for four years, and make them available to the employees, their legal representatives and the labour inspectorate. Raccord targets the stricter digital record being regulated: entries are made electronically by the employee at the moment of work, are tamper-evident with every change traceable, and are designed to be accessible remotely and immediately to the labour inspectorate, subject to verification of the final applicable requirements.

## Rationale

Imposed by Spanish labour law. The product owner chose to target the stricter digital rules early, while keeping the wording subject to legal verification before release.

## Consequences

Raccord keeps a legal workday record for every employee, separate from project hours, append-only with audited corrections.
