namespace StudyMate.Settings;

public class AzureStorageSettings
{
    public const string AzureStorage = "AzureBlobStorage";
    public string ConnectionString { get; set; }
    public ContainersOptions Containers { get; set; }
    
}

public class ContainersOptions
{
    public string Images { get; set; }
    public string Files { get; set; }
}