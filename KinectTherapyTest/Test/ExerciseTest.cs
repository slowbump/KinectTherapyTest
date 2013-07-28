using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Kinect;

namespace SWENG.Criteria
{
    [TestFixture]
    public class ExerciseTest
    {
        private string Directory = System.AppDomain.CurrentDomain.BaseDirectory + "/GitHub/KinectTherapyTest/KinectTherapyContent/Exercises/";
        private float TestVariance = 10f;
        Exercise Exercise;
        AlignmentCriterion VerticalAlignmentCriterion;
        AlignmentCriterion HorizontalAlignmentCriterion;
        AlignmentCriterion ThreeJointAlignmentCriterion;
        AngleCriterion AngleCriterion;

        [SetUp]
        public void Init()
        {
            //Load test exercise from XML
            XmlSerializer serializer = new XmlSerializer(typeof(Exercise));
            StreamReader reader = new StreamReader(Directory + "EXELAE.xml");
            // deserialize the xml and create an Exercise
            Exercise = (Exercise)serializer.Deserialize(reader);

            // create vertical alignment criterion
            XmlJointType[] RightHipAndAnkle = new XmlJointType[2];
            RightHipAndAnkle[0] = new XmlJointType(JointType.HipRight.ToString());
            RightHipAndAnkle[1] = new XmlJointType(JointType.AnkleRight.ToString());
            VerticalAlignmentCriterion = new AlignmentCriterion(RightHipAndAnkle, Alignment.Vertical, TestVariance);

            // create horizontal alignment criterion
            HorizontalAlignmentCriterion = new AlignmentCriterion(RightHipAndAnkle, Alignment.Horizontal, TestVariance);

            // create three joint alignment criterion
            XmlJointType RightKnee = new XmlJointType(JointType.KneeRight.ToString());
            ThreeJointAlignmentCriterion = new AlignmentCriterion(RightKnee, RightHipAndAnkle, TestVariance);

            // create angle criterion
            AngleCriterion = new AngleCriterion(90, RightKnee, RightHipAndAnkle, TestVariance);

        }

        [TestAttribute(Description = "Reference ID: UC-8 Step 4")]
        public void TestAlignmentCheckForm()
        {
            #region Setup Skeleton
            Skeleton[] skeletonData = new Skeleton[1];
            Skeleton skeleton = new Skeleton();
            skeleton.TrackingState = SkeletonTrackingState.Tracked;
            /* this needs to be done because you can't set properties directly on the skelton's Joint object:
             * you must create a new joint, 
             * set the properties,
             * then set the new joint on the skeleton */
            SkeletonPoint sp = new SkeletonPoint();
            sp.X = 1.0F;
            sp.Y = 1.0F;
            sp.Z = 1.0F;
            Joint hip = skeleton.Joints[JointType.HipRight];
            hip.Position = sp;
            skeleton.Joints[JointType.HipRight] = hip;
            Joint knee = skeleton.Joints[JointType.KneeRight];
            knee.Position = sp;
            skeleton.Joints[JointType.KneeRight] = knee;
            Joint ankle = skeleton.Joints[JointType.AnkleRight];
            ankle.Position = sp;
            skeleton.Joints[JointType.AnkleRight] = ankle;
            skeletonData[0] = skeleton;

            System.Console.WriteLine("Hip Position X: " + hip.Position.X);
            System.Console.WriteLine("Skel Hip Position X: " + skeleton.Joints[JointType.HipRight].Position.X);
            SkeletonStamp skeletonStamp = new SkeletonStamp(skeletonData,1);
            #endregion

            Exercise alignmentExercise = new Exercise();

            #region Check Vertical Alignment
            alignmentExercise.TrackingCriteria = new Criterion[] { VerticalAlignmentCriterion };
            double[] percentBad = alignmentExercise.CheckForm(skeletonStamp);

            /* check the hip and ankle percent bad */
            Assert.AreEqual(0, percentBad[(int)JointType.HipRight]);
            Assert.IsNaN(percentBad[(int)JointType.KneeRight]); /* this is not part of the criteria and should not be set */;
            Assert.AreEqual(0, percentBad[(int)JointType.AnkleRight]);
            #endregion

            #region Check Horizontal Alignment
            alignmentExercise.TrackingCriteria = new Criterion[] { HorizontalAlignmentCriterion };
            percentBad = alignmentExercise.CheckForm(skeletonStamp);

            /* check the hip and ankle percent bad */
            Assert.AreEqual(0, percentBad[(int)JointType.HipRight]);
            Assert.IsNaN(percentBad[(int)JointType.KneeRight]); /* this is not part of the criteria and should not be set */;
            Assert.AreEqual(0, percentBad[(int)JointType.AnkleRight]);
            #endregion

            #region Check Three Joint Alignment
            alignmentExercise.TrackingCriteria = new Criterion[] { ThreeJointAlignmentCriterion };
            percentBad = alignmentExercise.CheckForm(skeletonStamp);

            /* check the hip and ankle percent bad */
            Assert.AreEqual(0, percentBad[(int)JointType.HipRight]);
            Assert.AreEqual(0,percentBad[(int)JointType.KneeRight]); /* this is NOW part of the criteria and should be set */;
            Assert.AreEqual(0, percentBad[(int)JointType.AnkleRight]);
            #endregion
        }

