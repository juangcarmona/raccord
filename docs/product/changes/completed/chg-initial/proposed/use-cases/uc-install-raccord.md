---
id: UC-INSTALL-RACCORD
type: use-case
title: "Install Raccord"
status: draft
primary-actor: "ACT-INSTALLATION-ADMINISTRATOR"
supporting-actors: []
governed-by: []
uses-terms: []
---

## Goal

A healthy installation is ready for the issuer.

## Trigger

A company adopts Raccord.

## Preconditions

A server with a container runtime, or an Azure subscription.

## Main Flow

1. The administrator sets the configuration, including a database password.
2. The administrator starts the installation; the database schema is prepared before the application accepts requests.
3. The installation reports itself healthy.

## Alternative Flows

Demo installation: fictional demo data is loaded into an empty installation.

## Failure Conditions

Missing required configuration stops the installation with a clear message.

## Postconditions

The issuer can be defined.
