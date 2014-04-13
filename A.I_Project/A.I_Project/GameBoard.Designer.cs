namespace A.I_Project
{
    partial class GameBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.endButton = new System.Windows.Forms.Button();
            this.humanLabel = new System.Windows.Forms.Label();
            this.machineLabel = new System.Windows.Forms.Label();
            this.boardPanel = new System.Windows.Forms.Panel();
            this.GameDrawLabel = new System.Windows.Forms.Label();
            this.boardPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.Location = new System.Drawing.Point(67, 268);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(85, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // endButton
            // 
            this.endButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endButton.Location = new System.Drawing.Point(195, 268);
            this.endButton.Name = "endButton";
            this.endButton.Size = new System.Drawing.Size(85, 23);
            this.endButton.TabIndex = 1;
            this.endButton.Text = "End";
            this.endButton.UseVisualStyleBackColor = true;
            this.endButton.Click += new System.EventHandler(this.endButton_Click);
            // 
            // humanLabel
            // 
            this.humanLabel.AutoSize = true;
            this.humanLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.humanLabel.Location = new System.Drawing.Point(75, 23);
            this.humanLabel.Name = "humanLabel";
            this.humanLabel.Size = new System.Drawing.Size(86, 20);
            this.humanLabel.TabIndex = 2;
            this.humanLabel.Text = "Human (X)";
            // 
            // machineLabel
            // 
            this.machineLabel.AutoSize = true;
            this.machineLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineLabel.Location = new System.Drawing.Point(185, 23);
            this.machineLabel.Name = "machineLabel";
            this.machineLabel.Size = new System.Drawing.Size(95, 20);
            this.machineLabel.TabIndex = 3;
            this.machineLabel.Text = "Machine (O)";
            // 
            // boardPanel
            // 
            this.boardPanel.Controls.Add(this.GameDrawLabel);
            this.boardPanel.Location = new System.Drawing.Point(53, 61);
            this.boardPanel.Name = "boardPanel";
            this.boardPanel.Size = new System.Drawing.Size(239, 186);
            this.boardPanel.TabIndex = 4;
            // 
            // GameDrawLabel
            // 
            this.GameDrawLabel.AutoSize = true;
            this.GameDrawLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameDrawLabel.Location = new System.Drawing.Point(71, 84);
            this.GameDrawLabel.Name = "GameDrawLabel";
            this.GameDrawLabel.Size = new System.Drawing.Size(94, 20);
            this.GameDrawLabel.TabIndex = 5;
            this.GameDrawLabel.Text = "Game Draw";
            this.GameDrawLabel.Visible = false;
            // 
            // GameBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 301);
            this.Controls.Add(this.boardPanel);
            this.Controls.Add(this.machineLabel);
            this.Controls.Add(this.humanLabel);
            this.Controls.Add(this.endButton);
            this.Controls.Add(this.startButton);
            this.Name = "GameBoard";
            this.Text = "GameBoard";
            this.boardPanel.ResumeLayout(false);
            this.boardPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button endButton;
        private System.Windows.Forms.Label humanLabel;
        private System.Windows.Forms.Label machineLabel;
        private System.Windows.Forms.Panel boardPanel;
        private System.Windows.Forms.Label GameDrawLabel;
    }
}

