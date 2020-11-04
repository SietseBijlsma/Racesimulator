using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{

    [TestFixture]
    class Controller_Race_MovePlayer
    {
        private Race race;
        private IParticipant participant;
        [SetUp]
        public void Setup()
        {
            Data.Initialize();
            Data.NextRace();

            race = Data.CurrentRace;
        }

        private Section GetCurrentSection(IParticipant participant, LinkedList<Section> sections)
        {
            Section result = sections.Where(x =>
            {
                var data = race.GetSectionData(x);
                return (data.Left == participant || data.Right == participant);
            }).FirstOrDefault();
            return result;
        }

        [TestCase(4)]
        [TestCase(5)]
        public void Move_To_NextSection(int playerID)
        {
            participant = Data.Competition.Participants[playerID];

            participant.Equipment.Speed = 500;

            var currentSection = GetCurrentSection(participant, race.Track.Sections);
            var firstIndex = race.GetIndexOfSection(currentSection);

            race.MovePlayer();

            var currentSection2 = GetCurrentSection(participant, race.Track.Sections);
            var secondIndex = race.GetIndexOfSection(currentSection2);

            Assert.AreNotEqual(firstIndex, secondIndex);
        }
    }
}
