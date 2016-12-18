using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class CPenalty
    {
        public TimeSpan PenTime { get; set; }
        public int Value { get; set; }

        public CPenalty(TimeSpan time, int val)
        {
            PenTime = time;
            Value = val;
        }

      
    }
}
