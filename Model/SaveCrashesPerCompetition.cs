using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SaveCrashesPerCompetition : ITemplateParticipant<SaveCrashesPerCompetition>
    {
        public string Name { get; set; }
        public int CrashesPerCompetition { get; set; }

        public void Add(List<SaveCrashesPerCompetition> list)
        {
            SaveCrashesPerCompetition match = list.Find(x => x.Name == Name);
            if (match != null)
                match.CrashesPerCompetition += CrashesPerCompetition;
            else
            {
                list.Add(this);
            }
        }

        public string GetBest(List<SaveCrashesPerCompetition> list)
        {
            string result = list.Find(y => y.CrashesPerCompetition == list.Min(x => x.CrashesPerCompetition)).Name;
            if (list.Count > 0)
                return result;
            return "";
        }

        public SaveCrashesPerCompetition()
        {
            CrashesPerCompetition = this.CrashesPerCompetition;
        }
    }
}