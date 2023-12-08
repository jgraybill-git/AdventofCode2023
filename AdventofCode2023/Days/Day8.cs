using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day8 : IDayDbl
    {
        private int DayNum = 8;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day8()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public double Problem1()
        {
            string? line;
            string directions = String.Empty;
            List<Node> map = new List<Node>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                bool firstLine = true;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if(firstLine)
                    {
                        directions = line;
                        firstLine = false;
                        continue;
                    }

                    string[] mappingParts = line.Split('=');
                    string locationName = mappingParts[0].Trim();
                    string[] adjacencies = mappingParts[1].Split(',');
                    string leftAdj = adjacencies[0].Replace("(", String.Empty).Trim();
                    string rightAdj = adjacencies[1].Replace(")", String.Empty).Trim();
                    
                    map.Add(new Node(locationName, leftAdj, rightAdj));
                    
                }
            }

            string destination = "ZZZ";
            Node currNode = map.Where(x => x.Label == "AAA").First();
            int directionIndex = 0;
            int stepCount = 0;


            while (destination != currNode.Label)
            {
                if (directionIndex == directions.Length)
                {
                    directionIndex = 0;
                }

                char direction = directions[directionIndex];

                currNode = direction == 'L'
                    ? map.Where(x => x.Label == currNode.LeftNodeLabel).First()
                    : map.Where(x => x.Label == currNode.RightNodeLabel).First();
                directionIndex++;
                stepCount++;

            }

            return stepCount;
        }

        public double Problem2()
        {
            string? line;
            string directions = String.Empty;
            List<Node> map = new List<Node>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                bool firstLine = true;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if (firstLine)
                    {
                        directions = line;
                        firstLine = false;
                        continue;
                    }

                    string[] mappingParts = line.Split('=');
                    string locationName = mappingParts[0].Trim();
                    string[] adjacencies = mappingParts[1].Split(',');
                    string leftAdj = adjacencies[0].Replace("(", String.Empty).Trim();
                    string rightAdj = adjacencies[1].Replace(")", String.Empty).Trim();

                    map.Add(new Node(locationName, leftAdj, rightAdj));

                }
            }

            char destinationEnd = 'Z';
            List<Node> currNodes = map.Where(x => x.Label.Last() == 'A').ToList();
            List<int> cycleCount = new List<int>();
            int directionIndex = 0;
            

            foreach(var node in currNodes)
            {
                int stepCount = 0;
                var iterationNode = node;
                while (iterationNode.Label.Last() != destinationEnd)
                {
                    if (directionIndex == directions.Length)
                    {
                        directionIndex = 0;
                    }

                    char direction = directions[directionIndex];

                    iterationNode = direction == 'L'
                        ? map.Where(x => x.Label == iterationNode.LeftNodeLabel).First()
                        : map.Where(x => x.Label == iterationNode.RightNodeLabel).First();

                    directionIndex++;
                    stepCount++;

                }
                cycleCount.Add(stepCount);
            }

            cycleCount = cycleCount.OrderBy(x => x).ToList();
            double convergedSteps = 0;
                
            for (int i = 0; i < cycleCount.Count - 1; i++)
            {
                if(i == 0)
                {
                    convergedSteps = ConvergeSteps(Convert.ToDouble(cycleCount.ElementAt(i)),
                        Convert.ToDouble(cycleCount.ElementAt(i + 1)));
                }
                else
                {
                    convergedSteps = ConvergeSteps(convergedSteps, Convert.ToDouble(cycleCount.ElementAt(i + 1)));
                }
            }

            return convergedSteps;
        }

        private double ConvergeSteps(double stepCount1, double stepCount2)
        {
            double max = Math.Max(stepCount1, stepCount2);
            double min = Math.Min(stepCount1, stepCount2);
            double runningMax = max;
            double runningMin = min;

            while(runningMin != runningMax)
            {
                while (runningMin < runningMax)
                {
                    runningMin += min;
                }

                if(runningMin > runningMax)
                {
                    runningMax += max;
                }
            }

            return runningMin;
        }

        public class Node
        {
            public string Label;
            public string LeftNodeLabel;
            public string RightNodeLabel;

            public Node(string label, string left, string right)
            {
                Label = label;
                LeftNodeLabel = left;
                RightNodeLabel = right;
            }
        }
    }

}

