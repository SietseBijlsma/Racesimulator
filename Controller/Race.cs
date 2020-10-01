using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
       
       public SectionData GetSectionData(Section section)
        {
            if(!_positions.ContainsKey(section))
            {
                _positions.Add(section, new SectionData());
            }

            return _positions.GetValueOrDefault(section);
        }

        public void RandomizeEquipement()
        {
            foreach(IEquipement participant in Participants)
            {
                participant.Quality = _random.Next();
                participant.Performance = _random.Next();
            }
        }

        public void PlaceParticipants()
        {
            for (int i = 0; i < Participants.Count; i++)
            {
                Section s = GetStartGrid().ElementAt(i / 2);

                SectionData data = GetSectionData(s);
                IParticipant participant = Participants[i];

                if (data.Left == null)
                {
                    data.Left = participant;
                }
                else
                {
                    data.Right = participant;
                }
            }
        }

        public List<Section> GetStartGrid()
        {
            List<Section> list = new List<Section>();
            foreach (Section section in Track.Sections)
            {
                if(section.SectionType == SectionTypes.StartGrid)
                    list.Add(section);
            }
            return list;
        }

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            StartTime = new DateTime();
            _positions = new Dictionary<Section, SectionData>();
            PlaceParticipants();
        }
    }
}
