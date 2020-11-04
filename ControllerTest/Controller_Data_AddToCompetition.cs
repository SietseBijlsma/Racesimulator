using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Data_AddToCompetition
    {
        private Competition _competition1;
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_if_Any_Tracks()
        {
            _competition1 = new Competition();

            Data.AddTracks(_competition1);

            var result = _competition1.Tracks;

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void Test_If_Any_Participants()
        {
            _competition1 = new Competition();

            Data.AddParticipants(_competition1);

            var result = _competition1.Participants;

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void Test_NextRace()
        {
            Data.Initialize();

            Data.NextRace();
            var result = Data.CurrentRace;
            Data.NextRace();
            var result2 = Data.CurrentRace;

            Assert.AreNotEqual(result, result2);
        }
    }
}
