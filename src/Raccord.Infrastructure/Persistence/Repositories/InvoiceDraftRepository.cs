using Microsoft.EntityFrameworkCore;
using Raccord.Application.Invoicing;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Repositories;

internal sealed class InvoiceDraftRepository(RaccordDbContext db) : IInvoiceDraftRepository
{
    public Task<InvoiceDraft?> FindAsync(Guid id, CancellationToken cancellationToken) =>
        db.InvoiceDrafts.FirstOrDefaultAsync(draft => draft.Id == id, cancellationToken);

    public async Task<IReadOnlyList<InvoiceDraft>> ListOpenAsync(CancellationToken cancellationToken) =>
        await db.InvoiceDrafts.Where(draft => draft.Status == DraftStatus.Open).OrderByDescending(draft => draft.CreatedAt).ToListAsync(cancellationToken).ConfigureAwait(false);

    public void Add(InvoiceDraft draft) => db.InvoiceDrafts.Add(draft);

    public void Remove(InvoiceDraft draft) => db.InvoiceDrafts.Remove(draft);
}
