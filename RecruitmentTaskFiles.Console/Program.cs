using RecruitmentTaskFiles.Console.Interfaces;

namespace RecruitmentTaskFiles.Console;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Added wrappers to make the code testable
        var console = new ConsoleWrapper();
        var file = new FileWrapper();

        var fileProcessor = new FileProcessor(console, file);
        var program = new Program();

        await program.Run(console, fileProcessor);
    }

    public async Task Run(IConsoleWrapper consoleWrapper, IFileProcessor fileProcessor)
    {
        var filePaths = new List<string>();

        consoleWrapper.WriteLine("Enter the number of files to process:");

        if (!int.TryParse(consoleWrapper.ReadLine(), out int fileCount))
        {
            consoleWrapper.WriteLine("Invalid count provided");
            return;
        }

        consoleWrapper.WriteLine("Enter the file paths:");

        for (int i = 0; i < fileCount; i++)
        {
            consoleWrapper.WriteLine($"Enter file path {i + 1}:");
            var filePath = consoleWrapper.ReadLine();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                continue;
            }

            filePaths.Add(filePath);
        }

        var wordCounts = await fileProcessor.ProcessFiles(filePaths);

        foreach (var kvp in wordCounts)
        {
            consoleWrapper.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}
