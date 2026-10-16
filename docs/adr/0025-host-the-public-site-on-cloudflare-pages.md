---
name: host-the-public-site-on-cloudflare-pages
description: Host the standalone public homepage through Cloudflare Pages Git integration.
status: Accepted
date: 2026-10-16
deciders: [Juan G. Carmona]
tags: [website, deployment, cloudflare]
---

# ADR-0025: Host the public site through Cloudflare Pages

## Context

The repository's GitHub Pages artifact serves generated architecture, product-model, and nightly browser evidence through one serialized engineering publisher. The public homepage has a separate source subtree under `src/public-site/` and needs previews and a distinct production output without replacing that artifact. The Owner has purchased `raccord.dev`; this delivery change must keep domain binding and production publication under explicit Owner control.

## Decision

We will host the standalone public site through a Cloudflare Pages Git integration rooted at `src/public-site/`, with repository build watches restricted to `src/public-site/*` and the build output restricted to `dist/`. Pull-request and non-production branch builds use preview deployments, while `main` is the designated production branch. Keep automatic production branch deployments disabled and do not bind `raccord.dev` until the Owner approves publication. Use Cloudflare's Git integration instead of a GitHub Actions deployment token so the repository does not store a long-lived Cloudflare credential.

## Consequences

- **Positive:** Public-site previews and production output are isolated from the engineering GitHub Pages artifact; the repository stores project/output configuration without a deployment credential.
- **Negative:** The Cloudflare Git integration, branch controls, and eventual custom-domain binding require account-level setup outside the repository.
- **Neutral:** `main` remains the designated production branch, but its production deployments stay disabled until explicit Owner approval; the site remains available only through previews until then.

## Alternatives considered

- **Publish the homepage into the engineering GitHub Pages artifact:** rejected because one site's upload can replace the other site's output and the public homepage would share the engineering publisher's lifecycle.
- **Deploy with Wrangler from GitHub Actions:** rejected because it requires a Cloudflare API token and account identifier in GitHub Actions secrets, while the Pages Git integration can build previews directly from the repository.

## References

- Issue: [#247](https://github.com/juangcarmona/raccord/issues/247)
- Related: [ADR-0024](0024-publish-static-engineering-snapshots-on-github-pages.md)
- Operations: [public-site deployment](../operations/public-site-deployment.md)
- Cloudflare: [Pages Git integration](https://developers.cloudflare.com/pages/configuration/git-integration/) and [branch deployment controls](https://developers.cloudflare.com/pages/configuration/branch-build-controls/)
