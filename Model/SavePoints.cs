using System;
using System.Collections.Generic;
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
                this.Points = Points;
        }

        public void GetBest(List<SavePoints> list)
        {

        }

        public SavePoints()
        {
            Points = this.Points;
            Name = this.Name;
        }
    }
}
