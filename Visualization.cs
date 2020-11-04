using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Controller;
using Model;

namespace Racebaan
{
    public static class Visualization
    {
        public static void InitializeTrack(Track track)
        {
            Race.CalcDirection(track);
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.RaceEnded += RaceEnded;
            Console.CursorVisible = false;
        }

        #region graphics
        private static string[] _finishHorizontal = {"----", "2 # ", " 1# ", "----" };
        private static string[] _finishVertical = {"| 2|", "|1 |", "|##|", "|  |"};
        private static string[] _startHorizontal = {"----", "2>  ", " 1> ", "----"};
        private static string[] _startVerticalDown = {"|2 |", "|v1|", "|  v|", "|  |"};
        private static string[] _startVerticalUp = { "|  |", "|^ |", "| ^|", "|  |"};
        private static string[] _straightHorizontal = {"----", " 1 ", "  2 ", "----"};
        private static string[] _straightVertical = {"|  |", "|21|", "|  |", "|  |"};
        private static string[] _cornerRightDown = {@"--\ ", @" 1 \", " 2 |", @"\  |"};
        private static string[] _cornerDownLeft = {@"/  |", " 1 |", @" 2 /", @"--/"};
        private static string[] _cornerLeftUp = {@"|  \", "| 1 ", @"\ 2 ", @" \--" };
        private static string[] _cornerUpRight = {@" /--", @"/ 1 ", "| 2 ", @"|  /"};
        #endregion

        //replaces the strings where there is a participant
        public static string DrawParticipants(string replacedSection, IParticipant participant1, IParticipant participant2)
        {
            replacedSection = replacedSection.Replace("1", (participant1 == null ? " " : participant1.Name));
            replacedSection = replacedSection.Replace("2", (participant2 == null ? " " : participant2.Name));

            if (participant1 != null)
            {
                if (participant1.Equipment.IsBroken)
                    replacedSection = replacedSection.Replace(participant1.Name, "x");
            }
            if (participant2 != null)
            {
                if (participant2.Equipment.IsBroken)
                    replacedSection = replacedSection.Replace(participant2.Name, "x");
            }
            
            return replacedSection;
        }

        //draws the track
        public static void DrawTrack(Track track)
        {
            foreach (Section section in track.Sections)
            {
                DrawTrackArray(section);
            }
            DrawRaceInfo();
        }

        //draws the track based off the coords
        public static void DrawTrackArray(Section section)
        {
            string[] trackArray = {};
            if (section.SectionType == SectionTypes.StartGrid)
            {
                if (section.Direction == 1 || section.Direction == 3)
                {
                    trackArray = _startVerticalUp;
                }
                else
                {
                    trackArray = _startHorizontal;
                }
            }
            if (section.SectionType == SectionTypes.Finish)
            {
                if (section.Direction == 1 || section.Direction == 3)
                {
                    trackArray = _finishVertical;
                }
                else
                {
                    trackArray = _finishHorizontal;
                }
            }

            if (section.SectionType == SectionTypes.Straight)
            {
                if (section.Direction == 1 || section.Direction == 3)
                {
                    trackArray = _straightVertical;
                }
                if (section.Direction == 2 || section.Direction == 4)
                {
                    trackArray = _straightHorizontal;
                }
            }

            if (section.SectionType == SectionTypes.RightCorner)
            {
                switch (section.Direction)
                {
                    case 1:
                        trackArray = _cornerUpRight;
                        break;
                    case 2:
                        trackArray = _cornerRightDown;
                        break;
                    case 3:
                        trackArray = _cornerDownLeft;
                        break;
                    case 4:
                        trackArray = _cornerLeftUp;
                        break;
                }
            }

            if (section.SectionType == SectionTypes.LeftCorner)
            {
                switch (section.Direction)
                {
                    case 1:
                        trackArray = _cornerRightDown;
                        break;
                    case 2:
                        trackArray = _cornerDownLeft;
                        break;
                    case 3:
                        trackArray = _cornerLeftUp;
                        break;
                    case 4:
                        trackArray = _cornerUpRight;
                        break;
                }
            }

            SectionData data = Data.CurrentRace.GetSectionData(section);

            for (int i = 0; i < trackArray.Length; i++)
            {
                Console.SetCursorPosition(section.X * 4, (section.Y * 4) + i);
                Console.Write(DrawParticipants(trackArray[i], data.Left, data.Right));
            }
        }

        //events that gets called with DriversChangedEventArgs
        public static void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }

        //event that gets called with RaceEnded
        public static void RaceEnded(object sender, RaceEndedEventArgs e)
        {
            Console.Clear();
            Data.NextRace();
            Console.WriteLine(Data.CurrentRace.Track.Name);
            InitializeTrack(Data.CurrentRace.Track);
            DrawTrack(Data.CurrentRace.Track);
        }

        //shows some additional info on the console
        public static void DrawRaceInfo()
        {
            int y = 0;
            int x = 90;
            Console.SetCursorPosition(x, y);
            Console.Write("------------------");
            foreach (IParticipant participant in Data.CurrentRace.Participants)
            {
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"| Participant: {participant.Name}");
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"|     Speed: {participant.Equipment.Speed}");
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"|     RaceTime: {participant.RaceTime / 2}");
            }
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write("------------------");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write($"|      MostPoints: {Data.Competition.Points.GetBestInfo()} ");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write($"|      LeastCrashes: {Data.Competition.CrashesTotal.GetBestInfo()} ");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write($"|      FastestTime: {Data.Competition.Time.GetBestInfo()} ");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write("------------------");

        }
    }
}
