using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class OnlineDemo
    {
        public string no { get; set; }
        public string name { get; set; }
        public string lap1 { get; set; }
        public string lap2 { get; set; }
        public string lap3 { get; set; }
        public string pen { get; set; }

        public OnlineDemo(string no1, string name1, string lap11, string lap21, string lap31, string pen1)
        {
            no = no1;
            name = name1;
            lap1 = lap11;
            lap2 = lap21;
            lap3 = lap31;
            pen = pen1;
        }
    }
}
