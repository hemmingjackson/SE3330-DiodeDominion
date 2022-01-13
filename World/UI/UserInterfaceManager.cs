using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// This singleton is used to draw and update all user interfaces to the screen.
	/// </summary>
	public class UserInterfaceManager
	{
		private static UserInterfaceManager _instance;

		/// <summary>
		/// Lazy singleton instance of the manager
		/// </summary>
		public static UserInterfaceManager Instance => _instance ?? (_instance = new UserInterfaceManager());

		/// <summary>
		/// List of user interfaces that are drawn and updated to the screen
		/// </summary>
		internal List<IUserInterface> UserInterfaces { get; }

		/// <summary>
		/// Private so nothing can instantiate this object outside of
		/// the instance
		/// </summary>
		private UserInterfaceManager()
		{
			UserInterfaces = new List<IUserInterface>();
		}

		#region Methods

		/// <summary>
		/// Closes all of the UIs in the <see cref="UserInterfaces"/>
		/// </summary>
		public void CloseAll()
		{
			UserInterfaces.Clear();
		}

		/// <summary>
		/// Returns whether the mouse is intersecting one or more of the
		/// user interfaces
		/// </summary>
		/// <returns></returns>
		public bool MouseIntersecting()
		{
			bool intersecting = false;

			// Look from top to bottom for the mouse to be intersecting the UIs
			for (int i = UserInterfaces.Count - 1; i >= 0; i--)
			{
				// Check to see if it intersects
				if (UserInterfaces[i].Intersects())
				{
					intersecting = true;

					// Force out since intersection was found
					i = -1;
				}
			}

			return intersecting;
		}

		/// <summary>
		/// This will add a user interface to the screen and then tell it
		/// to open
		/// </summary>
		/// <param name="userInterface"></param>
		public void AddUserInterface(IUserInterface userInterface)
		{
			UserInterfaces.Add(userInterface);
		}

		/// <summary>
		/// This will remove a user interface from the screen manager
		/// </summary>
		/// <param name="userInterface"></param>
		public void RemoveUserInterface(IUserInterface userInterface)
		{
			UserInterfaces.Remove(userInterface);
		}

		/// <summary>
		/// This will update every user interface that the screen manager
		/// maintains
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			// Do NOT change to for each. This is because the integrity of the list cannot
			// be guaranteed as they can be removed mid-list update
			for (int i = 0; i < UserInterfaces.Count; i++)
			{
				UserInterfaces[i].Update(gameTime);
			}
			//foreach (IUserInterface userInterface in UserInterfaces)
			//{
			//	userInterface.Update(gameTime);
			//}
		}

		/// <summary>
		/// This will draw every user interface that the screen manager
		/// maintains
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (IUserInterface userInterface in UserInterfaces)
			{
				userInterface.Draw(gameTime, spriteBatch);
			}
		}

		#endregion
	}
}