using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Crew;
using Raccord.SharedKernel.Common;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591, MA0004
public sealed class CrewAvailabilityCatalog(RaccordDbContext db)
{
    public async Task<CrewAvailabilityResponse> SetAvailabilityAsync(
        SetCrewAvailabilityRequest request,
        CancellationToken ct
    )
    {
        if (request.EndDate < request.StartDate)
            throw new BusinessRuleViolationException(
                "crew.availability-period-invalid",
                "Availability must have a valid period."
            );
        var row = new CrewAvailabilityRow
        {
            Id = Guid.CreateVersion7(),
            CrewMemberId = request.CrewMemberId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Kind = request.Kind,
            Remark = request.Remark?.Trim(),
        };
        db.CrewAvailability.Add(row);
        await db.SaveChangesAsync(ct);
        return ToAvailability(row);
    }

    public async Task<IReadOnlyList<CrewAvailabilityResponse>> ListAvailabilityAsync(
        Guid crewMemberId,
        CancellationToken ct
    ) =>
        (
            await db
                .CrewAvailability.Where(row => row.CrewMemberId == crewMemberId)
                .OrderBy(row => row.StartDate)
                .ToListAsync(ct)
        )
            .Select(ToAvailability)
            .ToList();

    public async Task<CrewInvitationResponse?> InviteAsync(
        Guid functionId,
        InviteCrewRequest request,
        CancellationToken ct
    )
    {
        if (await db.CrewMembers.AllAsync(row => row.Id != request.CrewMemberId, ct))
            throw new BusinessRuleViolationException(
                "crew.member-not-found",
                "The crew member does not exist."
            );
        if (
            await db.CrewInvitations.AnyAsync(
                row => row.FunctionId == functionId && row.CrewMemberId == request.CrewMemberId,
                ct
            )
        )
            throw new BusinessRuleViolationException(
                "crew.invitation-duplicate",
                "The crew member is already invited."
            );
        var row = new CrewInvitationRow
        {
            Id = Guid.CreateVersion7(),
            FunctionId = functionId,
            CrewMemberId = request.CrewMemberId,
            Remark = request.Remark?.Trim(),
        };
        db.CrewInvitations.Add(row);
        await db.SaveChangesAsync(ct);
        return ToInvitation(row);
    }

    public async Task<CrewInvitationResponse?> RespondAsync(
        Guid invitationId,
        RespondToCrewInvitationRequest request,
        CancellationToken ct
    )
    {
        var row = await db.CrewInvitations.SingleOrDefaultAsync(
            candidate => candidate.Id == invitationId,
            ct
        );
        if (row is null)
            return null;
        row.Status = request.Status;
        await db.SaveChangesAsync(ct);
        return ToInvitation(row);
    }

    public async Task<CrewJobResponse> PostAsync(
        Guid functionId,
        PostFunctionRequest request,
        CancellationToken ct
    )
    {
        var row = new CrewJobRow
        {
            Id = Guid.CreateVersion7(),
            FunctionId = functionId,
            Brief = request.Brief?.Trim(),
            PostedAt = DateTimeOffset.UtcNow,
        };
        db.CrewJobs.Add(row);
        await db.SaveChangesAsync(ct);
        return ToJob(row);
    }

    public async Task<CrewSignupResponse?> SignupAsync(
        Guid jobId,
        Guid crewMemberId,
        CancellationToken ct
    )
    {
        if (await db.CrewMembers.AllAsync(row => row.Id != crewMemberId, ct))
            throw new BusinessRuleViolationException(
                "crew.member-not-found",
                "The crew member does not exist."
            );
        var job = await db
            .CrewJobs.Include(row => row.Signups)
            .SingleOrDefaultAsync(row => row.Id == jobId, ct);
        if (job is null)
            return null;
        if (job.Signups.Any(row => row.CrewMemberId == crewMemberId))
            throw new BusinessRuleViolationException(
                "crew.signup-duplicate",
                "The crew member already signed up."
            );
        var row = new CrewSignupRow
        {
            Id = Guid.CreateVersion7(),
            JobId = jobId,
            CrewMemberId = crewMemberId,
            SignedUpAt = DateTimeOffset.UtcNow,
        };
        job.Signups.Add(row);
        await db.SaveChangesAsync(ct);
        return new(row.Id, row.JobId, row.CrewMemberId, row.SignedUpAt);
    }

    private static CrewAvailabilityResponse ToAvailability(CrewAvailabilityRow row) =>
        new(row.Id, row.CrewMemberId, row.StartDate, row.EndDate, row.Kind, row.Remark);

    private static CrewInvitationResponse ToInvitation(CrewInvitationRow row) =>
        new(row.Id, row.FunctionId, row.CrewMemberId, row.Status, row.Remark);

    private static CrewJobResponse ToJob(CrewJobRow row) =>
        new(row.Id, row.FunctionId, row.Brief, row.PostedAt, row.Signups.Count);
}
#pragma warning restore CS1591, MA0004
