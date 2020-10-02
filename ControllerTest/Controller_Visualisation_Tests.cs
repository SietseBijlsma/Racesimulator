using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Visualisation_Tests
    {
        [SetUp]
        public void Setup()
        {
            Visualisatie.InitializeTrack(Data.CurrentRace.Track);
            Visualisatie.DrawTrack(Data.CurrentRace.Track);
        }

        [Test]
        public void TestTrack()
        {

        }
    }
}
