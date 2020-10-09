using System;
using System.Collections.Generic;
using System.Text;
using Racebaan;
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
            Visualization.InitializeTrack(Data.CurrentRace.Track);
        }

        [Test]
        public void Test_If_Track_Can_Be_Drawn()
        {
            Visualization.DrawTrack(Data.CurrentRace.Track);
        }
    }
}
