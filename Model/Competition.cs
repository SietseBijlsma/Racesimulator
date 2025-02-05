﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();
        public SavePoints Point { get; set; }
        public SaveRaceInfo<SavePoints> Points { get; set; } = new SaveRaceInfo<SavePoints>();
        public SaveRaceInfo<SaveTime> Time { get; set; } = new SaveRaceInfo<SaveTime>();
        public SaveRaceInfo<SaveCrashesPerRace> Crashes { get; set; } = new SaveRaceInfo<SaveCrashesPerRace>();
        public SaveRaceInfo<SaveCrashesPerCompetition> CrashesTotal { get; set; } = new SaveRaceInfo<SaveCrashesPerCompetition>();
        public SaveRaceInfo<SaveTimePerSection> TimePerSection { get; set; } = new SaveRaceInfo<SaveTimePerSection>();

        public void SetPoints(List<IParticipant> list)
        {
            int points = list.Count;
            list.Reverse();
            foreach (IParticipant participant in list)
            {
                participant.Points = points;
                Points.AddToList(new SavePoints() {Name = participant.Name, Points = points});
                if(points >= 0)
                    points--;
            }
        }

        public Track NextTrack()
        {
            if(Tracks.Count > 0)
            {
                return Tracks.Dequeue();
            }

            return null;
        }
    }
}
