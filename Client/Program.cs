using Client;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

if (args.Length < 1)
{
	Console.WriteLine("Использование: Client <путь_к_файлу> [IP_сервера] [порт]");
	Console.WriteLine("Пример: Client document.txt 127.0.0.1 5000");
	return;
}

try
{
	string filePath = args[0];
	string serverIp = args.Length > 1 ? args[1] : "127.0.0.1";
	int port = args.Length > 2 ? int.Parse(args[2]) : 5000;

	if (!File.Exists(filePath))
	{
		Console.WriteLine($"Файл не найден: {filePath}");
		return;
	}

	Console.WriteLine($"Отправка файла {filePath} на сервер {serverIp}:{port}...");

	var client = new TextFileAnalyzerClient();
	await client.SendFileForAnalysisAsync(serverIp, port, filePath);
}
catch (Exception ex)
{
	Console.WriteLine($"Ошибка: {ex.Message}");
}