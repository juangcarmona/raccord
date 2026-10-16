# Public site deployment

The public homepage source and Cloudflare Pages project configuration live in `src/public-site/`; the build emits only `src/public-site/dist/`. GitHub Actions independently builds and checks this output on pull requests and relevant pushes to `main` through `.github/workflows/public-site.yml`.

## Cloudflare Pages project setup

The repository contains the build command, project name, and output directory, but the Pages project must be connected to the Git repository in the Cloudflare dashboard. Set the project root to `src/public-site`, the build command to `npm run build`, the build output directory to `dist`, and the production branch to `main`. Under Build watch paths, include `src/public-site/*` and leave the exclude list empty so unrelated repository changes do not start Pages builds.

Enable preview deployments for non-production branches so pull requests can be reviewed on a Pages preview URL. Keep automatic production branch deployments disabled until the Owner approves production publication. The Cloudflare Git integration builds and deploys the Pages output directly; no Cloudflare API token is stored in the repository or GitHub Actions.

## Domain publication

Do not bind `raccord.dev` or enable production deployment as part of issue #247. After explicit Owner approval, connect the purchased domain to the production Pages project using Cloudflare's custom-domain setup. Until then, use preview deployments for review and keep production branch deployment disabled.
