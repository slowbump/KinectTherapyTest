using System;
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
        private const float Scale = .75f;
        private readonly float[] _dataPoints = { 1f, .33f, 1f, .67f, 1f, -1f, 0f, .33f, 0f, 0f, 0f, .67f, 0f, 1f, 0f };
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

            string[] files = Directory.GetFiles(@"c:\school");

            foreach (string file in files)
            {
                File.Delete(file);
            }

        }

        [Test(Description = "UC-10: The Chart creates the texture the x-axis legend")]
        public void CreateXAxisTexture()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, Scale, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(1024, 780), Vector2.Zero, _guiChartOptions);

            Texture2D testing = _guiChart.CreateXAxisTitleTexture(_game, _contentManager, _spriteBatch);

            FileStream fs = File.Open(@"c:\school\Chart_X-Axis.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
            Assert.Pass("GuiChart.CreateXAxisTitleTexture passed.");
        }

        [Test(Description = "UC-10: The Chart creates the texture the y-axis legend")]
        public void CreateYAxisTexture()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, Scale, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(1024, 780), Vector2.Zero, _guiChartOptions);

            Texture2D testing = _guiChart.CreateYAxisTitleTexture(_game, _contentManager, _spriteBatch);

            FileStream fs = File.Open(@"c:\school\Chart_Y-Axis.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
            Assert.Pass("GuiChart.CreateYAxisTitleTexture passed.");
        }


        [Test(Description = "UC-10: Create Chart Texture")]
        public void DrawChartBase()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, Scale, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(1024, 780), Vector2.Zero, _guiChartOptions);
            _guiChart.LoadContent(_game, _contentManager, _spriteBatch);

            Texture2D testing = _guiChart.CreateChartTexture(_game, _contentManager, _spriteBatch);

            FileStream fs = File.Open(@"c:\school\ChartTexture.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
            Assert.Pass("GuiChart.CreateChartTexture passed.");
        }

        [Test(Description = "UC-10: The data point array is created")]
        public void DrawDataPoints()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, Scale, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(1024,780), Vector2.Zero, _guiChartOptions);
            _guiChart.LoadContent(_game, _contentManager, _spriteBatch);

            Texture2D testing = _guiChart.DrawDataPointTexture(_game, _spriteBatch, _dataPoints, _timeSpan);

            FileStream fs = File.Open(@"c:\school\DataPointTexture.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
            Assert.Pass("GuiChart.DrawDataPointTexture passed.");
        }

        [Test(Description = "UC-?: Clicking in the chart generates an X-coord / Y-coord percentage pair")]
        public void ClickMouse()
        {
            _timeSpan = _dataPoints.Length;

            _guiChartOptions = new GuiChartOptions(_axesNames, ChartType, ChartLines, TickMarks, Scale, _dataPoints, _timeSpan, RepDuration);
            _guiChart = new GuiChart("Title", new Vector2(1024, 780), Vector2.Zero, _guiChartOptions);
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
            Assert.Pass("GuiChart.Update passed.");
        }
    }
}
