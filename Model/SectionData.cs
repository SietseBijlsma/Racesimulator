using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SectionData : Section, IParticipant
    {
        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipement Equipement { get; set; }
        public TeamColors TeamColor { get; set; }

    }
}
