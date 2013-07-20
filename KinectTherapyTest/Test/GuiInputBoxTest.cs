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
    class GuiInputBoxTest
    {
        private GuiInputBox gui;
        private ContentManager contentManager;
        private Game game;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private CatalogManager catalogManager;

        [TestFixtureSetUp]
        public void SetUp()
        {
            game = new Game();
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

        [Test(Description = "Use an input box")]
        public void InputBox_Usage()
        {
            gui = new GuiInputBox("button", new Vector2(500f, 500f), Vector2.Zero, game, 10f, 100f, 0f);

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
                    ButtonState.Released,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);
            Assert.AreEqual(gui.Value, "10");
            Assert.AreEqual(gui.State, CheckboxState.Default);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());

            Assert.AreEqual(gui.State, CheckboxState.Hovered);

            oldMouseState =
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

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());
            Assert.AreEqual(gui.State, CheckboxState.Blinking);

            currentMouseState =
                new MouseState(
                    -100,
                    -100,
                    0,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released
                );

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());
            Assert.AreEqual(gui.State, CheckboxState.Blinking);

            /** uncheck */
            currentMouseState =
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

            oldMouseState =
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

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());
            Assert.AreEqual(gui.State, CheckboxState.Blinking);

            currentMouseState =
                new MouseState(
                    -100,
                    -100,
                    0,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released
                );

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());
            Assert.AreEqual(gui.State, CheckboxState.Blinking);
        }
    }
}
