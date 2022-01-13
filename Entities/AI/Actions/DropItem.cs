using Diode_Dominion.DiodeDominion.Entities.Items;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Handle the logic of how the AI will drop an item
	/// </summary>
	public class DropItem : IAction
	{
		/// <summary>
		/// Entity that should drop the item
		/// </summary>
		private readonly Entity entity;

		/// <summary>
		/// Item that is going to be dropped
		/// </summary>
		private readonly Item itemDrop;

		/// <summary>
		/// Create an object that can drop an item
		/// </summary>
		/// 
		/// <param name="entity">Entity that will drop the item</param>
		/// <param name="item">Item to be dropped</param>
		public DropItem(Entity entity, Item item)
		{
			this.entity = entity;
			itemDrop = item;
		}

		/// <summary>
		/// Action can be complete immediately
		/// </summary>
		/// <returns></returns>
		public bool Update()
		{
			Drop();
			return true;
		}

		/// <summary>
		/// Handle the logic that will be used to drop an item
		/// </summary>
		private void Drop()
		{
			entity.Holdables().Remove(itemDrop);
			itemDrop.Drop();
		}
	}
}
