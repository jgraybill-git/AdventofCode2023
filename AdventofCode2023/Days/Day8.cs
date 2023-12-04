using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day8 : IDay
    {
        private int DayNum = 8;
        private string InputFile;

        public Day8()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}.txt";
        }

        public int Problem1()
        {
            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {

                }
            }

            return 0;
        }

        public int Problem2()
        {
            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {

                }
            }

            return 0;
        }
    }
}

