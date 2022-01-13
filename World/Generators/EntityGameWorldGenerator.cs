
using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.World.Generators.AnimalGenerators;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators
{
	internal class EntityGameWorldGenerator
	{
        #region Fields

        private const int AnteaterStartLocationX = 250, AnteaterStartLocationY = 2890;
		private const int CapybaraStartLocationX = 4000, CapybaraStartLocationY = 3958;
		private const int GiraffeStartLocationX = 3000, GiraffeStartLocationY = 1640;
		private const int GooseStartLocationX = 1000, GooseStartLocationY = 364;
		private const int OstrichStartLocationX = 2137, OstrichStartLocationY = 4000;

        #endregion

        #region Methods

        internal IEnumerable<Entity> GenerateEntities(MapTile[,] mapTiles, Random random)
		{
			// Create the lists for the animals
			List<Entity> entities = new List<Entity>();
			List<Anteater> anteaters = new List<Anteater>();
			List<Capybara> capybaras = new List<Capybara>();
			List<Giraffe> giraffes = new List<Giraffe>();
			List<Goose> geese = new List<Goose>();
			List<Ostrich> ostriches = new List<Ostrich>();

			// Add the animal pack leaders
			anteaters.Add(new Anteater(AnteaterStartLocationX, AnteaterStartLocationY, true, random, mapTiles));
			capybaras.Add(new Capybara(CapybaraStartLocationX, CapybaraStartLocationY, true, random, mapTiles));
			giraffes.Add(new Giraffe(GiraffeStartLocationX, GiraffeStartLocationY, true, random, mapTiles));
			geese.Add(new Goose(GooseStartLocationX, GooseStartLocationY, true, random, mapTiles));
			ostriches.Add(new Ostrich(OstrichStartLocationX, OstrichStartLocationY, true, random, mapTiles));

			// Add all of the generated animals
			entities.AddRange(new AnteaterGenerator(random, anteaters, mapTiles).Generate());
			entities.AddRange(new CapybaraGenerator(random, capybaras, mapTiles).Generate());
			entities.AddRange(new GiraffeGenerator(random, giraffes, mapTiles).Generate());
			entities.AddRange(new GooseGenerator(random, geese, mapTiles).Generate());
			entities.AddRange(new OstrichGenerator(random, ostriches, mapTiles).Generate());

			// Load all of the animals textures
			foreach (Entity entity in entities)
			{
				if (entity is Animal animal)
				{
					animal.LoadContent(Settings.Content);
				}
			}

			// Create all of the trees for the map
			entities.AddRange(new TreeGenerator(mapTiles, random).Generate());

			entities.AddRange(new OreGenerator(mapTiles, random).Generate());

			return entities;
		}

        #endregion
    }
}
