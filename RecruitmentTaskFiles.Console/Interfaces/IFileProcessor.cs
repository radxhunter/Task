namespace RecruitmentTaskFiles.Console.Interfaces;

public interface IFileProcessor
{
    public Task<Dictionary<string, int>> ProcessFiles(List<string> filePaths);
}
