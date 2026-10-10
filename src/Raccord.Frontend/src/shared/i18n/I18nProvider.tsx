import { createContext, use, useEffect, useMemo, useState, type ReactNode } from "react";
import { createFormatters, type Formatters } from "../format/format";
import { catalogs, isMessageKey, locales, type Locale, type MessageKey } from "./messages";

const storageKey = "raccord.locale";

export interface I18n {
    locale: Locale;
    setLocale: (locale: Locale) => void;
    t: (key: MessageKey, params?: Record<string, string | number>) => string;
    /** Localized text for a server rule code, falling back to the server's English message. */
    describe: (prefix: "issue" | "error" | "audit" | "kind" | "billing", code: string, fallback?: string) => string;
    format: Formatters;
}

const I18nContext = createContext<I18n | null>(null);

function initialLocale(): Locale {
    const stored = safeStorage()?.getItem(storageKey);
    if (stored && (locales as readonly string[]).includes(stored)) {
        return stored as Locale;
    }
    return navigator.language.toLowerCase().startsWith("en") ? "en" : "es";
}

function safeStorage(): Storage | null {
    try {
        return globalThis.localStorage;
    } catch {
        return null;
    }
}

export function I18nProvider({ children, locale: fixedLocale }: { children: ReactNode; locale?: Locale }) {
    const [locale, setLocale] = useState<Locale>(() => fixedLocale ?? initialLocale());

    useEffect(() => {
        document.documentElement.lang = locale;
        safeStorage()?.setItem(storageKey, locale);
    }, [locale]);

    const value = useMemo<I18n>(() => {
        const catalog = catalogs[locale];
        const t = (key: MessageKey, params?: Record<string, string | number>) =>
            catalog[key].replace(/\{(\w+)\}/g, (match, name: string) =>
                params && name in params ? String(params[name]) : match,
            );
        return {
            locale,
            setLocale,
            t,
            describe: (prefix, code, fallback) => {
                const key = `${prefix}.${code}`;
                return isMessageKey(key) ? t(key) : (fallback ?? code);
            },
            format: createFormatters(locale),
        };
    }, [locale]);

    return <I18nContext value={value}>{children}</I18nContext>;
}

export function useI18n(): I18n {
    const context = use(I18nContext);
    if (!context) {
        throw new Error("useI18n must be used inside I18nProvider");
    }
    return context;
}
