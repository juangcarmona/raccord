---
id: ACT-B2B-E-INVOICE-PLATFORM
type: actor
title: "Public B2B e-invoice solution"
status: draft
actor-kind: "external-system"
---

## Purpose

The tax authority's public e-invoice solution, through which Spanish companies exchange structured B2B invoices and their statuses. Private exchange platforms are not used.

## Goals

Receive and deliver structured invoices and status messages between companies.

## Responsibilities

Accepts invoices sent by Raccord, delivers invoices addressed to the company, and carries acceptance, rejection and payment statuses.

## Boundaries

Never changes invoices; it transports them. Raccord connects to it directly with the company certificate.
