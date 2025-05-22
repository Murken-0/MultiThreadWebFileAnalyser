using System.Net.Sockets;
using System.Text;

namespace Client
{
	public class TextFileAnalyzerClient
	{
		public async Task SendFileForAnalysisAsync(string serverIp, int port, string filePath)
		{
			try
			{
				using var client = new TcpClient();
				await client.ConnectAsync(serverIp, port);

                Console.WriteLine("Соединение установлено");

				using var stream = client.GetStream();
				using var reader = new StreamReader(stream, Encoding.UTF8);
				using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

				string fileName = Path.GetFileName(filePath);
				await writer.WriteLineAsync(fileName);

				string fileContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
				await writer.WriteAsync(fileContent);
				await writer.WriteAsync("\n<<EOF>>\n");
				await writer.FlushAsync();

				string response = await reader.ReadToEndAsync();
				Console.WriteLine("\nРезультат анализа:");
				Console.WriteLine(response);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}
	}
}