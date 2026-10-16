# Public homepage

The homepage is a dependency-free static site. It is separate from the Raccord application, so previewing it never starts or exposes an application instance.

Build the site from the repository root:

```sh
node src/public-site/build.mjs
```

Preview the generated files locally:

```sh
python3 -m http.server 4173 --directory src/public-site/dist
```

Open <http://localhost:4173>. The generated `dist/` directory is a build artifact; edit the source files in this directory instead.

## Hosting

The planned production and preview host is Cloudflare Pages, connected to this repository through Cloudflare's Git integration. Configure the repository root directory as `src/public-site`, the build command as `npm run build`, and the build output directory as `dist`; the checked-in `wrangler.jsonc` records the Pages project name and output directory. The `main` branch is the production branch and other branches use Pages preview deployments. The Git integration builds this subtree only and does not use a repository deployment token.

The Git integration must be connected once in the Cloudflare dashboard. Bind `raccord.dev` to the production project only after the Owner approves the production domain change. Pull-request previews and the checked-in GitHub Actions build can be used before that binding; see [the deployment guide](../../docs/operations/public-site-deployment.md).
