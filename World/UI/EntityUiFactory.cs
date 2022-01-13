using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.Items.Weapons;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Used to instantiate the proper type of user interface
	/// for a specific entity type
	/// </summary>
	public class EntityUiFactory
	{
		/// <summary>
		/// Private constructor to follow the simple factory idiom pattern
		/// </summary>
		private EntityUiFactory() { }

		/// <summary>
		/// Constructor for the creation of UIs that require stockpile information
		/// </summary>
		/// <param name="stockpile">Stockpiles in game</param>
		/// <param name="entities">Entities in game</param>
		/// <param name="entity">Entity UI is focused on</param>
		/// <returns></returns>
		public static IUserInterface CreateUserInterface(List<ConglomerateStockpile> stockpile, List<Entity> entities, Entity entity)
		{
			IUserInterface userInterface = null;

			bool duplicateEntry = false;

			foreach (IUserInterface instanceUserInterface in UserInterfaceManager.Instance.UserInterfaces)
			{
				if (instanceUserInterface is EntityUi entityUi && entityUi.Entity.Equals(entity))
				{
					duplicateEntry = true;
				}
			}

			if (!duplicateEntry)
			{
				if (entity is Item item)
				{
					if (item is Meat meat)
					{
						userInterface = new MeatUi(meat);
					}
					else
					{
						userInterface = new ItemInfoUi(stockpile, entities, item);
					}
				}
				else if (entity is Crop crop)
				{
					userInterface = new CropUi(entities, crop);
				}
				else if (entity is StaticEntity staticEntity)
				{
					userInterface = new StaticEntityUi(entities, staticEntity);
				}
				else if (entity is Colonist colonist)
				{
					userInterface = new ColonistInfoUi(colonist);
				}
				else if (entity is Animal animal)
				{
					userInterface = new AnimalInfoUi(entities, animal);
				}
			}

			return userInterface;
		}

		public static IUserInterface CreateUserInterface(List<Entity> entities, Entity entity)
		{
			IUserInterface userInterface = null;

			bool duplicateEntry = false;

			foreach (IUserInterface instanceUserInterface in UserInterfaceManager.Instance.UserInterfaces)
			{
				if (instanceUserInterface is EntityUi entityUi && entityUi.Entity.Equals(entity))
				{
					duplicateEntry = true;
				}
			}

			if (!duplicateEntry)
			{
				if (entity is Item item)
				{
					if (item is Meat meat)
                    {
						userInterface = new MeatUi(meat);
					}
                    else
                    {
						userInterface = new ItemInfoUi(entities, item);
					}
				}
				else if (entity is Crop crop)
				{
					userInterface = new CropUi(entities, crop);
				}
				else if (entity is StaticEntity staticEntity)
				{
					userInterface = new StaticEntityUi(entities, staticEntity);
				}
				else if (entity is Colonist colonist)
				{
					if (colonist.Alive)
					{
						userInterface = new ColonistInfoUi(colonist);
					}
					else
					{
						userInterface = new ColonistDeadUi(colonist);
					}
				}
				else if (entity is Animal animal)
				{
					userInterface = new AnimalInfoUi(entities, animal);
				}
			}

			return userInterface;
		}
	}
}
