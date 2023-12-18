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

            PriorityQueue<GraphNode, int> pq = new PriorityQueue<GraphNode, int>();
            GraphNode start = new GraphNode(0, 0, null);
            List<GraphNode> explored = new List<GraphNode>();
            pq.Enqueue(start, 0);

            var choice = pq.Dequeue();
            while(choice != null)
            {

                if(choice.X == inputGrid.Count - 1 && choice.Y == inputGrid.ElementAt(0).Count - 1)
                {
                    return choice.CurrentPathCost;
                }

                var adjacencies = GetAdjacencies(choice, ref inputGrid);

                foreach(var adj in adjacencies)
                {
                    var edgeCost = adj.CurrentPathCost - choice.CurrentPathCost;
                    var newPathCost = adj.CurrentPathCost;

                    pq.Enqueue(adj, adj.CurrentPathCost);
                    var exploredNode = InExplored(adj, ref explored);
                    if (exploredNode != null)
                    {
                        if(exploredNode.CurrentPathCost < adj.CurrentPathCost)
                        {

                        }
                    }
                    else
                    {
                        
                    }

                    explored.Add(adj);
                }
            }

            return 0;

            /*
             * function Dijkstra(Graph, source):
2      dist[source] ← 0                           // Initialization
3
4      create vertex priority queue Q
5
6      for each vertex v in Graph.Vertices:
7          if v ≠ source
8              dist[v] ← INFINITY                 // Unknown distance from source to v
9              prev[v] ← UNDEFINED                // Predecessor of v
10
11         Q.add_with_priority(v, dist[v])
12
13
14     while Q is not empty:                      // The main loop
15         u ← Q.extract_min()                    // Remove and return best vertex
16         for each neighbor v of u:              // Go through all v neighbors of u
17             alt ← dist[u] + Graph.Edges(u, v)
18             if alt < dist[v]:
19                 dist[v] ← alt
20                 prev[v] ← u
21                 Q.decrease_priority(v, alt)
22
23     return dist, prev
             * 
             * */
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

        private List<GraphNode> GetAdjacencies(GraphNode node, ref List<List<int>> grid)
        {
            var previousNode = node.ParentNode;
            List<GraphNode> adj = new List<GraphNode>();
            GraphNode leftNode;
            GraphNode rightNode;
            GraphNode upNode;
            GraphNode downNode;

            if(node.X - 1 >= 0)
            {
                upNode = new GraphNode(node.X - 1, node.Y, node, node.CurrentPathCost + grid[node.X - 1][node.Y]);
                if(previousNode != null && !(upNode.X == previousNode.X && upNode.Y == previousNode.Y))
                {
                    adj.Add(upNode);
                }
                if(previousNode == null)
                {
                    adj.Add(upNode);
                }
            }
            if (node.X + 1 < grid.Count)
            {
                downNode = new GraphNode(node.X + 1, node.Y, node, node.CurrentPathCost + grid[node.X + 1][node.Y]);
                if (previousNode != null && !(downNode.X == previousNode.X && downNode.Y == previousNode.Y))
                {
                    adj.Add(downNode);
                }
                if (previousNode == null)
                {
                    adj.Add(downNode);
                }
            }
            if (node.Y - 1 >= 0)
            {
                leftNode = new GraphNode(node.X, node.Y - 1, node, node.CurrentPathCost + grid[node.X][node.Y - 1]);
                if (previousNode != null && !(leftNode.X == previousNode.X && leftNode.Y == previousNode.Y))
                {
                    adj.Add(leftNode);
                }
                if (previousNode == null)
                {
                    adj.Add(leftNode);
                }
            }
            if (node.Y + 1 < grid.ElementAt(0).Count)
            {
                rightNode = new GraphNode(node.X, node.Y + 1, node, node.CurrentPathCost + grid[node.X][node.Y + 1]);
                if (previousNode != null && !(rightNode.X == previousNode.X && rightNode.Y == previousNode.Y))
                {
                    adj.Add(rightNode);
                }
                if (previousNode == null)
                {
                    adj.Add(rightNode);
                }
            }

            return adj;

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
     }

    public class GraphNode
    {
        public int X;
        public int Y;
        public GraphNode? ParentNode { get; set; }
        public int CurrentPathCost { get; set; }
        public int CurrentStraightCount { get; set; }

        public GraphNode(int x, int y, GraphNode? parent, int cost = 0)
        {
            X = x;
            Y = y;
            ParentNode = parent;
            CurrentPathCost = cost;
            CurrentStraightCount = 0;
        }

    }

}

