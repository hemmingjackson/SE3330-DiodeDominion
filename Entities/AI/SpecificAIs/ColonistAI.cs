using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs
{
	public class ColonistAI : BaseAi
	{
		public ColonistAI(Colonist entity, MapTile[,] tiles) : base(entity, tiles)
		{
		}
	}
}
