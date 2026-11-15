const generated = /(^|[\\/])src[\\/]shared[\\/]api[\\/]schema\.d\.ts$/;
const quote = (file) => JSON.stringify(file);

export default {
    "*.{ts,tsx}": (files) => {
        const source = files.filter((file) => !generated.test(file));
        return source.length
            ? [
                  `eslint --fix --max-warnings=0 ${source.map(quote).join(" ")}`,
                  `prettier --write ${source.map(quote).join(" ")}`,
              ]
            : [];
    },
    "*.{json,yml,yaml,css}": (files) => {
        const formatted = files.filter((file) => !generated.test(file));
        return formatted.length ? [`prettier --write ${formatted.map(quote).join(" ")}`] : [];
    },
    // Markdown is owned by markdownlint-cli2 (repository-root config), not Prettier. Its ignores
    // ignore the ProductShape-managed product model.
    "*.md": (files) => [`npx -y markdownlint-cli2@0.23.3 --fix ${files.map(quote).join(" ")}`],
};
