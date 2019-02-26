using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SudokuGame
{
    public enum Level { Easy = 0b0001, Medium = 0b0010, Hard = 0b0100, Evil = 0b1000, All = 0b1111 }

    public enum Solving { Impossible, Easy, VeryEasy }

    public static class Commons
    {
        public static Random random = new Random();

        public static Grid GridFromString(string gridStr)
        {
            var lt = gridStr.Split(' ').Select(int.Parse).ToList();

            var grid0 = new Grid();
            grid0.allCells.ForEach(c => c.Content = lt[c.Id]);
            grid0.UpdateCellsState();
            return grid0;
        }

        public static Grid CloneGrid(Grid grid0)
        {
            Grid grid1 = new Grid();
            grid1.allCells.ForEach(c => c.Content = grid0.allCells[c.Id].Content);
            grid1.UpdateCellsState();
            return grid1;
        }

        public static void PrettyDisplay(Grid grid)
        {
            Console.WriteLine(grid);

            List<string> l = grid.rowCells
                .Select(r => string.Join("", r.Select(c => c.Content == 0 ? "." : c.Content.ToString())))
                .Select(r => r.Insert(6, "|").Insert(3, "|"))
                .ToList();

            string s = string.Join("", Enumerable.Repeat("-", 11));
            l.Insert(6, s);
            l.Insert(3, s);

            l.ForEach(Console.WriteLine);
            Console.WriteLine();
        }

        static bool SolveEasy(Grid grid)
        {
            grid.UpdateCellsState();
            BackTrack<GameGen, MoveGen> backTrack = new BackTrack<GameGen, MoveGen>(new GameGen(grid));
            backTrack.SetGameFunctions(g => g.EndGame(), g => g.MovesGenEasy());
            backTrack.SetMoveFunctions((g, m) => true, (g, m) => g.ApplyMove(m), (g, m) => g.UndoMove(m));
            backTrack.SearchToEnd();

            return backTrack.Solution != null;
        }

        static (int, int, Solving) CanSolve(Grid grid, int id0)
        {
            var grid0 = CloneGrid(grid);
            var possibles0 = grid0.allCells.Count(c => c.Content == 0 && c.Possibles.Count == 1);

            var old = grid0.allCells[id0].Content;
            grid0.allCells[id0].Content = 0;
            grid0.UpdateCellsState();
            var possibles1 = grid0.allCells.Count(c => c.Content == 0 && c.Possibles.Count == 1);

            var grid1 = CloneGrid(grid0);
            var test = SolveEasy(grid1);

            if (test)
            {
                if (possibles1 >= possibles0)
                    return (id0, possibles1, Solving.VeryEasy);
                else
                    return (id0, possibles1, Solving.Easy);
            }

            return (id0, possibles1, Solving.Impossible);
        }

        public static Grid CreateGrid(Grid grid, Func<Solving, bool> func, Level level, int k0 = 1000)
        {
            var grid0 = CloneGrid(grid);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var wait = $"Wait... {level}";
            Console.Write($"{wait, -16}");

            for (int m = 0; m <= k0; ++m)
            {
                grid0.UpdateCellsState();
                var cells = grid0.allCells.Where(c => c.Content != 0).Select(c => c.Id).ToList();
                var tuples = cells.Select(i => CanSolve(grid0, i)).Where(t => func(t.Item3)).OrderBy(t => random.NextDouble()).OrderByDescending(t => t.Item3).ThenByDescending(t => t.Item2).ToList();
                var best = tuples.FirstOrDefault();

                if (tuples.Count != 0 && m != k0)
                {
                    int id = best.Item1;
                    grid0.allCells[id].Content = 0;
                }
                else
                {
                    Console.WriteLine($"Time:{stopwatch.ElapsedMilliseconds,5} ms; Tuples:{tuples.Count,2}; Cells:{cells.Count}");
                    break;
                }
            }

            return grid0;
        }
    }
}
