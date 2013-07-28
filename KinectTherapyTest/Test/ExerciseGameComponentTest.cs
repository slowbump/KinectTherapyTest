using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml.Serialization;
using System.IO;
using SWENG.Criteria;
using Microsoft.Xna.Framework;

namespace SWENG.Criteria
{
    [TestFixture]
    class ExerciseGameComponentTest
    {
        private string Directory = System.AppDomain.CurrentDomain.BaseDirectory + "/GitHub/KinectTherapyTest/KinectTherapyContent/Exercises/";
        private ExerciseGameComponent testEgc;
        private Exercise Exercise;
        [SetUp]
        public void Init()
        {
            //Load test exercise from XML
            XmlSerializer serializer = new XmlSerializer(typeof(Exercise));
            StreamReader reader = new StreamReader(Directory + "EXELAE.xml");
            // deserialize the xml and create an Exercise
            Exercise = (Exercise)serializer.Deserialize(reader);

            testEgc = new ExerciseGameComponent(new Game(), Exercise);
        }

        [TestAttribute(Description = "Reference ID: UC-9 Step 4")]
        public void TestCountdownHandler()
        {
            List<string> receivedEvents = new List<String>();
            int countdownValue = 3;
            testEgc.CountdownChanged += delegate(object sender, CountdownEventArgs e)
            {
                Assert.AreEqual(countdownValue, e.Counter);
                receivedEvents.Add(e.ToString());
            };
            for (; countdownValue >= 0; countdownValue--)
            {
                testEgc.Counter = countdownValue;
            }
            Assert.AreEqual(4, receivedEvents.Count);
        }
    }
}
