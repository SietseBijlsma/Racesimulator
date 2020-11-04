using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SaveTime : ITemplateParticipant<SaveTime>
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public Track Track { get; set; }

        public void Add(List<SaveTime> list)
        {
            SaveTime match = list.Find(x => x.Name == Name && x.Track == Track);
            if (match != null)
                match.Time = Time;
            else
                list.Add(this);
        }

        public string GetBest(List<SaveTime> list)
        {
            string result = list.Find(y => y.Time == list.Min(x => x.Time)).Name;
            if (list.Count > 0)
                return result;
            return "";
        }

        public SaveTime()
        {
            Name = this.Name;
            Time = this.Time;
        }
    }
}
