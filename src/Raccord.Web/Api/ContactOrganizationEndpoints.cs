using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Contracts.Contacts;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Web.Authorization;

namespace Raccord.Web.Api;

internal static class ContactOrganizationEndpoints
{
    public static void Map(RouteGroupBuilder contacts)
    {
        MapTags(contacts.MapGroup("/tags").WithTags("Contacts"));
        MapFolders(contacts.MapGroup("/folders").WithTags("Contacts"));
        MapAssignments(contacts);
    }

    public static void MapDirectory(RouteGroupBuilder contacts)
    {
        contacts
            .MapGet(
                "/",
                async (
                    int? page,
                    int? pageSize,
                    string? role,
                    Guid? folderId,
                    Guid[]? tagIds,
                    string? tagMatch,
                    ContactsDirectory directory,
                    CancellationToken ct
                ) =>
                    TypedResults.Ok(
                        await directory.ListAsync(
                            page ?? 1,
                            pageSize ?? 25,
                            role,
                            folderId,
                            tagIds,
                            tagMatch,
                            ct
                        )
                    )
            )
            .RequirePermission(PermissionCatalog.ContactsRead);
    }

    private static void MapTags(RouteGroupBuilder tags)
    {
        tags.MapGet(
                "/",
                async (ContactOrganizationDirectory directory, CancellationToken ct) =>
                    TypedResults.Ok(await directory.ListTagsAsync(ct))
            )
            .RequirePermission(PermissionCatalog.ContactsRead);
        tags.MapPost(
                "/",
                async (
                    CreateContactTagRequest request,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                {
                    var tag = await directory.CreateTagAsync(request, ct);
                    return TypedResults.Created($"/api/contacts/tags/{tag.Id}", tag);
                }
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
        tags.MapDelete(
                "/{tagId:guid}",
                async Task<Results<NoContent, NotFound>> (
                    Guid tagId,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.DeleteTagAsync(tagId, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
    }

    private static void MapFolders(RouteGroupBuilder folders)
    {
        folders
            .MapGet(
                "/",
                async (ContactOrganizationDirectory directory, CancellationToken ct) =>
                    TypedResults.Ok(await directory.ListFoldersAsync(ct))
            )
            .RequirePermission(PermissionCatalog.ContactsRead);
        folders
            .MapPost(
                "/",
                async Task<Results<Created<ContactFolderResponse>, NotFound>> (
                    CreateContactFolderRequest request,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                {
                    var folder = await directory.CreateFolderAsync(request, ct);
                    return folder is null
                        ? TypedResults.NotFound()
                        : TypedResults.Created($"/api/contacts/folders/{folder.Id}", folder);
                }
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
        folders
            .MapPut(
                "/{folderId:guid}",
                async Task<Results<Ok<ContactFolderResponse>, NotFound>> (
                    Guid folderId,
                    UpdateContactFolderRequest request,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.UpdateFolderAsync(folderId, request, ct) is { } folder
                        ? TypedResults.Ok(folder)
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
        folders
            .MapDelete(
                "/{folderId:guid}",
                async Task<Results<NoContent, NotFound>> (
                    Guid folderId,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.DeleteFolderAsync(folderId, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
    }

    private static void MapAssignments(RouteGroupBuilder contacts)
    {
        contacts
            .MapGet(
                "/{id:guid}/organization",
                async Task<Results<Ok<ContactOrganizationResponse>, NotFound>> (
                    Guid id,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.GetContactAsync(id, ct) is { } state
                        ? TypedResults.Ok(state)
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsRead);
        contacts
            .MapPut(
                "/{id:guid}/tags/{tagId:guid}",
                async Task<Results<NoContent, NotFound>> (
                    Guid id,
                    Guid tagId,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.SetTagAsync(id, tagId, true, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
        contacts
            .MapDelete(
                "/{id:guid}/tags/{tagId:guid}",
                async Task<Results<NoContent, NotFound>> (
                    Guid id,
                    Guid tagId,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.SetTagAsync(id, tagId, false, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
        contacts
            .MapPut(
                "/{id:guid}/folder",
                async Task<Results<NoContent, NotFound>> (
                    Guid id,
                    AssignContactFolderRequest request,
                    ContactOrganizationDirectory directory,
                    CancellationToken ct
                ) =>
                    await directory.SetFolderAsync(id, request.FolderId, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .RequirePermission(PermissionCatalog.ContactsCreate);
    }
}
