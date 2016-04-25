using System.Threading.Tasks;

namespace Signing.Util
{
    public class File
    {
        public byte[] Read(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public async Task<byte[]> ReadAsync(string path)
        {
            return await Task.Run(() => System.IO.File.ReadAllBytes(path));
        }

        public void Write(string path, byte[] content)
        {
            System.IO.File.WriteAllBytes(path, content);
        }

        public async Task WriteAsync(string path, byte[] content)
        {
            await Task.Run(() => System.IO.File.WriteAllBytes(path, content));
        }

        public string ReadText(string path) {
            return System.IO.File.ReadAllText(path);
        }

        public void WriteText(string path, string content) {
            System.IO.File.WriteAllText(path, content);
        }

        public async Task<string> ReadTextAsync(string path) {
            return await Task.Run(() => ReadText(path));
        }

        public async Task WriteTextAsync(string path, string content)
        {
            await Task.Run(() => WriteText(path, content));
        }
    }
}
