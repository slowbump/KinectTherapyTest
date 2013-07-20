﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SWENG.Service;
using SWENG.UserInterface;

namespace KinectTherapyTest.Test
{
    [TestFixture]
    class GuiChartTest
    {
        private GuiChart _guiChart;
        private GuiChartOptions _guiChartOptions;
        private ContentManager _contentManager;
        private Game _game;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private readonly string[] _axesNames = { "Time - seconds", "Deviation" };
        private const string ChartType = "Repetitions";
        private const bool ChartLines = true;
        private const bool TickMarks = true;
        private const int MarkerSize = 2;
        private readonly float[] _dataPoints = { 1f, .33f, 1f, .67f, 1f, -1f, 0f, -.33f, 0f, 0f, 0f, -.67f, 0f, 1f, 0f };
        private float _timeSpan;
        private const float RepDuration = 47500;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _game = new Game();
            _graphicsDeviceManager = new GraphicsDeviceManager(_game) { GraphicsProfile = GraphicsProfile.HiDef };
            _contentManager = new ContentManager(_game.Services)
                {
                    RootDirectory = @"C:\Content"
                };
            _game.Run();

            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _game.Services.AddService(typeof(SpriteBatch), _spriteBatch);
        }

        [Test(Description = "Test creation of X-axis texture")]
        public void CreateXAxisTexture()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, MarkerSize, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(500, 500), Vector2.Zero, _guiChartOptions);

            Texture2D testing = _guiChart.CreateXAxisTitleTexture(_game, _contentManager, _spriteBatch);

            FileStream fs = File.Open(@"c:\school\Chart_X-Axis.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }

        [Test(Description = "Test creation of Y-axis texture")]
        public void CreateYAxisTexture()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, MarkerSize, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(500, 500), Vector2.Zero, _guiChartOptions);

            Texture2D testing = _guiChart.CreateYAxisTitleTexture(_game, _contentManager, _spriteBatch);

            FileStream fs = File.Open(@"c:\school\Chart_Y-Axis.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }

        [Test(Description = "First test is to create chart")]
        public void CreateChart()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, MarkerSize, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(500,500), Vector2.Zero, _guiChartOptions);
            _guiChart.LoadContent(_game, _contentManager, _spriteBatch);

        }

        [Test(Description = "Clicking in the chart generates an X-coord / Y-coord percentage pair")]
        public void ClickMouse()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, MarkerSize, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(500, 500), Vector2.Zero, _guiChartOptions);
            _guiChart.LoadContent(_game, _contentManager, _spriteBatch);

            MouseState currentMouseState = new MouseState(
                    50,
                    50,
                    0,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released
                );

            MouseState oldMouseState = new MouseState(
                    currentMouseState.X,
                    currentMouseState.Y,
                    currentMouseState.ScrollWheelValue,
                    ButtonState.Released,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);

            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));

            _guiChart.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.IsNotNull(_guiChart.MouseXCoord);
            Assert.IsNotNull(_guiChart.MouseYCoord);

            Assert.IsNotNull(_guiChart.MouseXPercent);
            Assert.IsNotNull(_guiChart.MouseYPercent);
        }
    }
}
