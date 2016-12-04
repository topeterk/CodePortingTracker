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

namespace CodePortingTracker
{
    /// <summary>
    /// State of code line
    /// </summary>
    public enum LineState
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Open
        /// </summary>
        Open,
        /// <summary>
        /// In progress
        /// </summary>
        Progress,
        /// <summary>
        /// Done
        /// </summary>
        Done,
    };

    /// <summary>
    /// Holds the state of a block of lines
    /// </summary>
    [Serializable()]
    public class BlockState
    {
        public int Line;
        public int Len;
        public LineState State;

        /// <summary>
        /// Internal default constructor for serialization
        /// </summary>
        public BlockState()
        {
            Line = 0;
            Len = 0;
            State = LineState.Unknown;
        }
    }

    /// <summary>
    /// Holds all data related to a project
    /// </summary>
    [Serializable()]
    public class CodePortingTrackerSourceFile
    {
        /// <summary>
        /// Name of tracked source file
        /// </summary>
        public string Filename;
        /// <summary>
        /// Amount of lines from the file
        /// </summary>
        public int LineCount;
        /// <summary>
        /// Line count of lines that are done already
        /// </summary>
        public int LinesDone;
        /// <summary>
        /// State of blocks
        /// </summary>
        public List<BlockState> Blocks;

        /// <summary>
        /// Internal default constructor for serialization
        /// </summary>
        private CodePortingTrackerSourceFile()
        {
            Filename = null;
            LineCount = 0;
            LinesDone = 0;
            Blocks = new List<BlockState>();
        }

        /// <summary>
        /// Creates a new source file tracking object
        /// </summary>
        /// <param name="filepath">Path to the source file</param>
        public CodePortingTrackerSourceFile(string filepath) : this()
        {
            Filename = filepath;
        }
    }

    /// <summary>
    /// Holds all data related to a project
    /// </summary>
    [Serializable()]
    public class CodePortingTrackerProjectFile
    {
        /// <summary>
        /// Tracked source file
        /// </summary>
        public List<CodePortingTrackerSourceFile> Files;

        /// <summary>
        ///  Creates a new project file tracking object
        /// </summary>
        public CodePortingTrackerProjectFile()
        {
            Files = new List<CodePortingTrackerSourceFile>();
        }

        /// <summary>
        /// Adds a file into the project
        /// </summary>
        /// <param name="filepath">Path to the source file to be added</param>
        public void AddSourceFile(string filepath)
        {
            Files.Add(new CodePortingTrackerSourceFile(filepath));
        }
    }
}
