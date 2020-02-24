using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordPlay
{
    public interface IWordCloudService
    {
        Task GenerateOutputAsync(string filePath, IEnumerable<string> words);
        Task<IEnumerable<string>> ReadFilesAsync();
    }
}