using Server.Models;
using System.Text;

namespace Server.Services;

public class FileAnalysisService : IFileAnalysisService
{
	private const string ResultsDirectory = "AnalysisResults";
	private const string ResultsFile = "analysis_results.txt";

	public async Task<FileAnalysisResult> AnalyzeFileAsync(string filePath)
	{
		try
		{
			string content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
			string fileName = Path.GetFileName(filePath);

			return new FileAnalysisResult
			{
				FileName = fileName,
				LineCount = CountLines(content),
				WordCount = CountWords(content),
				CharCount = content.Length,
				Success = true
			};
		}
		catch (Exception ex)
		{
			return new FileAnalysisResult
			{
				FileName = Path.GetFileName(filePath),
				Success = false,
				ErrorMessage = GetErrorMessage(ex)
			};
		}
	}

	public async Task SaveAnalysisResultAsync(FileAnalysisResult result)
	{
		Directory.CreateDirectory(ResultsDirectory);
		string resultPath = Path.Combine(ResultsDirectory, ResultsFile);

		await File.AppendAllTextAsync(resultPath,
			$"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {result}\n\n",
			Encoding.UTF8);
	}

	private int CountLines(string content) => content.Split('\n').Length;

	private int CountWords(string content) =>
		content.Split(new[] { ' ', '\t', '\n', '\r' },
			StringSplitOptions.RemoveEmptyEntries).Length;

	private string GetErrorMessage(Exception ex) => ex switch
	{
		FileNotFoundException _ => "Файл не найден",
		DirectoryNotFoundException _ => "Директория не найдена",
		IOException _ => "Ошибка ввода/вывода",
		UnauthorizedAccessException _ => "Нет доступа к файлу",
		_ => "Неизвестная ошибка"
	};
}