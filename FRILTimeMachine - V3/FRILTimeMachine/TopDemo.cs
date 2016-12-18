using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class TopDemo
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }

        public TopDemo(string no, string name, string time)
        {
            No = no;
            Name = name;
            Time = time;
        }
    }
}
