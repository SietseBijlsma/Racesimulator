using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Model
{
    public class SaveTimePerSection : ITemplateParticipant<SaveTimePerSection>
    {
        public string Name { get; set; }
        public TimeSpan TimePerSection { get; set; }
        public Track Track { get; set; }

        public void Add(List<SaveTimePerSection> list)
        {
            list.Add(this);
        }

        public List<SaveTimePerSection> GetAverages(Track track, List<SaveTimePerSection> list)
        {
            List<SaveTimePerSection> returnList = new List<SaveTimePerSection>();
            list.Where(x => x.Track == track).ToList().ForEach(sectionTime =>
            {
                if (returnList.Find(x => x.Name == sectionTime.Name && x.Track == sectionTime.Track) != null)
                    return;

                var avgList = list.Where(x => x.Name == sectionTime.Name && x.Track == sectionTime.Track).ToList();
                var timeSpan = new TimeSpan();
                avgList.ForEach(x =>
                {
                    timeSpan += x.TimePerSection;
                });
                returnList.Add(new SaveTimePerSection() { Name = sectionTime.Name, Track = sectionTime.Track, TimePerSection = timeSpan.Divide(avgList.Count)});
            });
            return returnList;
        }

        public string GetBest(List<SaveTimePerSection> list)
        {
            string result = list.Find(y => y.TimePerSection == list.Min(x => x.TimePerSection)).Name;
            if (list.Count > 0)
                return result;
            return "";
        }

        public SaveTimePerSection()
        {
            Name = this.Name;
            TimePerSection = this.TimePerSection;
        }
    }
}