using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators.AnimalGenerators
{
	public class OstrichGenerator : IGenerate
	{
		#region Fields

		/// <summary>
		/// Ostriches that are placed in the world
		/// </summary>
		private readonly IEnumerable<Ostrich> ostriches;

		private const int minAnimalNumBoundary = 3;
		private const int maxAnimalNumBoundary = 11;
		private const int locationBufferToLeader = 80;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor to generate random number of ostriches
		/// </summary>
		/// <param name="random"> random number from 3 to 11</param>
		/// <param name="Ostriches"> list of Ostriches from Game World</param>
		public OstrichGenerator(Random random, List<Ostrich> Ostriches, MapTile[,] tiles)
		{
			List<Ostrich> ostriches = new List<Ostrich>();
			ostriches = Ostriches;
			int ostNum = random.Next(minAnimalNumBoundary, maxAnimalNumBoundary);

			for (int i = 0; i < ostNum; i++)
			{
				int addBuffer = random.Next(-locationBufferToLeader, locationBufferToLeader);
				ostriches.Add(new Ostrich(Ostriches[0].EntitySprite.Origin.X + addBuffer, Ostriches[0].EntitySprite.Origin.Y + addBuffer, tiles));
			}

			this.ostriches = ostriches;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generate the ostriches for Game World
		/// </summary>
		/// <returns>Enumerable of the ostriches for the game world</returns>
		public IEnumerable<Entity> Generate()
		{
			return ostriches;
		}

		#endregion
	}
}
