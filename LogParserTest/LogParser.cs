using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogParserTest {
    class LogParser {

        static void Main(string[] args) {
            if (args.Any()) {
                var logFilePath = args[0];
                if (File.Exists(logFilePath)) {
                    var logFile = File.ReadAllLines(logFilePath);
                    List<string> LogLines = new List<string>(logFile);

                    Parser parser = new Parser();
                    try {
                        IEnumerable<ParseResult> results = parser.Parse(LogLines);

                        foreach (ParseResult parseResult in results) {
                            Console.WriteLine("Entity type: " + parseResult.Name + ", Count: " + parseResult.Count + ", Averaged Execution Time: " + parseResult.AverageExecutionSpeed);
                        }
                    } catch (LogParserException e) {
                        Console.WriteLine("Error: Error occurred while parsing file.\n" + e.Message);
                    }
                } else {
                    Console.WriteLine("Error: Log file path specified does not exist.");
                }
            } else {
                Console.WriteLine("Usage: Log file path expected as first argument.");
            }
        }
    }
}
