using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day15 : IDay
    {
        private int DayNum = 15;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day15()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            List<string> initializationSequence = new List<string>();
            List<int> initializationSequenceHashes = new List<int>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    initializationSequence.AddRange(line.Split(','));
                }
            }

            
            foreach(var sequenceItem in initializationSequence)
            {
                int currentValue = 0;
                foreach (var c in sequenceItem)
                {
                    currentValue = Hash(currentValue, c);
                }
                initializationSequenceHashes.Add(currentValue);
            }


            return initializationSequenceHashes.Sum();
        }

        public int Problem2()
        {
            string? line;
            List<string> initializationSequence = new List<string>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    initializationSequence.AddRange(line.Split(','));
                }
            }

            Dictionary<int, List<string>> boxes = new Dictionary<int, List<string>>();

            foreach(int boxNum in Enumerable.Range(0, 256))
            {
                boxes.Add(boxNum, new List<string>());
            }

            foreach (var sequenceStep in initializationSequence)
            {
                int boxNumber = 0;
                string stepLabel = String.Empty;
                bool placeLens = false;
                int lensFocalLength = 0;

                if (sequenceStep.Contains('='))
                {
                    stepLabel = sequenceStep.Split('=')[0];
                    placeLens = true;
                    lensFocalLength = Convert.ToInt32(sequenceStep.Split('=')[1]);
                }
                else
                {
                    stepLabel = sequenceStep.Split('-')[0];
                }


                foreach (var c in stepLabel)
                {
                    boxNumber = Hash(boxNumber, c);
                }

                List<string> boxContents = boxes[boxNumber];

                if (placeLens)
                {
                    string lensLabel = $"{stepLabel} {lensFocalLength}";
                    int replaceIndex = -1;

                    for(int i = 0; i < boxContents.Count; i++)
                    {
                        if (boxContents[i].Split(' ')[0] == stepLabel)
                        {
                            replaceIndex = i;
                        }
                    }

                    if(replaceIndex != -1)
                    {
                        boxContents[replaceIndex] = lensLabel;
                    }
                    else
                    {
                        boxContents.Add(lensLabel);
                    }
                }
                else
                {
                    for (int i = 0; i < boxContents.Count; i++)
                    {
                        if (boxContents[i].Split(' ')[0] == stepLabel)
                        {
                            boxContents.RemoveAt(i);
                        }
                    }
                }
            }

            List<int> lensFocusingPowers = new List<int>();

            foreach(var box in boxes)
            {
                int boxNumAddOne = box.Key + 1;

                for(int i = 0; i < box.Value.Count; i++)
                {
                    int slot = i + 1;
                    int lensFocalLength = Convert.ToInt32(box.Value[i].Split(' ')[1]);

                    lensFocusingPowers.Add(boxNumAddOne * slot * lensFocalLength);
                }
            }

           

            return lensFocusingPowers.Sum();
        }

        private int Hash(int currentValue, char sequenceItemChar)
        {
            int charAscii = sequenceItemChar;
            currentValue += charAscii;
            currentValue *= 17;
            currentValue %= 256;
            return currentValue;
        }
    }
}

