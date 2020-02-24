using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPlay.Helpers;

namespace WordPlay
{
    public class FileService : IFileService
    {
        public IEnumerable<string> GetAllTextFilesFromPath(string folderPath)
        {
            return Directory.EnumerateFiles(folderPath, "*.txt");
        }

        public async Task<string> ReadContentAsync(string file)
        {
            using (var reader = File.OpenText(file))
            {
               return await reader.ReadToEndAsync();
               
            }

        }

        public async Task WriteLinesAsync(string filePath, IEnumerable<string> words)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach (var word in words)
                    {
                        await sw.WriteLineAsync(word);
                    }

                }
            }
        }
    }
}
