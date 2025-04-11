using System.Text;

namespace StudyMate.Helpers;

public static class PDFService
{
    public static async Task<string> ExtractTextFromPdfAsync(IFormFile file)
    {
        var sb = new StringBuilder();
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using (var reader = new iTextSharp.text.pdf.PdfReader(stream))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    sb.Append(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }
        }
        return sb.ToString();
       
    }
}