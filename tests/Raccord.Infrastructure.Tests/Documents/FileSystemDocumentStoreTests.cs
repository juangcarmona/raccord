using Raccord.Infrastructure.Documents;

namespace Raccord.Infrastructure.Tests.Documents;

public sealed class FileSystemDocumentStoreTests : IDisposable
{
    private readonly string root = Path.Combine(
        Path.GetTempPath(),
        "raccord-tests",
        Guid.NewGuid().ToString("N")
    );

    [Fact]
    public async Task Saved_documents_Should_be_read_back()
    {
        var store = new FileSystemDocumentStore(root);
        byte[] content = [1, 2, 3, 4];

        await store.SaveAsync(
            "invoices/abc/F2026-00001.pdf",
            content,
            "application/pdf",
            TestContext.Current.CancellationToken
        );

        (
            await store.ReadAsync(
                "invoices/abc/F2026-00001.pdf",
                TestContext.Current.CancellationToken
            )
        ).ShouldBe(content);
    }

    [Fact]
    public async Task Missing_documents_Should_read_as_null()
    {
        var store = new FileSystemDocumentStore(root);

        (
            await store.ReadAsync("invoices/none.pdf", TestContext.Current.CancellationToken)
        ).ShouldBeNull();
    }

    [Theory]
    [InlineData("../outside.pdf")]
    [InlineData("invoices/../../outside.pdf")]
    [InlineData("/etc/passwd")]
    [InlineData("C:/Windows/win.ini")]
    public async Task Keys_Should_not_escape_the_storage_root(string key)
    {
        var store = new FileSystemDocumentStore(root);

        await Should.ThrowAsync<ArgumentException>(() =>
            store.ReadAsync(key, TestContext.Current.CancellationToken)
        );
    }

    public void Dispose()
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
