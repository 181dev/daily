using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily
{
    class TaskList
    {
        public List<Task> Data;

        internal void ReadTasks(string fileName)
        {
            Data = new List<Task>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string lines = sr.ReadToEnd();
                string nl = Environment.NewLine;
                string[] liness = lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string line in liness)
                {
                    string item = line.Trim();
                    if (item.Length > 0)
                    {
                        string time = line.Split(' ')[0];
                        string title = line.Split(' ')[1];
                        Data.Add(new Task(title, time));
                    }
                }
            }
        }

    }
}
