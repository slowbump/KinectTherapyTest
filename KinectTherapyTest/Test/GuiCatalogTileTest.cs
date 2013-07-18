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

        [SetUp]
        public void SetUp()
        {
            game = new Game();
            graphics = new GraphicsDeviceManager(game);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            contentManager = new ContentManager(game.Services);
            contentManager.RootDirectory = @"C:\Content\";
            game.Run();
        }

        [Test(Description = "Create the title texture: 500 x 100")]
        public void FirstTest()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            game.Services.AddService(typeof(SpriteBatch), spriteBatch);

            gui = new GuiCatalogTile(game, "itemId", "firstTest", "this is it description", new Vector2(500, 100), Vector2.Zero);

            gui.LoadContent(game, contentManager, spriteBatch);

            Texture2D testing = gui.CreateNewTexture();

            FileStream fs = File.Open(@"c:\school\" + testing + "2.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }

        [Test(Description = "Create the title texture: 500 x 500")]
        public void SecondTest()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            game.Services.AddService(typeof(SpriteBatch), spriteBatch);

            gui = new GuiCatalogTile(game, "itemId", "secondTest", "this is it description", new Vector2(500, 500), Vector2.Zero);

            gui.LoadContent(game, contentManager, spriteBatch);

            Texture2D testing = gui.CreateNewTexture();

            FileStream fs = File.Open(@"c:\school\" + testing + "2.png", FileMode.Create);
            testing.SaveAsPng(fs, testing.Width, testing.Height);
            fs.Close();

            Assert.IsNotNull(testing);
        }
    }
}
