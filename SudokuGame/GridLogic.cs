using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGame
{
    public enum CellStatus { GOOD, ERROR }

    public class Cell
    {
        public int Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int SubGrid { get; private set; }
        public CellStatus Status { get; set; } = CellStatus.GOOD;

        public Cell(int id)
        {
            Id = id;

            X = id % 9;
            Y = id / 9;

            SubGrid = (X / 3) + (Y / 3) * 3;
        }

        public int Content { get; set; } = 0;
        public List<int> Possibles = new List<int>();

        public void reset()
        {
            Possibles = new List<int>();
            Content = 0;
        }
    }

    public class Grid
    {
        public List<Cell> allCells = new List<Cell>();
        public Cell[][] subGrids, rowCells, colCells;

        public Grid()
        {
            allCells = Enumerable.Range(0, 81).Select(i => new Cell(i)).ToList();

            subGrids = new Cell[9][];
            rowCells = new Cell[9][];
            colCells = new Cell[9][];

            for (int i = 0; i < 9; ++i)
            {
                subGrids[i] = allCells.Where(c => c.SubGrid == i).OrderBy(c => c.Id).ToArray();
                rowCells[i] = allCells.Where(c => c.Y == i).OrderBy(c => c.Y).ToArray();
                colCells[i] = allCells.Where(c => c.X == i).OrderBy(c => c.X).ToArray();
            }
        }

        public void UpdateCellsState()
        {
            checkEmptyCell();
            checkPossiblesContentsAll();
        }

        public void UpdateCellsStateAndTest()
        {
            checkEmptyCell();
            checkPossiblesContentsAll();

            testCellsAll();
        }

        void checkEmptyCell()
        {
            foreach (var c in allCells)
            {
                if (c.Content == 0) c.Possibles = Enumerable.Range(1, 9).ToList();
                else c.Possibles.Clear();
            }
        }

        void checkPossiblesContentsAll()
        {
            System.Array.ForEach(rowCells, checkPossiblesContentsRange);
            System.Array.ForEach(colCells, checkPossiblesContentsRange);
            System.Array.ForEach(subGrids, checkPossiblesContentsRange);
        }

        void checkPossiblesContentsRange(Cell[] cells)
        {
            var emptyCells = cells.Where(c => c.Content == 0).ToList();
            var contentCells = cells.Where(c => c.Content != 0).Select(c => c.Content).ToList();
            foreach (var c in emptyCells)
                c.Possibles.RemoveAll(contentCells.Contains);
        }

        void testCellsAll()
        {
            allCells.ForEach(c => c.Status = CellStatus.GOOD);
            System.Array.ForEach(rowCells, testCellsRange);
            System.Array.ForEach(colCells, testCellsRange);
            System.Array.ForEach(subGrids, testCellsRange);
        }

        void testCellsRange(Cell[] cells)
        {
            var contentCells = cells.Where(c => c.Content != 0).ToList();
            foreach (var c in contentCells)
            {
                if (c.Content != 0 && contentCells.Count(c0 => c0.Content == c.Content) >= 2)
                    c.Status = CellStatus.ERROR;
            }
        }

        public bool CanFillAll => rowCells.All(canFillRange) && colCells.All(canFillRange) && subGrids.All(canFillRange);

        bool canFillRange(Cell[] cells)
        {
            var all = Enumerable.Range(1, 9).ToList();
            foreach (var c in cells)
            {
                if (c.Content == 0 && c.Possibles.Count == 0) return false;

                if (c.Content != 0) all.Remove(c.Content);
                else c.Possibles.ForEach(i => all.Remove(i));
            }

            return all.Count == 0;
        }

        public override string ToString()
        {
            return string.Join(" ", allCells.Select(c => c.Content));
        }
    }
}
