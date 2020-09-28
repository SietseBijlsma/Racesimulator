using Model;
using System;
using System.Collections.Generic;
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

        public static void PlaceParticiapnts(Track track, List<IParticipant> participants)
        {
            
        }

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            StartTime = new DateTime();
            _positions = new Dictionary<Section, SectionData>();
        }
    }
}
