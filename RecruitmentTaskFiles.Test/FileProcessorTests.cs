using Moq;
using NUnit.Framework;
using RecruitmentTaskFiles.Console;
using RecruitmentTaskFiles.Console.Interfaces;

namespace RecruitmentTaskFiles.Test;

public class FileProcessorTests
{
    private Mock<IFileWrapper> _mockFileWrapper = new();
    private Mock<IConsoleWrapper> _mockConsoleWrapper = new();
    private FileProcessor _fileProcessor = null!;

    [SetUp]
    public void Setup()
    {
        _mockFileWrapper.Reset();
        _mockConsoleWrapper.Reset();

        _fileProcessor = new FileProcessor(_mockConsoleWrapper.Object, _mockFileWrapper.Object);
    }

    [Test]
    public async Task ProcessFiles_WithValidFiles_ReturnsCorrectWordCount()
    {
        // Arrange
        var filePaths = new List<string> { "file1.txt", "file2.txt" };
        _mockFileWrapper.Setup(f => f.ReadAllTextAsync("file1.txt")).ReturnsAsync("Hello world! Hello.");
        _mockFileWrapper.Setup(f => f.ReadAllTextAsync("file2.txt")).ReturnsAsync("World, test.");

        // Act
        var result = await _fileProcessor.ProcessFiles(filePaths);

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result["hello"], Is.EqualTo(2));
        Assert.That(result["world"], Is.EqualTo(2));
        Assert.That(result["test"], Is.EqualTo(1));
    }

    [Test]
    public async Task ProcessFiles_WithEmptyFile_ReturnsEmptyDictionary()
    {
        // Arrange
        var filePaths = new List<string> { "empty.txt" };
        _mockFileWrapper.Setup(f => f.ReadAllTextAsync("empty.txt")).ReturnsAsync(string.Empty);

        // Act
        var result = await _fileProcessor.ProcessFiles(filePaths);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ProcessFiles_WithFileReadError_HandlesException()
    {
        // Arrange
        var filePaths = new List<string> { "error.txt" };
        _mockFileWrapper.Setup(f => f.ReadAllTextAsync("error.txt")).Throws(new IOException("File error"));

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await _fileProcessor.ProcessFiles(filePaths));
        _mockConsoleWrapper.Verify(c => c.WriteLine("Error reading file: 'error.txt'. File error"), Times.Once);
    }
}