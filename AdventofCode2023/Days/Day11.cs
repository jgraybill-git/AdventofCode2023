using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day11 : IDayDbl
    {
        private int DayNum = 11;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day11()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public double Problem1()
        {
            List<List<char>> inputImage = new List<List<char>>();
            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputImage.Add(line.ToCharArray().ToList());
                }
            }

            List<List<char>> expandedGalaxies = new List<List<char>>();

            for(int row = 0; row < inputImage.Count; row++)
            {
                expandedGalaxies.Add(inputImage[row].ToList());
                if (inputImage[row].All(x => x == '.'))
                {
                    expandedGalaxies.Add(inputImage[row].ToList());
                }
            }
            for (int col = 0; col < expandedGalaxies.ElementAt(0).Count; col++)
            {
                var columnn = Enumerable.Range(0, expandedGalaxies.Count).Select(x => expandedGalaxies[x][col]).ToList();

                if (columnn.All(x => x == '.'))
                {
                    expandedGalaxies.ForEach(x => { x.Insert(col + 1, '.'); });
                    col++;
                }
            }

            List<Tuple<int, int>> galaxies = new List<Tuple<int, int>>();
            for(int row = 0; row < expandedGalaxies.Count; row++)
            {
                for (int col = 0; col < expandedGalaxies[row].Count; col++)
                {
                    if (expandedGalaxies[row][col] == '#')
                    {
                        galaxies.Add(new Tuple<int, int>(row, col));
                    }
                }
            }

            List<int> distances = new List<int>();
            for (int gal1 = 0; gal1 < galaxies.Count; gal1++)
            {
                var galaxy1 = galaxies[gal1];
                for (int gal2 = gal1 + 1; gal2 < galaxies.Count; gal2++)
                {
                    var galaxy2 = galaxies[gal2];
                    distances.Add(Math.Abs(galaxy2.Item1 - galaxy1.Item1)
                        + Math.Abs(galaxy2.Item2 - galaxy1.Item2));
                }
            }


            return distances.Sum();
        }

        public double Problem2()
        {
            List<List<char>> inputImage = new List<List<char>>();
            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputImage.Add(line.ToCharArray().ToList());
                }
            }
 
            List<Tuple<int, int>> galaxies = new List<Tuple<int, int>>();
            List<int> emptySpaceRows = new List<int>();
            List<int> emptySpaceCols = new List<int>();

            for (int row = 0; row < inputImage.Count; row++)
            {
                if(OrientationIsEmptySpace(Orientation.Horizontal, row, inputImage))
                {
                    emptySpaceRows.Add(row);
                }

                for (int col = 0; col < inputImage[row].Count; col++)
                {
                    if (inputImage[row][col] == '#')
                    {
                        galaxies.Add(new Tuple<int, int>(row, col));
                    }

                    if (!emptySpaceCols.Contains(col))
                    {
                        if (OrientationIsEmptySpace(Orientation.Vertical, col, inputImage))
                        {
                            emptySpaceCols.Add(col);
                        }
                    }
                }
            }

            int scaleFactor = 1000000;
            List<double> distances = new List<double>();

            for (int gal1 = 0; gal1 < galaxies.Count; gal1++)
            {
                var galaxy1 = galaxies[gal1];
                for (int gal2 = gal1 + 1; gal2 < galaxies.Count; gal2++)
                {
                    var deltaX = 0;
                    var deltaY = 0;
                    var galaxy2 = galaxies[gal2];

                    if (galaxy2.Item1 > galaxy1.Item1)
                    {
                        for (int row = galaxy1.Item1; row < galaxy2.Item1; row++)
                        {
                            deltaX += emptySpaceRows.Contains(row) ? scaleFactor : 1;
                        }
                    }
                    else
                    {
                        for (int row = galaxy1.Item1; row > galaxy2.Item1; row--)
                        {
                            deltaX += emptySpaceRows.Contains(row) ? scaleFactor : 1;
                        }
                    }

                    if (galaxy2.Item2 > galaxy1.Item2)
                    {
                        for (int col = galaxy1.Item2; col < galaxy2.Item2; col++)
                        {
                            deltaY += emptySpaceCols.Contains(col) ? scaleFactor : 1;
                        }
                    }
                    else
                    {
                        for (int col = galaxy1.Item2; col > galaxy2.Item2; col--)
                        {
                            deltaY += emptySpaceCols.Contains(col) ? scaleFactor : 1;
                        }
                    }

                    

                    //Console.WriteLine($"INDEX ({galaxy1.Item1}, {galaxy1.Item2}) => ({galaxy2.Item1}, {galaxy2.Item2}) = {deltaX + deltaY}");
                    distances.Add(deltaX + deltaY);
                }
            }

            return distances.Sum();
        }

        private bool OrientationIsEmptySpace(Orientation orientation, int index, List<List<char>> image)
        {
            if(orientation == Orientation.Horizontal)
            {
                return image[index].All(x => x == '.');
            }
            else if(orientation == Orientation.Vertical)
            {
                return Enumerable.Range(0, image.Count).Select(x => image[x][index]).All(x => x == '.');
            }

            return false;
        }

        private enum Orientation
        {
            Horizontal,
            Vertical
        }
    }
}

