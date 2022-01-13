using System;
using Microsoft.Xna.Framework;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Handle moving the entity from one location to another
	/// </summary>
	public class Move : IAction
	{
		/// <summary>
		/// Entity that should move
		/// </summary>
		private readonly Entity entity;

		/// <summary>
		/// X location to move towards
		/// </summary>
		private float xLocation;

		/// <summary>
		/// X location to move towards
		/// </summary>
		private float yLocation;

		/// <summary>
		/// How far away the entity can be before it is considered arriving at it's destination.
		/// Defaults to 10.0
		/// </summary>
		public float DestinationArrivalDistance { get; set; } = 10f;

		/// <summary>
		/// Obtain the destination that the entity is moving towards
		/// </summary>
		public Vector2 Destination => new Vector2(xLocation, yLocation);

		/// <summary>
		/// Create a Move object that handles moving
		/// the entity from one location to another.
		/// </summary>
		/// 
		/// <param name="entity">Entity that should move</param>
		public Move(Entity entity)
		{
			this.entity = entity;
		}

		/// <summary>
		/// Handle moving the entity closer to it's destination
		/// </summary>
		/// 
		/// <returns>Whether the move action is complete</returns>
		public bool Update()
		{
			// Move the entity and then set the new location
			(float x, float y) = MoveEntity();
			entity.EntitySprite.OriginSet(x, y);

			return CalculateDistanceFromDestination() <= DestinationArrivalDistance;
		}

		/// <summary>
		/// Location that the entity should be moved to
		/// </summary>
		/// 
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		public void SetLocation(float x, float y)
		{
			xLocation = x;
			yLocation = y;
		}

		/// <summary>
		/// Handle moving the entity closer to the desired location
		/// </summary>
		private Vector2 MoveEntity()
		{
			return new Vector2(UpdatePositionX(), UpdatePositionY());
		}

		/// <summary>
		/// Handle moving the entity along the x-axis
		/// </summary>
		/// 
		/// <returns>Updated X location of the entity</returns>
		private float UpdatePositionX()
		{
			return entity.EntitySprite.Origin.X + 
			       CalculateMovement(entity.EntitySprite.Origin.X, xLocation);
		}
		
		/// <summary>
		/// Handle moving the entity along the y-axis
		/// </summary>
		/// 
		/// <returns>Updated Y location of the entity</returns>
		private float UpdatePositionY()
		{
			return entity.EntitySprite.Origin.Y + 
			       CalculateMovement(entity.EntitySprite.Origin.Y, yLocation);
		}
		
		/// <summary>
		/// Calculates how much the entity will move based on the passed in parameters
		/// </summary>
		/// 
		/// <param name="entityLocation">Location of the entity</param>
		/// <param name="destinationLocation">Location the entity is moving towards</param>
		/// 
		/// <returns>How much the entity will move to get closer to destination location</returns>
		private float CalculateMovement(float entityLocation, float destinationLocation)
		{
			float movement = 0f;

			if (entityLocation < destinationLocation)
			{
				movement = entity.MovementSpeed;
			}
			else if (entityLocation > destinationLocation)
			{
				movement = -entity.MovementSpeed;
			}

			return movement;
		}

		/// <summary>
		/// Handle calculating how far from the destination location
		/// the entity is
		/// </summary>
		/// 
		/// <returns>Distance from destination location</returns>
		private double CalculateDistanceFromDestination()
		{
			double distanceFromObject = 0f;

			// Add X/Y distance
			distanceFromObject += Math.Pow(xLocation - entity.EntitySprite.Origin.X, 2);
			distanceFromObject += Math.Pow(yLocation - entity.EntitySprite.Origin.Y, 2);
			distanceFromObject = Math.Sqrt(distanceFromObject);
			
			return distanceFromObject;
		}
	}
}
