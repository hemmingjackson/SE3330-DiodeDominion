using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diode_Dominion.Engine.Controls
{
    /// <summary>
    /// List of Components with extra framework for the
    /// rendering and updating of the contained components automatically.
    /// </summary>
    public class ComponentManager : List<Component>
    {
        #region Methods

        /// <summary>
        /// Used to update every Component held internally.
        /// </summary>
        /// 
        /// <param name="gameTime">Game time information</param>
        public virtual void Update(GameTime gameTime)
        {
            // Update each internal Component
            Parallel.For(0, Count, i =>
            {
               this[i].Update(gameTime);
            });
        }

        /// <summary>
        /// Draw each internal component
        /// </summary>
        /// 
        /// <param name="gameTime">Game time information</param>
        /// <param name="spriteBatch">Sprite information</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw each internal Component
            Parallel.For(0, Count, i =>
            {
	            this[i].Draw(gameTime, spriteBatch);
            });
        }

        /// <summary>
        /// Call this when every Component needs to be unloaded.
        /// Will also remove every component from the internal list
        /// </summary>
        public virtual void UnloadContent()
        {
            // Unload each internal Component
            foreach (Component component in this)
            {
                component.UnloadContent();
            }
            // Clear the list
            Clear();
        }

        #endregion
    }
}
