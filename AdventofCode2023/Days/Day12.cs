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
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(' ');
                    char[] springRow = lineParts[0].ToCharArray();
                    string springrRowStr = String.Empty;
                    int[] sequenceConstraints = lineParts[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                    List<char> charOpts = new List<char>() { '#', '.' };
                    char placeHolder = '?';

                    //springrRowStr = string.Concat(Enumerable.Repeat(new string(springRow) + "?", 5));
                    //springrRowStr = springrRowStr.Remove(springrRowStr.Length - 1);
                    ///springRow = springrRowStr.ToCharArray();

                    //sequenceConstraints = string.Join(',', Enumerable.Repeat(lineParts[1], 5)).Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                    // Binary search tree with pruning

                    List<TreeNode> tree = new List<TreeNode>();

                    TreeNode rootNode = new TreeNode(springRow.ElementAt(0), null);

                    if (springRow[0] == '?')
                    {
                        springRow[0] = '.';
                        rootNode.Value = '.';
                        ConstructTree(rootNode, springRow, sequenceConstraints, 1, ref tree);

                        springRow[0] = '#';
                        rootNode.Value = '#';
                        ConstructTree(rootNode, springRow, sequenceConstraints, 1, ref tree);
                    }
                    else
                    {
                        ConstructTree(rootNode, springRow, sequenceConstraints, 1, ref tree);
                    }

                    List<TreeNode> fd = tree.Where(x => x.RulesSatisfiedCount == sequenceConstraints.Length - 1).ToList();
                    Console.WriteLine(fd.Count);
                    

                }
            }

            return 0;
        }

        private void ConstructTree(TreeNode parentNode, char[] row, int[] rules, int layer, ref List<TreeNode> tree)
        {
            TreeNode leftNode = null;
            TreeNode rightNode = null;

            if (row[layer] == '?')
            {
                leftNode = new TreeNode('.', parentNode);
                leftNode.RulesSatisfiedCount = parentNode.RulesSatisfiedCount;
                rightNode = new TreeNode('#', parentNode);
                rightNode.RulesSatisfiedCount = parentNode.RulesSatisfiedCount;
            }
            else
            {
                // # or .   No need to branch a non-variable into two paths
                leftNode = new TreeNode(row[layer], parentNode);
                leftNode.RulesSatisfiedCount = parentNode.RulesSatisfiedCount;
            }


            for(int i = 0; i < 2; i++)
            {
                TreeNode? node = (i == 0) ? leftNode : rightNode;
                
                if (node == null)
                {
                    break;
                }

                string reverseSequence = Convert.ToString(node.Value);
               

                TreeNode parent = node.ParentNode;
                while (parent != null)
                {
                    reverseSequence += parent.Value;
                    parent = parent.ParentNode;
                }

                string sequence = String.Empty;
                

                for(int j = reverseSequence.Length - 1; j >= 0; j--)
                {
                    sequence += reverseSequence[j];
                }

                if(sequence == "..#..")
                {
                    int y = 6;
                }
                
                if (ValidToRule(sequence, rules, node.RulesSatisfiedCount) || sequence.All(x => x == '.'))
                {
                    Console.WriteLine(sequence + " " + node.RulesSatisfiedCount);
                    if (!sequence.All(x => x == '.'))
                    {
                        node.RulesSatisfiedCount++;
                    }
                    
                    tree.Add(node);
                    if(layer + 1 < row.Length)
                    {
                        ConstructTree(node, row, rules, ++layer, ref tree);
                    }
                    
                }
            }
            

        }


        private bool ValidToRule(string sequence, int[] rules, int ruleNumber)
        {
            List<int> groupings = new List<int>();
            int groupLen = 0;

            if(sequence.Length >=3 && sequence.Substring(0,3) == "..#")
            {
                int g = 34;
            }
            for(int i = 0; i < sequence.Length; i++)
            {
                if (sequence[i] == '#')
                {
                    groupLen++;
                }
                else if(groupLen > 0)
                {
                    groupings.Add(groupLen);
                    groupLen = 0;
                }
                else
                {
                    groupLen = 0;
                }
            }

            if(groupLen > 0)
            {
                groupings.Add(groupLen);
            }

            for(int i = 0; i < ruleNumber + 1; i++)
            {
                if (!(i < groupings.Count && rules[i] == groupings[i]))
                {
                    return false;
                }
                
            }

            return true;
        }

        private bool RulesSatisfied(string sequence, int[] rules)
        {
            string rule = "(^|[^#]+)";

            foreach (int matchCount in rules)
            {
                rule += $"#{{{matchCount}}}[^#]+";
            }
            rule = rule.Remove(rule.Length - 5) + "($|(?![^#]*#))";

            Regex constraint = new Regex(rule);
            Match match = constraint.Match(sequence);

            return match.Success && match.Value.Count(x => x == '#') == sequence.Count(x => x == '#');
        }

        public int Problem2Old()
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

        
        private class TreeNode
        {
            public TreeNode? ParentNode { get; set; }
            public char Value { get; set; }
            public int RulesSatisfiedCount { get; set; }

            public TreeNode(char val, TreeNode? node)
            {
                ParentNode = node;
                Value = val;
                RulesSatisfiedCount = 0;
            }

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

