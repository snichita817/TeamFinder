// Controllers/CvController.cs
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using OpenAI_API;
using OpenAI_API.Completions;
using TeamFinder.Models.Domain;

[Route("api/[controller]")]
[ApiController]
public class CvController : ControllerBase
{
    private readonly OpenAIAPI _openAiApi;
    private readonly IConfiguration _configuraton;
    public CvController(IConfiguration configuraton)
    {
        _configuraton = configuraton;
        _openAiApi = new OpenAIAPI(_configuraton["OpenAI:ApiKey"]);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadPdf(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        string extractedText;
        using (var reader = new PdfReader(file.OpenReadStream()))
        {
            using (var pdfDoc = new PdfDocument(reader))
            {
                extractedText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetFirstPage());
                // Consider looping through pages if the CV spans multiple pages
            }
        }

        var prompt = $"Extract the following information from this CV: {extractedText}\n\n" +
                     "1. First Name: \n" +
                     "2. Last Name: \n" +
                     "3. Email: \n" +
                     "4. University: \n" +
                     "5. Graduation Year: \n" +
                     "6. Skills: \n" +
                     "7. LinkedIn URL: \n" +
                     "8. GitHub URL: \n" +
                     "9. Portfolio URL: \n" +
                     "10. Bio:";

        var completionRequest = new CompletionRequest(prompt, model: "gpt-3.5-turbo-instruct", max_tokens: 500);
        var completionResult = await _openAiApi.Completions.CreateCompletionAsync(completionRequest);
        var extractedInfo = ParseCvInfo(completionResult.Completions[0].Text);
        return Ok(extractedInfo);
    }

    private CvInfo ParseCvInfo(string completionText)
    {
        var cvInfo = new CvInfo();

        cvInfo.FirstName = ExtractField(completionText, "First Name:");
        cvInfo.LastName = ExtractField(completionText, "Last Name:");
        cvInfo.Email = ExtractField(completionText, "Email:");
        cvInfo.University = ExtractField(completionText, "University:");
        cvInfo.GraduationYear = ExtractIntField(completionText, "Graduation Year:");
        cvInfo.Skills = ExtractField(completionText, "Skills:");
        cvInfo.LinkedInUrl = ExtractField(completionText, "LinkedIn URL:");
        cvInfo.GitHubUrl = ExtractField(completionText, "GitHub URL:");
        cvInfo.PortfolioUrl = ExtractField(completionText, "Portfolio URL:");
        cvInfo.Bio = ExtractField(completionText, "Bio:");

        return cvInfo;
    }

    private string ExtractField(string text, string fieldName)
    {
        var startIndex = text.IndexOf(fieldName) + fieldName.Length;
        if (startIndex < fieldName.Length) return string.Empty;

        var endIndex = text.IndexOf('\n', startIndex);
        if (endIndex == -1) endIndex = text.Length;

        return text.Substring(startIndex, endIndex - startIndex).Trim();
    }

    private int? ExtractIntField(string text, string fieldName)
    {
        var field = ExtractField(text, fieldName);
        if (int.TryParse(field, out int value))
        {
            return value;
        }
        return null;
    }
}