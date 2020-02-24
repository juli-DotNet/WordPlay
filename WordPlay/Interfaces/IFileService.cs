using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordPlay
{
    public interface IFileService
    {
        Task<string> ReadContentAsync(string file);
        Task WriteLinesAsync(string filePath, IEnumerable<string> words);
        IEnumerable<string> GetAllTextFilesFromPath(string folderPath);
    }
}