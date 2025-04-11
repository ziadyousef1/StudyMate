namespace StudyMate.Services.Interfaces;

public interface ISummarizeService
{
    Task<byte[]> CreateSummaryPdfAsync(string extractedText, string fileName = "Document Summary");
    
}