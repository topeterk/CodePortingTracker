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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CodePortingTracker
{
    public partial class ProjectForm : Form
    {
        private readonly SaveModule<CodePortingTrackerProjectFile> sm;
        private CodePortingTrackerProjectFile prj;
        private bool bPrjChangePending = false;
        private List<SourceForm> SrcForms;

        #region Loading project and form
        public ProjectForm(String filename, bool bDoCreate)
        {
            InitializeComponent();
            ProjectForm_SizeChanged(this, null);

            SrcForms = new List<SourceForm>();

            // Load the project..
            sm = new SaveModule<CodePortingTrackerProjectFile>(filename);
            if (bDoCreate)
            {
                // create new project and safe it one time, to be sure it potentionally works later on
                prj = new CodePortingTrackerProjectFile();
                if (!sm.WriteXML(prj))
                    throw new Exception();
            }
            else if (!sm.ReadXML(ref prj))
                throw new Exception();

            Text += " - " + System.IO.Path.GetFileName(filename);

            // add all files from the project file..
            foreach (CodePortingTrackerSourceFile src in prj.Files)
                AddFileToDgv(src.Filename, src.LineCount, src.LinesDone);
        }

        private void ProjectForm_SizeChanged(object sender, EventArgs e)
        {
            dgv.Width = this.ClientSize.Width - 24;
            dgv.Height = this.ClientSize.Height - 39;
        }

        #endregion

        #region Adding and deleting files to/from project

        private void AddFileToDgv(string filename, int LineCount = 0, int LinesDone = 0)
        {
            int idx = dgv.Rows.Add();
            DataGridViewCellCollection cc = dgv.Rows[idx].Cells;

            cc["SrcPath"].Value = filename;
            cc["SrcFile"].Value = System.IO.Path.GetFileName(filename);
            cc["LineCount"].Value = LineCount;
            cc["LinesDone"].Value = LinesDone;
            cc["Completion"].Value = ProjectHelper.GetSrcFilePercentage(LineCount, LinesDone);
        }

        private void AddFilesToProject(string[] filepaths)
        {
            // check for new files and add them..
            foreach (String filename in filepaths)
            {
                bool bFound = false;

                // check if files are already part of project..
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["SrcPath"].Value.ToString() == filename)
                    {
                        bFound = true;
                        break;
                    }
                }

                // in case the file is new, add it to the list..
                if (!bFound)
                {
                    AddFileToDgv(filename);
                    prj.AddSourceFile(filename);
                    bPrjChangePending = true;
                }
            }
        }
 
        private void addFilesToProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                AddFilesToProject(openFileDialog1.FileNames); // Add all selected files
        }
        private void dgv_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            bPrjChangePending = true;
        }

        private void dgv_DragDrop(object sender, DragEventArgs e)
        {
            string[] DroppedPathList = (string[])e.Data.GetData(DataFormats.FileDrop);

            // get all files of the dropped object(s) and add them..
            foreach (string path in DroppedPathList)
            {
                if (Directory.Exists(path))
                    AddFilesToProject(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
                else if (File.Exists(path))
                    AddFilesToProject(new string[1] { path });
            }
        }

        private void dgv_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Link; // Allow dopping files
        }

        #endregion

        #region Saving and closing project

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check for deleted source files from project file..
            for (int i = prj.Files.Count; i > 0; i--)
            {
                CodePortingTrackerSourceFile src = prj.Files[i - 1];
                bool bFound = false;

                // check if files are already part of project..
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    DataGridViewCellCollection cc = row.Cells;
                    if (cc["SrcPath"].Value.ToString() == src.Filename)
                    {
                        // update data accordingly
                        src.LineCount = (int)cc["LineCount"].Value;
                        src.LinesDone = (int)cc["LinesDone"].Value;
                        bFound = true;
                        break;
                    }
                }

                // in case the file was removed, remove it from the list..
                if (!bFound) prj.Files.Remove(src);
            }

            // Save current data..
            if (!sm.WriteXML(prj))
                MessageBox.Show("Project could not be saved!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                bPrjChangePending = false; // changes were saved, so no  more pending changes
                MessageBox.Show("Project saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Are changes pending?
            if (bPrjChangePending)
            {
                DialogResult result = MessageBox.Show(
                    "The project was modified but is not saved." + Environment.NewLine +
                    "Do you want to save changes now?", "Closing project..", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if ((result != DialogResult.Yes) && (result != DialogResult.No)) // cancel on invalid input
                {
                    e.Cancel = true;
                    return;
                }
                if (result == DialogResult.Yes) // user wants to save now?
                {
                    saveToolStripMenuItem_Click(sender, e);
                    if (bPrjChangePending) // saving failed?
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            // Look for open forms.
            if (0 < SrcForms.Count)
            {
                DialogResult result = MessageBox.Show(
                    "Some files of this project are still open." + Environment.NewLine +
                    "Closing the project will close all source files related to this project!" + Environment.NewLine +
                    "Are you sure closing the project?", "Closing project..", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

                // Close all forms..
                for (int i = SrcForms.Count; i > 0; i--)
                    SrcForms[i - 1].Close();
            }
        }

        #endregion

        #region Opening and closing source file form

        private void dgv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0)) // no selector or header
                return;

            string filename = dgv.Rows[e.RowIndex].Cells["SrcPath"].Value.ToString();

            // Check if form is already available
            foreach (SourceForm srcform in SrcForms)
            {
                if (srcform.Filename == filename) // Is form for this file open?
                {
                    srcform.Focus();
                    return;
                }
            }

            // search for the source file data to create new form..
            for (int i = prj.Files.Count; i > 0; i--)
            {
                CodePortingTrackerSourceFile src = prj.Files[i-1];
                if (src.Filename == filename)
                {
                    try
                    {
                        SourceForm form = new SourceForm(this, ref src);
                        form.MdiParent = MdiParent;
                        SrcForms.Add(form);
                        form.Show();
                    }
                    catch (Exception) { }
                    return;
                }
            }

            throw new Exception("Cannot open source file because of an error while parsing project file!");
        }

        /// <summary>
        /// Called by source form when closed
        /// </summary>
        /// <param name="srcform">form that was closed</param>
        public void SrcFormClosed(SourceForm srcform)
        {
            // remove from internal list of open forms..
            SrcForms.Remove(srcform);
        }

        /// <summary>
        /// Event fired by child form when source file changes
        /// </summary>
        public void SrcUpdated(CodePortingTrackerSourceFile src)
        {
            // update values with latest data..
            foreach (DataGridViewRow row in dgv.Rows)
            {
                DataGridViewCellCollection cc = row.Cells;

                if (cc["SrcPath"].Value.ToString() == src.Filename)
                {
                    // update data accordingly
                    cc["LineCount"].Value = src.LineCount;
                    cc["LinesDone"].Value = src.LinesDone;
                    cc["Completion"].Value = ProjectHelper.GetSrcFilePercentage(src.LineCount, src.LinesDone);
                    break;
                }
            }
        }

        #endregion
    }

    #region Static helper class
    public static class ProjectHelper
    {
        /// <summary>
        /// Calculates accurate percentage of lines that are marked Done
        /// </summary>
        /// <param name="LineCount">Amount of lines available</param>
        /// <param name="LinesDone">Amount of lines marked Done</param>
        /// <returns>Percentage</returns>
        public static int GetSrcFilePercentage(int LineCount, int LinesDone)
        {
            if (LineCount == 0) return 0; // prevent 1% at 0 lines done due to bad accuracy
            int Percentage = (int)(((Double)LinesDone / (Double)LineCount) * 100);
            if ((Percentage >= 100) && (LineCount != LinesDone)) return 99; // 100% should mean 100%!
            return ((Percentage >= 100) ? 100 : Percentage); // for safety, you know!
        }
    }
    #endregion
}
