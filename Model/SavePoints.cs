using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SavePoints : ITemplateParticipant<SavePoints>
    {
        public int Points { get; set; }
        public string Name { get; set; }

        public void Add(List<SavePoints> list)
        {
            SavePoints match = list.Find(x => x.Name == Name);
            if(match != null)
                match.Points += Points; 
            else
                list.Add(this);
        }

        public string GetBest(List<SavePoints> list)
        {
            if (list.Count > 0)
                return list.Find(y => y.Points == list.Max(x => x.Points)).Name;
            return "";
        }

        public SavePoints()
        {
            Points = this.Points;
            Name = this.Name;
        }
    }
}
