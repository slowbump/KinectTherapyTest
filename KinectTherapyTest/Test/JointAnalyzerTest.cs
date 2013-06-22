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

        [TestAttribute]
        public void TestJointsAligned()
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
    }
}
