using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventofCode2023.Days
{
    public class Day17 : IDay
    {
        private int DayNum = 17;
        private string InputFile;
        private bool UseTestingFile = true;

        public Day17()
        {
            InputFile = $@"/Users/jgraybill/Projects/AdventofCode2023/AdventofCode2023/Inputs/Day{DayNum}{(UseTestingFile ? "Testing" : String.Empty)}.txt";
        }

        public int Problem1()
        {
            string? line;
            List<List<int>> inputGrid = new List<List<int>>();

            using (var fileStream = File.OpenRead(InputFile))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    List<int> currLine = new List<int>();
                    foreach(char c in line)
                    {
                        currLine.Add(Convert.ToInt32(c.ToString()));
                    }
                    inputGrid.Add(currLine);
                }
            }

            PriorityQueue<GraphNode, int> frontier = new PriorityQueue<GraphNode, int>();
            GraphNode start = new GraphNode(0, 0, null);
            List<GraphNode> explored = new List<GraphNode>();
            frontier.Enqueue(start, 0);
            int rowCount = inputGrid.Count;
            int colCount = inputGrid.ElementAt(0).Count;
            

            while(frontier.Count > 0)
            {

                GraphNode choice = frontier.Dequeue();
                //if (choice.X == inputGrid.Count - 1 && choice.Y == inputGrid.ElementAt(0).Count - 1)
                //{
                //    break;
                //    //return choice.CurrentPathCost;
                //}
                if (choice.X == 0 && choice.Y == 8)
                {
                    int rre = 5;
                    //return choice.CurrentPathCost;
                }

                List<GraphNode> adjacencies = GetAdjacencies(choice, ref inputGrid);

                foreach(var adj in adjacencies)
                {
                    if(adj.X == 0 && adj.Y == 5)
                    {
                        int fdsa = 5;
                    }

                    if (adj.X == rowCount - 1 && adj.Y == colCount - 1)
                    {
                        explored.Add(adj);
                        break;
                        //return choice.CurrentPathCost;
                    }

                    //var edgeCost = adj.CurrentPathCost - choice.CurrentPathCost;
                    //var newPathCost = adj.CurrentPathCost;



                    GraphNode? exploredItem = explored.Where(x => x.X == adj.X && x.Y == adj.Y && x.Direction == adj.Direction).FirstOrDefault();
                    //frontier.Enqueue(adj, adj.CurrentPathCost);
                    if (exploredItem != null)
                    {
                        int exploredCost = exploredItem.CurrentPathCost;
                        
                        if (adj.CurrentPathCost < exploredCost)
                        {
                            //frontier.Enqueue(adj, adj.CurrentPathCost);
                            exploredItem = adj;
                        }
                    }
                    else
                    {
                        frontier.Enqueue(adj, adj.CurrentPathCost);
                        explored.Add(adj);
                    }
                     
                }

            }



            var tr = explored.Where(x => x.X == rowCount - 1 && x.Y == colCount - 1).ToList();
            var path = explored.Where(x => x.X == rowCount - 1 && x.Y == colCount - 1).First();

            List<GraphNode> parents = new List<GraphNode>() { path };
            var parent = path.ParentNode;
            while(parent != null)
            {
                parents.Add(parent);
                parent = parent.ParentNode;
            }

            for (int i = 0; i < rowCount; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < colCount; j++)
                {
                    if (parents.Where(x => x.X == i && x.Y == j).Any())
                    {
                        Console.Write(inputGrid[i][j]);
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
            Console.WriteLine();
            return explored.Where(x => x.X == rowCount - 1 && x.Y == colCount - 1).Select(x => x.CurrentPathCost).Min(); //.CurrentPathCost;

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

        private void PrintExplored(List<GraphNode> explored)
        {
            for(int i = 0; i < 13; i++)
            {
                Console.WriteLine();
                for(int j = 0; j < 13; j++)
                {
                    if(explored.Where(x => x.X == i && x.Y == j).Any())
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
            Console.WriteLine("============================");
        }

        private List<GraphNode> GetAdjacencies(GraphNode node, ref List<List<int>> grid)
        {
            List<GraphNode> adj = new List<GraphNode>();
            GraphNode leftNode;
            GraphNode rightNode;
            GraphNode upNode;
            GraphNode downNode;

            int straight = node.CurrentStraightCount;
            if (node.Direction != Direction.Up)
            {
                straight = 0;
            }

            
            int i = 1;

            int runningCost = 0;
            while(3 - straight > 0)
            {
                if(node.X - i >= 0)
                {
                    runningCost += grid[node.X - i][node.Y];
                    upNode = new GraphNode(node.X - i, node.Y, node, node.CurrentPathCost + runningCost);
                    upNode.Direction = Direction.Up;
                    upNode.CurrentStraightCount = i;
                    if (!InPathHistory(upNode, node) && node.Direction != Direction.Down)
                    {
                        adj.Add(upNode);
                    }
                    i++;
                    straight++;
                }
                else { break; }
            }


            runningCost = 0;
            straight = node.CurrentStraightCount;
            if (node.Direction != Direction.Down)
            {
                straight = 0;
            }

            i = 1;
               
            while (3 - straight > 0)
            {
                if (node.X + i < grid.Count)
                {
                    if (node.ParentNode is null)
                    {
                        var tmpParentNode = new GraphNode(node.X, node.Y, null);
                        tmpParentNode.Direction = Direction.Down;
                        runningCost += grid[node.X + i][node.Y];
                        downNode = new GraphNode(node.X + i, node.Y, tmpParentNode, node.CurrentPathCost + runningCost);
                        downNode.CurrentStraightCount = i;
                        downNode.Direction = Direction.Down;
                    }
                    else
                    {
                        runningCost += grid[node.X + i][node.Y];
                        downNode = new GraphNode(node.X + i, node.Y, node, node.CurrentPathCost + runningCost);
                        downNode.CurrentStraightCount = i;
                        downNode.Direction = Direction.Down;
                    }

                    if (!InPathHistory(downNode, node) && node.Direction != Direction.Up)
                    {
                        adj.Add(downNode);
                    }
                    i++;
                    straight++;
                }
                else { break; }
            }


            runningCost = 0;
            straight = node.CurrentStraightCount;
            if (node.Direction != Direction.Right)
            {
                straight = 0;
            }
            i = 1;

            while (3 - straight > 0)
            {
                if (node.Y + i < grid.ElementAt(0).Count)
                {
                    if (node.ParentNode is null)
                    {
                        var tmpParentNode = new GraphNode(node.X, node.Y, null);
                        tmpParentNode.Direction = Direction.Right;
                        runningCost += grid[node.X][node.Y + i];
                        rightNode = new GraphNode(node.X, node.Y + i, tmpParentNode, node.CurrentPathCost + runningCost);
                        rightNode.CurrentStraightCount = i;
                        rightNode.Direction = Direction.Right;
                    }
                    else
                    {
                        runningCost += grid[node.X][node.Y + i];
                        rightNode = new GraphNode(node.X, node.Y + i, node, node.CurrentPathCost + runningCost);
                        rightNode.CurrentStraightCount = i;
                        rightNode.Direction = Direction.Right;
                    }

                    if (!InPathHistory(rightNode, node) && node.Direction != Direction.Left)
                    {
                        adj.Add(rightNode);
                    }
                    i++;
                    straight++;
                }
                else { break; }
            }

            runningCost = 0;
            straight = node.CurrentStraightCount;
            if (node.Direction != Direction.Left)
            {
                straight = 0;
            }
            i = 1;

            while (3 - straight > 0)
            {
                if (node.Y - i >= 0)
                {
                    runningCost += grid[node.X][node.Y - i];
                    leftNode = new GraphNode(node.X, node.Y - i, node, node.CurrentPathCost + runningCost);
                    leftNode.CurrentStraightCount = i;
                    leftNode.Direction = Direction.Left;

                    if (!InPathHistory(leftNode, node) && node.Direction != Direction.Right)
                    {
                        adj.Add(leftNode);
                    }
                    i++;
                    straight++;
                }
                else { break; }
            }
            


            return adj;

        }

        private List<GraphNode> GetAdjacenciesOld(GraphNode node, ref List<List<int>> grid)
        {
            List<GraphNode> adj = new List<GraphNode>();
            GraphNode leftNode;
            GraphNode rightNode;
            GraphNode upNode;
            GraphNode downNode;

            if(node.X - 1 >= 0)
            {
                upNode = new GraphNode(node.X - 1, node.Y, node, node.CurrentPathCost + grid[node.X - 1][node.Y]);
                upNode.Direction = Direction.Up;
                //if (upNode.ParentNode is not null)
                //{
                //    upNode.ParentNode.Direction = Direction.Up;
                //}

                if (!InPathHistory(upNode, node) && MeetsThreeStraightConstraint(upNode))
                {
                    adj.Add(upNode);
                }
            }
            if (node.X + 1 < grid.Count)
            {
                if (node.ParentNode is null)
                {
                    var tmpParentNode = new GraphNode(node.X, node.Y, null);
                    tmpParentNode.Direction = Direction.Down;
                    downNode = new GraphNode(node.X + 1, node.Y, tmpParentNode, node.CurrentPathCost + grid[node.X + 1][node.Y]);
                    downNode.Direction = Direction.Down;
                }
                else
                {
                    downNode = new GraphNode(node.X + 1, node.Y, node, node.CurrentPathCost + grid[node.X + 1][node.Y]);
                    downNode.Direction = Direction.Down;
                }


                if (!InPathHistory(downNode, node) && MeetsThreeStraightConstraint(downNode))
                {
                    adj.Add(downNode);
                }
            }
            if (node.Y - 1 >= 0)
            {
                //if (node.ParentNode is null)
                //{
                //    node.Direction = Direction.Left;
                //}

                leftNode = new GraphNode(node.X, node.Y - 1, node, node.CurrentPathCost + grid[node.X][node.Y - 1]);
                leftNode.Direction = Direction.Left;
                //if (leftNode.ParentNode is not null)
                //{
                //    leftNode.ParentNode.Direction = Direction.Left;
                //}

                if (!InPathHistory(leftNode, node) && MeetsThreeStraightConstraint(leftNode))
                {
                    adj.Add(leftNode);
                }
            }
            if (node.Y + 1 < grid.ElementAt(0).Count)
            {
                if(node.ParentNode is null)
                {
                    var tmpParentNode = new GraphNode(node.X, node.Y, null);
                    tmpParentNode.Direction = Direction.Right;
                    rightNode = new GraphNode(node.X, node.Y + 1, tmpParentNode, node.CurrentPathCost + grid[node.X][node.Y + 1]);
                    rightNode.Direction = Direction.Right;
                }
                else
                {
                    rightNode = new GraphNode(node.X, node.Y + 1, node, node.CurrentPathCost + grid[node.X][node.Y + 1]);
                    rightNode.Direction = Direction.Right;
                }

                
                //if(rightNode.ParentNode is not null)
                //{
                //    rightNode.ParentNode.Direction = Direction.Right;
                //}
                
                if (!InPathHistory(rightNode, node) && MeetsThreeStraightConstraint(rightNode))
                {
                    adj.Add(rightNode);
                }
            }

            return adj;

        }

        private bool MeetsThreeStraightConstraint(GraphNode node)
        {
            int straightCount = 0;

            int xCount = 0;
            int yCount = 0;

            GraphNode parentNode = node.ParentNode;

            //if(node.X < 3 && node.Y < 3)
            //{
            //    return true;
            //}

            for (int i = 0; i < 3; i++)
            {
                if (parentNode != null)
                {
                    if (parentNode.Direction == node.Direction)
                    {
                        straightCount++;
                    }
                    parentNode = parentNode.ParentNode;
                }
            }

            if (straightCount == 3)
            {
                return false;
            }

            //if (xCount == 4 || yCount == 4)
            //{
            //    return false;
            //}

            return true;
        }

        private bool InPathHistory(GraphNode test, GraphNode source)
        {
            var parent = source.ParentNode;
            while(parent != null)
            {
                if(test.X == parent.X && test.Y == parent.Y)
                {
                    return true;
                }
                parent = parent.ParentNode;
            }

            return false;
        }

        private GraphNode? InExplored(GraphNode node, ref List<GraphNode> explored)
        {

            foreach(var expNode in explored)
            {
                if(expNode.X == node.X && expNode.Y == node.Y)
                {
                    return expNode;
                }
            }

            return null;
        }

        private class GraphNode
        {
            public int X;
            public int Y;
            public GraphNode? ParentNode { get; set; }
            public int CurrentPathCost { get; set; }
            public int CurrentStraightCount { get; set; }
            public Direction Direction { get; set; }
            public int DirectionCount { get; set; }

            public GraphNode(int x, int y, GraphNode? parent, int cost = 0)
            {
                X = x;
                Y = y;
                ParentNode = parent;
                CurrentPathCost = cost;
                CurrentStraightCount = 0;
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

