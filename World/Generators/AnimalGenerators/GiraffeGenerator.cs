using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators.AnimalGenerators
{
	public class GiraffeGenerator : IGenerate
	{
		#region Fields

		/// <summary>
		/// Giraffes that are placed in the world
		/// </summary>
		private readonly IEnumerable<Giraffe> giraffes;

		private const int minAnimalNumBoundary = 3;
		private const int maxAnimalNumBoundary = 11;
		private const int locationBufferToLeader = 80;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor to generate random number of giraffes
		/// </summary>
		/// <param name="random"> random number from 3 to 11</param>
		/// <param name="Giraffes"> list of Giraffes from Game World</param>
		public GiraffeGenerator(Random random, List<Giraffe> Giraffes, MapTile[,] tiles)
		{
			List<Giraffe> giraffes = new List<Giraffe>();
			giraffes = Giraffes;
			int girNum = random.Next(minAnimalNumBoundary, maxAnimalNumBoundary);

			for (int i = 0; i < girNum; i++)
			{
				int addBuffer = random.Next(-locationBufferToLeader, locationBufferToLeader);
				giraffes.Add(new Giraffe(Giraffes[0].EntitySprite.Origin.X + addBuffer, Giraffes[0].EntitySprite.Origin.Y + addBuffer, tiles));
			}

			this.giraffes = giraffes;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generate the giraffes for Game World
		/// </summary>
		/// <returns>Enumerable of the giraffes for the game world</returns>
		public IEnumerable<Entity> Generate()
		{
			return giraffes;
		}

		#endregion
	}
}
