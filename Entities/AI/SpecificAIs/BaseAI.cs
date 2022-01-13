using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities.AI.Actions;
using Diode_Dominion.DiodeDominion.Entities.AI.Actions.Movement;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Diode_Dominion.Engine.Events;
using Diode_Dominion.Engine.GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs
{
	/// <summary>
	/// Base AI that will specify general behavior for Entities
	/// </summary>
	public class BaseAi
	{
		#region Fields

		private const int ItemOutOfBoundsLocation = -5000;

		/// <summary>
		/// Entity associated with the AI. This will be used so that the AI can
		/// directly update the entity, telling it what to do.
		/// </summary>
		private readonly Entity entity;

		/// <summary>
		/// Map tiles that make up the game world
		/// </summary>
		private MapTile[,] mapTiles;

		#endregion

		#region Properties

		/// <summary>
		/// Holds a queue that dictates the actions that the entity must complete
		/// </summary>
		private Queue<IAction> Actions { get; }

		/// <summary>
		/// An action that will attempt to be completed before all others
		/// </summary>
		private IAction ImmediateAction { get; set; }

		/// <summary>
		/// Action that will only be worked on when there are no immediate actions
		/// or actions in the queue
		/// </summary>
		private IAction PassiveAction { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// AI for an Entity. Will dictate how it interacts with the world
		/// </summary>
		/// 
		/// <param name="entity">Entity associated with the AI</param>
		/// <param name="mapTiles">Map tiles in the game world</param>
		public BaseAi(Entity entity, MapTile[,] mapTiles)
		{
			this.entity = entity;
			this.mapTiles = mapTiles;

			Actions = new Queue<IAction>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns the number of actions remaining
		/// </summary>
		/// <returns></returns>
		public int ActionsRemaining()
		{
			return Actions.Count;
		}

		public void SetMapTiles(MapTile[,] tiles)
		{
			mapTiles = tiles;
		}

		/// <summary>
		/// Tell the AI to update the tasks it is currently
		/// attempting to achieve.
		/// </summary>
		public virtual void Update()
		{
			// Check for actions to perform
			if (ImmediateAction != null && ImmediateAction.Update())
			{
				// Set to null if the immediate action is complete
				ImmediateAction = null;
			}
			else if (Actions.Count > 0) // Check for normal priority actions to be complete
			{
				IAction currentAction = Actions.Peek();

				// Update the action and check if it is complete
				if (currentAction != null && currentAction.Update())
				{
					// Remove the completed action
					Actions.Dequeue();
				}
			}
			else if (PassiveAction != null && PassiveAction.Update()) // Complete a passive action
			{
				PassiveAction = null;
			}

		}
		
		/// <summary>
		/// An entry point where the AI will take stimulus from the game world.
		/// This is here as a design choice over a functional one as the
		/// functionality that is branches off to is available in other methods
		/// </summary>
		/// 
		/// <param name="baseEvent">Event that will need to be cast</param>
		public virtual void Update(BaseEvent baseEvent)
		{
			if (baseEvent is MoveEntityEvent moveEntity) // Move the entity
			{
				MoveEntityEvent(moveEntity);
			}
			else if (baseEvent is DamageDealtEvent damageEvent) // Have entity take damage
			{
				UpdateDamageEvent(damageEvent);
			}
			else if (baseEvent is HealEvent healEvent) // Have the entity heal
			{
				UpdateHealEvent(healEvent);
			}
			else if (baseEvent is PickUpItemEvent pickUpItemEvent) // Have the entity pick up an item
			{
				PickUpItemEvent(pickUpItemEvent);
			}
			else if (baseEvent is DropItemEvent dropItemEvent) // Have the entity drop an item
			{
				DropItemEvent(dropItemEvent);
			}
			else if (baseEvent is HarvestStaticEntityEvent harvestStaticEntityEvent) // Have the entity go harvest a static entity
			{
				HarvestStaticEntityEvent(harvestStaticEntityEvent);
			}
			else if (baseEvent is UpdateHungerEvent updateHungerEvent)
			{
				UpdateHungerEvent(updateHungerEvent);
			}
			else if (baseEvent is DepositItemInStockpileEvent depositItemEvent)
			{
				DepositItemEvent(depositItemEvent);
			}
			else if (baseEvent is CropGrowthEvent cropGrowthEvent) // Crop growth
			{
				CropGrowthEvent(cropGrowthEvent);
			}
			else if (baseEvent is StopActionsEvent) // Have the entity stop all actions
			{
				StopAllActionsEvent();
			}
			else if (baseEvent is TileTillEvent tileTillEvent)
			{
				TillGroundEvent(tileTillEvent);
			}
			else if (baseEvent is PlantCropEvent plantCropEvent) // Have the colonist plant each crop
			{
				PlantCropEvent(plantCropEvent);
			}
		}

		/// <summary>
		/// Tills ground from dirt into farmland
		/// </summary>
		/// <param name="tileTill"></param>
		public virtual void TillGroundEvent(TileTillEvent tileTill)
		{
			//Move to tile
			MoveEntityEvent(new MoveEntityEvent(tileTill.EntityToUse.Id, 
				tileTill.TileToTill.Location.X, tileTill.TileToTill.Location.Y));
			
			//Till the till
			Actions.Enqueue(new TillGround(tileTill.TileToTill));
		}

		/// <summary>
		/// Moves the colonist to each farmland tile and plants crop
		/// </summary>
		/// <param name="plantCrop"></param>
		public virtual void PlantCropEvent(PlantCropEvent plantCrop)
		{
			// Colonist will move to each individual farmland tile
			MoveEntityEvent(new MoveEntityEvent(plantCrop.EntityToUse.Id, 
				plantCrop.TileToPlant.Location.X, plantCrop.TileToPlant.Location.Y));

			// New crop is created for each selected farmland tile
			Crop crop = new Crop(CropType.CORN) {EntitySprite = 
				{Origin = new Vector2(ItemOutOfBoundsLocation, ItemOutOfBoundsLocation)}};
			crop.SetTexture(Settings.Content.Load<Texture2D>(
				TextureLocalization.Crops + crop.Name[0] + 
				crop.Name.Substring(1).ToLowerInvariant() + "Stage1"));
			Actions.Enqueue(new PlantCrop(crop, plantCrop.TileToPlant));
			plantCrop.GameWorld.Entities.Add(crop);
		}

		/// <summary>
		/// Deposits an item into a stockpile
		/// </summary>
		/// 
		/// <param name="depositItem"></param>
		public virtual void DepositItemEvent(DepositItemInStockpileEvent depositItem)
		{
			// Move to the stockpile
			MoveEntityEvent(new MoveEntityEvent(entity.Id, 
				depositItem.Stockpile.CenterLocation.X, depositItem.Stockpile.CenterLocation.Y));

			// Drop the item so it is displayed in the world
			DropItemEvent(new DropItemEvent(entity.Id, depositItem.Item));
			
			// Move the item to the stockpile
			Actions.Enqueue(new DepositInStockpile(depositItem.Item, depositItem.Stockpile));
		}

		/// <summary>
		/// Handle updating the health of the entity when it takes damage
		/// </summary>
		/// 
		/// <param name="damageDealt">Event with the damage</param>
		public virtual void UpdateDamageEvent(DamageDealtEvent damageDealt)
		{
			MoveEntityEvent(new MoveEntityEvent(entity.Id,
				damageDealt.Target.EntitySprite.Origin.X, damageDealt.Target.EntitySprite.Origin.Y));
			if (damageDealt.Attacker.Health > 0 && damageDealt.Target.Health > 0)
			{
				Actions.Enqueue(new Fight(entity, damageDealt.Target, damageDealt.Weapon, GameTimer.Time));
			}
		}

		/// <summary>
		/// Handle healing the entity
		/// </summary>
		/// 
		/// <param name="heal">Event that says how much the entity should heal by</param>
		public virtual void UpdateHealEvent(HealEvent heal)
		{
			new UpdateHealth(entity).IncrementHealth(heal.Heal);
		}

		/// <summary>
		/// Handle telling the entity what item it should be picking up
		/// </summary>
		/// 
		/// <param name="pickUpItem">Event that holds information of the item that must be picked up</param>
		public virtual void PickUpItemEvent(PickUpItemEvent pickUpItem)
		{
			MoveEntityEvent(new MoveEntityEvent(entity.Id, 
				pickUpItem.Item.EntitySprite.Origin.X, pickUpItem.Item.EntitySprite.Origin.Y));

			// Say to pick up an item
			PickupItem itemPickUp = new PickupItem(entity);
			itemPickUp.SetItemPickUp(pickUpItem.Item);

			// Add the action to a queue of things that must be complete
			Actions.Enqueue(itemPickUp);
		}

		/// <summary>
		/// Handle telling the entity that it should drop an item
		/// </summary>
		/// 
		/// <param name="dropItem">Event that holds item that should be dropped</param>
		public virtual void DropItemEvent(DropItemEvent dropItem)
		{
			Actions.Enqueue(new DropItem(entity, dropItem.Item));
		}

		/// <summary>
		/// Handle moving the entity from one location to the next
		/// </summary>
		/// 
		/// <param name="moveEntity">Event that has information on where the entity must move to</param>
		public virtual void MoveEntityEvent(MoveEntityEvent moveEntity)
		{
			IAction[] currentActions = Actions.ToArray();

			Vector2 entityStartLocation = new Vector2(entity.EntitySprite.Origin.X, entity.EntitySprite.Origin.Y);

			// Go from end to start to search for the last move
			for (int i = currentActions.Length - 1; i >= 0; i--)
			{
				if (currentActions[i] is Move movementAction)
				{
					entityStartLocation = movementAction.Destination;

					// Kill the loop
					i = -1;
				}
			}

			// Create the pathfinder
			Pathfinder pathfinder = new Pathfinder(entity, mapTiles, entityStartLocation);
			
			// Find all of the movement steps necessary to reach the destination
			List<Move> movements =
				pathfinder.CreateMovementSteps(new Vector2(moveEntity.XLocation, moveEntity.YLocation));

			// Add the action to the queue of things that must be complete
			foreach (Move movement in movements)
			{
				Actions.Enqueue(movement);
			}
		}

		/// <summary>
		/// Handle the harvesting of static entities
		/// </summary>
		/// <param name="harvestStaticEntity"></param>
		public virtual void HarvestStaticEntityEvent(HarvestStaticEntityEvent harvestStaticEntity)
		{
			// Move to the static entity
			MoveEntityEvent(new MoveEntityEvent(entity.Id, harvestStaticEntity.StaticEntity.EntitySprite.Origin.X, 
				harvestStaticEntity.StaticEntity.EntitySprite.Origin.Y));
			
			Actions.Enqueue(new HarvestStaticEntity(entity, harvestStaticEntity.StaticEntity));
		}

		/// <summary>
		/// Handle the harvesting of static entities
		/// </summary>
		/// <param name="updateHungerEvent"></param>
		public virtual void UpdateHungerEvent(UpdateHungerEvent updateHungerEvent)
		{
			updateHungerEvent.UpdateColonistHunger();
		}

		/// <summary>
		/// Handle the growth of crops
		/// </summary>
		/// <param name="cropGrowthEvent"></param>
		public virtual void CropGrowthEvent(CropGrowthEvent cropGrowthEvent)
		{
			cropGrowthEvent.UpdateCropGrowth();
		}

		/// <summary>
		/// Notifies the AI that the entity should stop all of it's current events that it is
		/// attempting to accomplish
		/// </summary>
		public void StopAllActionsEvent()
		{
			// Remove all actions from the queue
			Actions.Clear();
		}

		#endregion
	}
}