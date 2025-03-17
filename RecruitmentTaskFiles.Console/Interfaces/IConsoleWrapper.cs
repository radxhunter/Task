namespace RecruitmentTaskFiles.Console.Interfaces;

public interface IConsoleWrapper
{
    public void WriteLine(string message);
    public string? ReadLine();
}
