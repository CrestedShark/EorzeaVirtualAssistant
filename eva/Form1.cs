using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EorzeaVirtualAssistant
{
    public partial class EVAInitPopup : Form
    {
        const string defaultGameDir = "C:\\Program Files(x86)\\SquareEnix\\FINAL FANTASY XIV - A Realm Reborn\\";
        const string defaultDataDir = "C:\\ProgramData\\EorzeaVirtualAssistant\\";

        public EVAInitPopup()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //todo:  clear path
            if (checkBox1.Checked)
            {
                gameDirPath.Text = "";
            }
            else
            {
                gameDirPath.Text = defaultGameDir;
            }
        }

        private void SaintLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(saintLink.Text);
        }

        private void XivapiLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(xivapiLink.Text);
        }

        private void DataDirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                dataDirPath.Text = dlg.SelectedPath;
            }
        }

        private void InitFinished_Click(object sender, EventArgs e)
        {
            Setup.SetupData(gameDirPath.Text, dataDirPath.Text);
            Application.Exit();
        }

        private void GameDirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                gameDirPath.Text = dlg.SelectedPath;
            }
        }
    }
}
