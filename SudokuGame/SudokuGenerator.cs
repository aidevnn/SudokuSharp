using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGame
{
    public class SudokuGenerator
    {
        List<string> gridFull = new List<string>();
        string gridEmpty = string.Join(" ", Enumerable.Repeat(0, 81));

        int GridsPerLevel = 10;
        Level GenLevel = Level.Easy;

        Dictionary<Level, List<string>> AllGrids;

        public SudokuGenerator(int gridsPerLevel = 10, Level genLevel = Level.Easy)
        {
            GridsPerLevel = gridsPerLevel;
            GenLevel = genLevel;

            AllGrids = new Dictionary<Level, List<string>>()
            {
                [Level.Easy] = new List<string>(),
                [Level.Medium] = new List<string>(),
                [Level.Hard] = new List<string>(),
                [Level.Evil] = new List<string>(),
            };
        }

        void prepareFullGrids()
        {
            gridFull.Clear();
            SudokuSolver sudokuSolver = new SudokuSolver(gridEmpty);
            for (int k = 0; k < GridsPerLevel; ++k)
                gridFull.Add(sudokuSolver.Execute());

            var lvls = new Level[4] { Level.Easy, Level.Medium, Level.Hard, Level.Evil };
            foreach (var e in lvls)
                AllGrids[e] = new List<string>();
        }

        void createEasy(string gridStr)
        {
            var grid0 = Commons.GridFromString(gridStr);
            var grid1 = Commons.CreateGrid(grid0, l => l == Solving.VeryEasy, Level.Easy);
            var str = grid1.ToString();
            AllGrids[Level.Easy].Add(str);
        }

        void createMedium(string gridStr)
        {
            var grid0 = Commons.GridFromString(gridStr);
            var grid1 = Commons.CreateGrid(grid0, l => l == Solving.VeryEasy, Level.Medium);
            var grid2 = Commons.CreateGrid(grid1, l => l != Solving.Impossible, Level.Medium, 10);
            var str = grid2.ToString();
            AllGrids[Level.Medium].Add(str);
        }

        void createHard(string gridStr)
        {
            var grid0 = Commons.GridFromString(gridStr);
            var grid1 = Commons.CreateGrid(grid0, l => l != Solving.Impossible, Level.Hard);
            var str = grid1.ToString();
            AllGrids[Level.Hard].Add(str);
        }

        void createEvil(string gridStr)
        {
            while (true)
            {
                var grid0 = Commons.GridFromString(gridStr);
                var grid1 = Commons.CreateGrid(grid0, l => l != Solving.Impossible, Level.Evil);
                int c0 = grid1.allCells.Count(c => c.Content == 0);
                var grid2 = Commons.CreateGrid(grid1, l => l == Solving.Impossible, Level.Evil, 1);
                int c1 = grid2.allCells.Count(c => c.Content == 0);

                if (c0 != c1)
                {
                    var str = grid2.ToString();
                    AllGrids[Level.Evil].Add(str);
                    break;
                }
            }
        }

        void createAll(string gridStr)
        {
            var grid0 = Commons.GridFromString(gridStr);
            var grid1 = Commons.CreateGrid(grid0, l => l == Solving.VeryEasy, Level.Easy);
            AllGrids[Level.Easy].Add(grid1.ToString());

            var grid2 = Commons.CreateGrid(grid1, l => l != Solving.Impossible, Level.Medium, 10);
            AllGrids[Level.Medium].Add(grid2.ToString());

            var grid3 = Commons.CreateGrid(grid2, l => l != Solving.Impossible, Level.Hard);
            AllGrids[Level.Hard].Add(grid3.ToString());

            var grid4 = Commons.CreateGrid(grid3, l => l == Solving.Impossible, Level.Evil, 1);
            AllGrids[Level.Evil].Add(grid4.ToString());
        }

        void displayGeneratedGrids()
        {
            foreach(var e in AllGrids)
            {
                if (e.Value.Count == 0) continue;

                Console.WriteLine();
                Console.WriteLine(e.Key);
                foreach (var s in e.Value)
                    Console.WriteLine(s);
            }

            Console.WriteLine();
        }

        public void ExecuteAll()
        {
            prepareFullGrids();
            int ct = 0;

            foreach (var gridStr in gridFull)
            {
                Console.WriteLine($"Generation:{++ct}");
                createAll(gridStr);
            }

            displayGeneratedGrids();
        }

        public void Execute()
        {
            prepareFullGrids();
            int ct = 0;

            foreach (var gridStr in gridFull)
            {
                Console.WriteLine($"Generation:{++ct}");
                if ((GenLevel & Level.Easy) == Level.Easy) createEasy(gridStr);
                if ((GenLevel & Level.Medium) == Level.Medium) createMedium(gridStr);
                if ((GenLevel & Level.Hard) == Level.Hard) createHard(gridStr);
                if ((GenLevel & Level.Evil) == Level.Evil) createEvil(gridStr);
            }

            displayGeneratedGrids();
        }
    }
}
