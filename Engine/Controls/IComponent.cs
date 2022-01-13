using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Screens;
using Microsoft.Xna.Framework;

namespace Diode_Dominion.Engine.Controls
{
	/// <summary>
	/// General functionality of all components
	/// </summary>
	public interface IComponent : IUpdate, IDraw
	{
		/// <summary>
		/// Sets the container that the component it within
		/// </summary>
		/// <param name="container"></param>
		void SetContainer(Container container);

		/// <summary>
		/// Sets the location of the component
		/// </summary>
		/// <param name="origin"></param>
		void SetOrigin(Vector2 origin);

		/// <summary>
		/// Obtain the origin of the component
		/// </summary>
		/// <returns>Location of the component</returns>
		Vector2 GetOrigin();
	}
}