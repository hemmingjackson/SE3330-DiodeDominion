using Microsoft.Xna.Framework.Graphics;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.DiodeDominion.Textures;

namespace Diode_Dominion.Engine.Controls.EntityInfoComponents
{
	public class ItemComponent : Component
	{
		//public Sprite Sprite { get; set; }

		private int CurrentTexture { get; set; }

		private int count;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		public ItemComponent(Vector2 location)
		{
			Sprite = new Sprite(location, CreateAnimationSpritesList())
			{
				Texture = Settings.Content.Load<Texture2D>(TextureLocalization.ErrorAnimations + CurrentTexture)
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (count < 20)
			{
				count++;
			}
			else
			{
				if (CurrentTexture < 12)
				{
					Sprite.Texture = Sprite.AnimationFrames[CurrentTexture++];
				}
				else
				{
					CurrentTexture = 0;
					Sprite.Texture = Sprite.AnimationFrames[CurrentTexture++];
				}
				count = 0;
			}
			Sprite.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) 
		{
			//if (CurrentTexture > 12)
			//{
			//	Sprite.Texture = Sprite.AnimationFrames[CurrentTexture++];
			//}
			//else
			//{
			//	CurrentTexture = 0;
			//	Sprite.Texture = Sprite.AnimationFrames[CurrentTexture++];
			//}
				//Sprite.Update(gameTime);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		public override void SetContainer(Container container)
		{
			Container = container;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void UnloadContent()
		{
		}

		private Texture2D[] CreateAnimationSpritesList()
		{
			Texture2D[] textures = new Texture2D[12];
			for (int i = 0; i < 12; i++)
			{
				textures[i] = Settings.Content.Load<Texture2D>(TextureLocalization.ErrorAnimations + i);
			}
			return textures;
		}
	}
}
