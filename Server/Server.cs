using Server.Extentions;
using Server.Services;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

public class TextFileAnalyzerServer
{
	private readonly IFileAnalysisService _analysisService;
	private readonly TcpListener _listener;
	private readonly string _storageDirectory = "ReceivedFiles";

	public TextFileAnalyzerServer(int port, IFileAnalysisService analysisService)
	{
		_analysisService = analysisService;
		_listener = new TcpListener(IPAddress.Any, port);
		Directory.CreateDirectory(_storageDirectory);
	}

	public async Task StartAsync()
	{
		_listener.Start();
		Console.WriteLine($"Сервер запущен на порту {((IPEndPoint)_listener.LocalEndpoint).Port}");

		try
		{
			while (true)
			{
				var client = await _listener.AcceptTcpClientAsync();
				_ = HandleClientAsync(client);
			}
		}
		finally
		{
			_listener.Stop();
		}
	}

	private async Task HandleClientAsync(TcpClient client)
	{
		try
		{
			using (client)
			using (var stream = client.GetStream())
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
			{
				string fileName = await reader.ReadLineAsync() ?? string.Empty;
				string filePath = Path.Combine(_storageDirectory, GetUniqueFileName(fileName));

				string fileContent = await reader.ReadUntilAsync("\n<<EOF>>\n");
				await File.WriteAllTextAsync(filePath, fileContent, Encoding.UTF8);

				var analysisResult = await _analysisService.AnalyzeFileAsync(filePath);
				await _analysisService.SaveAnalysisResultAsync(analysisResult);

				await writer.WriteLineAsync(analysisResult.ToString());

				Console.WriteLine($"Обработан файл {fileName}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
		}
	}

	private string GetUniqueFileName(string fileName)
	{
		string name = Path.GetFileNameWithoutExtension(fileName);
		string ext = Path.GetExtension(fileName);
		int counter = 1;

		while (File.Exists(Path.Combine(_storageDirectory, fileName)))
		{
			fileName = $"{name}_{counter++}{ext}";
		}

		return fileName;
	}
}