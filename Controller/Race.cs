using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.Timers;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        public event EventHandler<DriversChangedEventArgs> OnDriversChanged; 

        private Timer _timer;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private readonly int _sectionLength = 250;

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
            foreach(IParticipant participant in Participants)
            {
                participant.Equipement.Quality = _random.Next(3, 10) + 5;
                participant.Equipement.Performance = _random.Next(3, 10) + 5;
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

        private void Start()
        {
            _timer.Enabled = true;
        }

        public int GetIndexOfSection(Section section)
        {
            return Track.Sections.TakeWhile(x => x != section).Count();
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            foreach (IParticipant participant in Participants)
            {
                int quality = participant.Equipement.Quality;
                int performance = participant.Equipement.Performance;
                int speed = (performance * quality);

                Section s = _positions.FirstOrDefault(x => (x.Value.Left == participant) || (x.Value.Right == participant)).Key;

                if (s == null)
                    continue;

                int index = GetIndexOfSection(s) + 1;

                if (index == Track.Sections.Count)
                {
                    index = 0;
                }
                SectionData data = GetSectionData(s);

                Section x = Track.Sections.ElementAt(index);
            
                SectionData data2 = GetSectionData(x);
                bool canMove = false;
                bool finished = !Participants.Select(x => x.LapCount < 2).Any();

                if (finished)
                {
                    ClearDriversChanged();
                    Console.Clear();
                    Data.NextRace();
                    Visualisatie.InitializeTrack(Data.CurrentRace.Track);
                    Visualisatie.DrawTrack(Data.CurrentRace.Track);
                }

                if (participant.LapCount < 2)
                {
                    if (data.Left == participant)
                    {
                        data.DistanceLeft += speed;
                        if (data.DistanceLeft > _sectionLength)
                        {
                            if (data2.Left == null)
                            {
                                data2.Left = participant;
                                canMove = true;
                            }
                            else if (data2.Right == null)
                            {
                                data2.Right = participant;
                                canMove = true;
                            }
                            if (canMove)
                            {
                                data.Left = null;
                                data.DistanceLeft = 0;
                                if (x.SectionType == SectionTypes.Finish)
                                {
                                    participant.LapCount++;
                                }
                            }
                        }
                    }
                    else if (data.Right == participant)
                    {
                        data.DistanceRight += speed;
                        if (data.DistanceRight > _sectionLength)
                        {
                            if (data2.Left == null)
                            {
                                data2.Left = participant;
                                canMove = true;
                            }
                            else if (data2.Right == null)
                            {
                                data2.Right = participant;
                                canMove = true;
                            }
                            if (canMove)
                            {
                                data.Right = null;
                                data.DistanceRight = 0;
                                if (x.SectionType == SectionTypes.Finish)
                                {
                                    participant.LapCount++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (participant == data.Right)
                        data.Right = null;
                    else if (participant == data.Left)
                        data.Left = null;
                }
            }
            OnDriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = Track });
        }

        public void ClearDriversChanged()
        {
            
        }

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            StartTime = new DateTime();
            _positions = new Dictionary<Section, SectionData>();
            PlaceParticipants();
            RandomizeEquipement();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
            OnTimedEvent(this, null);
            Start();
        }
    }
}
