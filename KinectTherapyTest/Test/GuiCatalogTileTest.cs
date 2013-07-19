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
    public class GuiCatalogTileTest
    {
        private GuiCatalogTile gui;
        private ContentManager contentManager;
        private Game game;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool isClickedOn = false;
        private CatalogManager catalogManager;

        public void clickedOn(object sender, EditCatalogSettingsArgs e)
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

            catalogManager = new CatalogManager();
            game.Services.AddService(typeof(CatalogManager), catalogManager);
        }

        [SetUp]
        public void TestSetUp()
        {
            catalogManager.ClearWorkout();
        }

        [Test(Description = "Create the title texture: 500 x 100")]
        public void Small_size()
        {
            gui = new GuiCatalogTile(game, "itemId", "firstTest", "this is it description", new Vector2(500, 100), Vector2.Zero);

            gui.LoadContent(game, contentManager, spriteBatch);

            Texture2D testing = gui.CreateNewTexture();

            FileStream fs = File.Open(@"c:\school\catalogTile_firstTest.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }

        [Test(Description = "Create the title texture: 500 x 500")]
        public void Big_size()
        {
            gui = new GuiCatalogTile(game, "itemId", "secondTest", "this is it description", new Vector2(500, 500), Vector2.Zero);

            gui.LoadContent(game, contentManager, spriteBatch);

            Texture2D testing = gui.CreateNewTexture();

            FileStream fs = File.Open(@"c:\school\catalogTile_secondTest.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }

        [Test(Description = "Create the title texture: 500 x 500: Very long description")]
        public void Long_description()
        {
            gui = new GuiCatalogTile(game, "itemId", "thirdTest", @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur quis scelerisque diam. Suspendisse id convallis dui. Mauris tincidunt lacinia purus, in iaculis nisl mattis sed. Etiam congue iaculis justo sit amet sollicitudin. Morbi faucibus dui quis accumsan varius. Fusce sit amet ante lectus. Duis non nisl quis tellus euismod ultrices eu vel elit. In consequat orci in elit tincidunt, quis vestibulum orci tempor. Nullam luctus nibh dolor, viverra placerat dolor sagittis suscipit. Mauris et dolor nunc. Phasellus convallis erat dui, at rhoncus nibh hendrerit nec. Morbi egestas gravida posuere. Suspendisse ut auctor quam, vel adipiscing risus. Aenean eget neque venenatis libero elementum iaculis id vel est. Sed non arcu at nisl faucibus interdum. Donec neque metus, accumsan sed neque non, iaculis lobortis lacus. Maecenas consequat velit quis odio sagittis lobortis. Donec non luctus urna. Quisque sit amet facilisis ligula. Aliquam erat volutpat. Curabitur nec sodales lorem. Curabitur sagittis sem lacus. Cras vel tristique augue, eu ultricies nibh. Vestibulum leo velit, euismod et quam vitae, fermentum auctor felis. Ut porta in nunc sit amet vehicula. Suspendisse potenti. Proin lorem erat, sagittis a mauris at, bibendum tempor neque. Nam nec nunc pretium justo vulputate vulputate eget eu quam. Integer nec auctor nibh, vitae convallis lacus. Vestibulum pharetra, nibh vel vulputate convallis, justo nulla sagittis elit, aliquam luctus quam ipsum in erat. Aenean eleifend ante non urna tincidunt volutpat. Sed sit amet facilisis magna, non pharetra erat. Suspendisse potenti. Cras mollis ligula felis, vel pharetra eros gravida et. Proin sodales, nisi ut ullamcorper tempor, lacus odio tempus sem, vitae sagittis nisi magna sed quam. Morbi velit justo, molestie vitae orci tincidunt, malesuada cursus est. Cras et eros sapien. Integer pellentesque pretium eleifend. Etiam quis mattis libero. Mauris adipiscing nisi ipsum, sit amet consectetur urna feugiat a. Morbi ac scelerisque neque. Donec sed diam odio. In eget scelerisque ante, sit amet posuere augue. Duis gravida ullamcorper commodo. Donec non enim mi. In luctus lacinia lectus. Donec sodales turpis id massa consectetur sollicitudin. Sed egestas ante sed congue vehicula. Aliquam massa dui, faucibus nec ultricies sed, sodales posuere odio. Integer a rhoncus nulla, a imperdiet sapien.", new Vector2(500, 500), Vector2.Zero);

            gui.LoadContent(game, contentManager, spriteBatch);

            Texture2D testing = gui.CreateNewTexture();

            FileStream fs = File.Open(@"c:\school\catalogTile_thirdTest.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }

        [Test(Description = "Hovering outside of the catalog")]
        public void Mouse_outside_and_click()
        {
            isClickedOn = false;
            gui = new GuiCatalogTile(game, "itemId", "fourthTest", "fourth description", new Vector2(500, 500), Vector2.Zero);
            gui.ClickEditSettingsEvent += clickedOn;
            gui.LoadContent(game, contentManager, spriteBatch);

            GuiDrawable[] buttons = gui.GuiDrawables;

            MouseState currentMouseState = new MouseState();
            foreach (GuiDrawable button in buttons)
            {
                if (button.Text.ToLower() == "updatequeue")
                {
                    currentMouseState = new MouseState(
                        -100,
                        -100,
                        0,
                       Microsoft.Xna.Framework.Input.ButtonState.Released,
                      Microsoft.Xna.Framework.Input.ButtonState.Released,
                     Microsoft.Xna.Framework.Input.ButtonState.Released,
                     Microsoft.Xna.Framework.Input.ButtonState.Released,
                     Microsoft.Xna.Framework.Input.ButtonState.Released
                    );
                }
            }

            MouseState oldMouseState = new MouseState(
                    currentMouseState.X,
                    currentMouseState.Y,
                    currentMouseState.ScrollWheelValue,
                    Microsoft.Xna.Framework.Input.ButtonState.Pressed,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);
            Assert.IsFalse(gui.Hovered);
            Assert.IsFalse(isClickedOn);

            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));
            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.IsFalse(gui.Hovered);
            Assert.IsFalse(isClickedOn);
            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);
        }

        [Test(Description = "Hovering on the edge")]
        public void Mouse_edge_case()
        {
            isClickedOn = false;
            gui = new GuiCatalogTile(game, "itemId", "fourthTest", "fourth description", new Vector2(500, 500), Vector2.Zero);
            gui.ClickEditSettingsEvent += clickedOn;
            gui.LoadContent(game, contentManager, spriteBatch);

            GuiDrawable[] buttons = gui.GuiDrawables;

            MouseState currentMouseState = new MouseState();
            foreach (GuiDrawable button in buttons)
            {
                if (button.Text.ToLower() == "editsettings")
                {
                    currentMouseState = new MouseState(
                        button.Rectangle.X,
                        button.Rectangle.Y,
                        0,
                       Microsoft.Xna.Framework.Input.ButtonState.Released,
                      Microsoft.Xna.Framework.Input.ButtonState.Released,
                     Microsoft.Xna.Framework.Input.ButtonState.Released,
                     Microsoft.Xna.Framework.Input.ButtonState.Released,
                      Microsoft.Xna.Framework.Input.ButtonState.Released
                    );
                }
            }

            MouseState oldMouseState = new MouseState(
                    currentMouseState.X,
                    currentMouseState.Y,
                    currentMouseState.ScrollWheelValue,
                   Microsoft.Xna.Framework.Input.ButtonState.Pressed,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);
            Assert.IsFalse(gui.Hovered);
            Assert.IsFalse(isClickedOn);

            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));
            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.True(gui.Hovered);
            Assert.True(isClickedOn);
            Assert.Greater(catalogManager.GetSelectedWorkouts().Length, 0);
        }

        #region Integration Testing
        [Test(Description = "Clicking the checkmark should save to the queue")]
        public void Save_to_queue()
        {
            isClickedOn = false;
            gui = new GuiCatalogTile(game, "itemId", "fourthTest", "fourth description", new Vector2(500, 500), Vector2.Zero);
            gui.ClickEditSettingsEvent += clickedOn;
            gui.LoadContent(game, contentManager, spriteBatch);

            GuiDrawable[] buttons = gui.GuiDrawables;

            MouseState currentMouseState = new MouseState();
            foreach (GuiDrawable button in buttons)
            {
                if (button.Text.ToLower() == "updatequeue")
                {
                    currentMouseState = new MouseState(
                        button.Rectangle.X + 1,
                        button.Rectangle.Y + 1,
                        0,
                        Microsoft.Xna.Framework.Input.ButtonState.Released,
                       Microsoft.Xna.Framework.Input.ButtonState.Released,
                       Microsoft.Xna.Framework.Input.ButtonState.Released,
                       Microsoft.Xna.Framework.Input.ButtonState.Released,
                      Microsoft.Xna.Framework.Input.ButtonState.Released
                    );
                }
            }

            MouseState oldMouseState = new MouseState(
                    currentMouseState.X,
                    currentMouseState.Y,
                    currentMouseState.ScrollWheelValue,
                    Microsoft.Xna.Framework.Input.ButtonState.Pressed,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);
            Assert.IsFalse(gui.Hovered);
            Assert.IsFalse(isClickedOn);

            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));
            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.IsTrue(gui.Hovered);
            Assert.IsFalse(isClickedOn);
            Assert.Greater(catalogManager.GetSelectedWorkouts().Length, 0);
        }

        [Test(Description = "Clicking the edit should save to the queue & fire an event")]
        public void Edit_And_Event()
        {
            isClickedOn = false;
            gui = new GuiCatalogTile(game, "itemId", "fourthTest", "fourth description", new Vector2(500, 500), Vector2.Zero);
            gui.ClickEditSettingsEvent += clickedOn;
            gui.LoadContent(game, contentManager, spriteBatch);

            GuiDrawable[] buttons = gui.GuiDrawables;

            MouseState currentMouseState = new MouseState();
            foreach (GuiDrawable button in buttons)
            {
                if (button.Text.ToLower() == "editsettings")
                {
                    currentMouseState = new MouseState(
                        button.Rectangle.X + 1,
                        button.Rectangle.Y + 1,
                        0,
                        Microsoft.Xna.Framework.Input.ButtonState.Released,
                        Microsoft.Xna.Framework.Input.ButtonState.Released,
                        Microsoft.Xna.Framework.Input.ButtonState.Released,
                        Microsoft.Xna.Framework.Input.ButtonState.Released,
                        Microsoft.Xna.Framework.Input.ButtonState.Released
                    );
                }
            }

            MouseState oldMouseState = new MouseState(
                    currentMouseState.X,
                    currentMouseState.Y,
                    currentMouseState.ScrollWheelValue,
                    Microsoft.Xna.Framework.Input.ButtonState.Pressed,
                    currentMouseState.MiddleButton,
                    currentMouseState.RightButton,
                    currentMouseState.XButton1,
                    currentMouseState.XButton2
                );

            Assert.IsNotNull(currentMouseState);
            Assert.IsNotNull(oldMouseState);
            Assert.IsFalse(gui.Hovered);
            Assert.IsFalse(isClickedOn);

            GameTime gt = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 100));
            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);

            gui.Update(currentMouseState, oldMouseState, new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1), gt);

            Assert.IsTrue(gui.Hovered);
            Assert.IsTrue(isClickedOn);
            Assert.Greater(catalogManager.GetSelectedWorkouts().Length, 0);
            Console.WriteLine(catalogManager.GetSelectedWorkouts().Length);
        }
        #endregion

        [TestFixtureTearDown]
        public void TearDown()
        {
        }
    }
}
