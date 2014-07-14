using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily
{
    class Task
    {
        public string Title;
        public string Time;

        public Task(string title, string time)
        {
            this.Title = title;
            this.Time = time;
        }

        
        public override string ToString()
        {
            return Time + " " + Title;
        }
    }
}
