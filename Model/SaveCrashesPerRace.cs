using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SaveCrashesPerRace : ITemplateParticipant<SaveCrashesPerRace>
    {
        public string Name { get; set; }
        public int CrashesPerRace { get; set; }
        public Track Track { get; set; }

        public void Add(List<SaveCrashesPerRace> list)
        {
            SaveCrashesPerRace match = list.Find(x => x.Name == Name && x.Track == Track);
            if (match != null)
                match.CrashesPerRace += CrashesPerRace;
            else
            {
                list.Add(this);
            }
        }

        public string GetBest(List<SaveCrashesPerRace> list)
        {
            string result = list.Find(y => y.CrashesPerRace == list.Min(x => x.CrashesPerRace)).Name;
            if (list.Count > 0)
                return result;
            return "";
        }

        public SaveCrashesPerRace()
        {
            CrashesPerRace = this.CrashesPerRace;
        }
    }
}
