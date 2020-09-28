using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public LinkedList<Section> ArrayToLinkedList(SectionTypes[] sectionTypes)
        {
            LinkedList<Section> sections = new LinkedList<Section>();

            foreach (var sectiontype in sectionTypes)
            {
                   Section section = new Section() { SectionType = sectiontype };
                   sections.AddLast(section);
            }

            return sections;
        }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = ArrayToLinkedList(sections);
        }

        
    }
}
