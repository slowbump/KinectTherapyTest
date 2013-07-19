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
    public class CatalogScreenTest
    {
        private CatalogScreen gui;
        private ContentManager contentManager;
        private Game game;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool isClickedOn = false;
        private CatalogManager catalogManager;

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

            catalogManager = new CatalogManager();
            game.Services.AddService(typeof(CatalogManager), catalogManager);
        }

        [SetUp]
        public void TestSetUp()
        {
            isClickedOn = false;
            catalogManager.ClearWorkout();
        }

        [Test(Description = "Open screen")]
        public void Open_screen()
        {
            //spriteBatch = new SpriteBatch(game.GraphicsDevice);
            //game.Services.AddService(typeof(SpriteBatch), spriteBatch);

            //CatalogManager catalogManager = new CatalogManager();
            //game.Services.AddService(typeof(CatalogManager), catalogManager);

            catalogManager.AddExerciseToSelected("blah1", "test");
            catalogManager.AddExerciseToSelected("blah2", "test2");

            gui = new CatalogScreen(game, new Rectangle(0, 0, 500, 500), ScreenState.Active);
            gui.contentManager = contentManager;

            gui.Initialize(); 
            gui.LoadContent();

            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 2);
            Assert.True(string.IsNullOrEmpty(gui.SelectedCategory));

            gui.OpenScreen();

            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);
            Assert.AreEqual(gui.SelectedCategory.ToLower(), "arms");
        }

        /** Update switching categories */
        [Test(Description = "Switching categories")]
        public void Switch_category()
        {
            //spriteBatch = new SpriteBatch(game.GraphicsDevice);
            //game.Services.AddService(typeof(SpriteBatch), spriteBatch);

            //CatalogManager catalogManager = new CatalogManager();
            //game.Services.AddService(typeof(CatalogManager), catalogManager);

            catalogManager.AddExerciseToSelected("blah1", "test");
            catalogManager.AddExerciseToSelected("blah2", "test2");

            gui = new CatalogScreen(game, new Rectangle(0, 0, 500, 500), ScreenState.Active);
            gui.contentManager = contentManager;

            gui.Initialize();
            gui.LoadContent();

            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 2);
            Assert.True(string.IsNullOrEmpty(gui.SelectedCategory));

            gui.OpenScreen();

            Assert.AreEqual(catalogManager.GetSelectedWorkouts().Length, 0);
            Assert.AreEqual(gui.SelectedCategory.ToLower(), "arms");
        }
    }
}
