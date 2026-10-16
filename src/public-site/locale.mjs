export function resolveLocale(languages = []) {
    for (const language of languages) {
        const baseLanguage = language.split(/[-_]/, 1)[0].toLowerCase();
        if (baseLanguage === "es" || baseLanguage === "en") {
            return baseLanguage;
        }
    }

    return "es";
}

export function rootLocalePath(languages = []) {
    return `/${resolveLocale(languages)}/`;
}

if (typeof window !== "undefined" && window.location.pathname === "/") {
    const languages = navigator.languages?.length
        ? navigator.languages
        : [navigator.language ?? ""];
    window.location.replace(rootLocalePath(languages));
}
