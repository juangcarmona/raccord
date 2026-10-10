import { Link } from "react-router";
import { api, unwrap } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async, Empty } from "../../shared/ui/States";

export function ProjectsPage() {
    const { t, format } = useI18n();
    const query = useQuery("projects", () => unwrap(api.GET("/api/projects")));
    return (
        <>
            <PageHeader
                title={t("projects.title")}
                actions={
                    <Link className="button button--primary" to="/projects/new">
                        {t("projects.new")}
                    </Link>
                }
            />
            <Async query={query}>
                {(projects) =>
                    projects.length === 0 ? (
                        <Empty>{t("projects.empty")}</Empty>
                    ) : (
                        <div className="table-scroll">
                            <table>
                                <caption className="visually-hidden">{t("projects.title")}</caption>
                                <thead>
                                    <tr>
                                        <th scope="col">{t("common.project")}</th>
                                        <th scope="col">{t("common.customer")}</th>
                                        <th scope="col">{t("projects.period")}</th>
                                        <th scope="col" className="num">
                                            {t("projects.unbilled")}
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {projects.map((project) => (
                                        <tr key={project.id}>
                                            <th scope="row">
                                                <Link to={`/projects/${project.id}`}>{project.name}</Link>
                                            </th>
                                            <td>{project.customerName}</td>
                                            <td>
                                                {format.date(project.startDate)} – {format.date(project.endDate)}
                                            </td>
                                            <td className="num">
                                                {format.money(project.unbilledNetAmount, project.currency)} (
                                                {project.unbilledLineCount})
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )
                }
            </Async>
        </>
    );
}
