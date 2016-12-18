using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class CEvent
    {
        public TimeSpan LapTime { get; set; }
        public int EventType { get; set; }
        public string EventName {
            get
            {
                string res = "";
                switch (EventType)
                {
                    case 1:
                         res =  "Lap"; break;
                        
                    case 2:
                        res =  "Penalty";break;
                    case 3:
                        res = "Finished";break;
                }
                return res;
            }
        }
    }
}
