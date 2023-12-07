using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
	public class Day2 : IDay
	{
		private int DayNum = 2;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day2()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
		{
			int validGameSum = 0;
			int redCubeCount = 12;
			int greenCubeCount = 13;
			int blueCubeCount = 14;
			string? line;

            using (var fileStream = File.OpenRead($"{InputFile}"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
			{
                Regex digitRegex = new Regex(@"\d+");

                while ((line = streamReader.ReadLine()) != null)
				{
					bool gameIsValid = true;
					string[] lineGameGroups = line.Split(':');

					string gameSubstring = lineGameGroups[0];
					Match match = digitRegex.Match(gameSubstring);
					int gameNum = Convert.ToInt32(match.Value);

					string[] drawGroups = lineGameGroups[1].Split(';');

					foreach(string group in drawGroups)
					{
						if (!gameIsValid)
						{
							break;
						}
						string[] draws = group.Split(',');

						foreach(string drawColorCount in draws)
						{
							int colorCount = Convert.ToInt32(digitRegex.Match(drawColorCount).Value);

                            if (drawColorCount.Contains("red"))
							{
								if(colorCount > redCubeCount)
								{
									gameIsValid = false;
									break;
								}
							}
							else if (drawColorCount.Contains("green"))
							{
                                if (colorCount > greenCubeCount)
                                {
                                    gameIsValid = false;
									break;
                                }
                            }
							else if (drawColorCount.Contains("blue"))
							{
                                if (colorCount > blueCubeCount)
                                {
                                    gameIsValid = false;
									break;
                                }
                            }
						}
                    }

					if (gameIsValid)
					{
						validGameSum += gameNum;
					}
                }

            }

			return validGameSum;
        }

        public int Problem2()
        {
            int powerSum = 0;
            
            string? line;

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                Regex digitRegex = new Regex(@"\d+");

                while ((line = streamReader.ReadLine()) != null)
                {
                    int minRedCubeCountRequired = 1;
                    int minGreenCubeCountRequired = 1;
                    int minBlueCubeCountRequired = 1;
                    string[] lineGameGroups = line.Split(':');

                    //string gameSubstring = lineGameGroups[0];
                    //Match match = digitRegex.Match(gameSubstring);
                    //int gameNum = Convert.ToInt32(match.Value);

                    string[] drawGroups = lineGameGroups[1].Split(';');

                    foreach (string group in drawGroups)
                    {
                        string[] draws = group.Split(',');

                        foreach (string drawColorCount in draws)
                        {
                            int colorCount = Convert.ToInt32(digitRegex.Match(drawColorCount).Value);

                            if (drawColorCount.Contains("red"))
                            {
                                if (colorCount > minRedCubeCountRequired)
                                {
                                    minRedCubeCountRequired = colorCount;
                                }
                            }
                            else if (drawColorCount.Contains("green"))
                            {
                                if (colorCount > minGreenCubeCountRequired)
                                {
                                    minGreenCubeCountRequired = colorCount;
                                }
                            }
                            else if (drawColorCount.Contains("blue"))
                            {
                                if (colorCount > minBlueCubeCountRequired)
                                {
                                    minBlueCubeCountRequired = colorCount;
                                }
                            }
                        }
                    }

                    powerSum += (minRedCubeCountRequired * minGreenCubeCountRequired * minBlueCubeCountRequired);
                    //Console.WriteLine(minRedCubeCountRequired * minGreenCubeCountRequired * minBlueCubeCountRequired);
                }

            }

            return powerSum;
        }
    }
}

