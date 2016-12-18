using System;
using System.Collections.Generic;

namespace FRILTimeMachine
{
    public class CRound
    {
        public int RoundNo { get; set; }
        public int NoLaps { get; set; }
        public List<CPilot> RoundPilots { get; set; }
        public DateTime RoundTime { get; set; }
        public int HeatNo { get; set; }
        public string ListItem { get { return ToString(); } }
        public List<CPilotRound> PilotsLaps { get; set; }
       

        public CRound()
        {
            PilotsLaps = new List<CPilotRound>();
        }

        public CRound(CRound copy,DateTime t,int heat)
        {
            NoLaps = copy.NoLaps;
            RoundNo = copy.RoundNo;
            RoundPilots = new List<CPilot>();
            HeatNo = heat;
            foreach (var pilot in copy.RoundPilots)
            {
                RoundPilots.Add(pilot.Clone());
            }
         
            RoundTime = t;
            PilotsLaps = new List<CPilotRound>();
            foreach (var lap in copy.PilotsLaps)
            {
                PilotsLaps.Add(lap.Clone());
            }
        }
        public CRound ShallowCopy()
        {
            return (CRound)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return "Round No:"+RoundNo + " | Heat No: "+HeatNo+" | "+ RoundTime.ToString("dd/MM/yy HH:mm");
        }
    }
}
