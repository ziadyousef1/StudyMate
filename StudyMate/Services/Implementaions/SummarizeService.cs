using Azure;
using Azure.AI.OpenAI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using StudyMate.Services.Interfaces;
using StudyMate.Settings;

namespace StudyMate.Services.Implementaions;

public class SummarizeService:ISummarizeService
{
    private readonly AiSettings _aiSettings;
    public SummarizeService(IOptions<AiSettings> aiSettings)
    {
        _aiSettings = aiSettings.Value;
    }
    public async Task<string> Summarize(string extractedText) 
    {
        AzureKeyCredential credential = new AzureKeyCredential(_aiSettings.Key); 

        AzureOpenAIClient azureClient = new(new Uri(_aiSettings.Endpoint), credential); 

        ChatClient chatClient = azureClient.GetChatClient("gpt-4o");  
    
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are an AI assistant that summarizes document content concisely."),
            new UserChatMessage($"Please summarize the following document:{extractedText}")
        };
        
        var options = new ChatCompletionOptions {  
            Temperature = (float)0.7,  
            TopP=(float)0.95,  
            FrequencyPenalty=(float)0,  
            PresencePenalty=(float)0,
       };
        ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);  
        if (completion != null)
        {
            var summary = completion.Content.ToList()[0].Text;
            return summary;
        }
        return "No summary generated.";
      
    }  
    public async Task<byte[]> CreateSummaryPdfAsync(string extractedText, string fileName = "Document Summary")
    {
        var summaryText = await Summarize(extractedText);

        using MemoryStream ms = new MemoryStream();
        var document = new Document(PageSize.A4, 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, ms);
            
        document.Open();
            
        var titleFont = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
        var title = new Paragraph($"{fileName} - Summary", titleFont);
        title.Alignment = Element.ALIGN_CENTER;
        title.SpacingAfter = 20;
        document.Add(title);
            
        var dateInfo = new Paragraph($"Generated on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        dateInfo.Alignment = Element.ALIGN_RIGHT;
        dateInfo.SpacingAfter = 20;
        document.Add(dateInfo);
            
        var contentFont = new Font(Font.FontFamily.HELVETICA, 11);
        var boldFont = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);
            
        var paragraphs = summaryText.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.None);
            
        foreach (var paragraph in paragraphs)
        {
            var processedText = paragraph;
                
            var p = new Paragraph();
                
            var boldStart = processedText.IndexOf("**");
            var pos = 0;
                
            while (boldStart >= 0)
            {
                p.Add(new Chunk(processedText.Substring(pos, boldStart - pos), contentFont));
                    
                var boldEnd = processedText.IndexOf("**", boldStart + 2);
                if (boldEnd < 0) break; 
                    
                var boldText = processedText.Substring(boldStart + 2, boldEnd - boldStart - 2);
                p.Add(new Chunk(boldText, boldFont));
                    
                pos = boldEnd + 2;
                boldStart = processedText.IndexOf("**", pos);
            }
                
            if (pos < processedText.Length)
            {
                p.Add(new Chunk(processedText.Substring(pos), contentFont));
            }
                
            p.SpacingAfter = 10;
            document.Add(p);
        }
            
        document.Close();
            
        return ms.ToArray();
    }
}