import { useState, type SubmitEvent } from "react";
import { PageHeader } from "../../shared/ui/PageHeader";

export function InvitationsPage() {
    const [email, setEmail] = useState("");
    const [role, setRole] = useState("Crew");
    const [link, setLink] = useState("");
    const [error, setError] = useState("");

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        setError("");
        const response = await fetch("/api/invitations", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, role }),
        });
        if (!response.ok) {
            setError("The invitation could not be created.");
            return;
        }
        const result = (await response.json()) as { token: string };
        setLink(`${window.location.origin}/join/${result.token}`);
    }

    return (
        <>
            <PageHeader title="Invite a person">
                <p className="subtitle">Create a single-use link to share with a new member.</p>
            </PageHeader>
            <form
                className="panel"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <label>
                    Email
                    <input
                        type="email"
                        value={email}
                        onChange={(event) => {
                            setEmail(event.target.value);
                        }}
                        required
                    />
                </label>
                <label>
                    Role
                    <select
                        value={role}
                        onChange={(event) => {
                            setRole(event.target.value);
                        }}
                    >
                        <option>Admin</option>
                        <option>Office</option>
                        <option>Warehouse</option>
                        <option>Crew</option>
                    </select>
                </label>
                <button type="submit" className="button button--primary">
                    Create invitation
                </button>
                <p role="alert">{error}</p>
                {link && (
                    <p role="status">
                        Share this link: <code>{link}</code>
                    </p>
                )}
            </form>
        </>
    );
}