        [TestAttribute(Description = "Reference ID: UC-7 Step 4")]
        public void TestAlignmentMatchesCriteria()
        {
            #region Setup Skeleton
            Skeleton[] skeletonData = new Skeleton[1];
            Skeleton skeleton = new Skeleton();
            skeleton.TrackingState = SkeletonTrackingState.Tracked;
            /* this needs to be done because you can't set properties directly on the skelton's Joint object:
             * you must create a new joint, 
             * set the properties,
             * then set the new joint on the skeleton */
            SkeletonPoint sp = new SkeletonPoint();
            sp.X = 1.0F;
            sp.Y = 1.0F;
            sp.Z = 1.0F;
            Joint hip = skeleton.Joints[JointType.HipRight];
            hip.Position = sp;
            skeleton.Joints[JointType.HipRight] = hip;
            Joint knee = skeleton.Joints[JointType.KneeRight];
            knee.Position = sp;
            skeleton.Joints[JointType.KneeRight] = knee;
            Joint ankle = skeleton.Joints[JointType.AnkleRight];
            ankle.Position = sp;
            skeleton.Joints[JointType.AnkleRight] = ankle;
            skeletonData[0] = skeleton;

            System.Console.WriteLine("Hip Position X: " + hip.Position.X);
            System.Console.WriteLine("Skel Hip Position X: " + skeleton.Joints[JointType.HipRight].Position.X);
            SkeletonStamp skeletonStamp = new SkeletonStamp(skeletonData, 1);
            #endregion

            Exercise alignmentExercise = new Exercise();
            #region Matches Alignment
            alignmentExercise.StartingCriteria = new Criterion[] { VerticalAlignmentCriterion };
            Assert.IsTrue(alignmentExercise.matchesCriteria(skeletonStamp,alignmentExercise.StartingCriteria));
            #endregion

            #region Does Not Match Alignment
            SkeletonPoint misalignedPoint = new SkeletonPoint();
            misalignedPoint.X = 100.0F;
            misalignedPoint.Y = 1.0F;
            misalignedPoint.Z = 1.0F;
            hip = skeleton.Joints[JointType.HipRight];
            hip.Position = misalignedPoint;
            skeleton.Joints[JointType.HipRight] = hip;

            System.Console.WriteLine("Hip Position X: " + hip.Position.X);
            System.Console.WriteLine("Skel Hip Position X: " + skeleton.Joints[JointType.HipRight].Position.X);
            System.Console.WriteLine("Skel Knee Position X: " + skeleton.Joints[JointType.KneeRight].Position.X);
            Assert.IsFalse(alignmentExercise.matchesCriteria(skeletonStamp,alignmentExercise.StartingCriteria));
            #endregion
        }

        [TestAttribute]
        public void TestAlignmentMinMaxCorrect()
        {
            // Tests to make sure the min value and max value are correct based on a given Variance
            Assert.AreEqual(0f + 1f * TestVariance * .01f, VerticalAlignmentCriterion.MaximumAcceptedRange);
            Assert.AreEqual(0f - 1f * TestVariance * .01f, VerticalAlignmentCriterion.MinimumAcceptedRange);
        }

