using RecruitmentTaskFiles.Console.Interfaces;

namespace RecruitmentTaskFiles.Console;

public class FileProcessor(IConsoleWrapper console, IFileWrapper fileWrapper) : IFileProcessor
{
    public async Task<Dictionary<string, int>> ProcessFiles(List<string> filePaths)
    {
        // We can consider using ConcurrentDictionary if we want to process files in parallel
        // If Dictionary as a structure is not enough, we can some data storage to store the results, like Azure Table Storage
        var uniqueWords = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var filePath in filePaths)
        {
            try
            {
                // We can use StreamReader for large files, to read line by line. Using this we can avoid filling RAM with large files
                var fileContent = await fileWrapper.ReadAllTextAsync(filePath);
                if (fileContent is null)
                {
                    continue;
                }

                var words = fileContent
                    .Split()
                    .Select(word => new string(
                        word.Where(c => !char.IsPunctuation(c) && !char.IsSymbol(c)).ToArray()  // We could regex alternatively, we are removing punctuation and symbols to avoid having entries like: 'do,' and 'do'
                    ))
                    .Where(word => !string.IsNullOrEmpty(word));

                foreach (var word in words)
                {
                    uniqueWords[word] = uniqueWords.ContainsKey(word) ? uniqueWords[word] + 1 : 1;
                }
            }
            catch (Exception e) // We don't want to fail the entire process if one file fails
            {
                console.WriteLine($"Error reading file: '{filePath}'. {e.Message}");
            }
        }

        return uniqueWords;
    }
}
