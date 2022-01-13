using Diode_Dominion.Engine.Controls;
using Diode_Dominion.Engine.Screens;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.Containers
{
	/// <summary>
	/// Generic container that can be used to display Components internally.
	/// A Components position is relative to the Container. X = 20 will be
	/// 20 pixels from the left of the container and Y = 20 will be 20 pixels
	/// down the container.
	/// </summary>
	public class Container : IContainer, IUpdate, IDraw
	{
		#region Fields

		/// <summary>
		/// Holds an instance of the sprite class that will be used to 
		/// hold the texture and bounds of the container.
		/// </summary>
		private readonly Sprite sprite;

		private Vector2 oldLocation;

		#endregion

		#region Properties

		/// <summary>
		/// Holds an instance of the sprite class that will be used to 
		/// hold the texture and bounds of the container.
		/// </summary>
		public Sprite Sprite => sprite;

		/// <summary>
		/// Default padding of components within the container
		/// </summary>
		public int Padding { get; set; } = 5;

		/// <summary>
		/// Holds the X coordinate of the Container
		/// </summary>
		public float X
		{
			get => sprite.X;
			set => sprite.X = value;
		}

		/// <summary>
		/// Holds the Y coordinate of the Container
		/// </summary>
		public float Y
		{
			get => sprite.Y;
			set => sprite.Y = value;
		}

		/// <summary>
		/// List of internal Components
		/// </summary>
		public ComponentManager ComponentManager { get; }

		#endregion

		#region Contructors

		/// <summary>
		/// Create a container that can be rendered on screen
		/// </summary>
		/// 
		/// <param name="containerTexture">Texture of the Container</param>
		/// <param name="xCoordinate">Left x coordinate</param>
		/// <param name="yCoordinate">Upper y coordinate</param>
		public Container(Texture2D containerTexture, int xCoordinate, int yCoordinate) : 
			this(containerTexture, new Vector2(xCoordinate, yCoordinate))
		{
			// Calls secondary constructor for the implementation
		}

		/// <summary>
		/// Create a container with the passed in texture at the coordinates
		/// specified by the passed in rectangle.
		/// </summary>
		/// 
		/// <param name="containerTexture">Texture of the container</param>
		/// <param name="coordinates">Location of the container</param>
		public Container(Texture2D containerTexture, Vector2 coordinates)
		{
			// Create the list of internal components
			ComponentManager = new ComponentManager();

			// Create the sprite with the location and texture
			sprite = new Sprite(coordinates, containerTexture);
			oldLocation = coordinates;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Update the variables of the internal Components
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			// Check if container location changed
			if (oldLocation != Sprite.Origin)
			{
				float deltaX = oldLocation.X - Sprite.Origin.X;
				float deltaY = oldLocation.Y - Sprite.Origin.Y;

				// Update the location of each internal component
				foreach (Component component in ComponentManager)
				{
					Vector2 newLocation =
						new Vector2(component.Sprite.Origin.X - deltaX, component.Sprite.Origin.Y - deltaY);
					component.Sprite.Origin = newLocation;
				}

				// Set to the new location
				oldLocation = Sprite.Origin;
			}

			ComponentManager.Update(gameTime);
		}

		/// <summary>
		/// Draw the internal Components
		/// </summary>
		/// 
		/// <param name="gameTime">Game time information</param>
		/// <param name="spriteBatch">Sprite information</param>
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Draw the container first so it is under the Components
			spriteBatch.Draw(Sprite.Texture, Sprite.Origin, Color.White);

			ComponentManager.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Draw the internal Components
		/// </summary>
		/// 
		/// <param name="gameTime">Game time information</param>
		/// <param name="spriteBatch">Sprite information</param>
		/// <param name="color">Color of the container</param>
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
		{
			// Draw the container first so it is under the Components
			spriteBatch.Draw(Sprite.Texture, Sprite.Origin, color);

			ComponentManager.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Unloads everything from the container
		/// </summary>
		public virtual void UnloadContent()
		{
			// Unload each component
			foreach (Component component in ComponentManager)
			{
				component.UnloadContent();
			}
		}

		/// <summary>
		/// Allows a Component to be added to the Container.
		/// Automatically sets this as it's container
		/// </summary>
		/// 
		/// <param name="component">Component to add to the Container</param>
		public virtual void AddComponent(IComponent component)
		{
			// Change the default container to this container
			component.SetContainer(this);

			// Move the Component position so that it aligns properly in the Container
			Vector2 compPosition;

			compPosition.X = Sprite.Origin.X + Padding;
			compPosition.Y = Sprite.Origin.Y + Padding * (ComponentManager.Count + 1);
			
			foreach (Component comp in ComponentManager)
			{
				compPosition.Y += comp.Sprite.Texture.Height;
			}
			
			component.SetOrigin(compPosition);

			// Add the component to the internal list
			ComponentManager.Add((Component) component);
		}

		#endregion
	}
}