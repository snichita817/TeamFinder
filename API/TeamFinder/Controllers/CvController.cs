﻿using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
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
    private const int MaxRetries = 5;

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

        StringBuilder extractedText = new StringBuilder();
        using (var reader = new PdfReader(file.OpenReadStream()))
        {
            using (var pdfDoc = new PdfDocument(reader))
            {
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    extractedText.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page)));
                    extractedText.Append('\n'); // Add a newline for separation between pages
                }
            }
        }

        var prompt = $"Extract the following information from this CV: {extractedText.ToString()}\n\n" +
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

        CvInfo finalCvInfo = new CvInfo();
        int attempt = 0;

        while (attempt < MaxRetries)
        {
            var completionRequest = new CompletionRequest(prompt, model: "gpt-3.5-turbo-instruct", max_tokens: 500);
            var completionResult = await _openAiApi.Completions.CreateCompletionAsync(completionRequest);
            var extractedInfo = ParseCvInfo(completionResult.Completions[0].Text);

            MergeCvInfo(finalCvInfo, extractedInfo);
            attempt++;
        }

        return Ok(finalCvInfo);
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

    private void MergeCvInfo(CvInfo finalCvInfo, CvInfo newInfo)
    {
        if (string.IsNullOrEmpty(finalCvInfo.FirstName))
            finalCvInfo.FirstName = newInfo.FirstName;
        if (string.IsNullOrEmpty(finalCvInfo.LastName))
            finalCvInfo.LastName = newInfo.LastName;
        if (string.IsNullOrEmpty(finalCvInfo.Email))
            finalCvInfo.Email = newInfo.Email;
        if (string.IsNullOrEmpty(finalCvInfo.University))
            finalCvInfo.University = newInfo.University;
        if (!finalCvInfo.GraduationYear.HasValue)
            finalCvInfo.GraduationYear = newInfo.GraduationYear;
        if (string.IsNullOrEmpty(finalCvInfo.Skills))
            finalCvInfo.Skills = newInfo.Skills;
        if (string.IsNullOrEmpty(finalCvInfo.LinkedInUrl))
            finalCvInfo.LinkedInUrl = newInfo.LinkedInUrl;
        if (string.IsNullOrEmpty(finalCvInfo.GitHubUrl))
            finalCvInfo.GitHubUrl = newInfo.GitHubUrl;
        if (string.IsNullOrEmpty(finalCvInfo.PortfolioUrl))
            finalCvInfo.PortfolioUrl = newInfo.PortfolioUrl;
        if (string.IsNullOrEmpty(finalCvInfo.Bio))
            finalCvInfo.Bio = newInfo.Bio;
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
