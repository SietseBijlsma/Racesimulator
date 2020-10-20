using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SaveTime : ITemplateParticipant<SaveTime>
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan TimePerSection { get; set; }

        public void Add(List<SaveTime> list)
        {
            SaveTime match = list.Find(x => x.Name == Name);
            if (match != null)
                match.Time += Time;
            else
                this.Time = Time;

            if (match != null)
                match.TimePerSection += TimePerSection;
            else
                this.TimePerSection = TimePerSection;
        }

        public void GetBest(List<SaveTime> list)
        {

        }

        public SaveTime()
        {
            Name = this.Name;
            Time = this.Time;
            TimePerSection = this.TimePerSection;
        }
    }
}
