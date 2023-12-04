using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day1 : IDay
    {
        private int DayNum = 1;
        private string InputFile;
        private Dictionary<string, int> Numbers = new Dictionary<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 }
            };

        public Day1()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}.txt";
        }

        public int Problem1()
        {
            int sum = 0;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                String? line;
                int n;

                while ((line = streamReader.ReadLine()) != null)
                {
                    string lineResult = String.Empty;
                    foreach (char c in line)
                    {
                        if (int.TryParse(c.ToString(), out n))
                        {
                            lineResult += c;
                            break;
                        }
                    }
                    foreach (char c in line.Reverse())
                    {
                        if (int.TryParse(c.ToString(), out n))
                        {
                            lineResult += c;
                            break;
                        }
                    }

                    sum += Convert.ToInt32(lineResult);
                }
            }

            return sum;
        }

        public int Problem2()
        {
            int sum = 0;
            
            // > 53706
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                String? line;
                int n;

                while ((line = streamReader.ReadLine()) != null)
                {
                    int firstNumberIndex = int.MaxValue;
                    int lastNumberIndex = int.MinValue;
                    bool firstNumberIsDigit = false;
                    bool lastNumberIsDigit = false;
                    KeyValuePair<string, int> firstItem = new KeyValuePair<string, int>();
                    KeyValuePair<string, int> lastItem = new KeyValuePair<string, int>();


                    foreach (var kvp in Numbers)
                    {
                        if (line.Contains(kvp.Key))
                        {
                            if (line.IndexOf(kvp.Key) < firstNumberIndex)
                            {
                                firstNumberIndex = line.IndexOf(kvp.Key);
                                firstItem = kvp;
                            }

                            if (line.LastIndexOf(kvp.Key) > lastNumberIndex)
                            {
                                lastNumberIndex = line.LastIndexOf(kvp.Key);
                                lastItem = kvp;
                            }
                        }
                    }

                    for(int i = 0; i < line.Length; i++)
                    {
                        string charAtIndex = Convert.ToString(line[i]);

                        if (int.TryParse(charAtIndex.ToString(), out n))
                        {
                            if(i < firstNumberIndex)
                            {
                                firstNumberIndex = i;
                                firstNumberIsDigit = true;
                                
                            }
                            if(i > lastNumberIndex)
                            {
                                lastNumberIndex = i;
                                lastNumberIsDigit = true;
                            }
                        }
                    }

                    string firstNum, lastNum;

                    if (firstNumberIsDigit)
                    {
                        firstNum = Convert.ToString(line[firstNumberIndex]);
                    }
                    else
                    {
                        firstNum = Convert.ToString(firstItem.Value);
                    }

                    if (lastNumberIsDigit)
                    {
                        lastNum = Convert.ToString(line[lastNumberIndex]);
                    }
                    else
                    {
                        lastNum = Convert.ToString(lastItem.Value);
                    }


                    string lineResult = $"{firstNum}{lastNum}";
                    //Console.WriteLine($"{line}\tRESULT: {lineResult}");
                    sum += Convert.ToInt32(lineResult);
                }
            }

            return sum;
        }

    }
}
