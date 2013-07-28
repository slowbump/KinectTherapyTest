using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Kinect;

namespace SWENG.Criteria
{
    [TestFixture]
    public class JointAnalyzerTest
    {

        [SetUp]
        public void Init()
        {
        }

        [TestAttribute(Description = "Reference ID: UC-9 Step 3")]
        public void TestThreeJointsAreAligned()
        {
            #region align on the x and z axis
            Joint center = new Joint();
            Joint[] otherJoints = new Joint[2];

            SkeletonPoint sp = new SkeletonPoint();
            sp.X = 1.0F;
            sp.Y = 1.0F;
            sp.Z = 1.0F;
            center.Position = sp;

            Joint joint1 = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            joint1.Position = point;

            Joint joint2 = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 2.0F;
            point2.Z = 1.0F;
            joint2.Position = point2;

            otherJoints[0] = joint1;
            otherJoints[1] = joint2;

            Assert.AreEqual(1.0F, JointAnalyzer.areJointsAligned(center, otherJoints), "Joints should be aligned but are not: X and Z axis");
            #endregion

            #region align on the y and z axis
            center = new Joint();
            otherJoints = new Joint[2];

            sp = new SkeletonPoint();
            sp.X = 1.0F;
            sp.Y = 1.0F;
            sp.Z = 1.0F;
            center.Position = sp;

            joint1 = new Joint();
            point = new SkeletonPoint();
            point.X = 21.0F;
            point.Y = 1.0F;
            point.Z = 1.0F;
            joint1.Position = point;

            joint2 = new Joint();
            point2 = new SkeletonPoint();
            point2.X = 0.0F;
            point2.Y = 1.0F;
            point2.Z = 1.0F;
            joint2.Position = point2;

            otherJoints[0] = joint1;
            otherJoints[1] = joint2;

            Assert.AreEqual(1.0F, JointAnalyzer.areJointsAligned(center, otherJoints), "Joints should be aligned but are not: Y and Z axis");
            #endregion

            #region align on the x and y axis
            center = new Joint();
            otherJoints = new Joint[2];

            sp = new SkeletonPoint();
            sp.X = 1.0F;
            sp.Y = 1.0F;
            sp.Z = 1.0F;
            center.Position = sp;

            joint1 = new Joint();
            point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 1.0F;
            point.Z = 0.0F;
            joint1.Position = point;

            joint2 = new Joint();
            point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 1.0F;
            point2.Z = 2.0F;
            joint2.Position = point2;

            otherJoints[0] = joint1;
            otherJoints[1] = joint2;

            Assert.AreEqual(1.0F, JointAnalyzer.areJointsAligned(center, otherJoints), "Joints should be aligned but are not: X and Y axis");
            #endregion
        }

        [TestAttribute]
        public void TestJointsAreVerticallyAligned()
        {
            #region align on the y axis

            Joint joint1 = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            joint1.Position = point;

            Joint joint2 = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = -66.0F;
            point2.Z = 1.0F;
            joint2.Position = point2;

            Assert.AreEqual(1.0F, JointAnalyzer.alignedVertically(joint1,joint2), "Joints should be aligned vertically");
            #endregion
        }

        [TestAttribute]
        public void TestJointsAreHorizontallyAligned()
        {
            #region align on the x axis

            Joint joint1 = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            joint1.Position = point;

            Joint joint2 = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 55.0F;
            point2.Y = 0.0F;
            point2.Z = 1.0F;
            joint2.Position = point2;

            Assert.AreEqual(1.0F, JointAnalyzer.alignedHorizontally(joint1, joint2), "Joints should be aligned horizontally");
            #endregion
        }

        [TestAttribute]
        public void TestJointsMisaligned()
        {
            #region Orthogonal
            Joint center = new Joint();
            Joint[] otherJoints = new Joint[2];

            SkeletonPoint sp = new SkeletonPoint();
            sp.X = 0.0F;
            sp.Y = 1.0F;
            sp.Z = 0.0F;
            center.Position = sp;

            Joint joint1 = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 0.0F;
            point.Y = 0.0F;
            point.Z = 0.0F;
            joint1.Position = point;

            Joint joint2 = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 0.0F;
            point2.Z = 0.0F;
            joint2.Position = point2;

            otherJoints[0] = joint1;
            otherJoints[1] = joint2;

            Assert.AreEqual(0.0F, JointAnalyzer.areJointsAligned(center, otherJoints), "Joint vectors should be orthogonal (90 degrees)");
            #endregion
        }

