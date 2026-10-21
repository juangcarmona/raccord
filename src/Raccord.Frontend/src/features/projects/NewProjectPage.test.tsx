import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { json, renderAt, stubApi } from "../../test/render";
import { NewProjectPage } from "./NewProjectPage";

describe("NewProjectPage", () => {
    it("creates a project with an eligible account manager", async () => {
        const requests = stubApi({
            "GET /api/customers": () => json([{ id: "customer-1", legalName: "Example Ltd" }]),
            "GET /api/projects/account-managers": () => json([{ id: "manager-1", displayName: "Office Manager" }]),
            "POST /api/projects": () => json({ id: "project-1" }, 201),
        });
        const router = renderAt("/projects/new", [
            { path: "/projects/new", element: <NewProjectPage /> },
            { path: "/projects/:id", element: <p>Project details</p> },
        ]);
        const user = userEvent.setup();

        await user.type(await screen.findByRole("textbox", { name: "Nombre del proyecto" }), "Studio day");
        await user.selectOptions(await screen.findByRole("combobox", { name: "Cliente" }), "customer-1");
        await user.selectOptions(await screen.findByRole("combobox", { name: "Responsable de cuenta" }), "manager-1");
        await user.type(screen.getByLabelText("Inicio"), "2026-10-21");
        await user.type(screen.getByLabelText("Fin"), "2026-10-21");
        await user.click(screen.getByRole("button", { name: "Guardar" }));

        await waitFor(() => {
            expect(router.state.location.pathname).toBe("/projects/project-1");
        });
        expect(requests.find((request) => request.method === "POST")?.body).toMatchObject({
            accountManagerId: "manager-1",
        });
    });
});
