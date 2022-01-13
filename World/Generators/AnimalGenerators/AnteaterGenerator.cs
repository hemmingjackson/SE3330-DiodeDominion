using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators.AnimalGenerators
{
    public class AnteaterGenerator : IGenerate
    {
        #region Fields

        /// <summary>
		/// Anteaters that are placed in the world
		/// </summary>
        private readonly IEnumerable<Anteater> anteaters;

        private const int minAnimalNumBoundary = 3;
        private const int maxAnimalNumBoundary = 11;
        private const int locationBufferToLeader = 80;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to generate random number of anteaters
        /// </summary>
        /// <param name="random"> random number from 3 to 11</param>
        /// <param name="Anteaters"> list of Anteaters from Game World</param>
        public AnteaterGenerator(Random random, List<Anteater> Anteaters, MapTile[,] tiles)
		{
			List<Anteater> anteaters = new List<Anteater>();
            anteaters = Anteaters;
			int antNum = random.Next(minAnimalNumBoundary, maxAnimalNumBoundary);

			for (int i = 0; i < antNum; i++)
			{
				int addBuffer = random.Next(-locationBufferToLeader, locationBufferToLeader);
				anteaters.Add(new Anteater(Anteaters[0].EntitySprite.Origin.X + addBuffer, Anteaters[0].EntitySprite.Origin.Y + addBuffer, tiles));
			}

			this.anteaters = anteaters;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Generate the anteaters for Game World
        /// </summary>
        /// <returns>Enumerable of the anteaters for the game world</returns>
        public IEnumerable<Entity> Generate()
        {
			return anteaters;
        }

        #endregion
    }
}
