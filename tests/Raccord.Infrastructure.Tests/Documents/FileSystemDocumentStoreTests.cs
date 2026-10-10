using Raccord.Infrastructure.Documents;

namespace Raccord.Infrastructure.Tests.Documents;

public sealed class FileSystemDocumentStoreTests : IDisposable
{
    private readonly string root = Path.Combine(Path.GetTempPath(), "raccord-tests", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task Saved_documents_can_be_read_back()
    {
        var store = new FileSystemDocumentStore(root);
        byte[] content = [1, 2, 3, 4];

        await store.SaveAsync("invoices/abc/F2026-00001.pdf", content, "application/pdf", TestContext.Current.CancellationToken);

        Assert.Equal(content, await store.ReadAsync("invoices/abc/F2026-00001.pdf", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Missing_documents_read_as_null()
    {
        var store = new FileSystemDocumentStore(root);

        Assert.Null(await store.ReadAsync("invoices/none.pdf", TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData("../outside.pdf")]
    [InlineData("invoices/../../outside.pdf")]
    [InlineData("/etc/passwd")]
    [InlineData("C:/Windows/win.ini")]
    public async Task Keys_cannot_escape_the_storage_root(string key)
    {
        var store = new FileSystemDocumentStore(root);

        await Assert.ThrowsAsync<ArgumentException>(() => store.ReadAsync(key, TestContext.Current.CancellationToken));
    }

    public void Dispose()
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
