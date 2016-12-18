using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class CLap
    {
        public TimeSpan LapTime { get; set; }
        public TimeSpan LapDuration { get; set; }
        public int LapNo { get; set; }
        public List<CPenalty> PenaltiesList { get; set; }
        public bool legit { get; set; } 

        public CLap(int no,bool leg)
        {
            LapNo = no;
            legit = leg;
            PenaltiesList = new List<CPenalty>();
        }
        public CLap() { PenaltiesList = new List<CPenalty>();
            legit = true;
        }
    }
}
