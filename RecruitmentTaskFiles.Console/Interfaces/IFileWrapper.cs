namespace RecruitmentTaskFiles.Console.Interfaces;

public interface IFileWrapper
{
    public Task<string?> ReadAllTextAsync(string filePath);
}
