import { useEffect, useState, type SyntheticEvent } from "react";
import { useNavigate } from "react-router";
import { useI18n } from "../../shared/i18n/I18nProvider";

export function SetupPage() {
    const { t } = useI18n();
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [invalid, setInvalid] = useState(false);

    useEffect(() => {
        void fetch("/api/setup/status")
            .then((response) => response.json() as Promise<{ required: boolean }>)
            .then((status) => {
                if (!status.required) {
                    void navigate("/sign-in", { replace: true });
                }
            });
    }, [navigate]);

    async function submit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault();
        setInvalid(false);
        const response = await fetch("/api/setup", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password }),
        });
        if (!response.ok) {
            setInvalid(true);
            return;
        }
        await navigate("/sign-in", { replace: true });
    }

    return (
        <main className="auth-page">
            <form className="auth-form" onSubmit={(event) => void submit(event)}>
                <h1 data-page-heading tabIndex={-1}>
                    {t("setup.title")}
                </h1>
                <p>{t("setup.intro")}</p>
                <label>
                    {t("auth.email")}
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
                    {t("auth.password")}
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
                {invalid && <p role="alert">{t("setup.invalid")}</p>}
                <button className="button button--primary" type="submit">
                    {t("setup.submit")}
                </button>
            </form>
        </main>
    );
}
