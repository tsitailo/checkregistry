using Amazon.DynamoDBv2.DataModel;

namespace CheckRegistry.DataAccess.Entities;

[DynamoDBTable(DatabaseConstants.DynamoDBTableName)]
public class Check
{
    [DynamoDBProperty("id")]
    [DynamoDBHashKey]
    public string Id { get; set; }
    [DynamoDBProperty("issue_date")]
    [DynamoDBRangeKey]
    public string IssueDate { get; set; }

    [DynamoDBProperty(DatabaseConstants.DataAttributeName)]
    public string Data { get; set; }
    public string Status { get; set; }
    public List<RegistrationResult> RegistrationResults { get; set; } = new List<RegistrationResult>();
    [DynamoDBVersion] public int? VersionNumber { get; set; }
}