using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FRILTimeMachine
{
    public class CPilotRound
    {
        public CPilot Pilot { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan BestLap { get; set; }
        public TimeSpan LastLap { get; set; }
        public int BestLapNo { get; set; }
        public List<CLap> Laps { get; set; }
        public int PenaltyCount { get; set; }
        public TimeSpan ClearBestLap { get; set; }
        public int ClearBestLapNo { get; set; }
        public int HeatNo { get; set; }
        public int CompletedLaps {
            get
            {
                if (Laps == null)
                    return 0;
                var laps = Laps.Count;
                if (laps - 1 < 0)
                {
                    return 0;
                }
                return laps-1;
            }
        }
        public int CompletedLapsFrom1
        {
            get
            {
                if (Laps == null)
                    return 0;
                var laps = Laps.Count;
                return laps;
            }
        }

        public string BestLapStr { get { return BestLapNo + " / " + BestLap; }}
        public string ClearBestLapStr { get { return ClearBestLapNo + " / " + ClearBestLap; } }

       
        public bool Active { get; set; }

        public void RemovteDummy()
        {
            Laps.RemoveAt(Laps.Count-1);
        }

        public void CalculateRound()
        {
            TotalTime = TimeSpan.Zero;
           
            for (int i = 0; i < Laps.Count; i++)
            {
                if (i == 0)
                {
                    Laps[i].LapDuration = Laps[i].LapTime;
                    TotalTime += Laps[i].LapDuration;
                    foreach (var pen in Laps[i].PenaltiesList)
                    {
                        TotalTime += TimeSpan.FromSeconds(pen.Value);
                    }
                }
                else
                {
                    Laps[i].LapDuration = Laps[i].LapTime - Laps[i - 1].LapTime;
                    TotalTime += Laps[i].LapDuration;
                    foreach (var pen in Laps[i].PenaltiesList)
                    {
                        TotalTime += TimeSpan.FromSeconds(pen.Value);
                    }
                }
            }
           

        }
       


        public CPilotRound Clone()
        {
            var clone = new CPilotRound();
            clone.Pilot = Pilot.Clone();
            clone.TotalTime = TotalTime;
            clone.BestLap = BestLap;
            clone.LastLap = LastLap;
            clone.ClearBestLap = ClearBestLap;
            clone.ClearBestLapNo = ClearBestLapNo;
            clone.BestLapNo = BestLapNo;
            clone.PenaltyCount = PenaltyCount;
            clone.Laps = new List<CLap>(Laps);
            return clone;
        }
    }
}
