using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface IParticipant
    {
         string Name { get; set; }
         int Points { get; set; }
         IEquipment Equipment { get; set; }
         TeamColors TeamColor { get; set; }
         int LapCount { get; set; }
         int RaceTime { get; set; }
         bool FinishedCurrentRace { get; set; }
        
    }

    public enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Cyan,
        Blue,
        Orange
    }
}
