namespace StudyMate.Settings;

public class CloudStorageSettings
{
    public const string AzureStorage = "BlobStorage";
    public string ConnectionString { get; set; }
    public ContainersOptions Containers { get; set; }
    
}

public class ContainersOptions
{
    public string Images { get; set; }
    public string Files { get; set; }
}