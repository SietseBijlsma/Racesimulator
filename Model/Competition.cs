using System;
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
        public SaveRaceInfo<SaveCrashes> Crashes { get; set; } = new SaveRaceInfo<SaveCrashes>();

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

        public void SetTime(List<IParticipant> list, Track track)
        {
            int sectionCount = track.Sections.Count;

            foreach (IParticipant participant in list)
            {
                Time.AddToList(new SaveTime()
                {
                    Name = participant.Name, Time = TimeSpan.FromSeconds(participant.RaceTime / 2),
                    TimePerSection = TimeSpan.FromSeconds(participant.RaceTime / 2 / sectionCount)
                });
            }
        }

        public void SetCrashes(List<IParticipant> list, Track track) 
        {
            foreach (IParticipant participant in list)
            {
                Crashes.AddToList(new SaveCrashes() {Name = participant.Name, CrashesPerRace = participant.AmountCrashedPerRace, CrashesPerCompetition = participant.AmountCrashedPerCompetition, Track = track});
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
