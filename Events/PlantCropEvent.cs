using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.World;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Event for colonist to plant crop
	/// </summary>
	public class PlantCropEvent : BaseEvent
	{
		#region Properties

		/// <summary>
		/// Entity to plant on the tile
		/// </summary>
		public Entity EntityToUse { get; }

		/// <summary>
		/// Tile to plant
		/// </summary>
		public MapTile TileToPlant { get; }

		public GameWorld GameWorld { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="gameWorld"></param>
		/// <param name="tile"></param>
		/// <param name="entity"></param>
		public PlantCropEvent(GameWorld gameWorld, MapTile tile, Entity entity)
		{
			GameWorld = gameWorld;
			TileToPlant = tile;
			EntityToUse = entity;
		}

		#endregion
	}
}
