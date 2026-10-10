import { render } from "@testing-library/react";
import type { ReactElement } from "react";
import { createMemoryRouter, RouterProvider } from "react-router";
import { vi } from "vitest";
import { I18nProvider } from "../shared/i18n/I18nProvider";

/** Renders routes at a URL with Spanish UI, like a real session. */
export function renderAt(url: string, routes: { path: string; element: ReactElement }[]) {
    const router = createMemoryRouter(routes, { initialEntries: [url] });
    render(
        <I18nProvider locale="es">
            <RouterProvider router={router} />
        </I18nProvider>,
    );
    return router;
}

type Handler = (request: Request) => Response | Promise<Response>;

/** Replaces fetch with handlers keyed by "METHOD /path"; records every request. */
export function stubApi(handlers: Record<string, Handler>) {
    const requests: { method: string; path: string; body: unknown }[] = [];
    vi.stubGlobal("fetch", async (request: Request) => {
        const url = new URL(request.url);
        const key = `${request.method} ${url.pathname}`;
        const text = await request.clone().text();
        requests.push({
            method: request.method,
            path: url.pathname,
            body: text ? (JSON.parse(text) as unknown) : undefined,
        });
        const handler = handlers[key];
        if (!handler) {
            return json({ title: "Not found" }, 404);
        }
        return handler(request);
    });
    return requests;
}

export function json(body: unknown, status = 200): Response {
    return new Response(JSON.stringify(body), {
        status,
        headers: { "Content-Type": status >= 400 ? "application/problem+json" : "application/json" },
    });
}