        [TestAttribute]
        public void Test90DegreeAngle()
        {
            Joint vertex = new Joint();
            Joint[] otherJoints = new Joint[2];

            SkeletonPoint sp = new SkeletonPoint();
            sp.X = 0.0F;
            sp.Y = 0.0F;
            sp.Z = 0.0F;
            vertex.Position = sp;

            Joint joint1 = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 0.0F;
            point.Y = 1.0F;
            point.Z = 0.0F;
            joint1.Position = point;

            Joint joint2 = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 0.0F;
            point2.Z = 0.0F;
            joint2.Position = point2;

            otherJoints[0] = joint1;
            otherJoints[1] = joint2;

            Assert.AreEqual(90, JointAnalyzer.findAngle(vertex,otherJoints), "Angle should be 90 degrees");
        }

        [TestAttribute]
        public void TestIsRightOf()
        {
            #region align on the x axis

            Joint leftJoint = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            leftJoint.Position = point;

            Joint rightJoint = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 55.0F;
            point2.Y = 0.0F;
            point2.Z = 1.0F;
            rightJoint.Position = point2;

            Assert.IsTrue(JointAnalyzer.isRightOf(rightJoint, leftJoint), "Right Joint should be to the right of left Joint");
            #endregion
        }

        [TestAttribute]
        public void TestIsLeftOf()
        {
            #region align on the x axis

            Joint leftJoint = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            leftJoint.Position = point;

            Joint rightJoint = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 55.0F;
            point2.Y = 0.0F;
            point2.Z = 1.0F;
            rightJoint.Position = point2;

            Assert.IsTrue(JointAnalyzer.isLeftOf(leftJoint,rightJoint), "Right Joint should be to the right of left Joint");
            #endregion
        }

        [TestAttribute]
        public void TestIsAbove()
        {
            #region align on the x axis

            Joint aboveJoint = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 99.0F;
            point.Z = 1.0F;
            aboveJoint.Position = point;

            Joint belowJoint = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 0.0F;
            point2.Z = 1.0F;
            belowJoint.Position = point2;

            Assert.IsTrue(JointAnalyzer.isAbove(aboveJoint, belowJoint), "Right Joint should be to the right of left Joint");
            #endregion
        }

        [TestAttribute]
        public void TestIsBelow()
        {
            #region align on the x axis

            Joint aboveJoint = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 99.0F;
            point.Z = 1.0F;
            aboveJoint.Position = point;

            Joint belowJoint = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 0.0F;
            point2.Z = 1.0F;
            belowJoint.Position = point2;

            Assert.IsTrue(JointAnalyzer.isBelow(belowJoint, aboveJoint), "Right Joint should be to the right of left Joint");
            #endregion
        }

        [TestAttribute]
        public void TestIsAhead()
        {
            #region align on the x axis

            Joint aheadPoint = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            aheadPoint.Position = point;

            Joint behindJoint = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 0.0F;
            point2.Z = 777.0F;
            behindJoint.Position = point2;

            Assert.IsTrue(JointAnalyzer.isAhead(aheadPoint, behindJoint), "Right Joint should be to the right of left Joint");
            #endregion
        }

        [TestAttribute]
        public void TestIsBehind()
        {
            #region align on the x axis

            Joint aheadPoint = new Joint();
            SkeletonPoint point = new SkeletonPoint();
            point.X = 1.0F;
            point.Y = 0.0F;
            point.Z = 1.0F;
            aheadPoint.Position = point;

            Joint behindJoint = new Joint();
            SkeletonPoint point2 = new SkeletonPoint();
            point2.X = 1.0F;
            point2.Y = 0.0F;
            point2.Z = 777.0F;
            behindJoint.Position = point2;

            Assert.IsTrue(JointAnalyzer.isBehind(behindJoint, aheadPoint), "Right Joint should be to the right of left Joint");
            #endregion
        }
    }
}
