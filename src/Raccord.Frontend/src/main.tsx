import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router";
import { routes } from "./app/routes";
import { AuthProvider } from "./features/auth/AuthProvider";
import { I18nProvider } from "./shared/i18n/I18nProvider";
import "./styles.css";

const container = document.getElementById("root");
if (!container) {
    throw new Error("Missing #root element");
}

createRoot(container).render(
    <StrictMode>
        <I18nProvider>
            <AuthProvider>
                <RouterProvider router={createBrowserRouter(routes)} />
            </AuthProvider>
        </I18nProvider>
    </StrictMode>,
);
