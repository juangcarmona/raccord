import { useState, type SyntheticEvent } from "react";
import { useNavigate } from "react-router";
import { useI18n } from "../../shared/i18n/I18nProvider";

export function SignInPage() {
    const { t } = useI18n();
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [invalid, setInvalid] = useState(false);

    async function submit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault();
        setInvalid(false);
        const response = await fetch("/api/auth/sign-in", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password, rememberMe }),
        });
        if (!response.ok) {
            setInvalid(true);
            return;
        }
        await navigate("/projects", { replace: true });
    }

    return (
        <main className="auth-page">
            <form
                className="auth-form"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <h1 data-page-heading tabIndex={-1}>
                    {t("auth.title")}
                </h1>
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
                        value={password}
                        onChange={(event) => {
                            setPassword(event.target.value);
                        }}
                        required
                    />
                </label>
                <label>
                    <input
                        type="checkbox"
                        checked={rememberMe}
                        onChange={(event) => {
                            setRememberMe(event.target.checked);
                        }}
                    />
                    {t("auth.remember")}
                </label>
                {invalid && <p role="alert">{t("auth.invalid")}</p>}
                <button className="button button--primary" type="submit">
                    {t("auth.submit")}
                </button>
            </form>
        </main>
    );
}
