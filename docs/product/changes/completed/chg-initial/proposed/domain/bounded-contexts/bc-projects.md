---
id: BC-PROJECTS
type: bounded-context
title: "Projects"
status: draft
---

## Responsibility

Audiovisual jobs and the billable work they produce.

## Language

Projects with planning and usage periods, project types and templates, project statuses, internal requests, tasks, notes and billable lines.

## Boundaries

Equipment planning detail and availability belong to Equipment, physical handling to Warehouse, crew and vehicles to Crew and transport, prices and quotes to Commercial, invoices to Invoicing.

## External Relationships

Invoicing reserves and consumes billable lines; Projects records which lines are in a draft or invoiced.
