using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
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
                participant.Equipment.Speed = participant.Equipment.Quality * participant.Equipment.Performance;
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
            _timer.Start();
        }

        //calculates the direction of all the corners and places the track on the console if one goes below zero
        public static void CalcDirection(Track track)
        {
            //direction 1 is north
            //direction 2 is east
            //direction 3 is south
            //direction 4 is west

            int direction = 2;
            int x = 1;
            int y = 1;
            int lowestX = 0;
            int lowestY = 0;

            foreach (Section section in track.Sections)
            {
                section.Direction = direction;
                switch (direction)
                {
                    case 1:
                        y--;
                        break;
                    case 2:
                        x++;
                        break;
                    case 3:
                        y++;
                        break;
                    case 4:
                        x--;
                        break;
                }

                if (section.SectionType == SectionTypes.LeftCorner)
                {
                    if (direction == 1) direction = 4;
                    else direction--;
                }
                else if (section.SectionType == SectionTypes.RightCorner)
                {
                    if (direction == 4) direction = 1;
                    else direction++;
                }

                section.X = x;
                section.Y = y;
                if (x < lowestX) lowestX = x;
                if (y < lowestY) lowestY = y;
            }
            lowestX = lowestX * -1;
            lowestY = lowestY * -1;
            foreach (Section section in track.Sections)
            {
                section.X += lowestX;
                section.Y += lowestY;
            }
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
                participant.Equipment.Speed--;

                Data.Competition.Crashes.AddToList(new SaveCrashesPerRace() { Name = participant.Name, CrashesPerRace = 1, Track = Data.CurrentRace.Track });
                Data.Competition.CrashesTotal.AddToList(new SaveCrashesPerCompetition() { Name = participant.Name, CrashesPerCompetition = 1});
            }    
        }

        //Determines the position of the players
        public List<IParticipant> DeterminePosition()
        {
            return Participants.OrderByDescending(x => x.RaceTime).ToList();
        }

        //This gets called at the end of a race
        public void EndOfRace()
        {
            Data.Competition.SetPoints(DeterminePosition());
            ClearDriversChanged();
            foreach (IParticipant participant in Participants)
            {
                participant.LapCount = 0;
                participant.FinishedCurrentRace = false;
                participant.RaceTime = 0;
                ((Driver)participant).StartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            RaceEnded?.Invoke(this, new RaceEndedEventArgs());
        }

        //OnTimedEvent is being called every 0.5 seconds
        private void OnTimedEvent(object sender, EventArgs e)
        {
            //clears the event handler and starts the event to begin the nextRace. when all the participants finished all their laps.
            bool finished = Participants.FindAll(x => x.LapCount == 2).Count == Participants.Count;
            if (finished)
            {
                EndOfRace();
            }

            MovePlayer();
            
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = Track });
        }
        //moves the participants across the track
        public void MovePlayer()
        {
            foreach (IParticipant participant in Participants)
            {
                if (participant.FinishedCurrentRace == false)
                {
                    Data.Competition.Time.AddToList(new SaveTime() { Name = participant.Name, Time = TimeSpan.FromMilliseconds(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - 
                        ((Driver)participant).StartTime), Track = Data.CurrentRace.Track });
                }

                if (participant.LapCount == 2) participant.FinishedCurrentRace = true;
                if (participant.FinishedCurrentRace == false) participant.RaceTime++;

                Section s = _positions.FirstOrDefault(x => (x.Value.Left == participant) || (x.Value.Right == participant)).Key;
                if (s == null)
                    continue;

                int index = GetIndexOfSection(s) + 1;
                if (index == Track.Sections.Count)
                    index = 0;

                SectionData data = GetSectionData(s);
                Section x = Track.Sections.ElementAt(index);
                SectionData data2 = GetSectionData(x);
                bool canMove = false;

                IsCarBroken(participant);
                const int laps = 1;

                if (participant.LapCount <= laps && participant.Equipment.IsBroken == false)
                {
                    if (data.Left == participant)
                    {
                        data.DistanceLeft += participant.Equipment.Speed;
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
                                Data.Competition.TimePerSection.AddToList(new SaveTimePerSection() { Name = participant.Name, TimePerSection = TimeSpan.FromMilliseconds(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - 
                                    ((Driver)participant).SectionEnterTime), Track = Data.CurrentRace.Track });
                                ((Driver) participant).SectionEnterTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                            }
                        }
                    }
                    else if (data.Right == participant)
                    {
                        data.DistanceRight += participant.Equipment.Speed;
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
                                Data.Competition.TimePerSection.AddToList(new SaveTimePerSection() { Name = participant.Name, TimePerSection = TimeSpan.FromMilliseconds(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - 
                                    ((Driver)participant).SectionEnterTime) / 2, Track = Data.CurrentRace.Track });
                                ((Driver)participant).SectionEnterTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                            }
                        }
                    }
                }
                else if (participant.LapCount == 2)
                {
                    if (participant == data.Right)
                        data.Right = null;
                    else if (participant == data.Left)
                        data.Left = null;
                }
            }
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
            Start();
        }
    }
}
