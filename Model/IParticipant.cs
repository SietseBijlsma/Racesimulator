using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface IParticipant
    {
         string Name { get; set; }
         int Points { get; set; }
         IEquipement Equipement { get; set; }
         TeamColors TeamColor { get; set; }

    }

    public enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }

}
