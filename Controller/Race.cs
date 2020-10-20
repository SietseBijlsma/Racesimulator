using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
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

        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public event EventHandler<RaceEndedEventArgs> RaceEnded;

        private Timer _timer;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private readonly int _sectionLength = 250;

        //gets the section data from the section you give as input
        public SectionData GetSectionData(Section section)
        {
            if(!_positions.ContainsKey(section))
            {
                _positions.Add(section, new SectionData());
            }

            return _positions.GetValueOrDefault(section);
        }

        //randomizes the quality and performance of the car
        public void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(3, 10) + 5;
                participant.Equipment.Performance = _random.Next(3, 10) + 5;
            }
        }

        //places all the participants on the right tracks
        public void PlaceParticipants()
        {
            for (int i = 0; i < Participants.Count; i++)
            {
                if (GetStartGrid().Count <= (i / 2))
                    return;
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

        //gets the start grids of the track
        public List<Section> GetStartGrid()
        {
            return Track.Sections.Where(x => x.SectionType == SectionTypes.StartGrid).ToList();
        }

        //starts the timer
        private void Start()
        {
            _timer.Enabled = true;
        }

        //get the index of the section you give as input
        public int GetIndexOfSection(Section section)
        {
            return Track.Sections.TakeWhile(x => x != section).Count();
        }

        //determines if and when a car breaks down
        public void IsCarBroken(IParticipant participant)
        {
            int randomNumber =_random.Next(1, 100);
            if (randomNumber <= 20)
                participant.Equipment.IsBroken = false;
            if (randomNumber <= 2)
            {
                participant.Equipment.IsBroken = true;
                participant.AmountCrashedPerRace++;
            }    
        }

        public List<IParticipant> DeterminePosition(List<IParticipant> participant)
        {
            List<IParticipant> ParticipantPosition;
            return ParticipantPosition = Participants.OrderByDescending(x => x.RaceTime).ToList();
        }

        //OnTimedEvent is being called every 0.5 seconds
        private void OnTimedEvent(object sender, EventArgs e)
        {
            //clears the event handler and starts the event to begin the nextRace when all the participants finished all the laps
            bool finished = Participants.FindAll(x => x.LapCount == 2).Count == Participants.Count;
            if (finished)
            {
                Data.Competition.SetPoints(DeterminePosition(Participants));
                Data.Competition.SetTime(Participants, Track);
                Data.Competition.SetCrashes(Participants, Track);
                ClearDriversChanged();
                foreach (IParticipant participant in Participants)
                {
                    participant.LapCount = 0;
                    participant.FinishedCurrentRace = false;
                    participant.RaceTime = 0;
                    participant.AmountCrashedPerCompetition += participant.AmountCrashedPerRace;
                    participant.AmountCrashedPerRace = 0;
                }
                RaceEnded?.Invoke(this, new RaceEndedEventArgs());
            }

            //moves the participants across the track
            foreach (IParticipant participant in Participants)
            {
                int quality = participant.Equipment.Quality;
                int performance = participant.Equipment.Performance;
                int speed = (performance * quality);

                if (participant.LapCount == 2) participant.FinishedCurrentRace = true;
                if (participant.FinishedCurrentRace == false) participant.RaceTime++;

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

                IsCarBroken(participant);

                if (participant.LapCount < 2 && participant.Equipment.IsBroken == false)
                {
                    if (data.Left == participant)
                    {
                        data.DistanceLeft += speed;
                        if (data.DistanceLeft >= _sectionLength)
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
                            if (canMove && participant.Equipment.IsBroken == false)
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
                else if(participant.LapCount == 2)
                {
                    if (participant == data.Right)
                        data.Right = null;
                    else if (participant == data.Left)
                        data.Left = null;
                }
            }
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = Track });
        }

        //clears the event handler
        public void ClearDriversChanged()
        {
            DriversChanged = null;
        }

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            StartTime = new DateTime();
            _positions = new Dictionary<Section, SectionData>();
            PlaceParticipants();
            RandomizeEquipment();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
            OnTimedEvent(this, null);
            Start();
        }
    }
}
