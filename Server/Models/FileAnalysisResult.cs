namespace Server.Models;

public class FileAnalysisResult
{
	public string FileName { get; set; } = string.Empty;
	public int LineCount { get; set; }
	public int WordCount { get; set; }
	public int CharCount { get; set; }
	public bool Success { get; set; }
	public string ErrorMessage { get; set; } = string.Empty;

	public override string ToString()
	{
		return Success
			? $"Имя файла: {FileName}, Строк: {LineCount}, Слов: {WordCount}, Символов: {CharCount}"
			: $"Ошибка при анализе {FileName}: {ErrorMessage}";
	}
}