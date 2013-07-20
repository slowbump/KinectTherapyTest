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
    class GuiCheckboxTest
    {
        private GuiCheckbox gui;
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

        [Test(Description = "Use a checkbox")]
        public void Checkbox_Usage()
        {
            gui = new GuiCheckbox("button", new Vector2(500f, 500f), Vector2.Zero);

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
            Assert.IsFalse(gui.Hovered);
            Assert.IsFalse(gui.Checked);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), new GameTime());

            Assert.IsTrue(gui.Hovered);
            Assert.IsFalse(gui.Checked);

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
            Assert.IsTrue(gui.Hovered);
            Assert.IsTrue(gui.Checked);

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
            Assert.IsTrue(gui.Hovered);
            Assert.IsTrue(gui.Checked);

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
            Assert.IsTrue(gui.Hovered);
            Assert.IsFalse(gui.Checked);
        }
    }
}
