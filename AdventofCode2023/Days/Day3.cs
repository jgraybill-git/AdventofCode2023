using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day3 : IDay
    {
        private int DayNum = 3;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day3()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? fileLine;
            int partsSum = 0;
            List<string> lines = new List<string>();
            Regex digitRegex = new Regex(@"\d+");
            Regex symbolsRegex = new Regex(@"[^\.\d]");

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((fileLine = streamReader.ReadLine()) != null)
                {
                    lines.Add(fileLine);
                }
            }

            Dictionary<int, List<int>> numbersAlreadyAdded = new Dictionary<int, List<int>>();
            for(int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                MatchCollection numberMatches = digitRegex.Matches(line);

                foreach(Match match in numberMatches)
                {
                    int numberIndex = match.Index;
                    int numberLength = match.Length;
                    int number = Convert.ToInt32(match.Value);

                    Dictionary<int, List<int>> adjacencies = new Dictionary<int, List<int>>()
                    {
                        // This row
                        { i, new List<int>() { numberIndex - 1, numberIndex + numberLength }}
                    };
                    // Previous row
                    if(i > 0)
                    {
                        List<int> prevRowIndexes = new List<int>() { numberIndex - 1 };
                        for (int j = 0; j < numberLength; j++)
                        {
                            prevRowIndexes.Add(numberIndex + j);
                        }
                        prevRowIndexes.Add(numberIndex + numberLength);
                        adjacencies.Add(i - 1, prevRowIndexes);
                    }
                    // Next row
                    if (i < lines.Count - 1)
                    {
                        List<int> nextRowIndexes = new List<int>() { numberIndex - 1 };
                        for (int j = 0; j < numberLength; j++)
                        {
                            nextRowIndexes.Add(numberIndex + j);
                        }
                        nextRowIndexes.Add(numberIndex + numberLength);
                        adjacencies.Add(i + 1, nextRowIndexes);
                    }

                    /////////
                    foreach(KeyValuePair<int, List<int>> kvp in adjacencies)
                    {
                        var rowIndex = kvp.Key;
                        
                        foreach (int colIndex in kvp.Value)
                        {
                            if(colIndex > -1 && colIndex < lines[rowIndex].Length &&
                                symbolsRegex.Match(Convert.ToString(lines[rowIndex][colIndex])).Success)
                            {
                                if((numbersAlreadyAdded.ContainsKey(i) &&
                                    !numbersAlreadyAdded[i].Contains(numberIndex))
                                    || !numbersAlreadyAdded.ContainsKey(i))
                                {
                                    partsSum += number;

                                    if (!numbersAlreadyAdded.ContainsKey(i))
                                    {
                                        numbersAlreadyAdded[i] = new List<int>() { numberIndex };
                                    }
                                    else
                                    {
                                        numbersAlreadyAdded[i].Add(numberIndex);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return partsSum;
        }


        public int Problem2()
        {
            string? fileLine;
            int partsSum = 0;
            List<string> lines = new List<string>();
            Regex digitRegex = new Regex(@"\d+");
            Regex gearRegex = new Regex(@"\*");

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((fileLine = streamReader.ReadLine()) != null)
                {
                    lines.Add(fileLine);
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                MatchCollection gearMatches = gearRegex.Matches(line);

                foreach (Match match in gearMatches)
                {
                    int gearIndex = match.Index;

                    Dictionary<int, List<int>> adjacencies = new Dictionary<int, List<int>>()
                    {
                        { i, new List<int>() { gearIndex - 1, gearIndex + 1 }}
                    };

                    if (i > 0)
                    {
                        List<int> prevRowIndexes = new List<int>() { gearIndex, gearIndex - 1, gearIndex + 1 };
                        adjacencies.Add(i - 1, prevRowIndexes);
                    }

                    if (i < lines.Count - 1)
                    {
                        List<int> nextRowIndexes = new List<int>() { gearIndex, gearIndex - 1, gearIndex + 1 };
                        adjacencies.Add(i + 1, nextRowIndexes);
                    }

                    List<int> adjNums = new List<int>();
                    Dictionary<int, List<int>> consumedIndexes = new Dictionary<int, List<int>>();

                    foreach (KeyValuePair<int, List<int>> kvp in adjacencies)
                    {
                        consumedIndexes.Add(kvp.Key, new List<int>());
                        var lineKey = kvp.Key;
                        List<int> adjacentIndexes = kvp.Value;
                        foreach(var adjancencyIndex in adjacentIndexes)
                        {
                            Match dMatch = digitRegex.Match(Convert.ToString(lines[lineKey][adjancencyIndex]));
                            if (dMatch.Success)
                            {
                                string numStr = "";
                                int tmpAdjIndex = adjancencyIndex;
                                if (tmpAdjIndex == gearIndex)
                                {
                                    string readRight = "";
                                    string readLeft = "";
                                    List<int> leftI = new List<int>();
                                    List<int> rightI = new List<int>();
                                    char thisChar = lines[lineKey][tmpAdjIndex];

                                    
                                    while (tmpAdjIndex > -1 && tmpAdjIndex < line.Length
                                        && digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Success)
                                    {
                                        readRight += digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Value;
                                        rightI.Add(tmpAdjIndex);
                                        tmpAdjIndex++;
                                    }
                                    tmpAdjIndex = adjancencyIndex;
                                    while (tmpAdjIndex > -1 && tmpAdjIndex < line.Length
                                        && digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Success)
                                    {
                                        readLeft += digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Value;
                                        leftI.Add(tmpAdjIndex);
                                        tmpAdjIndex--;
                                    }
                                    char[] numStrArr = readLeft.ToCharArray();
                                    Array.Reverse(numStrArr);
                                    readLeft = new string(numStrArr);

                                    if(tmpAdjIndex > -1 && tmpAdjIndex < line.Length
                                        && digitRegex.Match(Convert.ToString(thisChar)).Success
                                        && readLeft.Length > 1 && readRight.Length > 1)
                                    {
                                        numStr = readLeft.Substring(0, readLeft.Length - 1) + readRight;
                                        adjNums.Add(Convert.ToInt32(numStr));
                                        foreach (var leftindex in leftI)
                                        {
                                            consumedIndexes[lineKey].Add(leftindex);
                                        }
                                        foreach (var rightindex in rightI)
                                        {
                                            consumedIndexes[lineKey].Add(rightindex);
                                        }
                                    }
                                    else
                                    {
                                        if (readLeft.Length > readRight.Length)
                                        {
                                            numStr = readLeft;

                                            if (!consumedIndexes[lineKey].Intersect(leftI).Any())
                                            {
                                                adjNums.Add(Convert.ToInt32(numStr));
                                                foreach (var leftindex in leftI)
                                                {
                                                    consumedIndexes[lineKey].Add(leftindex);
                                                }
                                            }


                                        }
                                        else
                                        {
                                            numStr = readRight;
                                            if (!consumedIndexes[lineKey].Intersect(rightI).Any())
                                            {
                                                adjNums.Add(Convert.ToInt32(numStr));
                                                foreach (var rightIndex in rightI)
                                                {
                                                    consumedIndexes[lineKey].Add(rightIndex);
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                                else if (tmpAdjIndex > gearIndex)
                                {
                                    List<int> rightI = new List<int>();
                                    while (tmpAdjIndex > -1 && tmpAdjIndex < line.Length
                                        && digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Success)
                                    {
                                        numStr += digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Value;
                                        rightI.Add(tmpAdjIndex);
                                        tmpAdjIndex++;
                                    }

                                    if (!consumedIndexes[lineKey].Intersect(rightI).Any())
                                    {
                                        adjNums.Add(Convert.ToInt32(numStr));
                                        foreach (var rightIndex in rightI)
                                        {
                                            consumedIndexes[lineKey].Add(rightIndex);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    List<int> leftI = new List<int>();
                                    while (tmpAdjIndex > -1 && tmpAdjIndex < line.Length
                                        && digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Success)
                                    {
                                        numStr += digitRegex.Match(Convert.ToString(lines[lineKey][tmpAdjIndex])).Value;
                                        leftI.Add(tmpAdjIndex);
                                        tmpAdjIndex--;
                                    }
                                    char[] numStrArr = numStr.ToCharArray();
                                    Array.Reverse(numStrArr);

                                    if (!consumedIndexes[lineKey].Intersect(leftI).Any())
                                    {
                                        adjNums.Add(Convert.ToInt32(new string(numStrArr)));
                                        foreach (var leftIndex in leftI)
                                        {
                                            consumedIndexes[lineKey].Add(leftIndex);
                                        }
                                    }
                                        
                                }

                                
                            }
                        }
                    }

                    if(adjNums.Count == 2)
                    {
                        partsSum += (adjNums[0] * adjNums[1]);
                    }
                }
            }

            return partsSum;
        }

    }
}

