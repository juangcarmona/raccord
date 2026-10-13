#pragma warning disable CS1591

namespace Raccord.Domain.Projects;

public sealed class ProjectTemplate
{
    private ProjectTemplate()
    {
        Name = string.Empty;
    }

    private ProjectTemplate(string name, Guid customerId)
    {
        Id = Guid.CreateVersion7();
        Name = name.Trim();
        CustomerId = customerId;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid CustomerId { get; private set; }

    public static ProjectTemplate Create(string name, Guid customerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Template name is required.", nameof(name));
        return new(name, customerId);
    }
}

#pragma warning restore CS1591
