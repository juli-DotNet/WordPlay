using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            // please remove the result.txt from the files
            Run().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private async static Task Run()
        {
            // usually done through DI, but for simplicity i put it here
            string folderPath = ConfigurationManager.AppSettings["FolderPath"];

            IFileService fileService = new FileService();
            IWordCloudService cloudService = new WordCloudService(folderPath, fileService);

            var words = await cloudService.ReadFilesAsync();
            await cloudService.GenerateOutputAsync($"{folderPath}/result.txt", words);
        }
    }
}
