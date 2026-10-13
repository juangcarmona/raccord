#pragma warning disable CS1591

namespace Raccord.Infrastructure.Persistence;

public static class PermissionCatalog
{
    public const string RolesRead = "workspace:roles.read";
    public const string RolesEdit = "workspace:roles.edit";
    public const string RolesAssign = "workspace:roles.assign";
    public const string OrganizationRead = "workspace:organization.read";
    public const string OrganizationEdit = "workspace:organization.edit";
    public const string ContactsRead = "contacts:read";
    public const string ContactsCreate = "contacts:create";
    public const string ProjectsRead = "projects:read";
    public const string ProjectsCreate = "projects:create";
    public const string ProjectsEdit = "projects:edit";
    public const string InvoicingRead = "invoicing:read";
    public const string InvoicingCreate = "invoicing:create";
    public const string InvoicingEdit = "invoicing:edit";
    public const string ClaimType = "raccord:permission";
    public const string All = "*";

    public static readonly IReadOnlySet<string> AllPermissions = new HashSet<string>(
        [
            RolesRead,
            RolesEdit,
            RolesAssign,
            OrganizationRead,
            OrganizationEdit,
            ContactsRead,
            ContactsCreate,
            ProjectsRead,
            ProjectsCreate,
            ProjectsEdit,
            InvoicingRead,
            InvoicingCreate,
            InvoicingEdit,
        ],
        StringComparer.Ordinal
    );

    public static readonly IReadOnlyDictionary<string, IReadOnlySet<string>> PredefinedRoles =
        new Dictionary<string, IReadOnlySet<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Admin"] = new HashSet<string>(AllPermissions, StringComparer.Ordinal),
            ["Office"] = new HashSet<string>(
                [
                    OrganizationRead,
                    OrganizationEdit,
                    ContactsRead,
                    ContactsCreate,
                    ProjectsRead,
                    ProjectsCreate,
                    ProjectsEdit,
                    InvoicingRead,
                    InvoicingCreate,
                    InvoicingEdit,
                ],
                StringComparer.Ordinal
            ),
            ["Warehouse"] = new HashSet<string>(
                [ContactsRead, ProjectsRead, ProjectsEdit],
                StringComparer.Ordinal
            ),
            ["Crew"] = new HashSet<string>([ProjectsRead], StringComparer.Ordinal),
        };
}

#pragma warning restore CS1591
