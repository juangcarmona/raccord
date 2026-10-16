# Public site deployment

The public homepage source and Cloudflare Pages project configuration live in `src/public-site/`; the build emits only `src/public-site/dist/`. GitHub Actions independently builds and checks this output on pull requests and relevant pushes to `main` through `.github/workflows/public-site.yml`.

## Cloudflare Pages project

The deployed hostname reported by the Owner is `raccord.pages.dev`; `src/public-site/wrangler.jsonc` uses the same Pages project name. Configure the GitHub repository `juangcarmona/raccord` with root directory `src/public-site`, build command `npm run build`, output directory `dist`, and production branch `main`. Restrict Build watch paths to `src/public-site/*` so unrelated repository changes do not trigger builds. Enable branch previews for pull requests; keep production deployments paused until publication is approved.

If the existing project is a Direct Upload Pages project, Cloudflare does not allow switching it to Git integration. Confirm the project's deployment type before changing it. If a Git-integrated project must be created, keep its production branch and custom domain unconfigured until its preview has been reviewed. The Git integration builds and deploys the Pages output directly; do not add a Cloudflare API token to this repository or GitHub Actions.

See Cloudflare's [Git integration guide](https://developers.cloudflare.com/pages/configuration/git-integration/) and [branch deployment controls](https://developers.cloudflare.com/pages/configuration/branch-build-controls/) for current dashboard behavior.

## raccord.dev domain plan

The Pages deployment currently works at `raccord.pages.dev`. The purchased apex domain `raccord.dev` is not yet confirmed as bound to that project. The Wrangler project name is `raccord`, so the expected Pages CNAME target is `raccord.pages.dev`.

For `raccord.dev`, first ensure its DNS zone is active in the same Cloudflare account as the Pages project. Cloudflare requires an apex domain to be a zone in that account. If the domain uses another DNS provider, the Owner must review all existing records, add the zone to Cloudflare and change the registrar's nameservers to the values shown for that zone. Do not guess nameserver values or replace existing records without a DNS snapshot and Owner approval.

After the Pages production preview is approved, open **Workers & Pages → raccord → Custom domains → Set up a domain**, enter `raccord.dev`, review the record Cloudflare proposes, and activate it. The expected apex record is a proxied CNAME from `@` to `raccord.pages.dev`; Cloudflare manages apex flattening and certificate provisioning. Use the Pages custom-domain flow before adding DNS manually: a CNAME created without associating the hostname with the Pages project can fail with HTTP 522.

The required production sequence is: approve the exact current DNS changes, bind the hostname in Pages, wait for HTTPS to become active, and verify the site content, links, mobile layout, locale switching and application boundary. `raccord.dev` must serve only the static site; do not route it to the Raccord application or API.

For rollback, retain a before-change export or screenshot of the DNS zone and current nameservers. Remove `raccord.dev` from the Pages project's Custom domains, then restore only the records or nameserver delegation changed for this publication. Keep `raccord.pages.dev` available, and verify the apex no longer routes to the new site.

See Cloudflare's [custom domains guide](https://developers.cloudflare.com/pages/configuration/custom-domains/) for the domain association and apex-zone requirements.

## Future updates

Merge approved changes to `src/public-site/` into `main`; Cloudflare Pages should build that source and publish the result once production deployment is enabled. Review pull-request previews before merge. The separate GitHub Actions workflow checks the same build output, and does not publish to Cloudflare.
