import { useEffect, useRef } from "react";
import { NavLink, Outlet, useLocation } from "react-router";
import { useI18n } from "../shared/i18n/I18nProvider";
import { locales, type Locale } from "../shared/i18n/messages";
import { useAuth } from "../features/auth/AuthProvider";
import { useNavigate } from "react-router";

const navigation = [
    { to: "/projects", key: "nav.projects" },
    { to: "/drafts", key: "nav.drafts" },
    { to: "/invoices", key: "nav.invoices" },
    { to: "/customers", key: "nav.customers" },
    { to: "/settings/issuer", key: "nav.settings" },
    { to: "/settings/roles", key: "nav.roles" },
] as const;

export function AppShell() {
    const { t, locale, setLocale } = useI18n();
    const location = useLocation();
    const main = useRef<HTMLElement>(null);
    const firstRender = useRef(true);
    const auth = useAuth();
    const navigate = useNavigate();

    // Move focus to the new page heading after client-side navigation, as a full page load would.
    useEffect(() => {
        if (firstRender.current) {
            firstRender.current = false;
            return;
        }
        const timer = setTimeout(() => {
            main.current?.querySelector<HTMLElement>("[data-page-heading]")?.focus();
        }, 0);
        return () => {
            clearTimeout(timer);
        };
    }, [location.pathname]);

    return (
        <div className="shell">
            <a className="skip-link" href="#main">
                {t("app.skipToContent")}
            </a>
            <div className="shell__layout">
                <aside className="shell__sidebar">
                    <span className="brand">
                        <img src="/raccord-mark.svg" alt="" aria-hidden="true" />
                        <span>{t("app.name")}</span>
                    </span>
                    <nav aria-label={t("nav.main")}>
                        <ul>
                            {navigation.map((item) => (
                                <li key={item.to}>
                                    <NavLink to={item.to}>{t(item.key)}</NavLink>
                                </li>
                            ))}
                        </ul>
                    </nav>
                </aside>
                <div className="shell__content">
                    <header className="shell__toolbar">
                        <label className="locale">
                            <span className="visually-hidden">{t("app.language")}</span>
                            <select
                                value={locale}
                                onChange={(event) => {
                                    setLocale(event.target.value as Locale);
                                }}
                            >
                                {locales.map((option) => (
                                    <option key={option} value={option}>
                                        {option.toUpperCase()}
                                    </option>
                                ))}
                            </select>
                        </label>
                        <span>{auth.user?.email}</span>
                        <button
                            type="button"
                            onClick={() => void auth.signOut().then(() => navigate("/sign-in", { replace: true }))}
                        >
                            {t("auth.signOut")}
                        </button>
                    </header>
                    <main id="main" ref={main} className="shell__main">
                        <Outlet />
                    </main>
                </div>
            </div>
        </div>
    );
}
