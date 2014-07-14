using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Daily
{
    public partial class Form1 : Form
    {
        TaskList model = new TaskList();

        public Form1()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            ReadTasks();
            checkedListBoxTasks.DataSource = model.Data;
        }

        /// <summary>
        /// タスク一覧をテキストファイルから読み込む
        /// </summary>
        private void ReadTasks()
        {
            model.ReadTasks(Properties.Settings.Default.Tasks);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = checkedListBoxTasks.SelectedIndex;
            if (idx < 0)
            {
                return;
            }
            CheckItem(idx);
            
        }

        /// <summary>
        /// アイテムをチェックし、ログに出力する
        /// </summary>
        /// <param name="idx"></param>
        private void CheckItem(int idx)
        {
            Task item = (Task)checkedListBoxTasks.SelectedItem;

            string output = DateTime.Now + "," + item.Title + "," + checkedListBoxTasks.GetItemChecked(idx);
            string path = Properties.Settings.Default.LogFile;

            if (!File.Exists(path))
            {
                File.CreateText(path);
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(output);
                Console.WriteLine(output);
            }
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();   
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           DialogResult result = MessageBox.Show("終了しますか？",
            "Dailyの終了",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);

           if (result == DialogResult.Yes)
           {
               Properties.Settings.Default.Save();
               notifyIcon1.Visible = false;
           }
           else if (result == DialogResult.No)
           {
               e.Cancel = true; 
               WindowState = FormWindowState.Minimized;
           }
           else if (result == DialogResult.Cancel)
           {
               e.Cancel = true;
           }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ウィンドウの位置・サイズを復元
            Bounds = Properties.Settings.Default.Bounds;
            WindowState = Properties.Settings.Default.WindowState;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckTaskNotification();
        }

        /// <summary>
        /// タスクの実行時刻が来たら通知する
        /// </summary>
        private void CheckTaskNotification()
        {
            List<string> ballon = model.CheckTaskNotification();
            if (ballon.Count > 0)
            {
                notifyIcon1.BalloonTipTitle = "お知らせ";
                notifyIcon1.BalloonTipText = string.Join(Environment.NewLine, ballon);
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.ShowBalloonTip(10000);
            }

        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }   
        }
    }
}
