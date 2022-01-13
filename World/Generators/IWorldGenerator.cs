using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators
{
	public interface IWorldGenerator
	{
		MapTile[,] GenerateWorld();		
	}
}
