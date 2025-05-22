using Server.Models;

namespace Server.Services;

public interface IFileAnalysisService
{
	Task<FileAnalysisResult> AnalyzeFileAsync(string filePath);
	Task SaveAnalysisResultAsync(FileAnalysisResult result);
}