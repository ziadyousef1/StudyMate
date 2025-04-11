using Microsoft.AspNetCore.Mvc;
using StudyMate.Helpers;
using StudyMate.Services.Interfaces;

namespace StudyMate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SummarizeController(ISummarizeService sumarizeService) : ControllerBase
{
    [HttpPost("summarize-as-pdf")]
    public async Task<IActionResult> SummarizeAsPdf(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Please upload a PDF file");

        if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("File must be a PDF");
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            
        var extractedText = await PDFService.ExtractTextFromPdfAsync(file);
            
        var pdfBytes = await sumarizeService.CreateSummaryPdfAsync(extractedText, fileName);
            
            return File(pdfBytes, "application/pdf", $"{fileName}-summary.pdf");
      
    }

    
}