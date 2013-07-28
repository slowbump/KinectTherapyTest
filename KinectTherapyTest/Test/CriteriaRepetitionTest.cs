using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SWENG.Criteria;
using System.Xml.Serialization;
using System.IO;

namespace SWENG.Criteria
{
    [TestFixture]
    class CriteriaRepetitionTest
    {
        private Repetition testRepetition;
        private string Directory = System.AppDomain.CurrentDomain.BaseDirectory + "/GitHub/KinectTherapyTest/KinectTherapyContent/Exercises/";
        private Exercise Exercise;

        [SetUp]
        public void Init()
        {
            //Load test exercise from XML
            XmlSerializer serializer = new XmlSerializer(typeof(Exercise));
            StreamReader reader = new StreamReader(Directory + "EXELAE.xml");
            // deserialize the xml and create an Exercise
            Exercise = (Exercise)serializer.Deserialize(reader);

            testRepetition = new Repetition(Exercise);
        }

        [TestAttribute(Description = "Reference ID: UC-7 Step 2, UC-8 Step 2")]
        public void TestCheckpointHandler()
        {
            List<string> receivedEvents = new List<String>();
            int checkpointId = 0;
            testRepetition.CheckpointChanged += delegate(object sender, CheckpointChangedEventArgs e)
            {
                Assert.AreEqual("EXELAE"+checkpointId, e.FileId);
                receivedEvents.Add(e.ToString());
            };
            for (; checkpointId <= 10; checkpointId++)
            {
                testRepetition.Checkpoint = checkpointId;
            }
            Assert.AreEqual(10, receivedEvents.Count);
        }
    }
}
