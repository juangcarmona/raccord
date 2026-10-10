---
id: "TERM-LIVE-UPDATE-CONNECTION"
type: domain-term
title: "Live update connection"
status: draft
defined-in: "BC-WORKSPACE"
synonyms:
  - "Live connection"
uses-terms:
  - "TERM-WORKSPACE"
  - "TERM-RECORD-PRESENCE"
provenance:
  source: ".private/research/help-center/articles/360013988420.md, .private/research/help-center/articles/360016035999.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The persistent connection between an open workspace session in the browser and the product, through which changes made by colleagues appear for the user without reloading, and through which presence (who is working in which project) is shared. State: connected, or lost. A lost connection means the screen may show outdated data until it is refreshed.

## Distinguish From

Change notification to an external endpoint: delivered to integrations, not to user screens. Notification: a message about an event for a person; live updates silently refresh the data the user is viewing.

## Usage

Opened automatically when a user opens a workspace in the browser; can be blocked by browser extensions, security software, proxies or VPNs.
