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
            
        }

        public SavePoints()
        {
            Points = this.Points;
            Name = this.Name;
        }
    }
}
