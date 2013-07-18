using System;
using Microsoft.Samples.Kinect.XnaBasics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWENG.Service;
using System.Collections.Generic;
using System.Diagnostics;

namespace SWENG.UserInterface
{
    /// <summary>
    /// This class implements the screen for its use with the Manager
    /// </summary>
    public class ExerciseScreen : Screen
    {
        private readonly Rectangle _viewableArea;
        private readonly GuiDrawable[] _guiDrawable;

        private SpriteFont _spriteFont;

        private const float MARGIN = 10f;

        private bool _isInitialized;

        #region ColorStreamRenderer Variables
        private readonly ColorStreamRenderer _colorStream;
        private Vector2 _colorStreamPosition;
        private Vector2 _colorStreamSize;
        #endregion

        #region ExerciseQueue Variables
        // queue of exercises
        // reference to the exercise queue service
        private ExerciseQueue ExerciseQueue
        {
            get
            {
                return (ExerciseQueue)Game.Services.GetService(typeof(ExerciseQueue));
            }
        }
        private ExerciseTile[] _exerciseTiles;
        private MouseState _oldMouseState;
        #endregion

        private Vector2 _tilePosition;
        private Vector2 _tileSize;
        private Vector2 _tileTextPosition;
        private string _repetitionString;

        private RecordingManager recordingManager
        {
            get
            {
                return (RecordingManager)Game.Services.GetService(typeof(RecordingManager));
            }
        }

        /// <summary>
        /// Initialize a new instance of the ExerciseScreen class.
        /// </summary>
        /// <param name="game">The related game object.</param>
        /// <param name="viewableArea">The desired canvas size to draw on.</param>
        /// <param name="startingState">The desired starting Screen State</param>
        public ExerciseScreen(Game game, Rectangle viewableArea, ScreenState startingState)
            : base(game)
        {
            ScreenState = startingState;
            _viewableArea = viewableArea;
            _colorStream = new ColorStreamRenderer(game);

            Title = "Exercise";

            _exerciseTiles = new ExerciseTile[0];

            #region Laying out the positions
            _colorStreamPosition = new Vector2(
                    (float)(viewableArea.X),
                    (float)(viewableArea.Y)
                );

            _colorStreamSize = new Vector2(
                    (float)(0.7 * viewableArea.Width),
                    (float)(0.7 * viewableArea.Height)
                );

            _tileSize = new Vector2(250f, 250f);
            _tilePosition = new Vector2(
                (float)(_colorStreamPosition.X + _colorStreamSize.X + (MARGIN * 2)),
                (float)(_colorStreamPosition.Y)
            );

            _tileTextPosition = new Vector2(
                _tilePosition.X,
                _tilePosition.Y + _tileSize.Y
            );

            Vector2 buttonSize = new Vector2(240f, 60f);
            Vector2 buttonBottom = new Vector2(
                _viewableArea.Right - buttonSize.X + MARGIN,
                _viewableArea.Bottom - buttonSize.Y);

            Dictionary<string, GuiDrawable> guiDrawables = new Dictionary<string, GuiDrawable>();
            guiDrawables.Add("Menu",
                new GuiButton("Menu",
                    buttonSize,                     
                    buttonBottom 
                    - (new Vector2(0f, 2 * MARGIN)) 
                    - (new Vector2(0f, 2 * buttonSize.Y))
            ));
            guiDrawables.Add("Skip",
                new GuiButton("Skip", 
                    buttonSize, 
                    buttonBottom 
                    - new Vector2(0f , MARGIN) 
                    - new Vector2(0f, buttonSize.Y)
            ));
            guiDrawables.Add("EndQueue",
                new GuiButton("EndQueue",
                    buttonSize,
                    buttonBottom)
            );

            guiDrawables.Add("SensorStatus",
                new GuiSensorStatus(
                    "SensorStatus",
                    new Vector2(99f, 32f),
                    new Vector2(
                        (_viewableArea.Right / 2) - (99f / 2),
                        _viewableArea.Bottom - 32f
                    ),
                    game
                )
            );

            guiDrawables.Add("KinectTherapy", new GuiHeader("KinectTherapy",
                new Vector2(326f, 52f),
                new Vector2(
                _viewableArea.Left,
                _viewableArea.Top - MARGIN - 52f
            )));

            #endregion

            _guiDrawable = new GuiDrawable[guiDrawables.Count];

            guiDrawables.Values.CopyTo(_guiDrawable, 0);

            _isInitialized = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _colorStream.Position = _colorStreamPosition;
            _colorStream.Size = _colorStreamSize;
            _colorStream.Initialize();

            foreach (GuiDrawable guiDrawable in _guiDrawable)
            {
                if (guiDrawable.GetType() == typeof(GuiButton))
                {
                    ((GuiButton)guiDrawable).ClickEvent += GuiButtonWasClicked;
                }
            }

            _isInitialized = true;

            base.Initialize();
        }

