using System;
using System.Linq;

namespace SudokuGame
{
    public class SudokuSolver
    {
        string GridStr = string.Empty;

        public SudokuSolver(string gridStr)
        {
            GridStr = gridStr;
        }

        public string Execute(bool displaySolution = false, bool displayGrid = false)
        {
            var grid = Commons.GridFromString(GridStr);
            if (displayGrid)
                Commons.PrettyDisplay(grid);

            BackTrack<GameGen, MoveGen> backTrack = new BackTrack<GameGen, MoveGen>(new GameGen(Commons.CloneGrid(grid)));
            backTrack.SetGameFunctions(g => g.EndGame(), g => g.MovesGen());
            backTrack.SetMoveFunctions((g, m) => true, (g, m) => g.ApplyMove(m), (g, m) => g.UndoMove(m));
            backTrack.SearchToEnd();

            var game0 = new GameGen(Commons.CloneGrid(grid));
            backTrack.Solution.ForEach(game0.ApplyMove);
            var solStr = game0.Grid.ToString();

            if (displaySolution)
            {
                var allMoves = backTrack.Solution;
                Console.WriteLine($"NbBacktrack = {backTrack.NbBacktrack}; Cells = {grid.allCells.Count(c => c.Content != 0)}; AllMoves = {allMoves.Count}");
                Console.WriteLine(string.Join(" ", allMoves.Select(m => m.GetMove)));
                Console.WriteLine(solStr);
                Console.WriteLine();
            }

            return solStr;
        }
    }
}
