using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Microsoft.Xna.Framework;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Handles the planting of crops by the colonist
	/// </summary>
	public class PlantCrop : IAction
	{
		#region Fields

		private readonly MapTile groundToPlant;
		
		private readonly Crop crop;

		#endregion

		#region Constructors

		public PlantCrop(Crop crop, MapTile tile)
		{
			this.crop = crop;
			groundToPlant = tile;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates the crop location to the specific farmland tile
		/// </summary>
		/// <returns></returns>
		public bool Update()
		{
			crop.EntitySprite.Origin = new Vector2(groundToPlant.Location.X, groundToPlant.Location.Y);

			return true;
		}

		#endregion
	}
}
