using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class CPilot
    {
        public string Name { get; set; }
        public CPilotRound PilotBestRound { get; set; }
        public bool Visible { get; set; }

 
        public int AssignedRound { get; set; }
      
        public CPilot(string name, int round)
        {
            Name = name;
            AssignedRound = round;

            PilotBestRound = new CPilotRound();
            PilotBestRound.ClearBestLap = TimeSpan.MaxValue;
            PilotBestRound.TotalTime = TimeSpan.MaxValue;
            Visible = false;

        }

        public CPilot Clone()
        {
            var r = new CPilot(Name,AssignedRound);
           
            return r;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
