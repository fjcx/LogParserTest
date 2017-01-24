using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParserTest {
    public class Parser {
        public IEnumerable<ParseResult> Parse(IEnumerable<string> input) {
            List<ParseResult> parseResults = new List<ParseResult>();

            string selector = "entity type";
            string[] stringSeparators = new string[] { "entity type " };

            // Making assumption that a different type of log line does not contain this string
            IEnumerable<string> filteredLines = input.Where(line => line.Contains(selector));

            Dictionary<string, ParseResult> hashedResults = new Dictionary<string, ParseResult>();

            foreach (string line in filteredLines) {
                string[] strArray = line.Split('|');
                string queryName = strArray[strArray.Length - 1].Split(stringSeparators, StringSplitOptions.None).Last().Trim();

                ParseResult parseResult;
                if (hashedResults.ContainsKey(queryName)) {
                    parseResult = hashedResults[queryName];
                } else {
                    parseResult = new ParseResult();
                    parseResult.Name = queryName;
                    parseResult.Count = 0;
                }

                parseResult.Count += 1;
                // Making assumption that values are only in format 'double ms:'
                string timeString = strArray[4].Replace("ms:", "").Trim();
                double time;

                if (Double.TryParse(timeString, out time)) {
                    // just adding all times here, averaging later with total count for efficiency
                    parseResult.AverageExecutionSpeed += time;  
                } else {
                    throw new LogParserException("Log file not in expected format. Error parsing time on line: " + line);
                }
                hashedResults[queryName] = parseResult;
            }

            foreach (ParseResult parseResult in hashedResults.Values) {
                // getting actual average using total count
                parseResult.AverageExecutionSpeed = parseResult.AverageExecutionSpeed / parseResult.Count;
                parseResults.Add(parseResult);
            }

            return parseResults;
        }
    }
}
