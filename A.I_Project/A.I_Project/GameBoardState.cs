using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A.I_Project
{
    public enum GameState
    { 
        NoChange,
        HumanWin,
        MachineWin,
        Draw
    }

    public enum Direction
    { 
        Up,
        Down,
        Left,
        Right,
        LeftUp,
        LeftDown,
        RightUp,
        RightDown
    }

    class GameBoardState
    {
        public bool?[,] boardState;
        private int mCount;
        private int nCount;
        private int kCount;
        private int MaxStoneCount;
        //the number of stone on the board
        private int CurrentStoneCount;

        //Constructor
        public GameBoardState(int m, int n, int k)
        {
            mCount = m;
            nCount = n;
            kCount = k;
            boardState = new bool?[mCount, nCount];
            MaxStoneCount = mCount * nCount;
            CurrentStoneCount = 0;

            for (int i = 0; i < mCount; i++)
            {
                for (int j = 0; j < nCount; j++)
                {
                    boardState[i, j] = null;
                }
            }
        }

        public GameBoardState(GameBoardState boardToCopy)
        {
            mCount = boardToCopy.mCount;
            nCount = boardToCopy.nCount;
            kCount = boardToCopy.kCount;
            boardState = new bool?[mCount, nCount];
            MaxStoneCount = boardToCopy.MaxStoneCount;
            CurrentStoneCount = boardToCopy.CurrentStoneCount;

            for (int i = 0; i < mCount; i++)
            {
                for (int j = 0; j < nCount; j++)
                {
                    boardState[i, j] = boardToCopy.boardState[i, j];
                }
            }
        }

        public GameState PerformClick(int row, int column, bool isMachine, out List<String> configList)
        {
            CurrentStoneCount++;
            if (boardState[row, column] != null)
            {
                throw new Exception("Already clicked");
            }

            boardState[row, column] = isMachine;
            return GetGameState(row, column, isMachine, false, out configList);
        }

        public void UndoClick(int row, int column)
        {
            CurrentStoneCount--;
            boardState[row, column] = null;
        }

        // return the state of the game based on the currently clicked item
        public GameState GetGameState(int row, int column,
            bool isMachine, bool isBlankConsidered, out List<String> configList)
        {
            configList = new List<String>();
            configList.Add(String.Format("{0}_{1}", row, column));

            bool won = false;
            int count = 1;

            List<String> configListLeftUp = new List<String>();
            List<String> configListRightUp = new List<String>();
            List<String> configListLeftDown = new List<String>();
            List<String> configListRightDown = new List<String>();
            List<String> configListLeft = new List<String>();
            List<String> configListRight = new List<String>();
            List<String> configListUp = new List<String>();
            List<String> configListDown = new List<String>();
            int countLeftUp = GetCountsInDirection(Direction.LeftUp,
                row, column, isMachine, isBlankConsidered, out configListLeftUp);
            int countRightUp = GetCountsInDirection(Direction.RightUp,
                row, column, isMachine, isBlankConsidered, out configListRightUp);
            int countLeftDown = GetCountsInDirection(Direction.LeftDown,
                row, column, isMachine, isBlankConsidered, out configListLeftDown);
            int countRightDown = GetCountsInDirection(Direction.RightDown,
                row, column, isMachine, isBlankConsidered, out configListRightDown);
            int countLeft = GetCountsInDirection(Direction.Left,
                row, column, isMachine, isBlankConsidered, out configListLeft);
            int countRight = GetCountsInDirection(Direction.Right,
                row, column, isMachine, isBlankConsidered, out configListRight);
            int countUp = GetCountsInDirection(Direction.Up,
                row, column, isMachine, isBlankConsidered, out configListUp);
            int countDown = GetCountsInDirection(Direction.Down,
                row, column, isMachine, isBlankConsidered, out configListDown);

            if ((count + countLeftUp + countRightDown) >= kCount)
            {
                won = true;

                foreach (String entry in configListLeftUp)
                    configList.Add(entry);
                foreach (String entry in configListRightDown)
                    configList.Add(entry);
            }
            else if ((count + countRightUp + countLeftDown) >= kCount)
            {
                won = true;

                foreach (String entry in configListRightUp)
                    configList.Add(entry);
                foreach (String entry in configListLeftDown)
                    configList.Add(entry);
            }
            else if ((count + countUp + countDown) >= kCount)
            {
                won = true;

                foreach (String entry in configListUp)
                    configList.Add(entry);
                foreach (String entry in configListDown)
                    configList.Add(entry);
            }
            else if ((count + countRight + countLeft) >= kCount)
            {
                won = true;

                foreach (String entry in configListRight)
                    configList.Add(entry);
                foreach (String entry in configListLeft)
                    configList.Add(entry);
            }

            if (won)
            {
                if (isMachine)
                {
                    return GameState.MachineWin;
                }
                else 
                {
                    return GameState.HumanWin;
                }
            }

            if (CurrentStoneCount == MaxStoneCount)
            {
                return GameState.Draw;
            }

            return GameState.NoChange;
        }

        private int GetCountsInDirection(Direction direction,
            int row, int column, bool isMachine, bool isBlankConsidered,
            out List<String> configList)
        {
            configList = new List<String>();
            int directionCount = 0;
            int rowOffset = 0;
            int columnOffset = 0;
            int startingPointRow = 0;
            int startingPointColumn = 0;
            bool notContinue = false;

            switch (direction)
            { 
                case Direction.LeftDown:
                    rowOffset = 1;
                    columnOffset = -1;
                    break;
                case Direction.LeftUp:
                    rowOffset = -1;
                    columnOffset = -1;
                    break;
                case Direction.RightUp:
                    rowOffset = -1;
                    columnOffset = 1;
                    break;
                case Direction.RightDown:
                    rowOffset = 1;
                    columnOffset = 1;
                    break;
                case Direction.Down:
                    rowOffset = 1;
                    columnOffset = 0;
                    break;
                case Direction.Up:
                    rowOffset = -1;
                    columnOffset = 0;
                    break;
                case Direction.Right:
                    rowOffset = 0;
                    columnOffset = 1;
                    break;
                case Direction.Left:
                    rowOffset = 0;
                    columnOffset = -1;
                    break;
            }

            startingPointRow = row + rowOffset;
            startingPointColumn = column + columnOffset;

            for (int i = startingPointRow, j = startingPointColumn;
                i >= 0 && i < mCount && j >= 0 && j < nCount; 
                i += rowOffset, j += columnOffset)
            {
                if (boardState[i, j] == isMachine && !notContinue)
                {
                    configList.Add(String.Format("{0}_{1}", i, j));
                    directionCount++;
                }
                else if (boardState[i, j] == isMachine || (isBlankConsidered && boardState[i, j] == null))
                {
                    directionCount++;
                    notContinue = true;
                }
                else
                {
                    return directionCount;
                }
            }
            return directionCount;
        }

        public bool GetWinningCell(bool playerOrMachine, out int row, out int column)
        {
            row = -1;
            column = -1;
            bool isMachine = true;
            for(int i = 0; i < mCount; i++)
            {
                for(int j = 0; j < nCount; j++)
                {
                    if (boardState[i, j] == null)
                    {
                        List<string> configList = null;
                        GameState gameState = GetGameState(i, j, playerOrMachine, false, out configList);
                        if ((gameState == GameState.HumanWin && playerOrMachine == !isMachine) ||
                            (gameState == GameState.MachineWin && playerOrMachine == isMachine))
                        {
                            row = i;
                            column = j;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void GetBestCell(bool humanOrMachine, out int row, out int column)
        {
            row = -1;
            column = -1;
            int currentMax = -1;
            bool firstEmptyFound = false;
            for (int i = 0; i < mCount; i++)
            {
                for (int j = 0; j < nCount; j++)
                {
                    if (boardState[i, j] == null)
                    {
                        if (!firstEmptyFound)
                        {
                            row = i;
                            column = j;
                            firstEmptyFound = true;
                        }

                        int score = GetScoreForOneCell(i, j, humanOrMachine);

                        if (score > currentMax)
                        {
                            currentMax = score;
                            row = i;
                            column = j;
                        }
                    }
                }
            }   
        }

        // this function returns the score of each cell 
        // based on chances of selecting this cell for winning
        // this checks in all directions on how far this can go ahead 
        // and use the cells in future
        // the cells which were already clicked take more precedence
        public int GetScoreForOneCell(int row, int column, bool humanOrMachine)
        {
            List<String> configList1 = new List<String>();
            List<String> configList2 = new List<String>();
            
            int maxOfAllCounts = -1;
            int score = 0;
            
            int leftDiagonalCount = 1
                + GetCountsInDirection(Direction.LeftUp, row, column,
                humanOrMachine, true, out configList1)
                + GetCountsInDirection(Direction.RightDown, row, column,
                humanOrMachine, true, out configList2);

            // Consider the counts only if there are chances to win
            if (leftDiagonalCount >= kCount)
            {
                score += leftDiagonalCount
                    + (10 * (configList1.Count + configList1.Count + 1));
                if ((configList1.Count + configList2.Count + 1) > maxOfAllCounts)
                {
                    maxOfAllCounts = configList1.Count + configList2.Count + 1;
                }
            }

            int rightDiagonalCount = 1
                + GetCountsInDirection(Direction.RightUp, row, column,
                humanOrMachine, true, out configList1)
                + GetCountsInDirection(Direction.LeftDown, row, column,
                humanOrMachine, true, out configList2);

            if (rightDiagonalCount >= kCount)
            {
                score += rightDiagonalCount
                    + (10 * (configList1.Count + configList1.Count + 1));
                if ((configList1.Count + configList2.Count + 1) > maxOfAllCounts)
                {
                    maxOfAllCounts = configList1.Count + configList2.Count + 1;
                }
            }

            int horizontalCount = 1
                + GetCountsInDirection(Direction.Left, row, column,
                humanOrMachine, true, out configList1)
                + GetCountsInDirection(Direction.Right, row, column,
                humanOrMachine, true, out configList2);

            if (horizontalCount >= kCount)
            {
                score += horizontalCount
                    + (10 * (configList1.Count + configList1.Count + 1));
                if ((configList1.Count + configList2.Count + 1) > maxOfAllCounts)
                {
                    maxOfAllCounts = configList1.Count + configList2.Count + 1;
                }
            }

            int verticalCount = 1
                + GetCountsInDirection(Direction.Up, row, column,
                humanOrMachine, true, out configList1)
                + GetCountsInDirection(Direction.Down, row, column,
                humanOrMachine, true, out configList2);

            if (verticalCount >= kCount)
            {
                score += verticalCount
                    + (10 * (configList1.Count + configList1.Count + 1));
                if ((configList1.Count + configList2.Count + 1) > maxOfAllCounts)
                {
                    maxOfAllCounts = configList1.Count + configList2.Count + 1;
                }
            }

            if (maxOfAllCounts > 0)
                score += 100 * maxOfAllCounts;

            return score;
        }

        public int GetBoardScore(bool isMachine)
        {
            int score = 0;

            // for each cell, get the scores of horizontal,
            // vertical, all diagonals
            for (int i = 0; i < mCount; i++)
            {
                for (int j = 0; j < nCount; j++)
                {
                    score += GetCellScore(isMachine, i, j);
                }
            }

            return score;
        }

        private int GetCellScore(bool isMachine, int row, int column)
        {
            int score = 0;

            // go in horizontal direction
            score += GetScoreForSequence(isMachine, row, column, 0, 1);

            // go in vertical direction
            score += GetScoreForSequence(isMachine, row, column, 1, 0);

            // go in left diagonal direction
            score += GetScoreForSequence(isMachine, row, column, 1, -1);

            // go in right diagonal direction
            score += GetScoreForSequence(isMachine, row, column, 1, 1);

            return score;
        }

        public int GetScoreForSequence(bool isMachine, int startRow, int startColumn,
            int rowOffset, int columnOffset)
        {
            int machineCurrentPower = 0;
            int humanCurrentPower = 0;

            double score = 0;
            for (int i = startRow, j = startColumn, k = 0;
                i > 0 && i < mCount && j > 0 && j < nCount && k < kCount;
                i += rowOffset, j += columnOffset, k++)
            {
                if (boardState[i, j] == isMachine)
                {
                    score += Math.Pow(10, machineCurrentPower);
                    machineCurrentPower++;
                    if (humanCurrentPower > 0)
                    {
                        humanCurrentPower--;
                    }
                }
                
                else if (boardState[i, j] == !isMachine)
                {
                    score += -1 * Math.Pow(10, humanCurrentPower);
                    humanCurrentPower++;
                    if (machineCurrentPower > 0)
                    {
                        machineCurrentPower--;
                    }
                }
            }

            return (int)score;
        }

        public int[] Minimax(int depth, bool isMachine,
            int alpha, int beta, int lastMoveRow, int lastMoveColumn,
            DateTime startTime)
        {
            // Generate possible next moves in a list of int[2] of {row, col}
            List<int[]> nextMoves = GetNextMoves(lastMoveRow, lastMoveColumn);

            // machineSeed is maximizing; while humanSeed is minimizing
            int bestScore;
            int bestRow = -1;
            int bestColumn = -1;

            if (nextMoves.Count == 0 || depth == 0 || DateTime.Now.AddSeconds(-10) > startTime)
            {
                // Gameover or depth reached, evaluate score
                bestScore = GetBoardScore(isMachine);
                return new int[] { bestScore, bestRow, bestColumn };
            }
            else
            {
                Console.WriteLine("Separation");

                foreach (int[] move in nextMoves)
                {
                    List<String> configList;

                    // Try this move for the current "player"
                    this.PerformClick(move[0], move[1], isMachine, out configList);

                    if (isMachine == true)
                    {
                        // machine is maximizing human
                        bestScore = Minimax(depth - 1, !isMachine,
                            alpha, beta, move[0], move[1], startTime)[0];
                        if (bestScore > alpha)
                        {
                            alpha = bestScore;
                            bestRow = move[0];
                            bestColumn = move[1];
                        }
                        for (int i = 0; i < mCount; i++)
                        {
                            for (int j = 0; j < nCount; j++)
                            {
                                if (boardState[i, j] == true)
                                    Console.Write("O");
                                else if (boardState[i, j] == false)
                                    Console.Write("X");
                                else
                                    Console.Write("_");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine(bestScore);
                        Console.WriteLine();
                    }
                    else
                    {
                        // human is minimizing machine
                        bestScore = Minimax(depth - 1, !isMachine, 
                            alpha, beta, move[0], move[1], startTime)[0];
                        if (bestScore < beta)
                        {
                            beta = bestScore;
                            bestRow = move[0];
                            bestColumn = move[1];
                        }
                        for (int i = 0; i < mCount; i++)
                        {
                            for (int j = 0; j < nCount; j++)
                            {
                                if (boardState[i, j] == true)
                                    Console.Write("O");
                                else if (boardState[i, j] == false)
                                    Console.Write("X");
                                else
                                    Console.Write("_");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine(bestScore);
                        Console.WriteLine();
                    }

                    // Undo move
                    this.UndoClick(move[0], move[1]);

                    // cut off
                    if (alpha >= beta) break;
                } 
            }

            return new int[] { isMachine ? alpha : beta, bestRow, bestColumn };
        }

        private List<int[]> GetNextMoves(int lastMoveRow, int lastMoveColumn)
        {
            List<int[]> nextMoves = new List<int[]>();
            if (lastMoveRow > 0 && lastMoveColumn > 0)
            { 
                // proper move. find if winning or draw
                List<String> configList;
                GameState gameState = GetGameState(lastMoveRow, lastMoveColumn,
                    boardState[lastMoveRow, lastMoveColumn].Value, false, out configList);
                if (gameState != GameState.NoChange)
                {
                    return nextMoves;
                }
            }

            for (int i = 0; i < mCount; i++)
            {
                for (int j = 0; j < nCount; j++)
                {
                    if (boardState[i, j] == null)
                    {
                        nextMoves.Add(new int[] { i, j });
                    }
                }
            }

            return nextMoves;
        }
    }
}
