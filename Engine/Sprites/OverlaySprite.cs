using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.Sprites
{
	/// <summary>
	/// Extends the functionality of the <see cref="Sprite"/> to allow
	/// for an overlay texture to be drawn and applied to the texture of
	/// the sprite.
	/// </summary>
	public class OverlaySprite : Sprite
	{
		#region Properties

		/// <summary>
		/// Whether the overlay for the sprite should be drawn to the screen.
		/// </summary>
		public bool DisplayOverlay { get; set; } = false;

		#endregion

		/// <summary>
		/// Allows for the creation of a overlay sprite. The first n-1 <see cref="Texture2D"/>
		/// passed to the parameters are for an animated sprite, while the last texture will be
		/// the overlay. If it is not animated, pass only the texture and the overlay texture.
		/// </summary>
		/// 
		/// <param name="coordinates">Location of the sprite</param>
		/// <param name="animationTextures">Textures 1 -> n-1 are for the sprite and texture n is the overlay</param>
		public OverlaySprite(Vector2 coordinates, params Texture2D[] animationTextures) : base(coordinates, animationTextures)
		{

		}

		/// <summary>
		/// Create the sprite at the specified coordinates
		/// </summary>
		/// 
		/// <param name="coordinates"></param>
		public OverlaySprite(Vector2 coordinates) : base(coordinates){ }

		/// <summary>
		/// Handles drawing the overlay and passes the rest of the drawing to the base <see cref="Sprite"/>
		/// class.
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			if (DisplayOverlay)
			{
				spriteBatch.Draw(AnimationFrames[AnimationFrames.Length - 1], Origin, Color.White);
			}
		}
	}
}
