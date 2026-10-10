// The manifest of installed JS packages lives under src/Raccord.Frontend (the only place node
// tooling is installed in this repo). @commitlint/resolve-extends resolves `extends` relative to
// this config file's directory, so the bare package name "@commitlint/config-conventional"
// cannot be found from the repo root. An absolute path into the frontend's node_modules works.
// The .cjs extension matters: no package.json governs .config/, and Node treats a .js file as an
// ES module only when one does — but commitlint's cosmiconfig loader imports .js as ESM here,
// which has no `module.exports`.
const conventionalRelative = require.resolve("@commitlint/config-conventional", {
  paths: [require("node:path").join(__dirname, "..", "src", "Raccord.Frontend")],
});

module.exports = {
  extends: [conventionalRelative],
  rules: {
    "type-enum": [2, "always", ["feat", "fix", "chore", "docs", "refactor", "test", "ci"]],
  },
};
