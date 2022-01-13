using Diode_Dominion.DiodeDominion.Entities.Items;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Handles how the entity should go about picking up an item that exists
	/// within the game world.
	/// </summary>
	public class PickupItem : IAction
	{
		#region Fields

		/// <summary>
		/// Entity that will pickup the item
		/// </summary>
		private readonly Entity entity;

		#endregion
		
		#region Properties

		/// <summary>
		/// Item that will be picked up
		/// </summary>
		public Item Item { get; private set; }
		
		#endregion
		
		/// <summary>
		/// Creates an object that dictates how an entity will go about
		/// picking up an item
		/// </summary>
		/// 
		/// <param name="entity">Entity to pickup the item</param>
		public PickupItem(Entity entity)
		{
			this.entity = entity;
		}

		#region Methods
		
		/// <summary>
		/// Handle updating the entity to move it towards the item
		/// and then picking the item up.
		/// </summary>
		/// 
		/// <returns>Whether the action is complete. Should keep updating until it is complete</returns>
		public bool Update()
		{
			if (Item != null)
			{
				PickUp();
			}
			
			return true;
		}

		/// <summary>
		/// Tells the entity that this is an item that should be picked up
		/// </summary>
		/// <param name="item"></param>
		public void SetItemPickUp(Item item)
		{
			Item = item;
		}
		
		/// <summary>
		/// Handle having the entity add the item to it's inventory and telling
		/// the item it is being picked up
		/// </summary>
		private void PickUp()
		{
			// Do not add the item if it is already picked up
			if (!Item.IsInInventory)
			{
				// Tell the item it has been picked up and add the item to the entity's list
				Item.Pickup(entity);
			}

			// This should help make certain that updates do not occur when they should not
			Item = null;
		}

		#endregion
	}
}