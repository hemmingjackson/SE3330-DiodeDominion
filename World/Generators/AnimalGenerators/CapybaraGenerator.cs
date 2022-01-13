using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators.AnimalGenerators
{
	public class CapybaraGenerator : IGenerate
	{
		#region Fields

		/// <summary>
		/// Capybaras that are placed in the world
		/// </summary>
		private readonly IEnumerable<Capybara> capybaras;

		private const int minAnimalNumBoundary = 3;
		private const int maxAnimalNumBoundary = 11;
		private const int locationBufferToLeader = 80;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor to generate random number of capybaras
		/// </summary>
		/// <param name="random"> random number from 3 to 11</param>
		/// <param name="Capybaras"> list of Capybaras from Game World</param>
		public CapybaraGenerator(Random random, List<Capybara> Capybaras, MapTile[,] tiles)
		{
			List<Capybara> capybaras = new List<Capybara>();
			capybaras = Capybaras;
			int capyNum = random.Next(minAnimalNumBoundary, maxAnimalNumBoundary);

			for (int i = 0; i <capyNum; i++)
			{
				int addBuffer = random.Next(-locationBufferToLeader, locationBufferToLeader);
				capybaras.Add(new Capybara(Capybaras[0].EntitySprite.Origin.X + addBuffer, Capybaras[0].EntitySprite.Origin.Y + addBuffer, tiles));
			}

			this.capybaras = capybaras;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generate the capybaras for Game World
		/// </summary>
		/// <returns>Enumerable of the capybaras for the game world</returns>
		public IEnumerable<Entity> Generate()
		{
			return capybaras;
		}

		#endregion
	}
}
