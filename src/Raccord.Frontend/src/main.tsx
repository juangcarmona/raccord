import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router";
import { routes } from "./app/routes";
import { AuthProvider } from "./features/auth/AuthProvider";
import { I18nProvider } from "./shared/i18n/I18nProvider";
import "./styles.css";
import { useEffect } from "react";
import { useAuth } from "./features/auth/AuthProvider";
import { useI18n } from "./shared/i18n/I18nProvider";

const router = createBrowserRouter(routes);

const container = document.getElementById("root");
if (!container) {
    throw new Error("Missing #root element");
}

function SessionLanguage() {
    const { user, status } = useAuth();
    const { setLocale } = useI18n();
    useEffect(() => {
        if (status === "signed-in" && user) setLocale(user.preferredLanguage === "en" ? "en" : "es");
    }, [status, user, setLocale]);
    return <RouterProvider router={router} />;
}

createRoot(container).render(
    <StrictMode>
        <I18nProvider>
            <AuthProvider>
                <SessionLanguage />
            </AuthProvider>
        </I18nProvider>
    </StrictMode>,
);
