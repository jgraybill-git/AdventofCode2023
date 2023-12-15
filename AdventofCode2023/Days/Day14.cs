using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day14 : IDay
    {
        private int DayNum = 14;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day14()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            int totalLoad = 0;
            List<List<char>> input = new List<List<char>>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    input.Add(new List<char>(line.ToCharArray()));
                }
            }

            char[,] originalPlatform = new char[input.Count, input.ElementAt(0).Count];

            for(int i = 0; i < originalPlatform.GetLength(0); i++)
            {
                for (int j = 0; j < originalPlatform.GetLength(1); j++)
                {
                    originalPlatform[i, j] = input.ElementAt(i).ElementAt(j);
                }
            }

            char[,] northShiftedPlatform = originalPlatform.Clone() as char[,];
            
            for (int col = 0; col < northShiftedPlatform.GetLength(1); col++)
            {
                for (int row = 0; row < northShiftedPlatform.GetLength(0); row++)
                {
                    char platformSpace = northShiftedPlatform[row, col];

                    if(platformSpace == '.')
                    {
                        for(int tmpRow = row + 1; tmpRow < northShiftedPlatform.GetLength(0); tmpRow++)
                        {
                            if(northShiftedPlatform[tmpRow, col] == '#')
                            {
                                break;
                            }
                            else if(northShiftedPlatform[tmpRow, col] == 'O')
                            {
                                northShiftedPlatform[row, col] = 'O';
                                northShiftedPlatform[tmpRow, col] = '.';
                                break;
                            }
                        }
                    }
                }
            }

            int rockLoadAtLayer = 1;
            for(int i = northShiftedPlatform.GetLength(0) - 1; i >= 0; i--)
            {
                for(int j = 0; j < northShiftedPlatform.GetLength(1); j++)
                {
                    if (northShiftedPlatform[i,j] == 'O')
                    {
                        totalLoad += (rockLoadAtLayer);
                    }
                }
                rockLoadAtLayer++;
            }

            //PrintArray(northShiftedPlatform);
            return totalLoad;
        }

        public int Problem2()
        {
            string? line;
            int totalLoad = 0;
            List<List<char>> input = new List<List<char>>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    input.Add(new List<char>(line.ToCharArray()));
                }
            }

            char[,] originalPlatform = new char[input.Count, input.ElementAt(0).Count];

            for (int i = 0; i < originalPlatform.GetLength(0); i++)
            {
                for (int j = 0; j < originalPlatform.GetLength(1); j++)
                {
                    originalPlatform[i, j] = input.ElementAt(i).ElementAt(j);
                }
            }

            int cycleCount = 1000000000;

            char[,] directionShiftedPlatform = originalPlatform.Clone() as char[,];

            int rowCount = directionShiftedPlatform.GetLength(0);
            int colCount = directionShiftedPlatform.GetLength(1);
            List<char[,]> knownPlatformStates = new List<char[,]>() { directionShiftedPlatform };
            bool previousCycleFound = false;
            int cyclesRan = 0;

            while(cycleCount > 0)
            {
                directionShiftedPlatform = directionShiftedPlatform.Clone() as char[,];

                for (int cycleDir = 0; cycleDir < 4; cycleDir++)
                {
                    bool northSouth = cycleDir % 2 == 0;
                    int outerLoop = northSouth ? colCount : rowCount;
                    int innerLoop = northSouth ? rowCount : colCount;

                    if(cycleDir == 0 || cycleDir == 1)
                    {
                        for (int i = 0; i < outerLoop; i++)
                        {
                            for (int j = 0; j < innerLoop; j++)
                            {
                                char platformSpace = directionShiftedPlatform[i, j];

                                if (platformSpace == '.')
                                {
                                    if (cycleDir == 0)
                                    {
                                        for (int tmp = i + 1; tmp < rowCount; tmp++)
                                        {
                                            if (directionShiftedPlatform[tmp, j] == '#')
                                            {
                                                break;
                                            }
                                            else if (directionShiftedPlatform[tmp, j] == 'O')
                                            {
                                                directionShiftedPlatform[i, j] = 'O';
                                                directionShiftedPlatform[tmp, j] = '.';
                                                break;
                                            }
                                        }
                                    }
                                    else if (cycleDir == 1)
                                    {
                                        for (int tmp = j + 1; tmp < colCount; tmp++)
                                        {
                                            if (directionShiftedPlatform[i, tmp] == '#')
                                            {
                                                break;
                                            }
                                            else if (directionShiftedPlatform[i, tmp] == 'O')
                                            {
                                                directionShiftedPlatform[i, j] = 'O';
                                                directionShiftedPlatform[i, tmp] = '.';
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        for (int i = rowCount - 1; i >= 0; i--)
                        {
                            for (int j = colCount - 1; j >= 0; j--)
                            {
                                char platformSpace = directionShiftedPlatform[i, j];

                                if (platformSpace == '.')
                                {
                                    if (cycleDir == 2)
                                    {
                                        for (int tmp = i - 1; tmp >= 0; tmp--)
                                        {
                                            if (directionShiftedPlatform[tmp, j] == '#')
                                            {
                                                break;
                                            }
                                            else if (directionShiftedPlatform[tmp, j] == 'O')
                                            {
                                                directionShiftedPlatform[i, j] = 'O';
                                                directionShiftedPlatform[tmp, j] = '.';
                                                break;
                                            }
                                        }
                                    }
                                    else if (cycleDir == 3)
                                    {
                                        for (int tmp = j - 1; tmp >= 0; tmp--)
                                        {
                                            if (directionShiftedPlatform[i, tmp] == '#')
                                            {
                                                break;
                                            }
                                            else if (directionShiftedPlatform[i, tmp] == 'O')
                                            {
                                                directionShiftedPlatform[i, j] = 'O';
                                                directionShiftedPlatform[i, tmp] = '.';
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    
                }

                cycleCount--;
                cyclesRan++;

                if (!previousCycleFound)
                {
                    foreach (var platform in knownPlatformStates)
                    {
                        if (PlatformsEqual(platform, directionShiftedPlatform))
                        {
                            Console.WriteLine(knownPlatformStates.IndexOf(platform));
                            Console.WriteLine(cyclesRan);
                            int ind = knownPlatformStates.IndexOf(platform);
                            previousCycleFound = true;
                            while (cycleCount > cyclesRan - ind)
                            {
                                cycleCount -= (cyclesRan - ind);
                            }
                        }
                    }

                    knownPlatformStates.Add(directionShiftedPlatform);
                }
            }


            int rockLoadAtLayer = 1;
            for (int i = rowCount - 1; i >= 0; i--)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (directionShiftedPlatform[i, j] == 'O')
                    {
                        totalLoad += (rockLoadAtLayer);
                    }
                }
                rockLoadAtLayer++;
            }


            return totalLoad;
        }

        private void PrintArray(char[,] a)
        {
            Console.WriteLine();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write(a[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private bool PlatformsEqual(char[,] a, char[,] b)
        {
            int aRowLen = a.GetLength(0);
            int bRowLen = b.GetLength(0);
            int aColLen = a.GetLength(1);
            int bColLen = b.GetLength(1);

            if(aRowLen != bRowLen || aColLen != bColLen)
            {
                return false;
            }

            for (int i = 0; i < aRowLen; i++)
            {
                for (int j = 0; j < aColLen; j++)
                {
                    if (a[i, j] != b[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