        [TestAttribute(Description = "Reference ID: UC-8 Step 4")]
        public void TestAngleCheckForm()
        {
            #region Setup Skeleton
            Skeleton[] skeletonData = new Skeleton[1];
            Skeleton skeleton = new Skeleton();
            skeleton.TrackingState = SkeletonTrackingState.Tracked;
            /* this needs to be done because you can't set properties directly on the skelton's Joint object:
             * you must create a new joint, 
             * set the properties,
             * then set the new joint on the skeleton */
            SkeletonPoint hipPoint = new SkeletonPoint();
            hipPoint.X = 1.0F;
            hipPoint.Y = 1.0F;
            hipPoint.Z = 1.0F;
            Joint hip = skeleton.Joints[JointType.HipRight];
            hip.Position = hipPoint;
            skeleton.Joints[JointType.HipRight] = hip;

            SkeletonPoint kneePoint = new SkeletonPoint();
            kneePoint.X = 2.0F;
            kneePoint.Y = 1.0F;
            kneePoint.Z = 1.0F;
            Joint knee = skeleton.Joints[JointType.KneeRight];
            knee.Position = kneePoint;

            SkeletonPoint anklePoint = new SkeletonPoint();
            anklePoint.X = 2.0F;
            anklePoint.Y = 0.0F;
            anklePoint.Z = 1.0F;
            skeleton.Joints[JointType.KneeRight] = knee;
            Joint ankle = skeleton.Joints[JointType.AnkleRight];
            ankle.Position = anklePoint;
            skeleton.Joints[JointType.AnkleRight] = ankle;
            skeletonData[0] = skeleton;
            SkeletonStamp skeletonStamp = new SkeletonStamp(skeletonData, 1);
            #endregion
            Exercise angleExercise = new Exercise();

            #region Angle In Range
            angleExercise.TrackingCriteria = new Criterion[1] { AngleCriterion };
            double[] jointImperfection = angleExercise.CheckForm(skeletonStamp);
            // should be exactly 90 degrees
            Assert.AreEqual(0, jointImperfection[(int)JointType.KneeRight]);
            Assert.AreEqual(0, jointImperfection[(int)JointType.HipRight]);
            Assert.AreEqual(0, jointImperfection[(int)JointType.AnkleRight]);
            #endregion

            #region Angle Out Of Range
            // change the skeleton to be out of range
            SkeletonPoint misalignedPoint = new SkeletonPoint();
            misalignedPoint.X = 2.0F;
            misalignedPoint.Y = 0.0F;
            misalignedPoint.Z = 1.0F;
            hip = skeleton.Joints[JointType.HipRight];
            hip.Position = misalignedPoint;
            skeleton.Joints[JointType.HipRight] = hip;
            jointImperfection = angleExercise.CheckForm(skeletonStamp);
            Assert.AreNotEqual(0, jointImperfection[(int)JointType.KneeRight]);
            Assert.AreNotEqual(0, jointImperfection[(int)JointType.HipRight]);
            Assert.AreNotEqual(0, jointImperfection[(int)JointType.AnkleRight]);
            #endregion
        }

        [TestAttribute(Description = "Reference ID: UC-7 Step 4")]
        public void TestAngleMatchesCriteria()
        {
            #region Setup Skeleton
            Skeleton[] skeletonData = new Skeleton[1];
            Skeleton skeleton = new Skeleton();
            skeleton.TrackingState = SkeletonTrackingState.Tracked;
            /* this needs to be done because you can't set properties directly on the skelton's Joint object:
             * you must create a new joint, 
             * set the properties,
             * then set the new joint on the skeleton */
            SkeletonPoint hipPoint = new SkeletonPoint();
            hipPoint.X = 1.0F;
            hipPoint.Y = 1.0F;
            hipPoint.Z = 1.0F;
            Joint hip = skeleton.Joints[JointType.HipRight];
            hip.Position = hipPoint;
            skeleton.Joints[JointType.HipRight] = hip;

            SkeletonPoint kneePoint = new SkeletonPoint();
            kneePoint.X = 2.0F;
            kneePoint.Y = 1.0F;
            kneePoint.Z = 1.0F;
            Joint knee = skeleton.Joints[JointType.KneeRight];
            knee.Position = kneePoint;

            // slightly off 90 but should still match
            SkeletonPoint anklePoint = new SkeletonPoint();
            anklePoint.X = 2.0F;
            anklePoint.Y = 0.01F;
            anklePoint.Z = 1.0F;
            skeleton.Joints[JointType.KneeRight] = knee;
            Joint ankle = skeleton.Joints[JointType.AnkleRight];
            ankle.Position = anklePoint;
            skeleton.Joints[JointType.AnkleRight] = ankle;
            skeletonData[0] = skeleton;
            SkeletonStamp skeletonStamp = new SkeletonStamp(skeletonData, 1);
            #endregion
            Exercise angleExercise = new Exercise();

            #region Angle Matches
            angleExercise.StartingCriteria = new Criterion[1] { AngleCriterion };
            Assert.IsTrue(angleExercise.matchesCriteria(skeletonStamp,angleExercise.StartingCriteria));
            #endregion

            #region Angle Does Not Match
            SkeletonPoint misalignedPoint = new SkeletonPoint();
            misalignedPoint.X = 2.0F;
            misalignedPoint.Y = 0.0F;
            misalignedPoint.Z = 1.0F;
            hip = skeleton.Joints[JointType.HipRight];
            hip.Position = misalignedPoint;
            skeleton.Joints[JointType.HipRight] = hip;
            Assert.IsFalse(angleExercise.matchesCriteria(skeletonStamp, angleExercise.StartingCriteria));
            #endregion  
        }

