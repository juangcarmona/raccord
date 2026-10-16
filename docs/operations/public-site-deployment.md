# Public site deployment

The public homepage source and Cloudflare Pages project configuration live in `src/public-site/`; the build emits only `src/public-site/dist/`. GitHub Actions independently builds and checks this output on pull requests and relevant pushes to `main` through `.github/workflows/public-site.yml`.

## Cloudflare Pages project

The deployed hostname reported by the Owner is `raccord.pages.dev`; `src/public-site/wrangler.jsonc` uses the same Pages project name. Configure the GitHub repository `juangcarmona/raccord` with root directory `src/public-site`, build command `npm run build`, output directory `dist`, and production branch `main`. Restrict Build watch paths to `src/public-site/*` so unrelated repository changes do not trigger builds. Enable branch previews for pull requests; keep production deployments paused until publication is approved.

If the existing project is a Direct Upload Pages project, Cloudflare does not allow switching it to Git integration. Confirm the project's deployment type before changing it. If a Git-integrated project must be created, keep its production branch and custom domain unconfigured until its preview has been reviewed. The Git integration builds and deploys the Pages output directly; do not add a Cloudflare API token to this repository or GitHub Actions.

See Cloudflare's [Git integration guide](https://developers.cloudflare.com/pages/configuration/git-integration/) and [branch deployment controls](https://developers.cloudflare.com/pages/configuration/branch-build-controls/) for current dashboard behavior.

## raccord.dev domain plan

The Owner reported that the Pages deployment works at `raccord.pages.dev`. A read-only DNS and HTTPS check on 2026-10-16 found that `raccord.dev` also resolves through Cloudflare, returns HTTP 200, and serves byte-identical HTML to `raccord.pages.dev` (SHA-256 `98756897162061e4022fa1c42189c644a7013ad17a1bfca1129e26450356155a`). The active nameservers are `arushi.ns.cloudflare.com` and `hank.ns.cloudflare.com`. This indicates that the apex is already routed to the same published content; confirm the Pages Custom domains entry and DNS record in the dashboard before making any changes. The live HTML is the earlier Spanish homepage and does not yet include the approved rewrite or English locale.

For `raccord.dev`, confirm that its DNS zone is active in the same Cloudflare account as the Pages project. Cloudflare requires an apex domain to be a zone in that account. The current public DNS lookup already returns Cloudflare nameservers, but it cannot prove which Pages project is associated or show the account's flattened CNAME record. Do not change nameservers or DNS records unless the dashboard confirms the domain is not associated with the intended Pages project; if a change is needed, record the complete current zone first and get Owner approval for the exact change.

After the Pages production preview is approved, open **Workers & Pages → raccord → Custom domains** and confirm `raccord.dev` is listed on the intended project. If it is absent, select **Set up a domain**, enter `raccord.dev`, review the record Cloudflare proposes, and activate it. The expected apex record is a proxied CNAME from `@` to `raccord.pages.dev`; Cloudflare manages apex flattening and certificate provisioning. Use the Pages custom-domain flow before adding DNS manually: a CNAME created without associating the hostname with the Pages project can fail with HTTP 522.

The required production sequence is: approve the exact current DNS changes, bind the hostname in Pages, wait for HTTPS to become active, and verify the site content, links, mobile layout, locale switching and application boundary. `raccord.dev` must serve only the static site; do not route it to the Raccord application or API.

For a content-only deployment rollback, restore the previous successful Pages deployment or revert the homepage commit and redeploy; leave the existing domain binding and DNS in place. If a domain or DNS setting must be changed, retain a before-change export or screenshot of the zone, then detach `raccord.dev` only if it was newly attached and restore only the records changed for this publication. Keep `raccord.pages.dev` available and verify the intended site response after rollback.

See Cloudflare's [custom domains guide](https://developers.cloudflare.com/pages/configuration/custom-domains/) for the domain association and apex-zone requirements.

## Future updates

Merge approved changes to `src/public-site/` into `main`; Cloudflare Pages should build that source and publish the result once production deployment is enabled. Review pull-request previews before merge. The separate GitHub Actions workflow checks the same build output, and does not publish to Cloudflare.
