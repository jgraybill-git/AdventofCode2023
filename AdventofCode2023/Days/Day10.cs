using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day10 : IDay
    {
        private int DayNum = 10;
        private string InputFile;
        private bool UseTestingFile = true;

        public Day10()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            List<List<char>> map = new List<List<char>>();
            Tuple<int, int> startPos = new Tuple<int, int>(-1, -1);
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                int fileRowNum = 0;
                int fileColNum;

                while ((line = streamReader.ReadLine()) != null)
                {
                    fileColNum = 0;
                    List<char> lineChars = new List<char>();
                    foreach(char c in line)
                    {
                        lineChars.Add(c);

                        if(c == 'S')
                        {
                            startPos = new Tuple<int, int>(fileRowNum, fileColNum);
                        }
                        fileColNum++;
                    }
                    map.Add(lineChars);
                    fileRowNum++;
                }
            }

            List<Tuple<int, int>> nextPathTiles = new List<Tuple<int, int>>();
            var prevPathOneTile = startPos;
            var prevPathTwoTile = startPos;
            int distFromStart = 1;

            InitPathTiles(startPos, ref nextPathTiles, ref map);
            var pathOneTile = nextPathTiles[0];
            var pathTwoTile = nextPathTiles[1];

            while (!TuplesEqual(pathOneTile, pathTwoTile))
            {
                
                var newPathOneTile = FindNextPathTile(pathOneTile, prevPathOneTile, ref map);
                var newPathTwoTile = FindNextPathTile(pathTwoTile, prevPathTwoTile, ref map);

                prevPathOneTile = pathOneTile;
                prevPathTwoTile = pathTwoTile;
                pathOneTile = newPathOneTile;
                pathTwoTile = newPathTwoTile;

                distFromStart++;
            }
            return distFromStart;
        }

        public int Problem2()
        {
            string? line;
            List<List<char>> map = new List<List<char>>();
            Tuple<int, int> startPos = new Tuple<int, int>(-1, -1);
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                int fileRowNum = 0;
                int fileColNum;

                while ((line = streamReader.ReadLine()) != null)
                {
                    fileColNum = 0;
                    List<char> lineChars = new List<char>();
                    foreach (char c in line)
                    {

                        if (c == 'S')
                        {
                            startPos = new Tuple<int, int>(fileRowNum, fileColNum);
                        }

                        lineChars.Add(c);
                        fileColNum++;
                    }
                    map.Add(lineChars);
                    fileRowNum++;
                }
            }

            List<Tuple<int, int>> nextPathTiles = new List<Tuple<int, int>>();
            var prevPathOneTile = startPos;
            var prevPathTwoTile = startPos;


            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            List<Tuple<int, int>> pathForward = new List<Tuple<int, int>>();
            List<Tuple<int, int>> pathBack = new List<Tuple<int, int>>();
            pathForward.Add(startPos);

            InitPathTiles(startPos, ref nextPathTiles, ref map);
            var pathOneTile = nextPathTiles[0];
            var pathTwoTile = nextPathTiles[1];

            pathForward.Add(pathOneTile);
            pathBack.Add(pathTwoTile);

            while (!TuplesEqual(pathOneTile, pathTwoTile))
            {

                var newPathOneTile = FindNextPathTile(pathOneTile, prevPathOneTile, ref map);
                var newPathTwoTile = FindNextPathTile(pathTwoTile, prevPathTwoTile, ref map);

                pathForward.Add(newPathOneTile);
                pathBack.Add(newPathTwoTile);
                prevPathOneTile = pathOneTile;
                prevPathTwoTile = pathTwoTile;
                pathOneTile = newPathOneTile;
                pathTwoTile = newPathTwoTile;
            }

            path.AddRange(pathForward);
            path.RemoveAt(path.Count - 1);
            pathBack.Reverse();
            path.AddRange(pathBack);


            var nextChar = map[path.ElementAt(1).Item1][path.ElementAt(1).Item2];
            var lastChar = map[path.Last().Item1][path.Last().Item2];

            if ((lastChar == 'F' || lastChar == '7') && (nextChar == 'L' || nextChar == 'J'))
            {
                map[startPos.Item1][startPos.Item2] = '|';
            }
            if ((lastChar == 'F' || lastChar == 'L') && (nextChar == '-' || nextChar == 'J' || nextChar == '7'))
            {
                map[startPos.Item1][startPos.Item2] = '-';
            }
            if ((lastChar == 'F' || lastChar == '7' || lastChar == '|') && (nextChar == '-' || nextChar == 'F' || nextChar == 'L'))
            {
                map[startPos.Item1][startPos.Item2] = 'J';
            }
            if ((lastChar == 'L' || lastChar == 'F' || lastChar == '-') && (nextChar == '|' || nextChar == 'L' || nextChar == 'J'))
            {
                map[startPos.Item1][startPos.Item2] = '7';
            }
            if ((lastChar == '7' || lastChar == 'J' || lastChar == '-') && (nextChar == '|' || nextChar == 'L' || nextChar == 'J'))
            {
                map[startPos.Item1][startPos.Item2] = 'F';
            }
            if ((lastChar == '7' || lastChar == 'F' || lastChar == '|') && (nextChar == '-' || nextChar == '7' || nextChar == 'J'))
            {
                map[startPos.Item1][startPos.Item2] = 'L';
            }


            int enclosedTiles = 0;

            for (int row = 0; row < map.Count - 1; row++)
            {
                bool isEnclosed = false;
                for (int col = 0; col < map.ElementAt(0).Count - 1; col++)
                {
                    if (ListTupleContains(path, new Tuple<int, int>(row, col))
                        && new char[] { '|', 'L', 'J'}.Contains(map[row][col]))
                    {
                        isEnclosed = !isEnclosed;
                        continue;
                    }

                    if (isEnclosed && !ListTupleContains(path, new Tuple<int, int>(row, col)))
                    {
                        enclosedTiles++;
                    }

                }
            }

            return enclosedTiles;
        }


        private Tuple<int, int> FindNextPathTile(Tuple<int, int> pathTile, Tuple<int, int> prevPathTile, ref List<List<char>> map)
        {
            switch (map.ElementAt(pathTile.Item1).ElementAt(pathTile.Item2))
            {
                case '|':
                    if(prevPathTile.Item1 < pathTile.Item1)
                    {
                        return new Tuple<int, int>(pathTile.Item1 + 1, pathTile.Item2);
                    }
                    else
                    {
                        return new Tuple<int, int>(pathTile.Item1 - 1, pathTile.Item2);
                    }
                case '-':
                    if (prevPathTile.Item2 < pathTile.Item2)
                    {
                        return new Tuple<int, int>(pathTile.Item1, pathTile.Item2 + 1);
                    }
                    else
                    {
                        return new Tuple<int, int>(pathTile.Item1, pathTile.Item2 - 1);
                    }
                case 'L':
                    // Coming from north
                    if (prevPathTile.Item1 < pathTile.Item1)
                    {
                        return new Tuple<int, int>(pathTile.Item1, pathTile.Item2 + 1);
                    }
                    // Coming from east
                    else
                    {
                        return new Tuple<int, int>(pathTile.Item1 - 1, pathTile.Item2);
                    }
                case 'J':
                    // Coming from north
                    if (prevPathTile.Item1 < pathTile.Item1)
                    {
                        return new Tuple<int, int>(pathTile.Item1, pathTile.Item2 - 1);
                    }
                    // Coming from west
                    else
                    {
                        return new Tuple<int, int>(pathTile.Item1 - 1, pathTile.Item2);
                    }
                case '7':
                    // Coming from south
                    if (prevPathTile.Item1 > pathTile.Item1)
                    {
                        return new Tuple<int, int>(pathTile.Item1, pathTile.Item2 - 1);
                    }
                    // Coming from west
                    else
                    {
                        return new Tuple<int, int>(pathTile.Item1 + 1, pathTile.Item2);
                    }
                case 'F':
                    // Coming from south
                    if (prevPathTile.Item1 > pathTile.Item1)
                    {
                        return new Tuple<int, int>(pathTile.Item1, pathTile.Item2 + 1);
                    }
                    // Coming from east
                    else
                    {
                        return new Tuple<int, int>(pathTile.Item1 + 1, pathTile.Item2);
                    }
                default:
                    return new Tuple<int, int>(-1, -1);



            }
        }

        private void InitPathTiles(Tuple<int, int> pathTile, ref List<Tuple<int, int>> pathTiles, ref List<List<char>> map)
        {
            bool checkNorth = false;
            bool checkSouth = false;
            bool checkEast = false;
            bool checkWest = false;

            if (pathTile.Item1 > 0)
            {
                checkNorth = true;
            }
            if (pathTile.Item1 < map.Count - 1)
            {
                checkSouth = true;
            }
            if (pathTile.Item2 < map.ElementAt(pathTile.Item1).Count - 1)
            {
                checkEast = true;
            }
            if (pathTile.Item2 > 0)
            {
                checkWest = true;
            }


            char tileNorth = ' ';
            char tileSouth = ' ';
            char tileEast = ' ';
            char tileWest = ' ';

            if (checkNorth)
            {
                tileNorth = map[pathTile.Item1 - 1][pathTile.Item2];
            }
            if (checkSouth)
            {
                tileSouth = map[pathTile.Item1 + 1][pathTile.Item2];
            }
            if (checkEast)
            {
                tileEast = map[pathTile.Item1][pathTile.Item2 + 1];
            }
            if (checkWest)
            {
                tileWest = map[pathTile.Item1][pathTile.Item2 - 1];
            }
            
            bool northConnection = false;
            bool southConnection = false;
            bool eastConnection = false;
            bool westConnection = false;

            if (new char[] { '|', '7', 'F' }.Contains(tileNorth))
            {
                northConnection = true;
            }

            if (new char[] { '|', 'L', 'J' }.Contains(tileSouth))
            {
                southConnection = true;
            }

            if (new char[] { '-', 'J', '7' }.Contains(tileEast))
            {
                eastConnection = true;
            }

            if (new char[] { '-', 'L', 'F' }.Contains(tileWest))
            {
                westConnection = true;
            }

            if(northConnection && southConnection)
            {
                pathTiles = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(pathTile.Item1 - 1, pathTile.Item2),
                    new Tuple<int, int>(pathTile.Item1 + 1, pathTile.Item2)
                };
            }
            else if(northConnection && eastConnection)
            {
                pathTiles = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(pathTile.Item1 - 1, pathTile.Item2),
                    new Tuple<int, int>(pathTile.Item1, pathTile.Item2 + 1)
                };
            }
            else if (northConnection && westConnection)
            {
                pathTiles = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(pathTile.Item1 - 1, pathTile.Item2),
                    new Tuple<int, int>(pathTile.Item1, pathTile.Item2 - 1)
                };
            }
            else if (southConnection && eastConnection)
            {
                pathTiles = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(pathTile.Item1 + 1, pathTile.Item2),
                    new Tuple<int, int>(pathTile.Item1, pathTile.Item2 + 1)
                };
            }
            else if (southConnection && westConnection)
            {
                pathTiles = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(pathTile.Item1 + 1, pathTile.Item2),
                    new Tuple<int, int>(pathTile.Item1, pathTile.Item2 - 1)
                };
            }
            else
            {
                pathTiles = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(pathTile.Item1, pathTile.Item2 - 1),
                    new Tuple<int, int>(pathTile.Item1, pathTile.Item2 + 1)
                };
            }

        }
        private bool TuplesEqual(Tuple<int, int> tupleOne, Tuple<int, int> tupleTwo)
        {
            return tupleOne.Item1 == tupleTwo.Item1 && tupleOne.Item2 == tupleTwo.Item2;
        }

        private bool ListTupleContains(List<Tuple<int, int>> list, Tuple<int, int> tuple)
        {
            return list.Where(x => x.Item1 == tuple.Item1 && x.Item2 == tuple.Item2).Any();
            
        }
    }
}

