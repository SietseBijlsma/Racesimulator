using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race_AddToCompetition
    {
        private Competition _competition;

        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
        }

        [Test]
        public void AddRaceToQueue()
        {
            var track = new Track("testTrack", new SectionTypes[] { SectionTypes.StartGrid });

            _competition.Tracks.Enqueue(track);

            var result = _competition.Tracks.Count;
            Assert.AreEqual(1, result);
        }
    }
}
