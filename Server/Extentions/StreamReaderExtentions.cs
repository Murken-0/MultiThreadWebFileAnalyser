using System.Text;

namespace Server.Extentions;

static class StreamReaderExtentions
{
	public static async Task<string> ReadUntilAsync(this StreamReader reader, string delimiter)
	{
		var buffer = new StringBuilder();
		char[] delimBuffer = new char[delimiter.Length];

		while (true)
		{
			int charsRead = await reader.ReadAsync(delimBuffer, 0, delimBuffer.Length);
			if (charsRead == 0) break;

			buffer.Append(delimBuffer, 0, charsRead);

			if (buffer.ToString().EndsWith(delimiter))
			{
				return buffer.ToString()[..^delimiter.Length];
			}
		}

		return buffer.ToString();
	}
}
