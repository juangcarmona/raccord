import { useState, type SubmitEvent } from "react";
import { useNavigate, useParams } from "react-router";

export function JoinPage() {
    const { token = "" } = useParams();
    const navigate = useNavigate();
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const response = await fetch(`/api/invitations/${token}/accept`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ password }),
        });
        if (!response.ok) {
            setError("This invitation is invalid, expired, or already used.");
            return;
        }
        void navigate("/sign-in");
    }

    return (
        <main className="auth-page">
            <form
                className="panel"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <h1>Join Raccord</h1>
                <p>Choose a password to activate your account.</p>
                <label>
                    Password
                    <input
                        type="password"
                        minLength={12}
                        value={password}
                        onChange={(event) => {
                            setPassword(event.target.value);
                        }}
                        required
                    />
                </label>
                <button type="submit" className="button button--primary">
                    Join installation
                </button>
                <p role="alert">{error}</p>
            </form>
        </main>
    );
}
