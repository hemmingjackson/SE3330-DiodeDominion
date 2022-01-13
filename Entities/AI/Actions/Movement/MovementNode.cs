namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions.Movement
{
	/// <summary>
	/// Holds a location that the path finding algorithm visited
	/// and some general information about where it can go from that
	/// location.
	/// </summary>
	public class MovementNode
	{
		public MovementNode PreviousNode { get; }

		/// <summary>
		/// Whether the character can move right
		/// </summary>
		public bool CanMoveRight { get; set; }

		/// <summary>
		/// Whether the character can move left
		/// </summary>
		public bool CanMoveLeft { get; set; }

		/// <summary>
		/// Whether the character can move up
		/// </summary>
		public bool CanMoveUp { get; set; }

		/// <summary>
		/// Whether the character can move down
		/// </summary>
		public bool CanMoveDown { get; set; }

		/// <summary>
		/// Tile number along the x-axis
		/// </summary>
		public int TileX { get; set; }

		/// <summary>
		/// Tile number along the y-axis
		/// </summary>
		public int TileY { get; set; }

		/// <summary>
		/// Create the next node as an offshoot from the previous node
		/// </summary>
		/// <param name="previousNode"></param>
		/// <param name="xTile">What tile along the x-axis this is</param>
		/// <param name="yTile">>What tile along the y-axis this is</param>
		internal MovementNode(MovementNode previousNode, int xTile, int yTile)
		{
			PreviousNode = previousNode;
			TileX = xTile;
			TileY = yTile;
		}

		/// <summary>
		/// Checks to see if the x and y tiles are the same
		/// between two movement nodes
		/// </summary>
		/// 
		/// <param name="node"></param>
		/// 
		/// <returns>Whether the nodes are equal</returns>
		internal bool Equals(MovementNode node)
		{
			return TileX == node.TileX && TileY == node.TileY;
		}
	}
}
