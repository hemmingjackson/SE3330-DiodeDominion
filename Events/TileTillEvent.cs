using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Event used to till a dirt tile into a farmland tile
	/// </summary>
	public class TileTillEvent: BaseEvent
	{
		/// <summary>
		/// Entity to till the tile
		/// </summary>
		public Entity EntityToUse { get; }

		/// <summary>
		/// Tile to till
		/// </summary>
		public MapTile TileToTill { get; }

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="entity"></param>
		public TileTillEvent(MapTile tile, Entity entity)
		{
			TileToTill = tile;
			EntityToUse = entity;
		}
	}
}
