using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Model;

namespace Controller
{
    public static class Visualisatie
    {
        public static void InitializeTrack(Track track)
        {
            CalcCoords(track);
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

        public static string DrawParticipants(string replacedSection, IParticipant participant1, IParticipant participant2)
        {
            replacedSection = replacedSection.Replace("1", (participant1 == null ? " " : participant1.Name));
            replacedSection = replacedSection.Replace("2", (participant2 == null ? " " : participant2.Name));

            return replacedSection;
        }

        public static void DrawTrack(Track track)
        {
            foreach (Section section in track.Sections)
            {
                DrawTrackArray(section);
            }
        }

        public static void CalcCoords(Track track)
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
    }
}
