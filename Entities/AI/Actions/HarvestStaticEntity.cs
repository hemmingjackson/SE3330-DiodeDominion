using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.Items.Tools;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// This class defines the behaviour of how an AI will go about harvesting
	/// a <see cref="StaticEntity"/>.
	/// </summary>
	public class HarvestStaticEntity : IAction
	{
		#region Fields
		
		/// <summary>
		/// Reference to the entity that is harvesting the
		/// static entity
		/// </summary>
		private readonly Entity entity;

		/// <summary>
		/// Reference to the static entity that is being harvested
		/// </summary>
		private readonly StaticEntity staticEntity;

		/// <summary>
		/// Reference to the tool that the entity is using to harvest the
		/// static entity
		/// </summary>
		private Tool entityTool;

		#endregion

		#region Properties

		/// <summary>
		/// How far away the entity can be from the static entity
		/// until it can harvest it
		/// </summary>
		public float MaxHarvestDistance = 15f;

		#endregion

		#region Constructors

		/// <summary>
		/// Create the specific <see cref="Action"/> object that holds the
		/// logic pertaining to harvesting a <see cref="StaticEntity"/>
		/// </summary>
		/// 
		/// <param name="entity">Entity that will harvest the static entity</param>
		/// <param name="staticEntity">Static entity that is going to be harvested</param>
		public HarvestStaticEntity(Entity entity, StaticEntity staticEntity)
		{
			this.entity = entity;
			this.staticEntity = staticEntity;
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Update the harvesting completion of the static entity
		/// </summary>
		/// 
		/// <returns>True if the entity has been harvested</returns>
		public bool Update()
		{
			// Check if the entity tool has been determined yet
			if (entityTool == null)
			{
				entityTool = EntityHarvestTool();
			}

			bool harvested = false;

			// Check if the entity is close enough to harvest the static entity
			if (IsCloseEnoughToHarvest())
			{
				// Check if the tools are correct
				harvested = staticEntity.Harvest(entity, entityTool);
			}

			// Check if the static entity is finished being harvested
			bool finishedHarvesting = harvested && staticEntity.OnGround;

			// Return whether it could not be harvested or if it is finished being harvest
			return !harvested || finishedHarvesting;
		}

		/// <summary>
		/// Returns whether the entity is close enough to the
		/// static entity to harvest it
		/// </summary>
		/// <returns></returns>
		private bool IsCloseEnoughToHarvest()
		{
			// Find the distances using the pythagorean theorem 
			double distanceX = entity.Coordinate.X - staticEntity.Coordinate.X;
			double distanceY = entity.Coordinate.Y - staticEntity.Coordinate.Y;

			distanceX = Math.Pow(distanceX, 2);
			distanceY = Math.Pow(distanceY, 2);

			double totalDistance = Math.Sqrt(distanceX + distanceY);

			return totalDistance <= MaxHarvestDistance;
		}

		/// <summary>
		/// Returns the tool that the entity is using to harvest the
		/// static entity. If there are multiple tools that the entity
		/// can choose from, it will return the highest quality tool.
		/// </summary>
		/// 
		/// <returns>Tool entity will use to harvest the static entity</returns>
		private Tool EntityHarvestTool()
		{
			// Find all of the tools the entity can use
			List<Tool> entityTools = new List<Tool>();

			// Look for the proper tool in the entities inventory
			foreach (Item item in entity.Holdables())
			{
				if (item is Tool tool && staticEntity.CanInteract(tool))
				{
					entityTools.Add(tool);
				}
			}

			// Find the best tool the entity can use
			// TODO: Find why the tools are not being saved properly
			Tool bestTool = (entityTools.Count > 0) ? entityTools[0] : new Tool(ToolType.NONE);

			foreach (Tool tool in entityTools.Where(tool => tool.ToolQuality > bestTool.ToolQuality))
			{
				bestTool = tool;
			}
			
			return bestTool;
		}

		#endregion
	}
}