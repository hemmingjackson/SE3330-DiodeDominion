using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.World
{
	/// <summary>
	/// This class will handle events from the game world and send them to the
	/// respective entity that needs to be updated.
	/// </summary>
	public class EntityEventHandler
	{
		/// <summary>
		/// Holds a reference to the game world so that it can update the entities
		/// </summary>
		private readonly GameWorld game;

		/// <summary>
		/// Create an event handler for the game world
		/// </summary>
		/// 
		/// <param name="gameWorld">Game world to create the event handler on</param>
		public EntityEventHandler(GameWorld gameWorld)
		{
			game = gameWorld;
		}

		/// <summary>
		/// Pass the base event type into this class to facilitate the updating
		/// of the proper entity
		/// </summary>
		/// 
		/// <param name="baseEvent">Event that occurred</param>
		public void Update(BaseEvent baseEvent)
		{
			if (baseEvent is DamageDealtEvent damageDealtEvent)
			{
				DamageEvent(damageDealtEvent);
			}
			else if (baseEvent is DropItemEvent dropItemEvent)
			{
				DropItem(dropItemEvent);
			}
			else if (baseEvent is HealEvent healEvent)
			{
				Heal(healEvent);
			}
			else if (baseEvent is MoveEntityEvent moveEntityEvent)
			{
				MoveEntity(moveEntityEvent);
			}
			else if (baseEvent is PickUpItemEvent pickUpItemEvent)
			{
				PickUpItem(pickUpItemEvent);
			}
			else if (baseEvent is StopActionsEvent stopActionsEvent)
			{
				StopActions(stopActionsEvent);
			}
			else if (baseEvent is HarvestStaticEntityEvent harvestStaticEntityEvent)
			{
				HarvestStaticEntity(harvestStaticEntityEvent);
			}
		}

		/// <summary>
		/// Handle passing the instance of the damage event to the correct destination
		/// </summary>
		/// 
		/// <param name="damageDealt">Event where damage occurred to a specific target</param>
		private void DamageEvent(DamageDealtEvent damageDealt)
		{
			game.Entities.Find(entity => Equals(entity, damageDealt.Target)).BaseAi.UpdateDamageEvent(damageDealt);
		}

		/// <summary>
		/// Send an event to the specified entity that it should pick up a specified item
		/// </summary>
		/// 
		/// <param name="pickUpItem">Item pick up event</param>
		private void PickUpItem(PickUpItemEvent pickUpItem)
		{
			game.Entities.Find(entity => entity.Id == pickUpItem.EntityId).BaseAi.PickUpItemEvent(pickUpItem);
		}

		/// <summary>
		/// Handle passing the event for the specified entity to drop an item
		/// </summary>
		/// 
		/// <param name="dropItem">Drop item event</param>
		private void DropItem(DropItemEvent dropItem)
		{
			game.Entities.Find(entity => entity.Id == dropItem.EntityId).BaseAi.DropItemEvent(dropItem);
		}

		/// <summary>
		/// Send an event to the specified entity that it should heal by the specified amount
		/// </summary>
		/// 
		/// <param name="heal"></param>
		private void Heal(HealEvent heal)
		{
			game.Entities.Find(entity => entity.Id == heal.TargetId).BaseAi.UpdateHealEvent(heal);
		}

		/// <summary>
		/// Send an event to the specified entity that it should move to a specific location
		/// </summary>
		/// 
		/// <param name="moveEntity">Entity move event</param>
		private void MoveEntity(MoveEntityEvent moveEntity)
		{
			game.Entities.Find(entity => entity.Id == moveEntity.EntityId).BaseAi.MoveEntityEvent(moveEntity);
		}

		/// <summary>
		/// Send an event to the specified entity that it should begin harvesting the
		/// static entity
		/// </summary>
		/// 
		/// <param name="harvestStaticEntity">Event to harvest a static entity</param>
		private void HarvestStaticEntity(HarvestStaticEntityEvent harvestStaticEntity)
		{
			// Find the correct entity and go harvest it
			game.Entities.Find(entity => entity.Id == harvestStaticEntity.EntityId).BaseAi
				.HarvestStaticEntityEvent(harvestStaticEntity);
		}

		/// <summary>
		/// Stop the actions of a specific entity
		/// </summary>
		/// 
		/// <param name="stopActions"></param>
		private void StopActions(StopActionsEvent stopActions)
		{
			game.Entities.Find(entity => entity.Id == stopActions.EntityId).BaseAi.StopAllActionsEvent();
		}
	}
}
