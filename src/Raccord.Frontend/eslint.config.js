import js from "@eslint/js";
import { defineConfig } from "eslint/config";
import jsxA11y from "eslint-plugin-jsx-a11y";
import reactHooks from "eslint-plugin-react-hooks";
import globals from "globals";
import tseslint from "typescript-eslint";

export default defineConfig(
    { ignores: ["dist", "src/shared/api/schema.d.ts"] },
    {
        files: ["**/*.{ts,tsx,js}"],
        extends: [js.configs.recommended, ...tseslint.configs.strictTypeChecked, jsxA11y.flatConfigs.strict],
        languageOptions: {
            globals: globals.browser,
            parserOptions: {
                project: ["./tsconfig.json", "./tsconfig.node.json"],
                tsconfigRootDir: import.meta.dirname,
            },
        },
        plugins: { "react-hooks": reactHooks },
        rules: {
            ...reactHooks.configs.recommended.rules,
            "@typescript-eslint/restrict-template-expressions": ["error", { allowNumber: true }],
        },
    },
    {
        // Untyped third-party plugin objects in this config file cannot be type-checked.
        files: ["eslint.config.js"],
        extends: [tseslint.configs.disableTypeChecked],
    },
);
