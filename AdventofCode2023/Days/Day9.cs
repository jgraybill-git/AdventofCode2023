using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day9 : IDay
    {
        private int DayNum = 9;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day9()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            int sequenceResultSum = 0;
            Regex intRegex = new Regex(@"-?\d+");

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    MatchCollection lineNumbers = intRegex.Matches(line);

                    List<Tuple<int, int[]>> layers = new List<Tuple<int, int[]>>();
                    int layer = 1;
                    int seqResult;
                    int[] currentVals = lineNumbers.Select(x => Convert.ToInt32(x.Value)).ToArray();
                    int[] nextVals = new int[currentVals.Length - 1];
                    layers.Add(new Tuple<int, int[]>(layer, currentVals));

                    while(!currentVals.All(x => x == 0))
                    {
                        layer++;
                        for (int i = 0; i < currentVals.Length - 1; i++)
                        {
                            nextVals[i] = currentVals[i + 1] - currentVals[i];
                        }
                        currentVals = nextVals;
                        nextVals = new int[currentVals.Length - 1];
                        layers.Add(new Tuple<int, int[]>(layer, currentVals));
                    }

                    int layerResult = 0;

                    for(int i = layers.Count - 2; i >= 0; i--)
                    {
                        var currentLayer = layers[i];
                        layerResult = currentLayer.Item2[currentLayer.Item2.Length - 1] + layerResult;
                        
                    }

                    sequenceResultSum += layerResult;

                }
            }

            return sequenceResultSum;
        }

        public int Problem2()
        {
            string? line;
            int sequenceResultSum = 0;
            Regex intRegex = new Regex(@"-?\d+");

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    MatchCollection lineNumbers = intRegex.Matches(line);

                    List<Tuple<int, int[]>> layers = new List<Tuple<int, int[]>>();
                    int layer = 1;
                    int seqResult;
                    int[] currentVals = lineNumbers.Select(x => Convert.ToInt32(x.Value)).ToArray();
                    int[] nextVals = new int[currentVals.Length - 1];
                    layers.Add(new Tuple<int, int[]>(layer, currentVals));

                    while (!currentVals.All(x => x == 0))
                    {
                        layer++;
                        for (int i = 0; i < currentVals.Length - 1; i++)
                        {
                            nextVals[i] = currentVals[i + 1] - currentVals[i];
                        }
                        currentVals = nextVals;
                        nextVals = new int[currentVals.Length - 1];
                        layers.Add(new Tuple<int, int[]>(layer, currentVals));
                    }

                    int layerResult = 0;

                    for (int i = layers.Count - 2; i >= 0; i--)
                    {
                        var currentLayer = layers[i];
                        layerResult = currentLayer.Item2[0] - layerResult;

                    }

                    sequenceResultSum += layerResult;
                }
            }

            return sequenceResultSum;
        }
    }
}

