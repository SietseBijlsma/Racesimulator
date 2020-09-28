using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using Model;
using System.Security.Cryptography.X509Certificates;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void Setup()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var track = new Track("testTrack", new SectionTypes[] { SectionTypes.StartGrid });

            _competition.Tracks.Enqueue(track);

            var result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            var track = new Track("testTrack2", new SectionTypes[] { SectionTypes.StartGrid });

            _competition.Tracks.Enqueue(track);

            var result = _competition.NextTrack();
            result = _competition.NextTrack();

            Assert.IsNull(result);

        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            var track = new Track("testTrack3", new SectionTypes[] { SectionTypes.StartGrid });
            var track2 = new Track("testTrack4", new SectionTypes[] { SectionTypes.StartGrid });
            _competition.Tracks.Enqueue(track);
            _competition.Tracks.Enqueue(track2);

            var result1 = _competition.NextTrack();
            var result2 = _competition.NextTrack();

            Assert.AreNotEqual(result1, result2);
        }
    }
}
