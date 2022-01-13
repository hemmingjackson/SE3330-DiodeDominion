using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.Controls
{
    /// <summary>
    /// Generic class that every non-game, drawn object should
    /// inherit from.
    /// </summary>
    public abstract class Component : IComponent
    {
        #region Properties

        /// <summary>
        /// The Container that the Component is located within.
        /// Used to determine absolute location
        /// </summary>
        public Container Container { get; set; }
        
        /// <summary>
        /// Whether the component is enabled.
        /// Defaults to true.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Whether the component is visible.
        /// Defaults to true.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Sprite for the component
        /// </summary>
        public Sprite Sprite { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Unload the component
        /// </summary>
        public abstract void UnloadContent();

        /// <summary>
        /// Draw the component to the game screen
        /// </summary>
        /// 
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Update the component
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Container that the Component is in.
        /// Null if directly on the game screen
        /// </summary>
        /// 
        /// <param name="container">Sets the container of the component</param>
        public abstract void SetContainer(Container container);

        /// <summary>
        /// Sets the starting location of the component
        /// </summary>
        /// <param name="origin"></param>
        public void SetOrigin(Vector2 origin)
        {
	        Sprite.Origin = origin;
        }

        /// <summary>
        /// Returns the origin of the component
        /// </summary>
        /// <returns></returns>
        public Vector2 GetOrigin()
        {
	        return Sprite.Origin;
        }

        #endregion
    }
}
