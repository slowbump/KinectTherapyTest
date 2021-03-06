﻿//------------------------------------------------------------------------------
// <copyright file="SkeletonStreamRenderer.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Kinect.Toolbox.Record;

namespace Microsoft.Samples.Kinect.XnaBasics
{
    using System;
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SWENG.Service;
    using SWENG;

    /// <summary>
    /// A delegate method explaining how to map a SkeletonPoint from one space to another.
    /// </summary>
    /// <param name="point">The SkeletonPoint to map.</param>
    /// <returns>The Vector2 representing the target location.</returns>
    public delegate Vector2 SkeletonStampPointMap(SkeletonPoint point);

    /// <summary>
    /// This class is responsible for rendering a skeleton stream.
    /// </summary>
    public class SkeletonStampStreamRenderer : Object2D
    {
        /// <summary>
        /// This is the map method called when mapping from
        /// skeleton space to the target space.
        /// </summary>
        private readonly SkeletonStampPointMap mapMethod;

        /// <summary>
        /// The last frames skeleton data.
        /// </summary>
        private static Skeleton[] skeletonData;

        /// <summary>
        /// This flag ensures only request a frame once per update call
        /// across the entire application.
        /// </summary>
        private static bool skeletonDrawn = true;

        /// <summary>
        /// The origin (center) location of the joint texture.
        /// </summary>
        private Vector2 jointOrigin;

        /// <summary>
        /// The joint texture.
        /// </summary>
        private Texture2D jointTexture;

        /// <summary>
        /// The origin (center) location of the bone texture.
        /// </summary>
        private Vector2 boneOrigin;
        
        /// <summary>
        /// The bone texture.
        /// </summary>
        private Texture2D boneTexture;

        /// <summary>
        /// Whether the rendering has been initialized.
        /// </summary>
        private bool initialized;

        private SkeletonPool SkeletonPool
        {
            get
            {
                return (SkeletonPool)this.Game.Services.GetService(typeof(SkeletonPool));
            }
        }

        /// <summary>
        /// Initializes a new instance of the SkeletonStreamRenderer class.
        /// </summary>
        /// <param name="game">The related game object passed in by the color and depth stream renderers.</param>
        /// <param name="map">The method used to map the SkeletonPoint to the target space.</param>
        public SkeletonStampStreamRenderer(Game game, SkeletonStampPointMap map)
            : base(game)
        {
            this.mapMethod = map;
            this.RecordingManager.SkeletonEventListener.Add(Replay_SkeletonFrameReady);
        }

        void Replay_SkeletonFrameReady(object sender, ReplaySkeletonFrameReadyEventArgs args)
        {
            // Reallocate if necessary
            if (null == skeletonData || skeletonData.Length != args.SkeletonFrame.Skeletons.Length)
            {
                skeletonData = new Skeleton[args.SkeletonFrame.Skeletons.Length];
            }

            args.SkeletonFrame.Skeletons.CopyTo(skeletonData, 0);
            skeletonDrawn = false;
        }

        /// <summary>
        /// This method initializes necessary values.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.initialized = true;
        }

        /// <summary>
        /// This method retrieves a new skeleton frame if necessary.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // If the sensor is not found, not running, or not connected, stop now
            if (null == this.Chooser.Sensor ||
                false == this.Chooser.Sensor.IsRunning ||
                KinectStatus.Connected != this.Chooser.Sensor.Status)
            {
                return;
            }

            if (this.RecordingManager.Status == RecordingManagerStatus.Replaying)
            {
                return;
            }

