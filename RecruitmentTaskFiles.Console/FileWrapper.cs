using RecruitmentTaskFiles.Console.Interfaces;

namespace RecruitmentTaskFiles.Console;

public class FileWrapper : IFileWrapper
{
    public async Task<string?> ReadAllTextAsync(string filePath) => await File.ReadAllTextAsync(filePath);
}
