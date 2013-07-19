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
    public class GuiButtonTest
    {
        private GuiButton gui;
        private ContentManager contentManager;
        private Game game;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool isClickedOn = false;

        public void clickedOn(object sender, GuiButtonClickedArgs e)
        {
            isClickedOn = true;
        }


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
        }

        [SetUp]
        public void TestSetUp()
        {
            isClickedOn = false;
        }

        [Test(Description = "Create the button & hover")]
        public void FirstTest()
        {
            gui = new GuiButton("button1", new Vector2(500, 500), Vector2.Zero);
            gui.LoadContent(game, contentManager, spriteBatch);

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
            Assert.IsFalse(gui.Hovered);

            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.IsTrue(gui.Hovered);
        }

        [Test(Description = "Clicking the button fires an event")]
        public void FourthTest()
        {
            gui = new GuiButton("UpdateQueue", new Vector2(500, 500), Vector2.Zero);
            gui.ClickEvent += clickedOn;
            gui.LoadContent(game, contentManager, spriteBatch);

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
                    ButtonState.Pressed,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);
            Assert.IsFalse(gui.Hovered);


            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));
            Assert.IsFalse(isClickedOn);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.IsTrue(gui.Hovered);
            Assert.IsTrue(isClickedOn);

            gui.ClickEvent -= clickedOn;
        }
    }
}
