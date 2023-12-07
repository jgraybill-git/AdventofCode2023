using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day6 : IDay
    {
        private int DayNum = 6;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day6()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            Regex numbers = new Regex(@"\d+");
            List<int> times = new List<int>();
            List<int> distances = new List<int>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(':');
                    MatchCollection mc = numbers.Matches(lineParts[1]);
                    if (lineParts[0] == "Time")
                    {
                        foreach(Match match in mc)
                        {
                            times.Add(Convert.ToInt32(match.Value));
                        }
                    }
                    else
                    {
                        foreach (Match match in mc)
                        {
                            distances.Add(Convert.ToInt32(match.Value));
                        }
                    }
                }
            }

            List<int> winScenarios = new List<int>();
            foreach (var time in times)
            {
                winScenarios.Add(0);
            }

            for (int i = 0; i < times.Count; i++)
            {
                var raceTime = times[i];
                var recordDistance = distances[i];

                for (int j = 1; j < raceTime; j++)
                {
                    var holdBtn = j;
                    var travelTime = raceTime - holdBtn;
                    var travelDistance = holdBtn * travelTime;
                    if (travelDistance > recordDistance)
                    {
                        winScenarios[i]++;
                    }
                }
            }

            int product = 1;
            foreach(var item in winScenarios)
            {
                product *= item;
            }
            return product;
        }

        public int Problem2()
        {
            string? line;
            Regex numbers = new Regex(@"\d+");
            List<int> times = new List<int>();
            List<int> distances = new List<int>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] lineParts = line.Split(':');
                    MatchCollection mc = numbers.Matches(lineParts[1]);
                    if (lineParts[0] == "Time")
                    {
                        foreach (Match match in mc)
                        {
                            times.Add(Convert.ToInt32(match.Value));
                        }
                    }
                    else
                    {
                        foreach (Match match in mc)
                        {
                            distances.Add(Convert.ToInt32(match.Value));
                        }
                    }
                }
            }

            int winScenarioCount = 0;
            var raceTime = Convert.ToDouble(String.Join(' ', times).Replace(" ", String.Empty).Trim());
            var recordDistance = Convert.ToDouble(String.Join(' ', distances).Replace(" ", String.Empty).Trim());

            for (double i = 1; i < raceTime; i++)
            {
                var holdBtn = i;
                var travelTime = raceTime - holdBtn;
                var travelDistance = holdBtn * travelTime;
                if (travelDistance > recordDistance)
                {
                    winScenarioCount++;
                }
            }

            return winScenarioCount;
        }
    }
}

