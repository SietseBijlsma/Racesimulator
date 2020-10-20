using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SaveCrashes : ITemplateParticipant<SaveCrashes>
    {
        public string Name { get; set; }
        public int CrashesPerRace { get; set; }
        public int CrashesPerCompetition { get; set; }
        public Track Track { get; set; }

        public void Add(List<SaveCrashes> list)
        {
            SaveCrashes match = list.Find(x => x.Name == Name);
            SaveCrashes match2 = list.Find(x => x.Track == Track && x.Track == Track);
            if (match != null)
                match.CrashesPerRace += CrashesPerRace;
            else if (match2 != null)
                this.CrashesPerRace = CrashesPerRace;
            if(match != null)
            {
                match.CrashesPerCompetition += CrashesPerCompetition;
            }
        }

        public void GetBest(List<SaveCrashes> list)
        {

        }

        public SaveCrashes()
        {
            CrashesPerCompetition = this.CrashesPerCompetition;
            CrashesPerRace = this.CrashesPerRace;
        }
    }
}
