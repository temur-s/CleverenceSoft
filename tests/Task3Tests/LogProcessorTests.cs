using System;
using System.IO;
using Task3;
using Xunit;

namespace Task3Tests
{
    public class LogProcessorTests
    {
        [Fact]
        public void Process_Format1_Valid()
        {
            string line = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'";
            string? result = LogProcessor.ProcessLine(line, out string? problem);

            Assert.NotNull(result);
            Assert.Null(problem);
            Assert.Equal("10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы: '3.4.0.48729'", result);
        }

        [Fact]
        public void Process_Format2_Valid()
        {
            string line = "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'";
            string? result = LogProcessor.ProcessLine(line, out string? problem);

            Assert.NotNull(result);
            Assert.Null(problem);
            Assert.Equal("10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'", result);
        }

        [Theory]
        [InlineData("31.02.2025 15:14:49.523 INFORMATION Message")]
        [InlineData("10.03.2025 25:14:49.523 INFORMATION Message")]
        [InlineData("10.03.2025 15:61:49.523 INFORMATION Message")]
        [InlineData("10.03.2025 15:14:49.523 INVALIDLEVEL Message")]
        [InlineData("Just random text")]
        public void Test_Format1_Invalid(string line)
        {
            string? result = LogProcessor.ProcessLine(line, out string? problem);

            Assert.Null(result);
            Assert.Equal(line, problem);
        }

        [Theory]
        [InlineData("2025-02-31 15:14:51.5882| INFO|11|SomeMethod| Message")]
        [InlineData("2025-03-10 25:14:51.5882| INFO|11|SomeMethod| Message")]
        [InlineData("2025-03-10 15:61:51.5882| INFO|11|SomeMethod| Message")]
        [InlineData("2025-03-10 15:14:51.5882| INVALID|11|SomeMethod| Message")]
        [InlineData("2025-03-10 15:14:51.5882| INFO|11|SomeMethod")]
        public void Test_Format2_Invalid(string line)
        {
            string? result = LogProcessor.ProcessLine(line, out string? problem);

            Assert.Null(result);
            Assert.Equal(line, problem);
        }

        [Fact]
        public void Process_EndToEnd_Success()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            string inputPath = Path.Combine(tempDir, "input.txt");
            string outputPath = Path.Combine(tempDir, "output.txt");
            string problemsPath = Path.Combine(tempDir, "problems.txt");

            string[] inputLines = 
            {
                "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
                "invalid line here",
                "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'"
            };

            File.WriteAllLines(inputPath, inputLines);

            Program.Main(new string[] { inputPath, outputPath });

            Assert.True(File.Exists(outputPath));
            Assert.True(File.Exists(problemsPath));

            string[] outputs = File.ReadAllLines(outputPath);
            Assert.Equal(2, outputs.Length);
            Assert.Equal("10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы: '3.4.0.48729'", outputs[0]);
            Assert.Equal("10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'", outputs[1]);

            string[] problems = File.ReadAllLines(problemsPath);
            Assert.Single(problems);
            Assert.Equal("invalid line here", problems[0]);

            Directory.Delete(tempDir, true);
        }
    }
}