        /// <summary>
        /// Central button click management.
        /// </summary>
        private void GuiButtonWasClicked(object sender, GuiButtonClickedArgs e)
        {
            switch (e.ClickedOn)
            {
                case "Menu":
                    ScreenState = UserInterface.ScreenState.Hidden;
                    OnTransition(new TransitionEventArgs(Title, e.ClickedOn));
                    break;
                case "Skip":
                    break;
                case "End Queue":
                    break;
            }
        }

        /// <summary>
        /// This method creates a new ContentManager 
        /// and loads the textures and fonts.
        /// </summary>
        public override void LoadContent()
        {
#if RELEASE
            if (null == contentManager)
            {
                contentManager = new ContentManager(Game.Services, "Content");
            }
#else
            if (null == contentManager)
            {
                contentManager = new ContentManager(Game.Services, @"C:\Content");
            }
#endif

            foreach (GuiDrawable guiDrawable in _guiDrawable)
            {
                guiDrawable.LoadContent(Game, contentManager, SharedSpriteBatch);
            }

            _spriteFont = contentManager.Load<SpriteFont>("Arial16");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (_isInitialized
                && (ScreenState & UserInterface.ScreenState.NonInteractive) == 0)
            {
                MouseState currentState = Mouse.GetState();
                Rectangle mouseBoundingBox = new Rectangle(currentState.X, currentState.Y, 1, 1);

                foreach (GuiDrawable guiDrawable in _guiDrawable)
                {
                    guiDrawable.Update(currentState, _oldMouseState, mouseBoundingBox, gameTime);
                }

                foreach (ExerciseTile exerciseTile in _exerciseTiles)
                {
                    exerciseTile.Update(currentState, _oldMouseState, mouseBoundingBox, gameTime);
                }

                _colorStream.Update(gameTime);

                _repetitionString = string.Format(
                    "Completed {0} of {1}",
                    ExerciseQueue.CurrentExercise.Repetitions,
                    ExerciseQueue.CurrentExercise.RepetitionsToComplete
                );

                _oldMouseState = currentState;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This method renders the current state of the ExerciseScreen.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            if (_isInitialized)
            {
                GraphicsDevice.Clear(Color.WhiteSmoke);
                var spriteBatch = SharedSpriteBatch;
                spriteBatch.Begin();

                foreach (GuiDrawable guiDrawable in _guiDrawable)
                {
                    guiDrawable.Draw(spriteBatch);
                }

                foreach (ExerciseTile exerciseTile in _exerciseTiles)
                {
                    if (exerciseTile.IsCurrentTile)
                    {
                        exerciseTile.Draw(spriteBatch);
                    }
                }

                if (!string.IsNullOrEmpty(_repetitionString))
                {
                    spriteBatch.DrawString(
                        _spriteFont,
                        _repetitionString,
                        _tileTextPosition,
                        Color.Black
                    );
                }

                spriteBatch.End();

                _colorStream.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// The method to use when the exercise screen
        /// should be triggered to close when an event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void QueueIsDone(object sender, EventArgs args)
        {
            ScreenState = UserInterface.ScreenState.Hidden;
            base.Transition();
        }

        public override void OpenScreen()
        {
            ExerciseGameComponent[] exercises = ExerciseQueue.Exercises;
            _exerciseTiles = new ExerciseTile[exercises.Length];

            /** Draw at the same height for cycling through */
            for (int i = 0; i < exercises.Length; i = i + 1)
            {
                _exerciseTiles[i] = new ExerciseTile(Game, exercises[i], i, _tileSize, _tilePosition);
                _exerciseTiles[i].LoadContent(Game, contentManager, SharedSpriteBatch);
                if (i == 0)
                {
                    _exerciseTiles[i].IsCurrentTile = true;
                }
            }

            base.Transition();
        }
    }
}
