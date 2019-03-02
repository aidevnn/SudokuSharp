# SudokuSharp
Sudoku solver and generator with  4 levels : Easy / Medium / Hard / Evil

Example.

Solving an empty grid

```
var gridEmpty = string.Join(" ", Enumerable.Repeat(0, 81)); // 0 0 0 .... 0 
SudokuSolver sudokuSolver = new SudokuSolver(gridEmpty);
sudokuSolver.Execute(displaySolution: true);
```

Will produce

```
NbBacktrack = 1; Cells = 0; AllMoves = 81
(5 1)[1] (5 4)[2] (5 0)[3] (5 7)[4] (5 3)[5] (5 8)[6] (5 2)[7] (5 5)[8] (5 6)[9] (4 0)[2] (3 0)[4] (3 1)[5] (3 2)[6] (4 1)[8] (4 2)[9] (3 3)[1] (3 4)[3] (3 5)[9] (4 4)[4] (4 5)[6] (4 3)[7] (3 7)[2] (3 8)[7] (3 6)[8] (4 8)[1] (4 6)[3] (4 7)[5] (7 6)[1] (6 7)[3] (7 7)[6] (8 7)[7] (1 7)[1] (0 7)[8] (2 7)[9] (6 6)[2] (8 6)[4] (0 6)[5] (1 6)[6] (2 6)[7] (0 8)[2] (1 8)[3] (2 8)[4] (2 1)[2] (8 8)[5] (6 8)[8] (7 8)[9] (7 0)[5] (6 2)[1] (0 2)[3] (8 2)[2] (7 2)[4] (7 4)[7] (7 1)[3] (7 5)[2] (7 3)[8] (8 1)[6] (2 2)[5] (1 2)[8] (2 0)[1] (2 5)[3] (8 5)[1] (2 3)[6] (2 4)[8] (8 4)[9] (8 3)[3] (1 4)[5] (0 4)[1] (8 0)[8] (6 3)[4] (6 4)[6] (0 3)[9] (1 3)[2] (6 5)[5] (6 1)[7] (6 0)[9] (1 0)[7] (0 1)[4] (1 1)[9] (0 0)[6] (1 5)[4] (0 5)[7]
6 7 1 4 2 3 9 5 8 4 9 2 5 8 1 7 3 6 3 8 5 6 9 7 1 4 2 9 2 6 1 7 5 4 8 3 1 5 8 3 4 2 6 7 9 7 4 3 9 6 8 5 2 1 5 6 7 8 3 9 2 1 4 8 1 9 2 5 4 3 6 7 2 3 4 7 1 6 8 9 5
```

Solving an Evil grid from resource

```
var gridsEvil = Properties.Resources.GridEvil.Split('\n').ToArray();
SudokuSolver sudokuSolver = new SudokuSolver(gridsEvil[0]);
sudokuSolver.Execute(displaySolution: true);
```

Will produce

```
NbBacktrack = 1; Cells = 24; AllMoves = 57
(5 0)[9] (0 8)[8] (7 2)[8] (2 8)[4] (5 4)[1] (5 1)[8] (4 8)[1] (5 5)[2] (3 8)[9] (8 6)[9] (6 6)[5] (6 7)[4] (7 7)[1] (7 3)[4] (7 0)[5] (7 5)[9] (3 7)[5] (3 3)[6] (8 3)[1] (6 3)[8] (6 1)[3] (1 6)[2] (4 6)[7] (4 7)[2] (2 6)[3] (3 6)[8] (2 0)[7] (2 7)[6] (2 5)[1] (2 2)[2] (8 2)[6] (8 4)[3] (2 3)[5] (8 0)[4] (0 3)[2] (3 4)[4] (4 4)[5] (8 1)[2] (3 5)[3] (4 2)[3] (1 2)[1] (4 0)[6] (0 0)[3] (4 1)[4] (0 7)[7] (0 5)[6] (1 4)[7] (0 4)[9] (1 5)[4] (1 1)[6] (6 5)[7] (2 4)[8] (0 1)[5] (6 4)[6] (2 1)[9] (3 1)[1] (3 2)[7]
3 8 7 2 6 9 1 5 4 5 6 9 1 4 8 3 7 2 4 1 2 7 3 5 9 8 6 2 3 5 6 9 7 8 4 1 9 7 8 4 5 1 6 2 3 6 4 1 3 8 2 7 9 5 1 2 3 8 7 4 5 6 9 7 9 6 5 2 3 4 1 8 8 5 4 9 1 6 2 3 7
```

Generating multiple grids of all difficulties

```
SudokuGenerator sudokuGenerator = new SudokuGenerator(gridsPerLevel: 1);
sudokuGenerator.ExecuteAll();
```

Will produce

```
Generation:1
Wait... Easy    Time: 3879 ms; Tuples: 0; Cells:45
Wait... Medium  Time: 1641 ms; Tuples:31; Cells:35
Wait... Hard    Time: 1613 ms; Tuples: 0; Cells:24
Wait... Evil    Time:  112 ms; Tuples:23; Cells:23

Easy
0 8 1 0 6 9 0 2 0 4 0 0 7 8 0 0 1 0 6 9 0 0 4 2 3 5 0 9 0 6 0 5 0 1 8 0 5 1 8 0 3 4 9 0 2 0 7 3 0 0 0 0 0 6 0 2 0 0 9 5 8 6 7 0 0 0 4 0 1 0 0 5 7 3 0 8 2 0 4 9 0

Medium
0 8 1 0 6 9 0 2 0 4 0 0 7 8 0 0 0 0 0 9 0 0 0 2 3 5 0 9 0 6 0 5 0 1 8 0 0 0 8 0 3 4 0 0 0 0 7 0 0 0 0 0 0 6 0 2 0 0 9 5 0 6 7 0 0 0 4 0 1 0 0 5 7 3 0 8 0 0 4 9 0

Hard
0 8 1 0 0 9 0 2 0 0 0 0 7 0 0 0 0 0 0 0 0 0 0 0 0 5 0 9 0 6 0 5 0 1 8 0 0 0 0 0 3 4 0 0 0 0 0 0 0 0 0 0 0 6 0 2 0 0 0 0 0 6 7 0 0 0 4 0 1 0 0 5 7 3 0 8 0 0 0 9 0

Evil
0 8 1 0 0 0 0 2 0 0 0 0 7 0 0 0 0 0 0 0 0 0 0 0 0 5 0 9 0 6 0 5 0 1 8 0 0 0 0 0 3 4 0 0 0 0 0 0 0 0 0 0 0 6 0 2 0 0 0 0 0 6 7 0 0 0 4 0 1 0 0 5 7 3 0 8 0 0 0 9 0
```
