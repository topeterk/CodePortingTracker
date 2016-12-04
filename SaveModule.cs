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
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

//  ------------------vv- SAMPLE -vv------------------
//SaveModule<TestType> tst = new SaveModule<TestType>("Test.xml");
//TestType test = new TestType();
//tst.ReadXML(ref test);
//test.test = "123";
//tst.WriteXML(test);
//
//
//[Serializable()]
//public class TestType
//{
//    public String test;
//}
//  ------------------^^- SAMPLE -^^------------------

namespace CodePortingTracker
{
    /// <summary>
    /// Abstracts the save/load functions of an object to/from a given file
    /// </summary>
    /// <typeparam name="T">any serializeable type</typeparam>
    class SaveModule<T>
    {
        private readonly String _Filename;
        private readonly XmlSerializer xml;

        /// <summary>
        /// Creates a new instance bound to a specified file
        /// </summary>
        /// <param name="Filename">path to the file</param>
        public SaveModule(String Filename)
        {
            _Filename = Filename;
            xml = new XmlSerializer(typeof(T));
        }

        /// <summary>
        /// Reads data from file into the object
        /// </summary>
        /// <param name="data">Reference to the object</param>
        /// <returns>true = success, false = failure</returns>
        public bool ReadXML(ref T data)
        {
            try
            {
                StreamReader file = new StreamReader(_Filename);
                data = (T)xml.Deserialize(file);
                file.Close();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Writes data into the file from an object
        /// </summary>
        /// <param name="data">object to be written</param>
        /// <returns>true = success, false = failure</returns>
        public bool WriteXML(T data)
        {
            try
            {
                StreamWriter file = new StreamWriter(_Filename);
                xml.Serialize(file, data);
                file.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: {0}", ex.ToString());
                if (Debugger.IsAttached) Debugger.Break();
                return false;
            }

            return true;
        }
    }
}