        [TestAttribute]
        public void TestAngleMinMaxCorrect()
        {
            // Tests to make sure the min value and max value are correct based on a given Variance
            Assert.AreEqual(90 + ((180 * TestVariance * .01f) / 2), AngleCriterion.MaximumAngle);
            Assert.AreEqual(90 - ((180 * TestVariance * .01f) / 2), AngleCriterion.MinimumAngle);
        }

        [TestAttribute(Description = "Reference ID: UC-7 Step 4b")]
        public void TestExerciseVariancePushedToCriterion()
        {
            //set variance to 10%
            System.Console.WriteLine("Setting the variance: " + TestVariance);
            Exercise.Variance = TestVariance;
            Assert.AreEqual(TestVariance, Exercise.Variance);

            // check all checkpoints to see if variance is 10%
            foreach (Checkpoint checkpoint in Exercise.Checkpoints)
            {
                AssertVarianceMatches(TestVariance, checkpoint.Criteria);
            }
            // check starting criteria to see if variance is 10%
            AssertVarianceMatches(TestVariance, Exercise.StartingCriteria);
            // check tracking criteria to see if variance is 10%
            AssertVarianceMatches(TestVariance, Exercise.TrackingCriteria);
        }

        /// <summary>
        /// Compares each Variance in a set of criterion to a specific value
        /// </summary>
        /// <param name="TestVariance"></param>
        /// <param name="Criteria"></param>
        private static void AssertVarianceMatches(float TestVariance, Criterion[] Criteria)
        {
            foreach (Criterion criterion in Criteria)
            {
                Assert.AreEqual(TestVariance, criterion.Variance);
            }
        }

        [TestAttribute]
        public void TestCreateStartingSkeleton()
        {
            #region Setup Skeleton
            Skeleton[] skeletonData = new Skeleton[1];
            Skeleton skeleton = new Skeleton();
            skeleton.TrackingState = SkeletonTrackingState.Tracked;
            /* this needs to be done because you can't set properties directly on the skelton's Joint object:
             * you must create a new joint, 
             * set the properties,
             * then set the new joint on the skeleton */
            SkeletonPoint sp = new SkeletonPoint();
            sp.X = 1.0F;
            sp.Y = 1.0F;
            sp.Z = 1.0F;
            Joint shoulder = skeleton.Joints[JointType.ShoulderLeft];
            shoulder.Position = sp;
            skeleton.Joints[JointType.ShoulderLeft] = shoulder;
            Joint elbow = skeleton.Joints[JointType.ElbowLeft];
            elbow.Position = sp;
            skeleton.Joints[JointType.ElbowLeft] = elbow;
            Joint wrist = skeleton.Joints[JointType.WristLeft];
            wrist.Position = sp;
            skeleton.Joints[JointType.WristLeft] = wrist;
            skeletonData[0] = skeleton;

            System.Console.WriteLine("Shoulder Position X: " + shoulder.Position.X);
            System.Console.WriteLine("Skel Shoulder Position X: " + skeleton.Joints[JointType.ShoulderLeft].Position.X);
            SkeletonStamp skeletonStamp = new SkeletonStamp(skeletonData, 1);

            Skeleton startingSkeleton = Exercise.createStartingSkeleton(skeletonStamp);
            Skeleton[] startingSkeletonData = new Skeleton[1];
            SkeletonStamp startingSkeletonStamp = new SkeletonStamp(startingSkeletonData, 999);
            Assert.IsTrue(Exercise.matchesCriteria(startingSkeletonStamp, Exercise.StartingCriteria), "Created skeleton does not match starting criteria");

            #endregion
        }


    }
}
