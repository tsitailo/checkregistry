// ReSharper disable InconsistentNaming

namespace CheckRegistry.DataAccess;

public static class DatabaseConstants
{
    public const string DynamoDBTableName = "CheckRegistry";

    public const string PartitionKeyName = "id";
    public const string SortKeyName = "issue_date";
    public const string DataAttributeName = "data";

    public const string KeySeparator = "_";
    
    public const string CRunnerIdPrefix = "check_";
   
}