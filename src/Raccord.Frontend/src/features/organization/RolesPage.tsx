import { useState, type SubmitEvent } from "react";
import { useQuery } from "../../shared/hooks/useQuery";
import { PageHeader } from "../../shared/ui/PageHeader";

type Role = { name: string; predefined: boolean; permissions: string[]; userCount: number };
const permissions = [
    "workspace:roles.read",
    "workspace:roles.edit",
    "workspace:roles.assign",
    "workspace:organization.read",
    "workspace:organization.edit",
    "contacts:read",
    "contacts:create",
    "projects:read",
    "projects:create",
    "projects:edit",
    "invoicing:read",
    "invoicing:create",
    "invoicing:edit",
];

export function RolesPage() {
    const query = useQuery("roles", async () => {
        const response = await fetch("/api/roles");
        if (!response.ok) throw new Error("Unable to load roles.");
        return (await response.json()) as Role[];
    });
    const [name, setName] = useState("");
    const [selected, setSelected] = useState<string[]>([]);
    const [error, setError] = useState("");

    async function create(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        setError("");
        const response = await fetch("/api/roles", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, permissions: selected }),
        });
        if (!response.ok) {
            setError("The role could not be saved.");
            return;
        }
        setName("");
        setSelected([]);
        query.reload();
    }

    return (
        <>
            <PageHeader title="Roles and permissions">
                <p className="subtitle">Control which modules and actions each role can use.</p>
            </PageHeader>
            <section className="panel">
                <h2>Current roles</h2>
                <ul>
                    {query.status === "success" &&
                        query.data.map((role: Role) => (
                            <li key={role.name}>
                                <strong>{role.name}</strong> · {role.userCount} user(s) · {role.permissions.length}{" "}
                                permissions
                            </li>
                        ))}
                </ul>
            </section>
            <form
                className="panel"
                onSubmit={(event) => {
                    void create(event);
                }}
            >
                <h2>Create custom role</h2>
                <label>
                    Role name
                    <input
                        value={name}
                        onChange={(event) => {
                            setName(event.target.value);
                        }}
                        required
                    />
                </label>
                <fieldset>
                    <legend>Permissions</legend>
                    {permissions.map((permission) => (
                        <label key={permission}>
                            <input
                                type="checkbox"
                                checked={selected.includes(permission)}
                                onChange={(event) => {
                                    setSelected((current) =>
                                        event.target.checked
                                            ? [...current, permission]
                                            : current.filter((value) => value !== permission),
                                    );
                                }}
                            />
                            {permission}
                        </label>
                    ))}
                </fieldset>
                <button type="submit" className="button button--primary">
                    Create role
                </button>
                <p role="alert">{error || (query.status === "error" ? "Unable to load roles." : "")}</p>
            </form>
        </>
    );
}
