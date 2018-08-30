namespace Sudoku
{
	partial class Form1
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
			this.panelBoard = new System.Windows.Forms.Panel();
			this.buttonReset = new System.Windows.Forms.Button();
			this.buttonGenerate = new System.Windows.Forms.Button();
			this.checkBoxAutoplay = new System.Windows.Forms.CheckBox();
			this.buttonSingle = new System.Windows.Forms.Button();
			this.buttonHiddenSingle = new System.Windows.Forms.Button();
			this.buttonNakedPair = new System.Windows.Forms.Button();
			this.textBoxSource = new System.Windows.Forms.TextBox();
			this.buttonLoad = new System.Windows.Forms.Button();
			this.buttonSinglePair = new System.Windows.Forms.Button();
			this.buttonPointingPair = new System.Windows.Forms.Button();
			this.buttonXReduction = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// panelBoard
			// 
			this.panelBoard.BackColor = System.Drawing.Color.Black;
			this.panelBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelBoard.Location = new System.Drawing.Point(12, 12);
			this.panelBoard.Name = "panelBoard";
			this.panelBoard.Size = new System.Drawing.Size(726, 726);
			this.panelBoard.TabIndex = 0;
			// 
			// buttonReset
			// 
			this.buttonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonReset.Location = new System.Drawing.Point(744, 12);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(131, 53);
			this.buttonReset.TabIndex = 1;
			this.buttonReset.Text = "Reset";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// buttonGenerate
			// 
			this.buttonGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonGenerate.Location = new System.Drawing.Point(744, 71);
			this.buttonGenerate.Name = "buttonGenerate";
			this.buttonGenerate.Size = new System.Drawing.Size(131, 53);
			this.buttonGenerate.TabIndex = 2;
			this.buttonGenerate.Text = "Generate";
			this.buttonGenerate.UseVisualStyleBackColor = true;
			this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
			// 
			// checkBoxAutoplay
			// 
			this.checkBoxAutoplay.AutoSize = true;
			this.checkBoxAutoplay.Checked = true;
			this.checkBoxAutoplay.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAutoplay.Location = new System.Drawing.Point(745, 720);
			this.checkBoxAutoplay.Name = "checkBoxAutoplay";
			this.checkBoxAutoplay.Size = new System.Drawing.Size(67, 17);
			this.checkBoxAutoplay.TabIndex = 3;
			this.checkBoxAutoplay.Text = "Autoplay";
			this.checkBoxAutoplay.UseVisualStyleBackColor = true;
			// 
			// buttonSingle
			// 
			this.buttonSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonSingle.Location = new System.Drawing.Point(745, 149);
			this.buttonSingle.Name = "buttonSingle";
			this.buttonSingle.Size = new System.Drawing.Size(131, 53);
			this.buttonSingle.TabIndex = 4;
			this.buttonSingle.Text = "Single";
			this.buttonSingle.UseVisualStyleBackColor = true;
			this.buttonSingle.Click += new System.EventHandler(this.buttonOnlyOneNote_Click);
			// 
			// buttonHiddenSingle
			// 
			this.buttonHiddenSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonHiddenSingle.Location = new System.Drawing.Point(745, 208);
			this.buttonHiddenSingle.Name = "buttonHiddenSingle";
			this.buttonHiddenSingle.Size = new System.Drawing.Size(131, 53);
			this.buttonHiddenSingle.TabIndex = 5;
			this.buttonHiddenSingle.Text = "Hidden single";
			this.buttonHiddenSingle.UseVisualStyleBackColor = true;
			this.buttonHiddenSingle.Click += new System.EventHandler(this.buttonOnlyOneInLine_Click);
			// 
			// buttonNakedPair
			// 
			this.buttonNakedPair.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonNakedPair.Location = new System.Drawing.Point(745, 267);
			this.buttonNakedPair.Name = "buttonNakedPair";
			this.buttonNakedPair.Size = new System.Drawing.Size(131, 53);
			this.buttonNakedPair.TabIndex = 6;
			this.buttonNakedPair.Text = "Naked Pair";
			this.buttonNakedPair.UseVisualStyleBackColor = true;
			this.buttonNakedPair.Click += new System.EventHandler(this.buttonNakedPair_Click);
			// 
			// textBoxSource
			// 
			this.textBoxSource.Location = new System.Drawing.Point(13, 745);
			this.textBoxSource.Name = "textBoxSource";
			this.textBoxSource.Size = new System.Drawing.Size(644, 20);
			this.textBoxSource.TabIndex = 7;
			this.textBoxSource.Text = "source.txt";
			// 
			// buttonLoad
			// 
			this.buttonLoad.Location = new System.Drawing.Point(663, 745);
			this.buttonLoad.Name = "buttonLoad";
			this.buttonLoad.Size = new System.Drawing.Size(75, 23);
			this.buttonLoad.TabIndex = 8;
			this.buttonLoad.Text = "Load";
			this.buttonLoad.UseVisualStyleBackColor = true;
			this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
			// 
			// buttonSinglePair
			// 
			this.buttonSinglePair.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonSinglePair.Location = new System.Drawing.Point(745, 326);
			this.buttonSinglePair.Name = "buttonSinglePair";
			this.buttonSinglePair.Size = new System.Drawing.Size(131, 53);
			this.buttonSinglePair.TabIndex = 9;
			this.buttonSinglePair.Text = "Single Pair";
			this.buttonSinglePair.UseVisualStyleBackColor = true;
			this.buttonSinglePair.Click += new System.EventHandler(this.buttonSinglePair_Click);
			// 
			// buttonPointingPair
			// 
			this.buttonPointingPair.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonPointingPair.Location = new System.Drawing.Point(745, 385);
			this.buttonPointingPair.Name = "buttonPointingPair";
			this.buttonPointingPair.Size = new System.Drawing.Size(131, 53);
			this.buttonPointingPair.TabIndex = 10;
			this.buttonPointingPair.Text = "Pointing Pair";
			this.buttonPointingPair.UseVisualStyleBackColor = true;
			this.buttonPointingPair.Click += new System.EventHandler(this.buttonPointingPair_Click);
			// 
			// buttonXReduction
			// 
			this.buttonXReduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.buttonXReduction.Location = new System.Drawing.Point(744, 444);
			this.buttonXReduction.Name = "buttonXReduction";
			this.buttonXReduction.Size = new System.Drawing.Size(131, 53);
			this.buttonXReduction.TabIndex = 11;
			this.buttonXReduction.Text = "X Reduction";
			this.buttonXReduction.UseVisualStyleBackColor = true;
			this.buttonXReduction.Click += new System.EventHandler(this.buttonXReduction_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(888, 784);
			this.Controls.Add(this.buttonXReduction);
			this.Controls.Add(this.buttonPointingPair);
			this.Controls.Add(this.buttonSinglePair);
			this.Controls.Add(this.buttonLoad);
			this.Controls.Add(this.textBoxSource);
			this.Controls.Add(this.buttonNakedPair);
			this.Controls.Add(this.buttonHiddenSingle);
			this.Controls.Add(this.buttonSingle);
			this.Controls.Add(this.checkBoxAutoplay);
			this.Controls.Add(this.buttonGenerate);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.panelBoard);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sudoku";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panelBoard;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.CheckBox checkBoxAutoplay;
		private System.Windows.Forms.Button buttonSingle;
		private System.Windows.Forms.Button buttonHiddenSingle;
		private System.Windows.Forms.Button buttonNakedPair;
		private System.Windows.Forms.TextBox textBoxSource;
		private System.Windows.Forms.Button buttonLoad;
		private System.Windows.Forms.Button buttonSinglePair;
		private System.Windows.Forms.Button buttonPointingPair;
		private System.Windows.Forms.Button buttonXReduction;
	}
}

