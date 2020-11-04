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
    public class Controller_Race_CalculateDirections
    {
        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
            Data.NextRace();
        }

        [Test]
        public void CalculateDirections()
        {
            Race.CalcDirection(Data.CurrentRace.Track);

            Section section = Data.CurrentRace.Track.Sections.ElementAt(1);

            // directions should be 1, 2, 3 or 4
            Assert.AreNotSame(section.Direction, 0);
        }

        [Test]
        public void AllXYAbove0()
        {
            Race.CalcDirection(Data.CurrentRace.Track);

            Section below0 = Data.CurrentRace.Track.Sections.FirstOrDefault(x => x.X < 0 || x.Y < 0);
            Assert.IsNull(below0);
        }


    }
}
