---
id: ACT-EXTERNAL-APPLICATION
type: actor
title: "External application"
status: draft
actor-kind: external-system
---

## Purpose

Accounting packages, web shops, calendars, automation platforms and custom software that exchange data with Raccord through its public interfaces.

## Goals

Read and write permitted data reliably, and learn about changes promptly.

## Responsibilities

Calls the public API with scoped credentials; receives change notifications; imports or exports files.

## Boundaries

Never bypasses permissions; is never the system of record for Raccord's data.
