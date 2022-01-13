using Diode_Dominion.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Diode_Dominion.Engine.Screens
{
    /// <summary>
    /// Base game screen that every subsequent screen should inherit from
    /// </summary>
    public class GameScreen : IDraw, IUpdate
    {
        #region Fields

        /// <summary>
        /// List of drawn components that make up this screen
        /// </summary>
        private List<Component> screenComponents = new List<Component>();

        #endregion

        #region Properties

        /// <summary>
        /// Controls where assets are loaded from
        /// </summary>
        protected ContentManager Content { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This is everything that should be loaded once the screen is ready to be displayed
        /// </summary>
        public virtual void Initialize()
        {
            screenComponents = new List<Component>();
        }

        /// <summary>
        /// Used to load objects that require the content manager to function
        /// </summary>
        /// 
        /// <param name="contentManager"></param>
        public virtual void LoadContent(ContentManager contentManager)
        {
            Content = contentManager;
        }

        /// <summary>
        /// Handle what should be unloaded when a game screen is no longer needed.
        /// Allows every internal component to unload themselves.
        /// </summary>
        public virtual void UnloadContent()
        {
            foreach (Component component in screenComponents)
            {
                component.UnloadContent();
            }
        }

        /// <summary>
        /// Update every internal component 
        /// </summary>
        /// 
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // Update every component
            foreach (Component component in screenComponents)
            {
                component.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw every component to the game screen
        /// </summary>
        /// 
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw every component
            foreach (Component component in screenComponents)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Add a component to the game screen
        /// </summary>
        /// 
        /// <param name="component">Component to add to the game screen</param>
        public void AddComponent(Component component)
        {
            screenComponents.Add(component);
        }

        /// <summary>
        /// Remove a specific component from the game screen
        /// </summary>
        /// 
        /// <param name="component">Component to remove</param>
        public void RemoveComponent(Component component)
        {
            screenComponents.Remove(component);
        }

        #endregion
    }
}
