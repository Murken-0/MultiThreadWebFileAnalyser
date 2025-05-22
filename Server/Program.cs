using Server;
using Server.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

try
{
	var analysisService = new FileAnalysisService();
	var server = new TextFileAnalyzerServer(5000, analysisService);

	Console.WriteLine("Запуск сервера анализа текстовых файлов...");
	Console.WriteLine("Нажмите Ctrl+C для остановки сервера");

	await server.StartAsync();
}
catch (Exception ex)
{
	Console.WriteLine($"Критическая ошибка: {ex.Message}");
}