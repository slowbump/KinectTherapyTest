using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using SWENG.Service;
using SWENG.UserInterface;
using SWENG.Criteria;

namespace KinectTherapyTest.Test
{
    [TestFixture]
    class ReplayTileTest
    {
        private ReplayTile gui;
        private ContentManager contentManager;
        private Game game;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CatalogManager catalogManager;
        private string _replayId;

        private void OnSelect(object sender, ReplaySelectedEventArgs args)
        {
            _replayId = args.ID;
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            game = new Game();
            _replayId = string.Empty;
            graphics = new GraphicsDeviceManager(game);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            contentManager = new ContentManager(game.Services);
            contentManager.RootDirectory = @"C:\Content\";
            game.Run();

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            game.Services.AddService(typeof(SpriteBatch), spriteBatch);

            catalogManager = new CatalogManager();
            catalogManager.AddExerciseToSelected("fakeExercise", "fake");

            game.Services.AddService(typeof(CatalogManager), catalogManager);
        }

        [Test(Description = "Reference ID: UC-3, Step 4, 4a.4")]
        public void ReplaySelected()
        {
            gui = new ReplayTile(new Vector2(500f, 500f), Vector2.Zero, "fileId", "Name of the exercise", 0);

            gui.OnSelected += OnSelect;
            gui.LoadContent(game, contentManager, spriteBatch);

            MouseState currentMouseState =
                new MouseState(
                    50,
                    50,
                    0,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released
                );

            MouseState oldMouseState =
                new MouseState(
                    currentMouseState.X,
                    currentMouseState.Y,
                    currentMouseState.ScrollWheelValue,
                    ButtonState.Pressed,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsEmpty(_replayId);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());

            Assert.AreEqual("fileId", _replayId);
        }
    }
}
