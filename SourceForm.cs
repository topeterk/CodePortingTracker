using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodePortingTracker
{
    public partial class SourceForm : Form
    {
        private readonly ProjectForm PrjForm;
        private CodePortingTrackerSourceFile src;
        /// <summary>
        /// Get filename of opened file
        /// </summary>
        public string Filename { get { return src.Filename; } }

        #region Loading source file and loading/closing form
        public SourceForm(ProjectForm projectform, ref CodePortingTrackerSourceFile srcdata)
        {
            PrjForm = projectform;
            src = srcdata;

            InitializeComponent();
            openToolStripMenuItem.BackColor = ConvertLineStateToColor(LineState.Open);
            inProgressToolStripMenuItem.BackColor = ConvertLineStateToColor(LineState.Progress);
            doneToolStripMenuItem.BackColor = ConvertLineStateToColor(LineState.Done);
            SourceForm_SizeChanged(this, null);


            Text += " - " + System.IO.Path.GetFileName(Filename);

            // load, validate and update file data
            rtb.LoadFile(Filename, RichTextBoxStreamType.PlainText);
            if (src.LineCount == 0) // new files loaded the first time
            {
                src.LineCount = rtb.Lines.Length;
            }
            else if (src.LineCount != rtb.Lines.Length) // file changed?
            {
                DialogResult result = MessageBox.Show("The source file has changed in the meantime:" + Environment.NewLine +
                    "\"" + Filename + "\"" + Environment.NewLine +
                    "Do you want to continue working on the changed file?" + Environment.NewLine +
                    "OK = Accept new file (saved data may not match with new file)" + Environment.NewLine +
                    "Cancel = Abort loading the file", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result != DialogResult.OK)
                    throw new Exception();

                if (rtb.Lines.Length < src.LineCount) // lines were removed?
                {
                    src.LineCount = rtb.Lines.Length; // cut length

                    // Remove cut off blocks from list
                    BlockState DummyBlock = new BlockState();
                    DummyBlock.Line = 0;
                    DummyBlock.Len = 0;
                    DummyBlock.State = LineState.Unknown; ;
                    InsertBlockState(DummyBlock);
                }
                else src.LineCount = rtb.Lines.Length; // increase length
            }

            // Load block data into rich text box..
            foreach (BlockState block in src.Blocks)
            {
                int LineStart = block.Line;
                int LineEnd = block.Line + block.Len - 1;

                int SelectionStart = rtb.GetFirstCharIndexFromLine(LineStart);
                int SelectionLength = rtb.GetFirstCharIndexFromLine(LineEnd) + rtb.Lines[LineEnd].Length - SelectionStart;
                rtb.Select(SelectionStart, SelectionLength);
                rtb.SelectionBackColor = ConvertLineStateToColor(block.State);
            }
            rtb.Select(0, 0);

            statusbar.Text = "File \"" + Filename + "\" loaded!";
        }

        private void SourceForm_Load(object sender, EventArgs e)
        {
            UpdateSrcData(); // Initial update
        }
        private void SourceForm_SizeChanged(object sender, EventArgs e)
        {
            rtb.Width = this.ClientSize.Width - 25;
            rtb.Height = this.ClientSize.Height - 52;
        }

        private void SourceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PrjForm.SrcFormClosed(this);
        }


        #endregion

        #region Updating/Modifying data

        private Color ConvertLineStateToColor(LineState state)
        {
            switch (state)
            {
                case LineState.Progress: return Color.DeepSkyBlue;
                case LineState.Done: return Color.LightGreen;
            }
            return Color.White;
        }

        private void MarkSelection(LineState MarkState, String MarkName)
        {
            int LineStart = rtb.GetLineFromCharIndex(rtb.SelectionStart);
            int LineEnd = rtb.GetLineFromCharIndex(rtb.SelectionStart + rtb.SelectionLength);

            int SelectionStart = rtb.GetFirstCharIndexFromLine(LineStart);
            int SelectionLength = rtb.GetFirstCharIndexFromLine(LineEnd) + rtb.Lines[LineEnd].Length - SelectionStart;
            rtb.Select(SelectionStart, SelectionLength);
            rtb.SelectionBackColor = ConvertLineStateToColor(MarkState);
            rtb.Select(rtb.GetFirstCharIndexFromLine(LineEnd), 0);

            BlockState block = new BlockState();
            block.State = MarkState;
            block.Line = LineStart;
            block.Len = LineEnd - LineStart + 1;
            InsertBlockState(block);

            statusbar.Text = "Lines " + LineStart.ToString() + " to " + LineEnd.ToString() + " marked as " + MarkName + "!";

            UpdateSrcData(); // Update due to user action
        }

        private void InsertBlockState(BlockState InsertBlock)
        {
            // Create linear array for each line..
            int MaxLines = src.LineCount;
            LineState[] linestates = new LineState[MaxLines];

            // Initlialize the array..
            for (int i = 0; i < linestates.Length; i++)
                linestates[i] = LineState.Unknown;

            // Load data we already have..
            foreach (BlockState block in src.Blocks)
            {
                for (int i = block.Line; i < block.Line + block.Len; i++)
                    linestates[i] = block.State;
            }

            // Update data with new block states..
            for (int i = InsertBlock.Line; i < InsertBlock.Line + InsertBlock.Len; i++)
                linestates[i] = InsertBlock.State;

            // Refill blocks with latest data..
            LineState state = LineState.Unknown;
            int linestart = 0;
            src.Blocks.Clear();
            for (int i = 0; i < linestates.Length; i++)
            {
                if (state != linestates[i]) // state changes, so we can create the previous block
                {
                    if ((state != LineState.Unknown) && (state != LineState.Open)) // filter useful states..
                    {
                        // Create and add actual block..
                        BlockState block = new BlockState();
                        block.Line = linestart;
                        block.Len = i - linestart;
                        block.State = state;
                        src.Blocks.Add(block);
                    }

                    // start collecting new block
                    state = linestates[i];
                    linestart = i;
                }
            }
            // in case one block was added, take for last one since previous loop will not insert last block:
            if ((state != LineState.Unknown) && (state != LineState.Open)) // filter useful states..
            {
                // Create and add actual block..
                BlockState block = new BlockState();
                block.Line = linestart;
                block.Len = (linestates.Length - 1) - linestart + 1;
                block.State = state;
                src.Blocks.Add(block);
            }

            // Update how many lines are marked as Done..
            src.LinesDone = 0;
            foreach (BlockState block in src.Blocks)
            {
                if (block.State == LineState.Done)
                    src.LinesDone += block.Len;
            }
        }

        private void UpdateSrcData()
        {
            // Update the status bar..
            completionbar.Text = "Completed: " + ProjectHelper.GetSrcFilePercentage(src.LineCount, src.LinesDone).ToString() + "% " +
                "(" + src.LinesDone.ToString() + " of " + src.LineCount.ToString() + " lines)";
            PrjForm.SrcUpdated(src); // Notify project form about source file changes
        }

        #endregion

        #region Form controls' handler
        private void rtb_SelectionChanged(object sender, EventArgs e)
        {
            int LineStart = rtb.GetLineFromCharIndex(rtb.SelectionStart);
            int LineEnd = rtb.GetLineFromCharIndex(rtb.SelectionStart + rtb.SelectionLength);

            int PosStart = rtb.SelectionStart - rtb.GetFirstCharIndexFromLine(LineStart);
            int PosEnd = rtb.SelectionStart + rtb.SelectionLength - rtb.GetFirstCharIndexFromLine(LineEnd);

            statusbar.Text = "Selected: [" + (LineStart + 1).ToString() + "," + PosStart++.ToString() + "] to [" + (LineEnd + 1).ToString() + "," + PosEnd++.ToString() + "]";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkSelection(LineState.Open, "OPEN");
        }
        private void inProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkSelection(LineState.Progress, "IN PROGRESS");
        }
        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarkSelection(LineState.Done, "DONE");
        }
        #endregion
    }
}
