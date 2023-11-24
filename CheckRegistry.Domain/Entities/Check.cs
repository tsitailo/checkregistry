namespace CheckRegistry.Domain.Entities;

public class Check
{
    public string Id { get; set; }
    public string IssueDate { get; set; }
    public string Data { get; set; }
    public string Status { get; set; }
    public List<RegistrationResult> RegistrationResults { get; set; } = new List<RegistrationResult>();
    public int? VersionNumber { get; set; }
}