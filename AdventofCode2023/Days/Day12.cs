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
        private bool UseTestingFile = true;

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

                    //springRow = ReplaceExternalSequences(springRow, sequenceConstraints);
                    //springRow = RemoveExistingSequenceBreakers(springRow, sequenceConstraints);
                    //springRow = RemoveInsufficientUnknowns(springRow, sequenceConstraints);
                    //springRow = CheckConditionsMet(springRow, sequenceConstraints);

                    for (int i = 0; i < springRow.Length; i++)
                    {
                        if (springRow[i] == placeHolder)
                        {
                            variableIndexes.Add(i);
                        }
                    }

                    List<string> potentialCombinations = new List<string>();

                    if (variableIndexes.Count == 0)
                    {
                        solutionCount += 1;
                        continue;
                    }
                    else if(variableIndexes.Count == 1)
                    {
                        potentialCombinations.Add("#");
                        potentialCombinations.Add(".");
                    }
                    else
                    {
                        potentialCombinations = GenerateCombinations(variableIndexes.Count, charOpts,
                            charOpts.Select(x => x.ToString()).ToList(), 1);
                    }
  

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
                    Console.WriteLine(validCount);

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

        private char[] RemoveInsufficientUnknowns(char[] springRow, int[] sequenceConstraints)
        {
            List<int> sequence = new List<int>();
            bool replace = false;

            for (int i = 0; i < springRow.Length; i++)
            {
                if (springRow[i] == '#')
                {
                    break;
                }
                else if (springRow[i] == '?')
                {
                    sequence.Add(i);
                }
                else if(springRow[i] != '?' && sequence.Count > 0)
                {
                    if(sequence.Count < sequenceConstraints.First())
                    {
                        replace = true;
                    }
                    break;
                }
                
            }

            if (replace)
            {
                for(int i = 0; i < sequence.Count; i++)
                {
                    springRow[sequence[i]] = '.';
                }
            }

            sequence.Clear();
            replace = false;

            for (int i = springRow.Length - 1; i >= 0; i--)
            {
                if (springRow[i] == '#')
                {
                    break;
                }
                else if (springRow[i] == '?')
                {
                    sequence.Add(i);
                }
                else if (springRow[i] != '?' && sequence.Count > 0)
                {
                    if (sequence.Count < sequenceConstraints.Last())
                    {
                        replace = true;
                    }
                    break;
                }
                else if (springRow[i] == '#')
                {
                    break;
                }
            }

            if (replace)
            {
                for (int i = 0; i < sequence.Count; i++)
                {
                    springRow[sequence[i]] = '.';
                }
            }

            return springRow;
        }

        private char[] CheckConditionsMet(char[] springRow, int[] sequenceConstraints)
        {
            List<int> sequence = new List<int>();
            int constraintIndex = 0;
            bool[] constraintsMet = new bool[sequenceConstraints.Length];

            for(int i = 0; i < constraintsMet.Length; i++)
            {
                constraintsMet[i] = false;
            }

            for (int i = 0; i < springRow.Length; i++)
            {
                if (springRow[i] == '#')
                {
                    sequence.Add(i);
                }
                else if (springRow[i] != '#' && sequence.Count > 0)
                {
                    if (sequence.Count == sequenceConstraints[constraintIndex])
                    {
                        constraintsMet[constraintIndex] = true;
                        constraintIndex++;
                        sequence.Clear();
                    }
                }
                else
                {
                    sequence.Clear();
                }
            }


            if(constraintsMet.All(x => x))
            {
                return new string(springRow).Replace('?', '.').ToCharArray();
            }

            return springRow;

        }


        private char[] RemoveExistingSequenceBreakers(char[] springRow, int[] sequenceConstraints)
        {
            List<int> sequence = new List<int>();
            List<int> qIndicies = new List<int>();
            for(int i = 0; i < springRow.Length; i++)
            {
                if (springRow[i] == '#')
                {
                    sequence.Add(i);
                }
                else if(springRow[i] != '#' && sequence.Count > 0)
                {
                    if(sequence.Count == sequenceConstraints.First())
                    {
                        if(qIndicies.Count > sequenceConstraints.First())
                        {
                            break;
                        }

                        if(sequence.First() > 0)
                        {
                            springRow[sequence.First() - 1] = '.';
                        }
                        if (sequence.Last() < springRow.Length - 1)
                        {
                            springRow[sequence.Last() + 1] = '.';
                        }
                    }

                    
                    //currentConstraintIndex++;

                    //if(currentConstraintIndex == sequenceConstraints.Length)
                    //{
                    break;
                    //}
                }
                else if (springRow[i] == '?')
                {
                    qIndicies.Add(i);
                }
            }


            sequence.Clear();
            qIndicies.Clear();

            for (int i = springRow.Length - 1; i >= 0; i--)
            {
                if (springRow[i] == '#')
                {
                    sequence.Add(i);
                }
                else if (springRow[i] != '#' && sequence.Count > 0)
                {
                    if (sequence.Count == sequenceConstraints.Last())
                    {
                        if (qIndicies.Count > sequenceConstraints.Last())
                        {
                            break;
                        }

                        if (sequence.Last() > 0)
                        {
                            springRow[sequence.Last() - 1] = '.';
                        }
                        if (sequence.First() < springRow.Length - 1)
                        {
                            springRow[sequence.First() + 1] = '.';
                        }
                    }

                    sequence.Clear();
                    //currentConstraintIndex++;

                    //if(currentConstraintIndex == sequenceConstraints.Length)
                    //{
                    break;
                    //}
                }
                else if (springRow[i] == '?')
                {
                    qIndicies.Add(i);
                }
            }

            return springRow;

        }


        private char[] ReplaceExternalSequences(char[] springRow, int[] sequenceConstraints)
        {
            int sequenceNeeded = sequenceConstraints.Last();
            List<int> sequence = new List<int>();

            for(int i = springRow.Length - 1; i >= 0; i--)
            {
                if (springRow[i] == '#')
                {
                    break;
                }
                else if (springRow[i] == '?')
                {
                    sequence.Add(i);
                }
                else if(sequence.Count > 0)
                {
                    if(sequence.Count < sequenceNeeded)
                    {
                        foreach(var item in sequence)
                        {
                            springRow[item] = '.';
                        }
                    }
                }
            }
            sequence.Clear();

            if(sequenceNeeded > 1)
            {
                for (int i = springRow.Length - 1; i >= 0; i--)
                {
                    if (new char[] { '?', '#' }.Contains(springRow[i]))
                    {
                        sequence.Add(i);

                        if (sequence.Count == sequenceNeeded)
                        {
                            // Match could contain next char, don't set it
                            if (i > 0 && new char[] { '?', '#' }.Contains(springRow[i - 1]))
                            {
                                break;
                            }

                            foreach (var item in sequence.Where(x => x != '#'))
                            {
                                springRow[item] = '#';
                            }
                            break;
                        }
                    }
                    else if (sequence.Count > 0)
                    {
                        break;
                    }
                }

                sequence.Clear();
            }

            sequenceNeeded = sequenceConstraints.First();

            for (int i = 0; i < springRow.Length; i++)
            {
                if (springRow[i] == '#')
                {
                    break;
                }
                if (springRow[i] == '?')
                {
                    sequence.Add(i);
                }
                else if (sequence.Count > 0)
                {
                    if (sequence.Count < sequenceNeeded)
                    {
                        foreach (var item in sequence)
                        {
                            springRow[item] = '.';
                        }
                    }
                }
            }
            sequence.Clear();

            if(sequenceNeeded > 1)
            {
                for (int i = 0; i < springRow.Length; i++)
                {
                    if (new char[] { '?', '#' }.Contains(springRow[i]))
                    {
                        sequence.Add(i);

                        if (sequence.Count == sequenceNeeded)
                        {
                            // Match could contain next char, don't set it
                            if (i < springRow.Length - 1
                                && new char[] { '?', '#' }.Contains(springRow[i + 1]))
                            {
                                break;
                            }

                            foreach (var item in sequence.Where(x => x != '#'))
                            {
                                springRow[item] = '#';
                            }
                            break;
                        }
                    }
                    else if (sequence.Count > 0)
                    {
                        break;
                    }

                }
            }
            

            return springRow;

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

        private List<string> GenerateCombinations(int count,
            List<char> chars,
            List<string> combinations, int layer)
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

