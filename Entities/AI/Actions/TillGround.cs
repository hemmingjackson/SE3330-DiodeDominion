using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	public class TillGround : IAction
	{
		private readonly MapTile groundToTill;

		/// <summary>
		/// Type of tile that can be tilled
		/// </summary>
		private const TileType TillableTileType = TileType.DIRT;

		public TillGround(MapTile tile)
		{
			groundToTill = tile;		
		}

		public bool Update()
		{
			// Check if the ground can be tilled
			if (groundToTill.TypeOfTile == TillableTileType)
			{
				groundToTill.ChangeTileType(TileType.FARMLAND);
			}

			return true;	
		}
	}
}
