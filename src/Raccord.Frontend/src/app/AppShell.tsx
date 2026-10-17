import { useEffect, useRef } from "react";
import { NavLink, Outlet, useLocation } from "react-router";
import { useI18n } from "../shared/i18n/I18nProvider";
import { useAuth } from "../features/auth/AuthProvider";
import { useNavigate } from "react-router";
import { Link } from "react-router";

const navigation = [
    { to: "/projects", key: "nav.projects" },
    { to: "/drafts", key: "nav.drafts" },
    { to: "/invoices", key: "nav.invoices" },
    { to: "/customers", key: "nav.customers" },
    { to: "/settings/issuer", key: "nav.settings" },
] as const;

export function AppShell() {
    const { t } = useI18n();
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
                            {auth.user?.canManageRoles && (
                                <li>
                                    <NavLink to="/settings/roles">{t("nav.roles")}</NavLink>
                                </li>
                            )}
                        </ul>
                    </nav>
                </aside>
                <div className="shell__content">
                    <header className="shell__toolbar">
                        <Link className="account-entry" to="/account" aria-label={t("account.title")}>
                            <span className="account-entry__name">{auth.user?.displayName || auth.user?.email}</span>
                            {auth.user?.displayName && <span className="account-entry__email">{auth.user.email}</span>}
                        </Link>
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
