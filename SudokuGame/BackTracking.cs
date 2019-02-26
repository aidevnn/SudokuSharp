using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGame
{
    interface IGame { }
    interface IMove
    {
        string GetMove { get; }
    }

    delegate bool EndGame<T>(T game) where T : IGame;
    delegate List<T2> GenMoves<T1, T2>(T1 game) where T1 : IGame where T2 : IMove;

    delegate bool CanMove<T1, T2>(T1 game, T2 move) where T1 : IGame where T2 : IMove;
    delegate void GameMove<T1, T2>(T1 game, T2 move) where T1 : IGame where T2 : IMove;

    class Moves<T> where T : IMove
    {
        public LinkedList<T> AllMoves { get; private set; }
        public LinkedListNode<T> CurrentMove { get; set; }

        public Moves(List<T> moves)
        {
            AllMoves = new LinkedList<T>(moves);
            CurrentMove = AllMoves.First;
        }
    }

    class MovesTrace<T> where T : IMove
    {
        public LinkedList<Moves<T>> AllSteps { get; private set; }
        public LinkedListNode<Moves<T>> CurrentStep { get; set; }

        public MovesTrace()
        {
            AllSteps = new LinkedList<Moves<T>>();
        }

        public void AddStep(List<T> moves)
        {
            if (moves.Count == 0) return;
            AllSteps.AddLast(new Moves<T>(moves));
            CurrentStep = AllSteps.Last;
        }

        public void RemoveLast()
        {
            AllSteps.RemoveLast();
            CurrentStep = AllSteps.Last;
        }
    }

    class BackTrack<T1, T2> where T1 : IGame where T2 : IMove
    {
        enum StepSearch { Backward, Forward }

        T1 Game;

        StepSearch stepSearch = StepSearch.Forward;

        EndGame<T1> endGame;
        GenMoves<T1, T2> genMoves;

        CanMove<T1, T2> canMove;
        GameMove<T1, T2> doMove, undoMove;

        public List<T2> Solution { get; set; }
        public bool FoundSolution { get; private set; }
        public int NbBacktrack = 0;

        public BackTrack(T1 game)
        {
            Game = game;
        }

        public void SetGameFunctions(EndGame<T1> endGame0, GenMoves<T1, T2> genMoves0)
        {
            endGame = endGame0;
            genMoves = genMoves0;
        }

        public void SetMoveFunctions(CanMove<T1, T2> canMove0, GameMove<T1, T2> doMove0, GameMove<T1, T2> undoMove0)
        {
            canMove = canMove0;
            doMove = doMove0;
            undoMove = undoMove0;
        }

        MovesTrace<T2> movesTrace;

        bool continueSearch => !FoundSolution && movesTrace.CurrentStep != null;

        public void SearchPrepare()
        {
            FoundSolution = false;
            Solution = null;
            NbBacktrack = 0;

            var validMoves = genMoves(Game).FindAll(m => canMove(Game, m));
            movesTrace = new MovesTrace<T2>();
            movesTrace.AddStep(validMoves);

            stepSearch = StepSearch.Forward;
        }

        public bool SearchContinue()
        {
            if (continueSearch)
            {
                doSearch();
                return true;
            }
            else
                return false;
        }

        public void SearchToEnd()
        {
            SearchPrepare();
            while (continueSearch)
                doSearch();
        }

        void checkSolution()
        {
            Solution = movesTrace.AllSteps.Select(t => t.CurrentMove.Value).ToList();
            FoundSolution = true;
        }

        void doSearch()
        {
            if (stepSearch == StepSearch.Backward)
            {
                if (movesTrace.CurrentStep == null) return;

                var step = movesTrace.CurrentStep.Value;
                var move = step.CurrentMove.Value;
                undoMove(Game, move);

                if (step.CurrentMove.Next != null)
                {
                    step.CurrentMove = step.CurrentMove.Next;
                    stepSearch = StepSearch.Forward;
                }
                else
                    movesTrace.RemoveLast();
            }
            else
            {
                var step = movesTrace.CurrentStep.Value;
                var move = step.CurrentMove.Value;
                doMove(Game, move);
                stepSearch = StepSearch.Forward;

                if (endGame(Game))
                {
                    checkSolution();
                    stepSearch = StepSearch.Backward;
                }
                else
                {
                    var validMoves = genMoves(Game).FindAll(m => canMove(Game, m));
                    if (validMoves.Count == 0)
                    {
                        stepSearch = StepSearch.Backward;
                        ++NbBacktrack;
                    }
                    else
                        movesTrace.AddStep(validMoves);
                }
            }
        }
    }
}
