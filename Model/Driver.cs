using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public int LapCount { get; set; }
        public int RaceTime { get; set; }
        public bool FinishedCurrentRace { get; set; }
        public double SectionEnterTime { get; set; } = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        public double StartTime { get; set; } = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        public Driver()
        {
            Equipment = new Car();
        }
    }
}
