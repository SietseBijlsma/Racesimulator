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

        public void Add(List<SaveCrashes> list)
        {
            if (CrashesPerRace == 0)
            {
                CrashesPerRace = 
            }
            else
            {

            }

            if (CrashesPerCompetition == 0)
            {

            }
            else
            {

            }
        }

        public SaveCrashes()
        {
            CrashesPerCompetition = this.CrashesPerCompetition;
            CrashesPerRace = this.CrashesPerRace;
        }
    }
}
