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
