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
    class CatalogTileEditScreenTest
    {
        private CatalogTileEditScreen gui;
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
            catalogManager.AddExerciseToSelected("fakeExercise", "fake");

            game.Services.AddService(typeof(CatalogManager), catalogManager);
        }

        [Test(Description = "Load and Edit the attributes")]
        public void Edit_Attributes()
        {
            gui = new CatalogTileEditScreen(game, new Rectangle(0, 0, 500, 500), ScreenState.Active);
            gui.contentManager = contentManager;

            gui.Initialize();
            gui.LoadContent();
            gui.OpenScreen("fakeExercise");

            foreach (GuiDrawable drawable in gui.GuiDrawables)
            {
                if (drawable.GetType() == typeof(GuiInputBox))
                {
                    GuiInputBox input = (GuiInputBox)drawable;

                    switch (input.Text.ToLower())
                    {
                        case "repetitions":
                            Assert.AreEqual(input.Value, "10");
                            input.Value = "2";
                            break;
                        case "variance":
                            Assert.AreEqual(input.Value, "10");
                            input.Value = "4";
                            break;
                    }
                }
            }

            foreach (GuiDrawable drawable in gui.GuiDrawables)
            {
                if (drawable.GetType() == typeof(GuiButton))
                {
                    GuiButton button = (GuiButton)drawable;

                    switch (button.Text.ToLower())
                    {
                        case "submit":
                            button.Click();
                            break;
                    }
                }
            }

            Exercise[] selected = catalogManager.GetSelectedWorkouts();

            Assert.AreEqual(selected[0].Repetitions, 2);
            Assert.AreEqual(selected[0].Variance, 4f);
            Assert.AreEqual(selected[0].Id, "fakeExercise");

            foreach (GuiDrawable drawable in gui.GuiDrawables)
            {
                if (drawable.GetType() == typeof(GuiInputBox))
                {
                    GuiInputBox input = (GuiInputBox)drawable;

                    switch (input.Text.ToLower())
                    {
                        case "repetitions":
                            Assert.AreEqual(input.Value, "2");
                            input.Value = "10";
                            break;
                        case "variance":
                            Assert.AreEqual(input.Value, "4");
                            input.Value = "10";
                            break;
                    }
                }
            }

            foreach (GuiDrawable drawable in gui.GuiDrawables)
            {
                if (drawable.GetType() == typeof(GuiButton))
                {
                    GuiButton button = (GuiButton)drawable;

                    switch (button.Text.ToLower())
                    {
                        case "cancel":
                            button.Click();
                            break;
                    }
                }
            }

            selected = catalogManager.GetSelectedWorkouts();

            Assert.AreEqual(selected[0].Repetitions, 2);
            Assert.AreEqual(selected[0].Variance, 4f);
            Assert.AreEqual(selected[0].Id, "fakeExercise");
        }
    }
}
