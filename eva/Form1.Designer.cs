﻿
namespace EorzeaVirtualAssistant
{
    partial class EVAInitPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EVAInitPopup));
            this.dataDirPath = new System.Windows.Forms.TextBox();
            this.dataDirLabel = new System.Windows.Forms.Label();
            this.dataDirButton = new System.Windows.Forms.Button();
            this.gameDirInstructions = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.saintLink = new System.Windows.Forms.LinkLabel();
            this.gameDirButton = new System.Windows.Forms.Button();
            this.gameDirPath = new System.Windows.Forms.TextBox();
            this.gameDirLabel = new System.Windows.Forms.Label();
            this.dataDirInstructions = new System.Windows.Forms.Label();
            this.xivapiLink = new System.Windows.Forms.LinkLabel();
            this.InitFinished = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dataDirPath
            // 
            this.dataDirPath.Location = new System.Drawing.Point(131, 134);
            this.dataDirPath.Name = "dataDirPath";
            this.dataDirPath.Size = new System.Drawing.Size(380, 20);
            this.dataDirPath.TabIndex = 0;
            this.dataDirPath.Text = "C:\\ProgramData\\EorzeaVirtualAssistant\\";
            // 
            // dataDirLabel
            // 
            this.dataDirLabel.AutoSize = true;
            this.dataDirLabel.Location = new System.Drawing.Point(7, 137);
            this.dataDirLabel.Name = "dataDirLabel";
            this.dataDirLabel.Size = new System.Drawing.Size(79, 13);
            this.dataDirLabel.TabIndex = 1;
            this.dataDirLabel.Text = "EVA Data Path";
            // 
            // dataDirButton
            // 
            this.dataDirButton.Image = ((System.Drawing.Image)(resources.GetObject("dataDirButton.Image")));
            this.dataDirButton.Location = new System.Drawing.Point(517, 132);
            this.dataDirButton.Name = "dataDirButton";
            this.dataDirButton.Size = new System.Drawing.Size(36, 23);
            this.dataDirButton.TabIndex = 2;
            this.dataDirButton.UseVisualStyleBackColor = true;
            this.dataDirButton.Click += new System.EventHandler(this.DataDirButton_Click);
            // 
            // gameDirInstructions
            // 
            this.gameDirInstructions.AutoSize = true;
            this.gameDirInstructions.Location = new System.Drawing.Point(7, 9);
            this.gameDirInstructions.Name = "gameDirInstructions";
            this.gameDirInstructions.Size = new System.Drawing.Size(74, 13);
            this.gameDirInstructions.TabIndex = 3;
            this.gameDirInstructions.Text = "<placeholder>";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(258, 98);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(295, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "I do not wish to have data extracted from my game client.";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // saintLink
            // 
            this.saintLink.AutoSize = true;
            this.saintLink.Location = new System.Drawing.Point(7, 163);
            this.saintLink.Name = "saintLink";
            this.saintLink.Size = new System.Drawing.Size(187, 13);
            this.saintLink.TabIndex = 5;
            this.saintLink.TabStop = true;
            this.saintLink.Text = "https://github.com/ufx/SaintCoinach/";
            this.saintLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SaintLink_LinkClicked);
            // 
            // gameDirButton
            // 
            this.gameDirButton.Image = ((System.Drawing.Image)(resources.GetObject("gameDirButton.Image")));
            this.gameDirButton.Location = new System.Drawing.Point(517, 75);
            this.gameDirButton.Name = "gameDirButton";
            this.gameDirButton.Size = new System.Drawing.Size(36, 23);
            this.gameDirButton.TabIndex = 6;
            this.gameDirButton.UseVisualStyleBackColor = true;
            this.gameDirButton.Click += new System.EventHandler(this.GameDirButton_Click);
            // 
            // gameDirPath
            // 
            this.gameDirPath.Location = new System.Drawing.Point(131, 77);
            this.gameDirPath.Name = "gameDirPath";
            this.gameDirPath.Size = new System.Drawing.Size(380, 20);
            this.gameDirPath.TabIndex = 7;
            this.gameDirPath.Text = "C:\\Program Files (x86)\\SquareEnix\\FINAL FANTASY XIV - A Realm Reborn\\";
            // 
            // gameDirLabel
            // 
            this.gameDirLabel.AutoSize = true;
            this.gameDirLabel.Location = new System.Drawing.Point(7, 80);
            this.gameDirLabel.Name = "gameDirLabel";
            this.gameDirLabel.Size = new System.Drawing.Size(118, 13);
            this.gameDirLabel.TabIndex = 8;
            this.gameDirLabel.Text = "FFXIV Game Directory: ";
            // 
            // dataDirInstructions
            // 
            this.dataDirInstructions.AutoSize = true;
            this.dataDirInstructions.Location = new System.Drawing.Point(7, 117);
            this.dataDirInstructions.Name = "dataDirInstructions";
            this.dataDirInstructions.Size = new System.Drawing.Size(527, 13);
            this.dataDirInstructions.TabIndex = 9;
            this.dataDirInstructions.Text = "If you wish to change the default location where your parsed or extracted data wi" +
    "ll end up, please do so below.";
            // 
            // xivapiLink
            // 
            this.xivapiLink.AutoSize = true;
            this.xivapiLink.Location = new System.Drawing.Point(195, 163);
            this.xivapiLink.Name = "xivapiLink";
            this.xivapiLink.Size = new System.Drawing.Size(98, 13);
            this.xivapiLink.TabIndex = 10;
            this.xivapiLink.TabStop = true;
            this.xivapiLink.Text = "https://xivapi.com/";
            this.xivapiLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.XivapiLink_LinkClicked);
            // 
            // InitFinished
            // 
            this.InitFinished.Location = new System.Drawing.Point(478, 160);
            this.InitFinished.Name = "InitFinished";
            this.InitFinished.Size = new System.Drawing.Size(75, 23);
            this.InitFinished.TabIndex = 11;
            this.InitFinished.Text = "Finished";
            this.InitFinished.UseVisualStyleBackColor = true;
            this.InitFinished.Click += new System.EventHandler(this.InitFinished_Click);
            // 
            // EVAInitPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 190);
            this.Controls.Add(this.InitFinished);
            this.Controls.Add(this.xivapiLink);
            this.Controls.Add(this.dataDirInstructions);
            this.Controls.Add(this.gameDirLabel);
            this.Controls.Add(this.gameDirPath);
            this.Controls.Add(this.gameDirButton);
            this.Controls.Add(this.saintLink);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.gameDirInstructions);
            this.Controls.Add(this.dataDirButton);
            this.Controls.Add(this.dataDirLabel);
            this.Controls.Add(this.dataDirPath);
            this.Name = "EVAInitPopup";
            this.Text = "Initializing EorzeaVirtualAssistant";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dataDirPath;
        private System.Windows.Forms.Label dataDirLabel;
        private System.Windows.Forms.Button dataDirButton;
        private System.Windows.Forms.Label gameDirInstructions;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.LinkLabel saintLink;
        private System.Windows.Forms.Button gameDirButton;
        private System.Windows.Forms.TextBox gameDirPath;
        private System.Windows.Forms.Label gameDirLabel;
        private System.Windows.Forms.Label dataDirInstructions;
        private System.Windows.Forms.LinkLabel xivapiLink;
        private System.Windows.Forms.Button InitFinished;
    }
}

