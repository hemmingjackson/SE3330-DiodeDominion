using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Screens;

namespace Diode_Dominion.Engine.Screens
{
	/// <summary>
	/// Manages every game screen that is running while the game is active.
	/// Every game screen should be added to this screen manager.
	/// </summary>
	internal class ScreenManager : IUpdate, IDraw
    {
        #region Fields

        /// <summary>
        /// Holds the reference to the current game screen
        /// </summary>
        private GameScreen currentScreen;

        /// <summary>
        /// Holds a reference to a game screen when switching between views is needed
        /// </summary>
        private GameScreen newScreen;

        /// <summary>
        /// Holds the references to the previous screens.
        /// Useful when it is necessary to move to a previous screen
        /// without loading and unloading it from memory.
        /// </summary>
        private Stack<GameScreen> screenStack;

        /// <summary>
        /// Lazy singleton reference
        /// </summary>
        private static ScreenManager _instance;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the dimensions of the game screen.
        /// Useful when using the game screen size is necessary
        /// </summary>
        public Vector2 Dimensions { get; set; }
        
        /// <summary>
        /// Lazy Singleton reference to allow for external reference to various screen
        /// components. Can only be instantiated once.
        /// </summary>
        public static ScreenManager Instance =>
	        _instance ?? (_instance = new ScreenManager());

        /// <summary>
        /// Holds the reference to the assets
        /// </summary>
        private ContentManager Content { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Private constructor to verify that nothing can instantiate this class
        /// </summary>
        private ScreenManager()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a game screen to this manager, which will then handle
        /// displaying it. Will switch immediately to the new screen.
        /// </summary>
        /// 
        /// <param name="gameScreen">Screen to add</param>
        public void AddScreen(GameScreen gameScreen)
        {
            // Add new screen to the stack
            screenStack.Push(gameScreen);
            currentScreen?.UnloadContent();
            newScreen = screenStack.Peek();
            currentScreen = newScreen;
            currentScreen.Initialize();
            currentScreen.LoadContent(Content);
        }

        /// <summary>
        /// Changes the screen back to the previous view. If there are no previous screens
        /// the view will not change.
        /// </summary>
        public void GoBackOneScreen()
        {
            // Make certain there are enough elements to remove
            if (screenStack.Count > 0)
            {
                screenStack.Pop();
                newScreen = screenStack.Peek();
                currentScreen.UnloadContent();
                currentScreen = newScreen;
            }
        }

        /// <summary>
        /// Handles the initialization of anything that does not need a content manager
        /// </summary>
        public void Initialize()
        {
            screenStack = new Stack<GameScreen>();
            
            screenStack.Push(currentScreen);
        }

        /// <summary>
        /// Handles the loading of anything that needs a content manager
        /// </summary>
        /// 
        /// <param name="content">Content manager that points to assets</param>
        public void LoadContent(ContentManager content)
        {
            Content = content;
        }

        /// <summary>
        /// Update anything that does not need to be rendered here.
        /// If the game starts running poorly this will still be called,
        /// but Draw will skip frames.
        /// </summary>
        /// 
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
	        currentScreen?.Update(gameTime);
        }

        /// <summary>
        /// Draws the current game screen. If performance starts to suffer draw calls
        /// will be skipped until the game catches up.
        /// </summary>
        /// 
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentScreen != null)
            {
                currentScreen.Draw(gameTime, spriteBatch);
            }
            else
            {
                // Set the title screen at the beginning of the game
                currentScreen = new TitleScreen();
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);
            }
        }

        #endregion
    }
}
