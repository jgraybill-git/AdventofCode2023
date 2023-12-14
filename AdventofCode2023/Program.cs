using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventofCode2023;
using AdventofCode2023.Days;

Dictionary<int, Tuple<IDay, bool, bool>> days = new Dictionary<int, Tuple<IDay, bool, bool>>()
{
    { 1, new Tuple<IDay, bool, bool>(new Day1(), false, false) },
    { 2, new Tuple<IDay, bool, bool>(new Day2(), false, false) },
    { 3, new Tuple<IDay, bool, bool>(new Day3(), false, false) },
    { 4, new Tuple<IDay, bool, bool>(new Day4(), false, false) },
    { 5, new Tuple<IDay, bool, bool>(new Day5(), false, false) },
    { 6, new Tuple<IDay, bool, bool>(new Day6(), false, false) },
    { 7, new Tuple<IDay, bool, bool>(new Day7(), false, false) },
    { 9, new Tuple<IDay, bool, bool>(new Day9(), false, false) },
    { 10, new Tuple<IDay, bool, bool>(new Day10(), false, false) },
    { 12, new Tuple<IDay, bool, bool>(new Day12(), false, false) },
    { 13, new Tuple<IDay, bool, bool>(new Day13(), false, true) },
    { 14, new Tuple<IDay, bool, bool>(new Day14(), false, false) },
    { 15, new Tuple<IDay, bool, bool>(new Day15(), false, false) },
    { 16, new Tuple<IDay, bool, bool>(new Day16(), false, false) },
    { 17, new Tuple<IDay, bool, bool>(new Day17(), false, false) },
    { 18, new Tuple<IDay, bool, bool>(new Day18(), false, false) },
    { 19, new Tuple<IDay, bool, bool>(new Day19(), false, false) },
    { 20, new Tuple<IDay, bool, bool>(new Day20(), false, false) },
    { 21, new Tuple<IDay, bool, bool>(new Day21(), false, false) },
    { 22, new Tuple<IDay, bool, bool>(new Day22(), false, false) },
    { 23, new Tuple<IDay, bool, bool>(new Day23(), false, false) },
    { 24, new Tuple<IDay, bool, bool>(new Day24(), false, false) },
    { 25, new Tuple<IDay, bool, bool>(new Day25(), false, false) }
};

Dictionary<int, Tuple<IDayDbl, bool, bool>> daysDbl = new Dictionary<int, Tuple<IDayDbl, bool, bool>>()
{
    { 8, new Tuple<IDayDbl, bool, bool>(new Day8(), false, false) },
    { 11, new Tuple<IDayDbl, bool, bool>(new Day11(), false, false) }
};


foreach (var day in days)
{
    bool runProblem1 = day.Value.Item2;
    bool runProblem2 = day.Value.Item3;

    if(runProblem1)
    {
        Console.WriteLine($"Day {day.Key} Problem 1: {day.Value.Item1.Problem1()}");
    }
    if (runProblem2)
    {
        Console.WriteLine($"Day {day.Key} Problem 2: {day.Value.Item1.Problem2()}");
    }
}

foreach (var day in daysDbl)
{
    bool runProblem1 = day.Value.Item2;
    bool runProblem2 = day.Value.Item3;

    if (runProblem1)
    {
        Console.WriteLine($"Day {day.Key} Problem 1: {day.Value.Item1.Problem1()}");
    }
    if (runProblem2)
    {
        Console.WriteLine($"Day {day.Key} Problem 2: {day.Value.Item1.Problem2()}");
    }
}
Console.ReadKey();
