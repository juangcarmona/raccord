import { useState, type SubmitEvent } from "react";
import { Link } from "react-router";
import { useAuth } from "./AuthProvider";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { Field, FormError, SelectField } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { formText } from "../../shared/ui/formText";

export function AccountPage() {
    const { t } = useI18n();
    const auth = useAuth();
    const user = auth.user;
    const [error, setError] = useState<unknown>(null);
    const [saved, setSaved] = useState(false);
    const [busy, setBusy] = useState(false);

    if (!user) return null;

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        setBusy(true);
        setError(null);
        setSaved(false);
        try {
            await auth.updateProfile(formText(data, "displayName"), formText(data, "preferredLanguage") as "es" | "en");
            setSaved(true);
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <>
            <PageHeader title={t("account.title")}>
                <p className="subtitle">{t("account.intro")}</p>
            </PageHeader>
            <form
                className="panel account-form"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <div className="grid-form">
                    <Field
                        label={t("account.displayName")}
                        name="displayName"
                        required
                        maxLength={100}
                        defaultValue={user.displayName ?? ""}
                        autoComplete="name"
                    />
                    <Field label={t("account.email")} name="email" value={user.email} readOnly autoComplete="email" />
                    <SelectField
                        label={t("account.language")}
                        name="preferredLanguage"
                        defaultValue={user.preferredLanguage}
                    >
                        <option value="es">Español</option>
                        <option value="en">English</option>
                    </SelectField>
                </div>
                <div className="actions">
                    <button type="submit" className="button button--primary" disabled={busy}>
                        {t("common.save")}
                    </button>
                    {user.canManageRoles && <Link to="/settings/roles">{t("account.roleManagement")}</Link>}
                </div>
                <p role="status" aria-live="polite">
                    {saved ? t("account.saved") : ""}
                </p>
                <FormError error={error} />
            </form>
        </>
    );
}
