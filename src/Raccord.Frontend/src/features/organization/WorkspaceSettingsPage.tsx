import { useEffect, useState, type SubmitEvent } from "react";
import { PageHeader } from "../../shared/ui/PageHeader";

export function WorkspaceSettingsPage() {
    const [language, setLanguage] = useState("en");
    const [timeZone, setTimeZone] = useState("UTC");
    const [saved, setSaved] = useState(false);

    useEffect(() => {
        void fetch("/api/organization/preferences").then(async (response) => {
            if (!response.ok) return;
            const preferences = (await response.json()) as { language: string; timeZone: string };
            setLanguage(preferences.language);
            setTimeZone(preferences.timeZone);
        });
    }, []);

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const response = await fetch("/api/organization/preferences", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ language, timeZone }),
        });
        setSaved(response.ok);
    }

    return (
        <>
            <PageHeader title="Workspace settings">
                <p className="subtitle">Set the installation language and time zone.</p>
            </PageHeader>
            <form
                className="panel"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <label>
                    Language
                    <select
                        value={language}
                        onChange={(event) => {
                            setLanguage(event.target.value);
                        }}
                    >
                        <option value="en">English</option>
                        <option value="es">Español</option>
                    </select>
                </label>
                <label>
                    Time zone
                    <input
                        value={timeZone}
                        onChange={(event) => {
                            setTimeZone(event.target.value);
                        }}
                        required
                    />
                </label>
                <button type="submit" className="button button--primary">
                    Save
                </button>
                <p role="status">{saved ? "Workspace settings saved." : ""}</p>
            </form>
        </>
    );
}
