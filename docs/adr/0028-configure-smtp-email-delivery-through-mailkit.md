---
name: smtp-email-delivery-through-mailkit
description: Deliver application email through operator-configured SMTP behind an application-owned port.
status: Accepted
date: 2026-10-18
deciders: ["Juan G. Carmona"]
tags: ["email", "self-hosting", "security", "dependency"]
---

# ADR-0028: Deliver application email through configured SMTP

## Context

Account invitations and recovery require email delivery, but the application currently has no mail transport. Raccord is self-hosted by one organization per deployment, so installations need to select and configure their own SMTP service. Mail transport credentials must stay in deployment configuration and outside source control. The application must remain testable without sending real messages.

## Decision

We will define an application-owned email delivery port and provide an SMTP adapter implemented with MailKit 4.17.0. Installations configure the SMTP host, port, TLS mode, sender address, and optional authentication through environment-backed application configuration. Tests replace the port with a capture implementation and do not connect to a real mail server. If email settings are absent, the application still starts and email-dependent actions fail with an explicit, retryable configuration error.

## Consequences

- **Positive:** Installations retain provider choice; SMTP authentication and transport security are handled by a maintained library; business flows and tests stay independent of SMTP.
- **Negative:** Operators must configure and maintain an SMTP service before email-dependent features are usable; MailKit and its transitive packages expand the dependency and vulnerability surface.
- **Neutral:** Credentials are consumed from runtime configuration only; setup documentation must describe configuration without embedding real secrets.

## Alternatives considered

- **Use `System.Net.Mail.SmtpClient`:** rejected because Microsoft does not recommend it for new development and it lacks support for many modern protocols.
- **Bind to one hosted email API:** rejected because it would force every self-hosted installation to choose that provider and expose its credentials to the application.
- **Write a private SMTP implementation:** rejected because protocol, authentication and TLS behavior are security-sensitive and already handled by MailKit.

## References

- Product Change and runtime slice: #339, #369; OpenSpec change `crew-account-association`.
- Related: ADR-0012, ADR-0013, ADR-0021.
- [Microsoft guidance for `SmtpClient`](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=net-10.0).
- [MailKit SMTP client documentation](https://mimekit.net/docs/html/T_MailKit_Net_Smtp_SmtpClient.htm).
