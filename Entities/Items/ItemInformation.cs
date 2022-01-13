namespace Diode_Dominion.DiodeDominion.Entities.Items
{
	/// <summary>
	/// Holds all of the information about items in the game
	/// </summary>
	public class ItemInformation
	{
		#region Properties

		/// <summary>
		/// Type of item
		/// </summary>
		public ItemType ItemType { get; set; }

		/// <summary>
		/// Description of the item
		/// </summary>
		public string ItemDescription { get; set; }

		/// <summary>
		/// Name of item
		/// </summary>
		public string ItemName { get; set; }

		/// <summary>
		/// Item health (durability)
		/// </summary>
		public double ItemHealth { get; set; }

		#endregion

		#region Constructors


		/// <summary>
		/// Default Item Constructor
		/// </summary>
		/// <param name="item"></param>
		public ItemInformation(Item item)
		{
			ItemType = item.ItemType;
			ItemDescription = "";
			ItemName = item.Name;
			ItemHealth = item.Health;
		}

		#endregion

	}
}
