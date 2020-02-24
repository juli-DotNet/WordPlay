
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPlay.Helpers;

namespace WordPlay
{
    public class WordCloudService : IWordCloudService
    {
        readonly string folderPath = "";
        private readonly IFileService fileService;


        public WordCloudService(string folderPath, IFileService fileService)
        {
            this.folderPath = folderPath;
            this.fileService = fileService;
        }



        public async Task<IEnumerable<string>> ReadFilesAsync()
        {
            ConcurrentBag<string> words = new ConcurrentBag<string>();
            var txtFiles = fileService.GetAllTextFilesFromPath(folderPath);

            foreach (string file in txtFiles)
            {
                await ReadFileContent(file, words);
            }
            return words;
        }


        private async Task ReadFileContent(string filepath, ConcurrentBag<string> words)
        {
            var fileText = await fileService.ReadContentAsync(filepath);
            foreach (string word in fileText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    words.Add(word.StripPunctuation());
                }

            }
        }

        public async Task GenerateOutputAsync(string filePath, IEnumerable<string> words)
        {
            var result = new List<string>();

            var list = words.GroupBy(a => a).Select(a =>
                new
                {
                    Word = a.Key,
                    Count = a.Count()
                }).OrderByDescending(a => a.Count);

            var maxCount = list.First().Count;

            foreach (var w in list.Where(a => a.Count > 1))
            {
                result.Add(GenerateLine(w.Word, w.Count, maxCount));
            }
            await fileService.WriteLinesAsync(filePath, result);

        }

        private string GenerateLine(string word, int count, int maxCount)
        {
            StringBuilder sb = new StringBuilder($"{word} {count}");

            if (count == maxCount)
            {
                sb.Append(" Huge");
            }
            else if (count > (maxCount * 0.6))
            {
                sb.Append(" Big");
            }
            else if (count > (maxCount * 0.3))
            {
                sb.Append(" Normal");
            }
            else
            {
                sb.Append(" Small");
            }
            return sb.AppendLine().ToString();
        }

    }
}
