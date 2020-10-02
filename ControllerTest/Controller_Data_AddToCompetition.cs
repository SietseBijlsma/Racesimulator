using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Data_AddToCompetition
    {
        [SetUp]
        public void Setup()
        {
            Data.Initialize();
        }

        [Test]
        public void TestAddTracks()
        {
            var result = Data.Competition.Tracks.Count;
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestAddParticipants()
        {
            var result = Data.Competition.Tracks.Count;
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestNextRace()
        {
            var result = Data.CurrentRace;
            Data.NextRace();
            var result2 = Data.CurrentRace;

            Assert.AreNotEqual(result, result2);
        }
    }
}
