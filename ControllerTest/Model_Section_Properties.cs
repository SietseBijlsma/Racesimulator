using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{

    [TestFixture]
    class Model_Section_Properties
    {
        private Section section;

        [SetUp]
        public void Setup()
        {
            section = new Section();
        }

        [Test]
        public void TestProperties()
        {
            section.Direction = 1;
            section.SectionType = SectionTypes.Finish;
            section.X = 2;
            section.Y = 3;

            Assert.AreEqual(section.Direction, 1);
            Assert.AreEqual(section.SectionType, SectionTypes.Finish);
            Assert.AreEqual(section.X, 2);
            Assert.AreEqual(section.Y, 3);
        }
    }
}
