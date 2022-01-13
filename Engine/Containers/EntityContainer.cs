using Diode_Dominion.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.Containers
{
	internal class EntityContainer : Container
	{
		public void AddComponent(Component component)
		{
			// Change the default container to this container
			component.SetContainer(this);

			// Move the Component position so that it aligns properly in the Container
			component.Sprite.Origin = new Vector2(Sprite.Origin.X + component.Sprite.Origin.X,
				Sprite.Origin.Y + component.Sprite.Origin.Y);

			// Add the component to the internal list
			ComponentManager.Add(component);
		}

		public EntityContainer(Texture2D texture, int xCoordinate, int yCoordinate) : base(texture, xCoordinate, yCoordinate)
		{

		}
	}
}
