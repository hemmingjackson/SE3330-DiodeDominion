using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators.AnimalGenerators
{
	public class GooseGenerator : IGenerate
	{
		#region Fields

		/// <summary>
		/// Geese that are placed in the world
		/// </summary>
		private readonly IEnumerable<Goose> geese;

		private const int minAnimalNumBoundary = 3;
		private const int maxAnimalNumBoundary = 11;
		private const int locationBufferToLeader = 80;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor to generate random number of geese
		/// </summary>
		/// <param name="random"> random number from 3 to 11</param>
		/// <param name="Geese"> list of Geese from Game World</param>
		public GooseGenerator(Random random, List<Goose> Geese, MapTile[,] tiles)
		{
			List<Goose> geese = new List<Goose>();
			geese = Geese;
			int gooNum = random.Next(minAnimalNumBoundary, maxAnimalNumBoundary);

			for (int i = 0; i < gooNum; i++)
			{
				int addBuffer = random.Next(-locationBufferToLeader, locationBufferToLeader);
				geese.Add(new Goose(Geese[0].EntitySprite.Origin.X + addBuffer, Geese[0].EntitySprite.Origin.Y + addBuffer, tiles));
			}

			this.geese = geese;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generate the geese for Game World
		/// </summary>
		/// <returns>Enumerable of the geese for the game world</returns>
		public IEnumerable<Entity> Generate()
		{
			return geese;
		}

		#endregion
	}
}
