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

        }

        public SaveTime()
        {
            Name = this.Name;
            Time = this.Time;
            TimePerSection = this.TimePerSection;
        }
    }
}
