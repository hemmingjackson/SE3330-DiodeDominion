using Diode_Dominion.DiodeDominion.Entities.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.Entities.StaticEntities
{
	/// <summary>
	/// A static entity is a type of entity that does not move or change unless
	/// an entity interacts with it.
	/// </summary>
	public class StaticEntity : Entity
	{
		#region Properties

		/// <summary>
		/// Type of static entity that the specific class is
		/// </summary>
		public StaticEntityType StaticEntityType { get; }

		/// <summary>
		/// Holds an instance of the tool type required to harvest/interact
		/// with the static entity
		/// </summary>
		public ToolType ToolTypeRequired { get; }

		/// <summary>
		/// Used to verify whether the item is on the ground after
		/// being harvested
		/// </summary>
		public bool OnGround { get; set; }

		/// <summary>
		/// Used to verify whether harvest-able or not
		/// </summary>
		public bool IsHarvestable { get; set; } = true;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a static entity object of a specific type
		/// </summary>
		/// 
		/// <param name="staticEntityType">Specific type of static entity</param>
		/// <param name="toolRequired">Tool needed to interact with the static entity</param>
		public StaticEntity(StaticEntityType staticEntityType, ToolType toolRequired)
		{
			StaticEntityType = staticEntityType;
			ToolTypeRequired = toolRequired;
		}

		/// <summary>
		/// Creates static entity object that doesn't need a tool
		/// </summary>
		/// <param name="staticEntityType">type of static entity</param>
		public StaticEntity(StaticEntityType staticEntityType)
		{
			StaticEntityType = staticEntityType;
			Name = staticEntityType.ToString();
			IsHarvestable = false;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns whether an entity is able to interact with the static entity.
		/// Specific static entities should override this method, checking if the
		/// entity has the proper tool to harvest them.
		/// </summary>
		/// 
		/// <param name="entity"></param>
		/// 
		/// <returns>Whether the entity can interact with the static entity</returns>
		public virtual bool CanInteract(Entity entity)
		{
			bool canInteract = false;

			// Look through the items that the entity has to see if they can interact
			for (int i = 0; i < entity.Holdables().Count; i++)
			{
				// Check the item is a tool and then if it is the correct tool
				if (entity.Holdables()[i].IsTool && 
				    ((Tool)entity.Holdables()[i]).ToolType == ToolTypeRequired)
				{
					canInteract = true;
				}
			}

			// No matter what, if the static entity does not require a tool, they can interact
			if (ToolTypeRequired == ToolType.NONE)
			{
				canInteract = true;
			}

			return canInteract;
		}

		/// <summary>
		/// Checks if the tool is able to interact with the static entity
		/// </summary>
		/// 
		/// <param name="tool">Tool interacting with the static entity</param>
		/// 
		/// <returns>Whether the tool interacts with the static entity</returns>
		public virtual bool CanInteract(Tool tool)
		{
			return ToolTypeRequired == tool.ToolType || ToolTypeRequired == ToolType.NONE;
		}

		/// <summary>
		/// Attempt to harvest the static entity. Pass in the tool
		/// being used to harvest the static entity and the entity
		/// doing the harvesting. It will check to see if the entity
		/// can interact on harvest.
		/// </summary>
		/// 
		/// <param name="entity">Entity harvesting the static entity</param>
		/// <param name="tool">Tool being used to harvest</param>
		///
		/// <returns>Whether the entity was able to harvest the item</returns>
		public virtual bool Harvest(Entity entity, Tool tool)
		{
			bool harvested = false;

			// Make certain the tool can interact with it and it is not on the ground
			if (CanInteract(tool) && !OnGround)
			{
				harvested = true;

				Health -= tool.ToolQuality * .1;

				// Check if it took enough damage to be destroyed
				if (Health <= 0d)
				{
					OnGround = true;
				}
			}

			return harvested;
		}

		/// <summary>
		/// Draw the static entity to the screen if it has not been harvested
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Only draw if not on the ground
			if (!OnGround)
			{
				EntitySprite.Draw(gameTime, spriteBatch);
			}
		}

		#endregion
	}
}
