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
            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            Competition.Participants.Add(new Driver());
            Competition.Participants.Add(new Driver());
        }

        public static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("track01", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Rightcorner, SectionTypes.Rightcorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Rightcorner, SectionTypes.Rightcorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Rightcorner, SectionTypes.Straight, SectionTypes.Rightcorner, SectionTypes.Finish}));
            Competition.Tracks.Enqueue(new Track("track02", new SectionTypes[] { SectionTypes.StartGrid }));
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
