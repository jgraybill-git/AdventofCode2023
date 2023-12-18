using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day16 : IDay
    {
        private int DayNum = 16;
        private string InputFile;
        private bool UseTestingFile = false;

        public Day16()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            List<List<char>> inputGrid = new List<List<char>>();


            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputGrid.Add(line.ToCharArray().ToList());
                }
            }

            char[,] grid = new char[inputGrid.Count, inputGrid.ElementAt(0).Count];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = inputGrid.ElementAt(i).ElementAt(j);
                }
            }

            inputGrid.Clear();

            char startChar = grid[0, 0];
            Direction startDirection = Direction.Right;

            GridTile startTile = new GridTile(0, 0, startDirection);

            List<GridTile> energizedTiles = new List<GridTile>()
            {
                new GridTile(startTile.X, startTile.Y, startTile.Direction)
            };

            List<GridTile> activePaths = new List<GridTile>()
            {
                new GridTile(startTile.X, startTile.Y, startTile.Direction)
            };


            while (!activePaths.All(x => x.Terminate))
            {
                List<GridTile> newTiles = new List<GridTile>();
                List<GridTile> removeTiles = new List<GridTile>();

                foreach (GridTile currentTile in activePaths)
                {
                    if (!currentTile.Terminate)
                    {
                        List<Direction> goInDirections = AnalyseTile(currentTile,
                            grid[currentTile.X, currentTile.Y]);

                        if (goInDirections.Count == 1)
                        {
                            currentTile.Direction = goInDirections.First();
                            GridTile moveResult = MoveToNextTile(currentTile, ref grid);

                            if (!moveResult.Terminate && !BeenToTileViaDirection(moveResult, ref energizedTiles))
                            {
                                currentTile.X = moveResult.X;
                                currentTile.Y = moveResult.Y;

                                energizedTiles.Add(new GridTile(moveResult.X, moveResult.Y, moveResult.Direction));
                            }
                            else
                            {
                                currentTile.Terminate = true;
                                removeTiles.Add(currentTile);
                            }
                        }
                        else
                        {
                            currentTile.Direction = goInDirections[0];
                            GridTile moveResultOne = MoveToNextTile(currentTile, ref grid);

                            currentTile.Direction = goInDirections[1];
                            GridTile moveResultTwo = MoveToNextTile(currentTile, ref grid);

                            if (!moveResultOne.Terminate && !BeenToTileViaDirection(moveResultOne, ref energizedTiles))
                            {
                                newTiles.Add(moveResultOne);
                                energizedTiles.Add(new GridTile(moveResultOne.X, moveResultOne.Y, moveResultOne.Direction));
                            }
                            if (!moveResultTwo.Terminate && !BeenToTileViaDirection(moveResultTwo, ref energizedTiles))
                            {
                                newTiles.Add(moveResultTwo);
                                energizedTiles.Add(new GridTile(moveResultTwo.X, moveResultTwo.Y, moveResultTwo.Direction));
                            }

                            if (newTiles.Count == 0 || newTiles.Count == 2)
                            {
                                currentTile.Terminate = true;
                                removeTiles.Add(currentTile);
                            }
                            else if(newTiles.Count == 1)
                            {
                                newTiles.Add(new GridTile(newTiles.First().X, newTiles.First().Y, newTiles.First().Direction));
                                removeTiles.Add(currentTile);
                            }
                        }
                    }
                }

                foreach(var tile in removeTiles)
                {
                    activePaths.Remove(tile);
                }
                foreach (var tile in newTiles)
                {
                    activePaths.Add(tile);
                }
            }

            var distinctTiles = GetDistinctTiles(energizedTiles);

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Console.WriteLine();
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (distinctTiles.Where(z => z.X == x && z.Y == y).Any())
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
            Console.WriteLine();

            return distinctTiles.Count;
        }


        public int Problem2()
        {
            List<List<char>> inputGrid = new List<List<char>>();


            string? line;
            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputGrid.Add(line.ToCharArray().ToList());
                }
            }

            char[,] grid = new char[inputGrid.Count, inputGrid.ElementAt(0).Count];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = inputGrid.ElementAt(i).ElementAt(j);
                }
            }

            inputGrid.Clear();

            char startChar = grid[0, 0];

            int maxEnergizedTiles = int.MinValue;

            List<GridTile> initialTileOpts = new List<GridTile>();
            for(int tileRow = 0; tileRow < grid.GetLength(0); tileRow++)
            {
                initialTileOpts.Add(new GridTile(tileRow, 0, Direction.Right));
                initialTileOpts.Add(new GridTile(tileRow, grid.GetLength(1) - 1, Direction.Left));
            }
            for (int tileCol = 0; tileCol < grid.GetLength(1); tileCol++)
            {
                initialTileOpts.Add(new GridTile(0, tileCol, Direction.Down));
                initialTileOpts.Add(new GridTile(grid.GetLength(0) - 1, tileCol, Direction.Up));
            }

            int fdsa = 0;
            foreach(var startOpt in initialTileOpts)
            {
                Direction startDirection = startOpt.Direction;

                GridTile startTile = new GridTile(startOpt.X, startOpt.Y, startDirection);

                List<GridTile> energizedTiles = new List<GridTile>()
                {
                    new GridTile(startTile.X, startTile.Y, startTile.Direction)
                };

                List<GridTile> activePaths = new List<GridTile>()
                {
                    new GridTile(startTile.X, startTile.Y, startTile.Direction)
                };


                while (!activePaths.All(x => x.Terminate))
                {
                    List<GridTile> newTiles = new List<GridTile>();
                    List<GridTile> removeTiles = new List<GridTile>();

                    foreach (GridTile currentTile in activePaths)
                    {
                        if (!currentTile.Terminate)
                        {
                            List<Direction> goInDirections = AnalyseTile(currentTile,
                                grid[currentTile.X, currentTile.Y]);

                            if (goInDirections.Count == 1)
                            {
                                currentTile.Direction = goInDirections.First();
                                GridTile moveResult = MoveToNextTile(currentTile, ref grid);

                                if (!moveResult.Terminate && !BeenToTileViaDirection(moveResult, ref energizedTiles))
                                {
                                    currentTile.X = moveResult.X;
                                    currentTile.Y = moveResult.Y;

                                    energizedTiles.Add(new GridTile(moveResult.X, moveResult.Y, moveResult.Direction));
                                }
                                else
                                {
                                    currentTile.Terminate = true;
                                    removeTiles.Add(currentTile);
                                }
                            }
                            else
                            {
                                currentTile.Direction = goInDirections[0];
                                GridTile moveResultOne = MoveToNextTile(currentTile, ref grid);

                                currentTile.Direction = goInDirections[1];
                                GridTile moveResultTwo = MoveToNextTile(currentTile, ref grid);

                                if (!moveResultOne.Terminate && !BeenToTileViaDirection(moveResultOne, ref energizedTiles))
                                {
                                    newTiles.Add(moveResultOne);
                                    energizedTiles.Add(new GridTile(moveResultOne.X, moveResultOne.Y, moveResultOne.Direction));
                                }
                                if (!moveResultTwo.Terminate && !BeenToTileViaDirection(moveResultTwo, ref energizedTiles))
                                {
                                    newTiles.Add(moveResultTwo);
                                    energizedTiles.Add(new GridTile(moveResultTwo.X, moveResultTwo.Y, moveResultTwo.Direction));
                                }

                                if (newTiles.Count == 0 || newTiles.Count == 2)
                                {
                                    currentTile.Terminate = true;
                                    removeTiles.Add(currentTile);
                                }
                                else if (newTiles.Count == 1)
                                {
                                    newTiles.Add(new GridTile(newTiles.First().X, newTiles.First().Y, newTiles.First().Direction));
                                    removeTiles.Add(currentTile);
                                }
                            }
                        }
                    }

                    foreach (var tile in removeTiles)
                    {
                        activePaths.Remove(tile);
                    }
                    foreach (var tile in newTiles)
                    {
                        activePaths.Add(tile);
                    }
                }

                var distinctTiles = GetDistinctTiles(energizedTiles);
                maxEnergizedTiles = Math.Max(maxEnergizedTiles, distinctTiles.Count);
                fdsa++;
            }

            

            //for (int x = 0; x < grid.GetLength(0); x++)
            //{
            //    Console.WriteLine();
            //    for (int y = 0; y < grid.GetLength(1); y++)
            //    {
            //        if (distinctTiles.Where(z => z.X == x && z.Y == y).Any())
            //        {
            //            Console.Write("#");
            //        }
            //        else
            //        {
            //            Console.Write(".");
            //        }
            //    }
            //}
            //Console.WriteLine();

            return maxEnergizedTiles;
        }

        private bool BeenToTileViaDirection(GridTile moveResult, ref List<GridTile> energizedTiles)
        {
            foreach(var tile in energizedTiles)
            {
                if(tile.X == moveResult.X && tile.Y == moveResult.Y && tile.Direction == moveResult.Direction)
                {
                    return true;
                }
            }

            return false;
        }

        private List<GridTile> GetDistinctTiles(List<GridTile> tiles)
        {
            List<GridTile> visitedTiles = new List<GridTile>();

            foreach (var tile in tiles)
            {
                if (!ListContains(visitedTiles, tile))
                {
                    visitedTiles.Add(tile);
                }
            }

            return visitedTiles;
        }

        private bool ListContains(List<GridTile> squares, GridTile square)
        {
            foreach(var s in squares)
            {
                if (s.X == square.X && s.Y == square.Y)
                {
                    return true;
                }
            }

            return false;
        }

        private List<Direction> AnalyseTile(GridTile tile, char squareItem)
        {
            switch (tile.Direction)
            {
                case Direction.Up:
                    switch (squareItem)
                    {
                        case '.':
                            return new List<Direction>() { tile.Direction};
                        case '-':
                            return new List<Direction>()
                            {
                                Direction.Left,
                                Direction.Right
                            };
                        case '|':
                            return new List<Direction>() { tile.Direction };
                        case '/':
                            return new List<Direction>() { Direction.Right };
                        case '\\':
                            return new List<Direction>() { Direction.Left };

                    }
                    break;
                case Direction.Down:
                    switch (squareItem)
                    {
                        case '.':
                            return new List<Direction>() { tile.Direction };
                        case '-':
                            return new List<Direction>()
                            {
                                Direction.Left,
                                Direction.Right
                            };
                        case '|':
                            return new List<Direction>() { tile.Direction };
                        case '/':
                            return new List<Direction>() { Direction.Left };
                        case '\\':
                            return new List<Direction>() { Direction.Right };

                    }
                    break;
                case Direction.Left:
                    switch (squareItem)
                    {
                        case '.':
                            return new List<Direction>() { tile.Direction };
                        case '-':
                            return new List<Direction>() { tile.Direction };
                        case '|':
                            return new List<Direction>()
                            {
                                Direction.Up,
                                Direction.Down
                            };
                        case '/':
                            return new List<Direction>() { Direction.Down };
                        case '\\':
                            return new List<Direction>() { Direction.Up };

                    }
                    break;
                case Direction.Right:
                    switch (squareItem)
                    {
                        case '.':
                            return new List<Direction>() { tile.Direction };
                        case '-':
                            return new List<Direction>() { tile.Direction };
                        case '|':
                            return new List<Direction>()
                            {
                                Direction.Up,
                                Direction.Down
                            };
                        case '/':
                            return new List<Direction>() { Direction.Up };
                        case '\\':
                            return new List<Direction>() { Direction.Down };

                    }
                    break;
            }

            return new List<Direction>() { Direction.Right };
        }

        private GridTile MoveToNextTile(GridTile current, ref char[,] grid)
        {
            int rightMove = current.Y + 1;
            int leftMove = current.Y - 1;
            int upMove = current.X - 1;
            int downMove = current.X + 1;

            if (current.Direction == Direction.Up && upMove >= 0)
            {
                return new GridTile(upMove, current.Y, current.Direction);
            }
            else if (current.Direction == Direction.Down && downMove < grid.GetLength(0))
            {
                return new GridTile(downMove, current.Y, current.Direction);
            }
            else if (current.Direction == Direction.Left && leftMove >= 0)
            {
                return new GridTile(current.X, leftMove, current.Direction);
            }
            else if (current.Direction == Direction.Right && rightMove < grid.GetLength(1))
            {
                return new GridTile(current.X, rightMove, current.Direction);
            }
            else
            {
                return new GridTile(current.X, current.Y, current.Direction, true);
            }
        }



        private class GridTile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Direction Direction { get; set; }
            public bool Terminate { get; set; }
            
            public GridTile(int x, int y, Direction d, bool t = false)
            {
                X = x;
                Y = y;
                Direction = d;
                Terminate = t;
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}

