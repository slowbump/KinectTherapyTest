using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SWENG.Service;
using Microsoft.Xna.Framework;
using SWENG.Criteria;

namespace KinectTherapyTest.Test
{
    [TestFixture]
    class ExerciseQueueTest
    {
        ExerciseQueue eq;
        [SetUp]
        public void Init()
        {
            eq = new ExerciseQueue(new Game());
        }

        [TestAttribute]
        public void TestLoadEvents()
        {
            List<string> receivedEvents = new List<String>();
            // call load exercises method and make sure loadstarted and loadcomplete events are thrown
            eq.LoadIsStarted += delegate (object sender, EventArgs e)
            {
                receivedEvents.Add(e.ToString());
            };

            eq.LoadIsDone += delegate(object sender, EventArgs e)
            {
                receivedEvents.Add(e.ToString());
            };

            Exercise[] exercises = new Exercise[1];
            exercises[0] = new Exercise();
            exercises[0].Id = "EXELAE";
            eq.LoadExercises(this, new CatalogCompleteEventArg(exercises));

            Assert.AreEqual(2, receivedEvents.Count);

        }
    }
}
