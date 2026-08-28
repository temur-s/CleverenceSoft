using System;
using System.Collections.Generic;
using System.IO;

namespace Task3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Usage: Task3 <input-file> <output-file>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];

            if(!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            string? directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            string problemsPath = Path.Combine(directory ?? "", "problems.txt");

            string[] lines = File.ReadAllLines(inputPath);

            var resultLines = new List<string>();
            var problemLines = new List<string>();

            foreach(string line in lines)
            {
                string? resultLine = LogProcessor.ProcessLine(line, out string? problem);

                if(resultLine != null)
                {
                    resultLines.Add(resultLine);
                }
                else if(problem != null)
                {
                    problemLines.Add(problem);
                }
            }

            File.WriteAllLines(outputPath, resultLines);

            // Delete or write to problems.txt
            if(problemLines.Count > 0)
            {
                File.WriteAllLines(problemsPath, problemLines);
            }
            else if(File.Exists(problemsPath))
            {
                File.Delete(problemsPath);
            }

            Console.WriteLine("Log processing complete.");
        }
    }
}
