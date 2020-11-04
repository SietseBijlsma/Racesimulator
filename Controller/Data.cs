using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants(Competition);
            AddTracks(Competition);
        }

        public static void AddParticipants(Competition competition)
        {
            competition.Participants.Add(new Driver() {Name = "e", TeamColor = TeamColors.Yellow});
            competition.Participants.Add(new Driver() {Name = "f", TeamColor = TeamColors.Red });
            competition.Participants.Add(new Driver() {Name = "c", TeamColor = TeamColors.Orange });
            competition.Participants.Add(new Driver() {Name = "d", TeamColor = TeamColors.Cyan });
            competition.Participants.Add(new Driver() {Name = "a", TeamColor = TeamColors.Green });
            competition.Participants.Add(new Driver() {Name = "b", TeamColor = TeamColors.Blue });
        }

        public static void AddTracks(Competition competition)
        {
            competition.Tracks.Enqueue(new Track("Circuit Elburg", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner}));
            competition.Tracks.Enqueue(new Track("track01", new SectionTypes[] {SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner}));
            competition.Tracks.Enqueue(new Track("track02", new SectionTypes[] {SectionTypes.StartGrid}));
        }

        public static void NextRace()
        {
            Track track = Competition.NextTrack();
            if (track != null)
            {
                CurrentRace = new Race(track, Competition.Participants);
            }
        }
    }
}
