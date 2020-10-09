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
        public void Test_if_Any_Tracks()
        {
            var result = Data.Competition.Tracks.Count;
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_If_Any_Participants()
        {
            var result = Data.Competition.Tracks.Count;
            Assert.IsNotNull(result);
            Assert.GreaterOrEqual(result, 2);
        }

        [Test]
        public void Test_NextRace()
        {
            var result = Data.CurrentRace;
            Data.NextRace();
            var result2 = Data.CurrentRace;

            Assert.AreNotEqual(result, result2);
        }
    }
}
