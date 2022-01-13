using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Microsoft.Xna.Framework;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions.Movement
{
	/// <summary>
	/// Used to find a path through the game world to the destination location
	/// while avoiding moving into unreachable locations.
	/// </summary>
	public class Pathfinder
	{
		#region Fields

		private const int TileSize = 100;
		private const int TileOffset = 50;

		/// <summary>
		/// How large the ideal path can be before it gives up
		/// </summary>
		private const int MaxIdealPath = 150;

		/// <summary>
		/// Entity to move in the game world
		/// </summary>
		private readonly Entity entity;

		/// <summary>
		/// Map tiles that make up the game world
		/// </summary>
		private readonly MapTile[,] mapTiles;

		/// <summary>
		/// Holds the most ideal list of movement nodes from the
		/// starting node to the ending node
		/// </summary>
		private readonly List<MovementNode> idealMovementPath;

		/// <summary>
		/// Location in the ending node where the entity is attempting to reach
		/// </summary>
		private Vector2 destination;

		/// <summary>
		/// Starting location where the movement will begin from
		/// </summary>
		private readonly Vector2 startLocation;

		#endregion

		#region Properties

		/// <summary>
		/// Starting point of the entity
		/// </summary>
		public MovementNode StartingNode { get; private set; }

		/// <summary>
		/// Location that the entity is attempting to reach
		/// </summary>
		public MovementNode EndingNode { get; private set; }

		#endregion

		/// <summary>
		/// Create the object to determine the best way to reach the destination location.
		/// </summary>
		/// 
		/// <param name="entity">Entity to move</param>
		/// <param name="gameTiles">Tiles that make up the game world</param>
		/// <param name="startingPoint"></param>
		public Pathfinder(Entity entity, MapTile[,] gameTiles, Vector2 startingPoint)
		{
			mapTiles = gameTiles;
			this.entity = entity;
			idealMovementPath = new List<MovementNode>();
			startLocation = startingPoint;
		}

		/// <summary>
		/// Creates the movement from one location to the destination location
		/// </summary>
		///
		/// <param name="destinationLocation">Location to move the entity to</param>
		/// 
		/// <returns>Queue of movement steps to move the entity to the destination location</returns>
		public List<Move> CreateMovementSteps(Vector2 destinationLocation)
		{
			destination = destinationLocation;

			CreateEndNode(destinationLocation);
			CreateStartNode();

			MovementNode path = StartingNode;

			idealMovementPath.Add(StartingNode);

			// Loop while there is a valid path, and path is not equal to the ending node
			do
			{
				path = CreateNextNode(path);
			} while (path != null && path.TileX != EndingNode.TileX && path.TileY != EndingNode.TileY);
			
			// Check if last point was bad
			if (path == null || path.TileX != EndingNode.TileX || path.TileY != EndingNode.TileY)
			{
				idealMovementPath.Add(EndingNode);
			}

			List<Move> steps = new List<Move>();

			// Go through the ideal move path and add the movement steps
			foreach (MovementNode movementNode in idealMovementPath)
			{
				Move movement = new Move(entity);

				// Watch for the last node and if it is the last move directly to the correct location
				if (movementNode.TileX != EndingNode.TileX && movementNode.TileY != EndingNode.TileY)
				{
					movement.SetLocation(movementNode.TileX * TileSize + TileOffset, 
						movementNode.TileY * TileSize + TileOffset);
				}
				else
				{
					movement.SetLocation(destination.X, destination.Y);
				}

				steps.Add(movement);
			}

			// Reduce the number of move steps
			steps = ReduceRedundantMoves(steps);

			return steps;
		}

		/// <summary>
		/// Obtains the list of ideal movement nodes that were generated from the
		/// movement from the start to the end
		/// </summary>
		/// 
		/// <returns></returns>
		public List<MovementNode> GetIdealMovementNodes()
		{
			return idealMovementPath;
		}

		/// <summary>
		/// Remove the movements that do not change the end movement result.
		/// </summary>
		/// 
		/// <param name="moves">List of movements</param>
		/// 
		/// <returns>Reduced list of movements</returns>
		private static List<Move> ReduceRedundantMoves(IReadOnlyList<Move> moves)
		{
			List<Move> updatedMoves = new List<Move>();

			if (moves.Count > 0)
			{
				updatedMoves.Add(moves[moves.Count - 1]);
			}

			// Move backwards over the list to start at the end result
			for (int i = moves.Count - 2; i >= 0; i--)
			{
				// Check if there is a difference between the x or y positions
				if (Math.Abs(moves[i].Destination.X - updatedMoves[updatedMoves.Count - 1].Destination.X) > 0 && 
				    Math.Abs(moves[i].Destination.Y - updatedMoves[updatedMoves.Count - 1].Destination.Y) > 0)
				{
					// Add the different move
					updatedMoves.Add(moves[i]);
				}
			}

			// Reverse the list to put it in the correct order
			updatedMoves.Reverse();

			return updatedMoves;
		}

		/// <summary>
		/// Create the node to find the ending location
		/// </summary>
		/// 
		/// <param name="destinationLocation">Location to move towards</param>
		private void CreateEndNode(Vector2 destinationLocation)
		{
			(float x, float y) = destinationLocation;

			int endingX = (int) Math.Floor(x / TileSize);
			int endingY = (int) Math.Floor(y / TileSize);

			// Fix in-case it ends up being out of bounds. Occurs on occasion
			if (endingY >= mapTiles.GetLength(0))
			{
				endingY = mapTiles.GetLength(0) - 1;
			} 
			else if (endingY < 0)
			{
				endingY = 0;
			}

			if (endingX >= mapTiles.GetLength(1))
			{
				endingX = mapTiles.GetLength(1) - 1;
			}
			else if (endingX < 0)
			{
				endingX = 0;
			}

			EndingNode = new MovementNode(null, endingX, endingY)
			{
				// Set whether adjacent tiles can be entered
				CanMoveLeft = endingX > 0 && mapTiles[endingY, endingX - 1].CanEnter,
				CanMoveRight = endingX + 1 < mapTiles.GetLength(1) && mapTiles[endingY, endingX + 1].CanEnter,
				CanMoveUp = endingY > 0 && mapTiles[endingY - 1, endingX].CanEnter,
				CanMoveDown = endingY + 1 < mapTiles.GetLength(0) && mapTiles[endingY + 1, endingX].CanEnter
			};
		}

		/// <summary>
		/// Create the node for the starting location
		/// </summary>
		private void CreateStartNode()
		{
			int startingX = (int) Math.Floor(startLocation.X / TileSize);
			int startingY = (int) Math.Floor(startLocation.Y / TileSize);

			// Fix in-case it ends up being out of bounds. Occurs on occasion
			if (startingY >= mapTiles.GetLength(0))
			{
				startingY = mapTiles.GetLength(0) - 1;
			} 
			else if (startingY < 0)
			{
				startingY = 0;
			}

			if (startingX >= mapTiles.GetLength(1))
			{
				startingX = mapTiles.GetLength(1) - 1;
			}
			else if (startingX < 0)
			{
				startingX = 0;
			}

			StartingNode = new MovementNode(null, startingX, startingY)
			{
				// Set whether adjacent tiles can be entered
				CanMoveLeft = startingX > 0 && mapTiles[startingY, startingX - 1].CanEnter,
				CanMoveRight = startingX + 1 < mapTiles.GetLength(1) && mapTiles[startingY, startingX + 1].CanEnter,
				CanMoveUp = startingY > 0 && mapTiles[startingY - 1, startingX].CanEnter,
				CanMoveDown = startingY + 1 < mapTiles.GetLength(0) && mapTiles[startingY + 1, startingX].CanEnter
			};
		}

		/// <summary>
		/// Creates the next node in the series
		/// </summary>
		/// <param name="previousNode"></param>
		/// <returns></returns>
		private MovementNode CreateNextNode(MovementNode previousNode)
		{
			// Find the distance yet to travel
			int distanceX = EndingNode.TileX - previousNode.TileX;
			int distanceY = EndingNode.TileY - previousNode.TileY;

			MovementNode nextNode = null;

			// Check for distance and then ability to move into location
			if (Math.Abs(distanceX) >= Math.Abs(distanceY))
			{
				if (distanceX > 0 && previousNode.CanMoveRight)
				{
					// Handle moving right
					nextNode = MoveRight(previousNode);
				}
				else if (distanceX < 0 && previousNode.CanMoveLeft)
				{
					// Handle moving left
					nextNode = MoveLeft(previousNode);
				}
				
			}
			else
			{
				if (distanceY > 0 && previousNode.CanMoveDown)
				{
					// Handle moving down
					nextNode = MoveDown(previousNode);
				}
				else if (distanceY < 0 && previousNode.CanMoveUp)
				{
					// Handle moving up
					nextNode = MoveUp(previousNode);
				}
			}

			// No direct path found
			if (nextNode == null && (distanceX > 0 || distanceY > 0))
			{
				nextNode = HandleNonIdealMovement(previousNode);
			}

			// Only add if a new valid node was generated and it is not a repeating node
			if (nextNode != null && idealMovementPath.Count > 0 && !idealMovementPath[idealMovementPath.Count - 1].Equals(nextNode))
			{
				idealMovementPath.Add(nextNode);
			}

			// Check if path is getting too long and stop it
			if (idealMovementPath.Count >= MaxIdealPath)
			{
				nextNode = null;
			}
			
			return nextNode;
		}

		/// <summary>
		/// Determines the next movement location based on the first direction it can move.
		/// </summary>
		/// 
		/// <param name="previousNode"></param>
		/// 
		/// <returns>The next node in the series or null if there is no possible solution</returns>
		private MovementNode HandleNonIdealMovement(MovementNode previousNode)
		{
			MovementNode nextNode;

			// Pick the first direction available
			if (previousNode.CanMoveRight)
			{
				nextNode = MoveRight(previousNode);
			}
			else if (previousNode.CanMoveLeft)
			{
				nextNode = MoveLeft(previousNode);
			}
			else if (previousNode.CanMoveDown)
			{
				nextNode = MoveDown(previousNode);
			}
			else if (previousNode.CanMoveUp)
			{
				nextNode = MoveUp(previousNode);
			}
			else
			{
				nextNode = ReverseMovementForBadPath(previousNode);
			}

			return nextNode;
		}

		/// <summary>
		/// Handle reversing the movement if a bad path was found. It will look at the
		/// node before the previous node and make it so it will not attempt to go that
		/// direction again and consider it a dead path.
		///
		/// Should only be called if the previous node has no possible direction to go.
		/// </summary>
		/// 
		/// <param name="previousNode"></param>
		/// 
		/// <returns>Node before the previous node</returns>
		private MovementNode ReverseMovementForBadPath(MovementNode previousNode)
		{
			MovementNode beforePreviousNode = previousNode.PreviousNode;

			// Can be null if it is the starting node
			// If it is the starting node, it means no valid path could be found
			if (beforePreviousNode != null)
			{
				// Set the location 
				if (previousNode.TileX > beforePreviousNode.TileX)
				{
					beforePreviousNode.CanMoveRight = false;
				}
				else if (previousNode.TileX < beforePreviousNode.TileX)
				{
					beforePreviousNode.CanMoveLeft = false;
				}
				else if (previousNode.TileY < beforePreviousNode.TileY)
				{
					beforePreviousNode.CanMoveUp = false;
				}
				else if (previousNode.TileY > beforePreviousNode.TileY)
				{
					beforePreviousNode.CanMoveDown = false;
				}

				// Remove the nodes after to rectify the path
				RectifyIdealPath(beforePreviousNode);
			}

			return beforePreviousNode;
		}

		/// <summary>
		/// Removes everything from the ideal path after the supplied node.
		/// The node that is supplied is still included.
		/// </summary>
		/// 
		/// <param name="node">Remove everything after this node</param>
		private void RectifyIdealPath(MovementNode node)
		{
			// Reverse over the ideal path
			for (int i = idealMovementPath.Count - 1; i >= 0; i--)
			{
				// Check if reached the node to move back towards
				if (idealMovementPath[i].Equals(node))
				{
					// Move out of bounds
					i = -1;
				}
				else
				{
					idealMovementPath.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Handles making a new node right of the previous node.
		/// Assumes that movement into this node is possible.
		/// </summary>
		///
		/// <param name="previousNode">Node left of the new node</param>
		///
		/// <returns>Node right of the previous node</returns>
		private MovementNode MoveRight(MovementNode previousNode)
		{
			previousNode.CanMoveRight = false;

			// Create the new node in the proper location
			return SetValidMovementDirections(
				new MovementNode(previousNode, previousNode.TileX + 1, previousNode.TileY));
		}

		/// <summary>
		/// Handles making a new node left of the previous node.
		/// Assumes that movement into this node is possible.
		/// </summary>
		///
		/// <param name="previousNode">Node right of the new node</param>
		///
		/// <returns>Node left of the previous node</returns>
		private MovementNode MoveLeft(MovementNode previousNode)
		{
			previousNode.CanMoveLeft = false;

			// Create the new node in the proper location
			return SetValidMovementDirections(
				new MovementNode(previousNode, previousNode.TileX - 1, previousNode.TileY));
		}

		/// <summary>
		/// Handles making a new node above the previous node.
		/// Assumes that movement into this node is possible.
		/// </summary>
		///
		/// <param name="previousNode">Node below the new node</param>
		///
		/// <returns>Node above the previous node</returns>
		private MovementNode MoveUp(MovementNode previousNode)
		{
			previousNode.CanMoveUp = false;

			// Create the new node in the proper location
			return SetValidMovementDirections(
				new MovementNode(previousNode, previousNode.TileX, previousNode.TileY - 1));
		}

		/// <summary>
		/// Handles making a new node below the previous node.
		/// Assumes that movement into this node is possible.
		/// </summary>
		///
		/// <param name="previousNode">Node above the new node</param>
		///
		/// <returns>Node below the previous node</returns>
		private MovementNode MoveDown(MovementNode previousNode)
		{
			previousNode.CanMoveDown = false;

			// Create the new node in the proper location
			return SetValidMovementDirections(
				new MovementNode(previousNode, previousNode.TileX, previousNode.TileY + 1));
		}

		/// <summary>
		/// Sets the valid movement directions on the passed in <see cref="MovementNode"/>
		/// and returns the object with the internal bool values set.
		/// </summary>
		/// 
		/// <param name="node">Node to check proper directions of</param>
		///
		/// <returns>Node with proper movement directions</returns>
		private MovementNode SetValidMovementDirections(MovementNode node)
		{
			// Check left of the node
			node.CanMoveLeft = node.TileX - 1 >= 0 && mapTiles[node.TileY, node.TileX - 1].CanEnter;

			// Check right of the node
			node.CanMoveRight = node.TileX + 1 < mapTiles.GetLength(1) && mapTiles[node.TileY, node.TileX + 1].CanEnter;

			// Check above the node
			node.CanMoveUp = node.TileY - 1 >= 0 && mapTiles[node.TileY - 1, node.TileX].CanEnter;

			// Check below the node
			node.CanMoveDown = node.TileY + 1 < mapTiles.GetLength(0) && mapTiles[node.TileY + 1, node.TileX].CanEnter;

			// Check the previous node's location and set that direction as a way it cannot go
			// This is to help avoid infinite loops
			if (node.PreviousNode.TileX < node.TileX)
			{
				// Previous node was left of this one
				node.CanMoveLeft = false;
			}
			else if (node.PreviousNode.TileX > node.TileX)
			{
				// Previous node was right of this one
				node.CanMoveRight = false;
			}
			else if (node.PreviousNode.TileY < node.TileY)
			{
				// Previous node was above this one
				node.CanMoveUp = false;
			}
			else if (node.PreviousNode.TileY > node.TileY)
			{
				// Previous node was below this one
				node.CanMoveDown = false;
			}

			return node;
		}
	}
}
