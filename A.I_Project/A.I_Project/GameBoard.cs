using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace A.I_Project
{
    public partial class GameBoard : Form
    {
        private GameBoardState gameBoardState;
        private bool isMachine;

        private String lastHumanButtonName;
        private String lastMachineButtonName;

        private bool machineFirstMove;

        public GameBoard()
        {
            InitializeComponent();
            int mCount = 4;
            int nCount = 5;
            int kCount = 4;

            int start_x = 10;
            int start_y = 20;
            int CellWidth = 40;
            int CellHeight = 40;

            // initialize game board
            for (int i = 0; i < mCount; i++)
            {
                for (int j = 0; j < nCount; j++)
                {
                    Button cellButton = new Button();
                    cellButton.Top = start_x + i * CellHeight;
                    cellButton.Left = start_y + j * CellWidth;
                    cellButton.Width = CellWidth;
                    cellButton.Height = CellHeight;
                    cellButton.Text = "";
                    cellButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, 
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    cellButton.Name = string.Format("button_{0}_{1}", i, j);
                    cellButton.Click += new EventHandler(cellClick);
                    cellButton.BackColor = Color.Gray;
                    this.boardPanel.Controls.Add(cellButton);
                }
            }

            // human starts first
            isMachine = false;
            machineFirstMove = true;
            this.gameBoardState = new GameBoardState(mCount, nCount, kCount);
        }

        void cellClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            String[] tokens = clickedButton.Name.Split('_');
            ClickOnButton(clickedButton);
            List<String> configList = new List<String>();
            // human plays a move
            GameState gameState = this.gameBoardState.PerformClick(
                int.Parse(tokens[1]), int.Parse(tokens[2]), isMachine, out configList);
            UpdateBoardBasedOnState(gameState, configList);

            // machine is on
            if (isMachine)
            {
                int row = -1;
                int column = -1;

                // in all game levels, this is common
                // if we have a chance to win by clicking anywhere, click
                // else if we know of a cell where opponent can win by clicking
                // we will try to stop
                bool machineCanWin = this.gameBoardState.GetWinningCell(
                    isMachine, out row, out column);
                if (machineCanWin)
                {
                    gameUpdate(row, column, isMachine);
                    return;
                }

                bool humanCanWin = this.gameBoardState.GetWinningCell(
                    !isMachine, out row, out column);
                if (humanCanWin)
                {
                    gameUpdate(row, column, isMachine);
                    return;
                }

                // row and column are unchanged in this stage
                // Hard strategy
                GameBoardState machineTempBoardState = new GameBoardState(this.gameBoardState);
                // try to figure out if machine wins in next one move
                machineTempBoardState.GetBestCell(isMachine, 
                     out row, out column);
                int tempRow = row;
                int tempColumn = column;
                machineTempBoardState.PerformClick(row, column,
                    isMachine, out configList);
                bool machineCanWinSecond = machineTempBoardState.GetWinningCell(isMachine, out row, out column);
                // row and column changed twice
                // first change started at GetBestCell
                // second change started at GetWinningCell
                // the change at GetBestCell should be saved 
                // if machne can win in one step 
                if (machineCanWinSecond || machineFirstMove)
                {
                    row = tempRow;
                    column = tempColumn;
                    machineFirstMove = false;
                }
                // row and column is unchanged 
                // if the winning cell is unfound
                else 
                {
                    // try to figure out if human wins in next two moves
                    GameBoardState humanTempBoardState = new GameBoardState(this.gameBoardState);
                    humanTempBoardState.GetBestCell(!isMachine, out row, out column);
                    tempRow = row;
                    tempColumn = column;
                    humanTempBoardState.PerformClick(row, column, 
                        !isMachine, out configList);
                    bool humanCanWinSecond = humanTempBoardState.GetWinningCell(
                        !isMachine, out row, out column);

                    if (humanCanWinSecond)
                    {
                        row = tempRow;
                        column = tempColumn;
                    }
                    else
                    {
                        Console.WriteLine("Alpha Beta");
                        // Apply the Alpha Beta Pruning
                        GameBoardState alphaBetaBoardState = new GameBoardState(
                            this.gameBoardState);

                        // if this depth increases, the time will increases
                        // but there is a hard limit of 10 seconds
                        int[] bestMove = alphaBetaBoardState.Minimax(
                            20, isMachine, int.MinValue, int.MaxValue, 
                            row, column, DateTime.Now);
                        row = bestMove[1];
                        column = bestMove[2];
                    }
                }

                gameUpdate(row, column, isMachine);
                return;
            }
        }

        private void gameUpdate(int row, int column, bool isMachine)
        {
            List<String> configList = new List<String>();
            this.ClickOnCell(row, column);
            GameState gameState = this.gameBoardState.PerformClick(row, column, 
                isMachine, out configList);
            UpdateBoardBasedOnState(gameState, configList);
        }

        private void ClickOnButton(Button clickedButton)
        {
            String lastClickedButtonName;
            if (isMachine)
            {
                lastClickedButtonName = lastHumanButtonName;
            }
            else
            {
                lastClickedButtonName = lastMachineButtonName;
            }

            if (!String.IsNullOrEmpty(lastClickedButtonName))
            { 
                (this.boardPanel.Controls.Find(lastClickedButtonName, false)[0]
                    as Button).BackColor = Color.Beige;
            }

            clickedButton.Text = isMachine ? "O" : "X";
            clickedButton.Enabled = false;
            if (isMachine)
            {
                clickedButton.BackColor = Color.Pink;
            }
            else
            {
                clickedButton.BackColor = Color.Yellow;
            }

            if (isMachine)
            {
                lastMachineButtonName = clickedButton.Name;
            }
            else 
            {
                lastHumanButtonName = clickedButton.Name;
            }
        }

        private void ClickOnCell(int row, int column)
        { 
            // find button
            String buttonName = String.Format("button_{0}_{1}", row, column);
            ClickOnButton(this.boardPanel.Controls.Find(
                buttonName, false)[0] as Button); 
        }

        public void UpdateBoardBasedOnState(GameState gameState, List<String> configList)
        {
            if (gameState == GameState.HumanWin)
            {
                humanLabel.Text += " Winner";
                humanLabel.BackColor = Color.Azure;
                DisableAllCells(configList);
            }
            else if (gameState == GameState.MachineWin)
            {
                machineLabel.Text += " Winner";
                machineLabel.BackColor = Color.Azure;
                DisableAllCells(configList);
            }
            else if (gameState == GameState.Draw)
            {
                this.GameDrawLabel.Visible = true;
            }
            else
            {
                isMachine = !isMachine;
            }
        }

        private void DisableAllCells(List<String> highlightList)
        {
            for (int i = 0; i < highlightList.Count; i++)
            {
                highlightList[i] = String.Format("button_{0}", highlightList[i]);
            }

            foreach (Control control in boardPanel.Controls)
            {
                control.Enabled = false;
                if (highlightList.Contains(control.Name))
                {
                    control.BackColor = Color.Aquamarine;
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

    }
}
