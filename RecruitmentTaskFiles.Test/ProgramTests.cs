using Moq;
using NUnit.Framework;
using RecruitmentTaskFiles.Console.Interfaces;
using Program = RecruitmentTaskFiles.Console.Program;

namespace RecruitmentTaskFiles.Test;

public class ProgramTests
{
    private readonly Mock<IConsoleWrapper> _mockConsoleWrapper = new();
    private readonly Mock<IFileProcessor> _mockFileProcessor = new();

    private Program _program = new();

    [SetUp]
    public void Setup()
    {
        _mockConsoleWrapper.Reset();
        _mockFileProcessor.Reset();

        _mockConsoleWrapper
            .SetupSequence(x => x.ReadLine())
            .Returns("2")
            .Returns("file1.txt")
            .Returns("file2.txt");

        _mockFileProcessor
            .Setup(x => x.ProcessFiles(It.IsAny<List<string>>()))
            .ReturnsAsync(new Dictionary<string, int>
            {
                { "hello", 1 },
                { "world", 2 }
            });
    }

    [Test]
    public async Task Run_WhenCalled_ReturnsUniqueWords()
    {
        // Act
        await _program.Run(_mockConsoleWrapper.Object, _mockFileProcessor.Object);

        // Assert
        _mockConsoleWrapper.Verify(x => x.WriteLine("hello: 1"), Times.Once);
        _mockConsoleWrapper.Verify(x => x.WriteLine("world: 2"), Times.Once);
        _mockFileProcessor.Verify(x => x.ProcessFiles(It.IsAny<List<string>>()), Times.Once);
    }

    [Test]
    public async Task Run_WithInvalidCountInput_WriteErrorToConsole()
    {
        // Arrange
        _mockConsoleWrapper
            .Setup(x => x.ReadLine())
            .Returns("invalid");

        // Act
        await _program.Run(_mockConsoleWrapper.Object, _mockFileProcessor.Object);

        // Assert
        _mockConsoleWrapper.Verify(x => x.WriteLine("Invalid count provided"), Times.Once);
        _mockFileProcessor.VerifyNoOtherCalls();
    }
}