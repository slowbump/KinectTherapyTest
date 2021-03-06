using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Samples.Kinect.XnaBasics;
using System.Diagnostics;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;

namespace SWENG.UserInterface
{
    /// <summary>
    /// This class implements the screen for its use with the Manager
    /// </summary>
    public class SensorTileEditScreen : Screen
    {
        private readonly Rectangle _viewableArea;
        private readonly GuiDrawable[] _guiDrawable;

        private const float MARGIN = 10f;

        private bool _isInitialized;
        private MouseState _oldMouseState;
        private Texture2D _blankTexture;
        
        private Texture2D _inputBoxTexture;
        private Rectangle _inputBoxDestination;
        
        private Texture2D _inputSensorTexture;

        #region ColorStreamRenderer Variables
        private readonly ColorStreamRenderer colorStream;
        private Vector2 colorStreamPosition;
        private Vector2 colorStreamSize;
        #endregion

        private GuiScrollable _scrollable;
        private const float SCROLL_WIDTH = 20f;
        private int _elevationAngle = 0;
        private double _timeStamp;

        /// <summary>
        /// Initialize a new instance of the ExerciseScreen class.
        /// </summary>
        /// <param name="game">The related game object.</param>
        /// <param name="viewableArea">The desired canvas size to draw on.</param>
        /// <param name="startingState">The desired starting Screen State</param>
        public SensorTileEditScreen(Game game, Rectangle viewableArea, ScreenState startingState)
            : base(game)
        {
            _timeStamp = double.MinValue;
            ScreenState = startingState;
            _viewableArea = viewableArea;

            Title = "Sensor Setup";

            colorStream = new ColorStreamRenderer(game);

            #region Laying out the positions
            Vector2 modalSize = new Vector2(512, 384);

            _inputBoxDestination = new Rectangle(
                (_viewableArea.Width / 2) - ((int)modalSize.X / 2),
                (_viewableArea.Height / 2) - ((int)modalSize.Y / 2),
                (int)modalSize.X,
                (int)modalSize.Y
            );

            Vector2 buttonSize = new Vector2(121f, 60f);
            Vector2 buttonBottom = new Vector2(
                _inputBoxDestination.Right - buttonSize.X - MARGIN,
                _inputBoxDestination.Bottom - buttonSize.Y);

            List<GuiDrawable> _buttonList = new List<GuiDrawable>();
            _buttonList.Add(
                new GuiButton("Submit", 
                    buttonSize,
                    buttonBottom
                ));

            colorStreamSize = new Vector2(
                    (float)((modalSize.X / 2) - (2 * MARGIN)),
                    (float)((modalSize.Y / 2))
                );

            colorStreamPosition = new Vector2(
                    (float)(_inputBoxDestination.Left + MARGIN),
                    (float)(_inputBoxDestination.Bottom - colorStreamSize.Y - MARGIN - buttonSize.Y)
                );

            _scrollable = new GuiScrollable(
                new Vector2(
                    SCROLL_WIDTH,
                    colorStreamSize.Y
                ),
                new Vector2(
                    _inputBoxDestination.Right - SCROLL_WIDTH - (2 * MARGIN),
                    colorStreamPosition.Y
                ),
                @"UI\Slider"
            );

            #endregion

            _guiDrawable = _buttonList.ToArray();

            _isInitialized = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            colorStream.Position = colorStreamPosition;
            colorStream.Size = colorStreamSize;
            colorStream.Initialize();

            _isInitialized = true;
            _scrollable.Initialize();

            foreach (GuiDrawable guiDrawable in _guiDrawable)
            {
                if (guiDrawable.GetType() == typeof(GuiButton))
                {
                    ((GuiButton)guiDrawable).ClickEvent += GuiButtonWasClicked;
                }
            }

            base.Initialize();
        }

