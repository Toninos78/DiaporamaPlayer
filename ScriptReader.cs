using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiaporamaPlayer
{
    internal class ScriptReader
    {
        public DiaporamaScript ReadFromFile(string source)
        {
            var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
            var rawText = File.ReadAllText(source);

            return JsonSerializer.Deserialize<DiaporamaScript>(rawText, options)!;
        }
    }
}
