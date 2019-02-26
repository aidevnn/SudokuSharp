using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGame
{
    public class MoveGen : IMove
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public string GetMove => $"({Id % 9} {Id / 9})[{Number}]";

        public MoveGen(int id, int number)
        {
            Id = id;
            Number = number;
            X = Id % 9;
            Y = Id / 9;
        }
    }

    public class GameGen : IGame
    {
        public int Turn { get; private set; }
        public Grid Grid { get; private set; }
        Stack<string> stacksMoves = new Stack<string>();

        public GameGen(Grid grid)
        {
            Grid = grid;
        }

        public bool EndGame() => Grid.allCells.Count(c => c.Content == 0) == 0;

        public List<MoveGen> MovesGen()
        {
            List<MoveGen> moveGens = new List<MoveGen>();
            if (!Grid.CanFillAll)
                return moveGens;

            var cell = Grid.allCells.Where(c => c.Content == 0)
                .OrderBy(c => c.Possibles.Count)
                .ThenBy(c => Commons.random.NextDouble())
                .FirstOrDefault();

            if (cell != null)
            {
                foreach (var i in cell.Possibles)
                    moveGens.Add(new MoveGen(cell.Id, i));
            }

            return moveGens;
        }

        public List<MoveGen> MovesGenEasy()
        {
            List<MoveGen> moveGens = new List<MoveGen>();
            if (!Grid.CanFillAll)
                return moveGens;

            var cell = Grid.allCells.Where(c => c.Content == 0 && c.Possibles.Count == 1)
                .OrderBy(c => Commons.random.NextDouble())
                .FirstOrDefault();

            if (cell != null)
                moveGens.Add(new MoveGen(cell.Id, cell.Possibles.First()));

            return moveGens;
        }

        public bool CanMove(MoveGen moveGen) => true;
        public void ApplyMove(MoveGen mv)
        {
            Turn++;
            var c = Grid.allCells[mv.Id];
            c.Content = mv.Number;
            stacksMoves.Push(mv.ToString());
            Grid.UpdateCellsState();
        }

        public void UndoMove(MoveGen mv)
        {
            Turn--;
            Grid.allCells[mv.Id].Content = 0;
            stacksMoves.Pop();
            Grid.UpdateCellsState();
        }
    }
}