        /// <summary>
        /// Central button click management.
        /// </summary>
        private void GuiButtonWasClicked(object sender, GuiButtonClickedArgs e)
        {
            switch (e.ClickedOn)
            {
                case "Submit":
                case "Cancel":
                    ScreenState = UserInterface.ScreenState.Hidden;
                    OnTransition(new TransitionEventArgs(Title, "Return"));
                    break;
            }
        }

        /** TODO: Make this better... seriously */
        public override void LoadContent()
        {
            if (null == contentManager)
            {
                contentManager = new ContentManager(Game.Services, "Content");
            }

            _blankTexture = contentManager.Load<Texture2D>(@"blank");
            _inputBoxTexture = contentManager.Load<Texture2D>(@"UI\SensorTileEdit");
            _inputSensorTexture = contentManager.Load<Texture2D>(@"UI\SensorSlider");

            foreach (GuiDrawable guiDrawable in _guiDrawable)
            {
                guiDrawable.LoadContent(Game, contentManager, SharedSpriteBatch);
            }

            _scrollable.LoadContent(Game, contentManager, SharedSpriteBatch);

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            contentManager.Unload();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (_isInitialized)
            {
                MouseState currentState = Mouse.GetState();
                Rectangle mouseBoundingBox = new Rectangle(currentState.X, currentState.Y, 1, 1);
                colorStream.Update(gameTime);

                foreach (GuiDrawable guiDrawable in _guiDrawable)
                {
                    guiDrawable.Update(currentState, _oldMouseState, mouseBoundingBox, gameTime);
                }

                if (null != Chooser.Sensor
                    && Chooser.Sensor.IsRunning)
                {
                    if (mouseBoundingBox.Intersects(_scrollable.GraceArea))
                    {
                        _scrollable.Update(currentState, _oldMouseState, mouseBoundingBox, gameTime);

                        if (currentState.LeftButton == ButtonState.Released
                            && _oldMouseState.LeftButton == ButtonState.Pressed)
                        {
                            _timeStamp = gameTime.TotalGameTime.TotalMilliseconds;
                            _elevationAngle = GetAngle();
                        }
                    }

                    if (_timeStamp != double.MinValue
                        && gameTime.TotalGameTime.TotalMilliseconds - _timeStamp > 1500)
                    {
                        _timeStamp = double.MinValue;
                        try
                        {
                            Chooser.Sensor.ElevationAngle = _elevationAngle;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }
                }

                _oldMouseState = currentState;
            }
            base.Update(gameTime);
        }

        public int GetAngle()
        {
            float scrollPercent = _scrollable.GetScrollTop();

            int r = 0;
            int splits = Math.Abs(Chooser.Sensor.MaxElevationAngle) + Math.Abs(Chooser.Sensor.MinElevationAngle) + 1;
            int degreesOfFreedom = Math.Abs(Chooser.Sensor.MaxElevationAngle);

            float paginationSegment = 100.0f / (splits);
            Debug.WriteLine(
                string.Format("pages = {0}", splits)
            );

            /** 100% scrolled would always give you a page 1 if this wasn't implemented */
            if (scrollPercent == 50.0f)
            {
                r = 0;
            }
            else if (scrollPercent < 50.0f)
            {
                r = (int)((scrollPercent / 100.0) * degreesOfFreedom);
            }
            else if (scrollPercent > 50.0f)
            {
                r = (int)(-1 * (
                    ((scrollPercent - 50) / 100.0)
                    * degreesOfFreedom
                ));
            }
            else
            {
                r = -27;
            }

            return r;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_isInitialized)
            {
                var spriteBatch = SharedSpriteBatch;
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                spriteBatch.Draw(
                    _blankTexture,
                    _viewableArea,
                    Color.WhiteSmoke * 0.5f
                );

                spriteBatch.End();

                spriteBatch.Begin();

                spriteBatch.Draw(
                    _inputBoxTexture,
                    _inputBoxDestination,
                    Color.White
                );

                foreach (GuiDrawable guiDrawable in _guiDrawable)
                {
                    guiDrawable.Draw(spriteBatch);
                }

                _scrollable.Draw(spriteBatch);
                spriteBatch.End();

                colorStream.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