            // If we have already drawn this skeleton, then we should retrieve a new frame
            // This prevents us from calling the next frame more than once per update
            if (skeletonDrawn)
            {
                using (var skeletonFrame = this.Chooser.Sensor.SkeletonStream.OpenNextFrame(0))
                {
                    // Sometimes we get a null frame back if no data is ready
                    if (null == skeletonFrame)
                    {
                        return;
                    }

                    if (this.RecordingManager.Status == RecordingManagerStatus.Recording)
                    {
                        this.RecordingManager.Record(skeletonFrame);
                    }

                    // Reallocate if necessary
                    if (null == skeletonData || skeletonData.Length != skeletonFrame.SkeletonArrayLength)
                    {
                        skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    SkeletonPool.Add(skeletonData, skeletonFrame.Timestamp);

                    skeletonDrawn = false;
                }
            }
        }

        /// <summary>
        /// This method draws the skeleton frame data.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            skeletonDrawn = true;
            // If the joint texture isn't loaded, load it now
            if (null == this.jointTexture)
            {
                this.LoadContent();
            }
            SkeletonStamp skeletonData = SkeletonPool.GetOldestProcessedSkeleton();
            // If we don't have data, lets leave
            if (null == skeletonData || null == this.mapMethod)
            {
                return;
            }
            Skeleton skeleton = skeletonData.GetTrackedSkeleton();
            if (null == skeleton)
            {
                return;
            }
            if (false == this.initialized)
            {
                this.Initialize();
            }

            this.SharedSpriteBatch.Begin();

            // Draw Bones
            this.DrawBone(skeleton.Joints, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton.Joints, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton.Joints, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton.Joints, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton.Joints, JointType.HipCenter, JointType.HipRight);

            this.DrawBone(skeleton.Joints, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton.Joints, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton.Joints, JointType.WristLeft, JointType.HandLeft);

            this.DrawBone(skeleton.Joints, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton.Joints, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton.Joints, JointType.WristRight, JointType.HandRight);

            this.DrawBone(skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

            this.DrawBone(skeleton.Joints, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton.Joints, JointType.AnkleRight, JointType.FootRight);

            // Now draw the joints
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            foreach (Joint j in skeleton.Joints)
            {
                /* its good unless proven bad.*/
                Color jointColor = Color.Green;
                
                /*  How do we know its bad? 
                 * Check the deviation field */
                double deviation = skeletonData.PercentBad[(int)j.JointType];
                if (!Double.IsNaN(deviation))
                {
                    if (deviation > .01 || deviation < -.01)
                    {
                        jointColor = Color.Red;
                    }

                    this.SharedSpriteBatch.Draw(
                    this.jointTexture,
                    this.mapMethod(j.Position),
                    null,
                    jointColor,
                    0.0f,
                    this.jointOrigin,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
                }
            }

            this.SharedSpriteBatch.End();
            // we are now done with this skeleton data remove all previous from the pool
            SkeletonPool.Remove(skeletonData.TimeStamp);
            base.Draw(gameTime);
        }

        /// <summary>
        /// This method loads the textures and sets the origin values.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            this.jointTexture = Game.Content.Load<Texture2D>("Joint");
            this.jointOrigin = new Vector2(this.jointTexture.Width / 2, this.jointTexture.Height / 2);

            this.boneTexture = Game.Content.Load<Texture2D>("Bone");
            this.boneOrigin = new Vector2(0.5f, 0.0f);
        }

        /// <summary>
        /// This method draws a bone.
        /// </summary>
        /// <param name="joints">The joint data.</param>
        /// <param name="startJoint">The starting joint.</param>
        /// <param name="endJoint">The ending joint.</param>
        private void DrawBone(JointCollection joints, JointType startJoint, JointType endJoint)
        {
            Vector2 start = this.mapMethod(joints[startJoint].Position);
            Vector2 end = this.mapMethod(joints[endJoint].Position);
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(1.0f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;

            Color color = Color.LightGreen;
            if (joints[startJoint].TrackingState != JointTrackingState.Tracked ||
                joints[endJoint].TrackingState != JointTrackingState.Tracked)
            {
                color = Color.Gray;
            }

            this.SharedSpriteBatch.Draw(this.boneTexture, start, null, color, angle, this.boneOrigin, scale, SpriteEffects.None, 1.0f);
        }
    }
}
