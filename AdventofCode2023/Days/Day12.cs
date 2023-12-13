using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day12 : IDay
    {
        private int DayNum = 12;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day12()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            int solutionCount = 0;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                int lineNum = 1;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine("PROCESSING LINE" + lineNum);
                    lineNum++;
                    string[] lineParts = line.Split(' ');
                    char[] springRow = lineParts[0].ToCharArray();
                    int[] sequenceConstraints = lineParts[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                    List<char> charOpts = new List<char>(){ '#', '.' };
                    List<int> variableIndexes = new List<int>();
                    char placeHolder = '?';

                    springRow = string.Concat(Enumerable.Repeat(new string(springRow) + "?", 5)).TrimEnd('?').ToCharArray();
                    sequenceConstraints = string.Join(',', Enumerable.Repeat(lineParts[1], 5)).Split(',').Select(x => Convert.ToInt32(x)).ToArray();



                    for (int i = 0; i < springRow.Length; i++)
                    {
                        if (springRow[i] == placeHolder)
                        {
                            variableIndexes.Add(i);
                        }
                    }
                    List<string> potentialCombinations = GenerateCombinations(variableIndexes.Count, charOpts,
                        charOpts.Select(x => x.ToString()).ToList(), 1);

                    int validCount = 0;

                    foreach(var combination in potentialCombinations)
                    {
                        int replaceIndex = 0;
                        StringBuilder replacedSpringRow = new StringBuilder();

                        for(int i = 0; i < springRow.Length; i++)
                        {
                            if (springRow[i] == placeHolder)
                            {
                                replacedSpringRow.Append(combination[replaceIndex]);
                                replaceIndex++;
                            }
                            else
                            {
                                replacedSpringRow.Append(springRow[i]);
                            }
                        }

                        bool valid = CheckPotentialCombination(replacedSpringRow.ToString().ToCharArray(), sequenceConstraints);
                        validCount += valid ? 1 : 0;

                        if (valid)
                        {
                            //Console.WriteLine(replacedSpringRow.ToString() + " : " + string.Join(',', sequenceConstraints));
                        }
                        
                    }
                    //Console.WriteLine(validCount);

                    solutionCount += validCount;

                }
            }

            return solutionCount;
        }

        public int Problem2()
        {
            string? line;
            int solutionCount = 0;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                int lineNum = 1;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine("PROCESSING LINE" + lineNum);
                    lineNum++;
                    string[] lineParts = line.Split(' ');
                    char[] springRow = lineParts[0].ToCharArray();
                    int[] sequenceConstraints = lineParts[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                    List<char> charOpts = new List<char>() { '#', '.' };
                    List<int> variableIndexes = new List<int>();
                    char placeHolder = '?';

                    springRow = string.Concat(Enumerable.Repeat(new string(springRow) + "?", 5)).TrimEnd('?').ToCharArray();
                    sequenceConstraints = string.Join(',', Enumerable.Repeat(lineParts[1], 5)).Split(',').Select(x => Convert.ToInt32(x)).ToArray();



                    for (int i = 0; i < springRow.Length; i++)
                    {
                        if (springRow[i] == placeHolder)
                        {
                            variableIndexes.Add(i);
                        }
                    }
                    List<string> potentialCombinations = GenerateCombinations(variableIndexes.Count, charOpts,
                        charOpts.Select(x => x.ToString()).ToList(), 1);

                    int validCount = 0;

                    foreach (var combination in potentialCombinations)
                    {
                        int replaceIndex = 0;
                        StringBuilder replacedSpringRow = new StringBuilder();

                        for (int i = 0; i < springRow.Length; i++)
                        {
                            if (springRow[i] == placeHolder)
                            {
                                replacedSpringRow.Append(combination[replaceIndex]);
                                replaceIndex++;
                            }
                            else
                            {
                                replacedSpringRow.Append(springRow[i]);
                            }
                        }

                        bool valid = CheckPotentialCombination(replacedSpringRow.ToString().ToCharArray(), sequenceConstraints);
                        validCount += valid ? 1 : 0;

                        if (valid)
                        {
                            //Console.WriteLine(replacedSpringRow.ToString() + " : " + string.Join(',', sequenceConstraints));
                        }

                    }
                    //Console.WriteLine(validCount);

                    solutionCount += validCount;

                }
            }

            return solutionCount;
        }


        private bool CheckPotentialCombination(char[] springRow, int[] sequenceConstraints)
        {
            string rule = "(^|[^#]+)";

            foreach (int matchCount in sequenceConstraints)
            {
                rule += $"#{{{matchCount}}}[^#]+";
            }
            rule = rule.Remove(rule.Length - 5) + "($|(?![^#]*#))";

            if (new string(springRow).Substring(0,7) == ".#....#")
            {
                int t = 54;
            }
            Regex constraint = new Regex(rule);
            Match match = constraint.Match(new string(springRow));



            return match.Success && match.Value.Count(x => x == '#') == springRow.Count(x => x == '#');
        }

        private List<string> GenerateCombinations(int count, List<char> chars, List<string> combinations, int layer)
        {
            List<string> remove = new List<string>();
            List<string> add = new List<string>();

            foreach (var combination in combinations)
            {
                remove.Add(combination);
                foreach (var c in chars)
                {
                    add.Add($"{combination}{c}");
                }
            }

            combinations.RemoveAll(x => remove.Contains(x));
            combinations.AddRange(add);

            if(combinations.First().Length != count)
            {
                return GenerateCombinations(count, chars, combinations, ++layer);
            }
            else
            {
                return combinations;
            }
            
            
        }

    }

}

