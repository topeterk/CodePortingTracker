//MIT License
//
//Copyright(c) 2016 Peter Kirmeier
//
//Permission Is hereby granted, free Of charge, to any person obtaining a copy
//of this software And associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
//copies of the Software, And to permit persons to whom the Software Is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice And this permission notice shall be included In all
//copies Or substantial portions of the Software.
//
//THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
//IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
//LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
//OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Windows.Forms;

namespace CodePortingTracker
{
    public partial class MainForm : Form
    {
        #region Loading form and handle command line options
        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadProject(String Filename, bool bDoCreate, String ErrorMessage)
        {
            try
            {
                Form form = new ProjectForm(Filename, bDoCreate);
                form.MdiParent = this;
                form.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Update Version
            Text += " - v" + Application.ProductVersion;

            // Set size of window to 80 percent by default
            Width = (int)(Screen.GetWorkingArea(DesktopLocation).Width * 0.80);
            Height = (int)(Screen.GetWorkingArea(DesktopLocation).Height * 0.80);
            CenterToScreen();

            // Load project from command line argument (when available)
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                LoadProject(Environment.GetCommandLineArgs()[1], false, "Project cannot be loaded!");
            }
        }
        #endregion

        #region Form controls' handler
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/topeterk/CodePortingTracker");
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.FileName = "*.cptprj";
            openFileDialog1.Filter = "Code Porting Tracker Project (*.cptprj)|*.cptprj|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.DefaultExt = "";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                LoadProject(openFileDialog1.FileName, false, "Project cannot be loaded!");
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.FileName = "MyCptProject";
            openFileDialog1.Filter = "Code Porting Tracker Project (*.cptprj)|*.cptprj|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.CheckFileExists = false; 
            openFileDialog1.DefaultExt = "cptprj";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if (System.IO.File.Exists(openFileDialog1.FileName))
                {
                    if (MessageBox.Show("Overwrite file?", "File already exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                }
                LoadProject(openFileDialog1.FileName, true, "Project cannot be created!");
            }
        }
        #endregion
    }

    #region Main program entry
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
    #endregion
}
