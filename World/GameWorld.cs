using System;
using Diode_Dominion.DiodeDominion.Entities;
using System.Collections.Generic;
using Diode_Dominion.Engine.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Diode_Dominion.Engine.Camera;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Microsoft.Xna.Framework.Input;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.World.UI;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Entities.Items.Tools;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Trees;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.DiodeDominion.World.Generators;
using Diode_Dominion.DiodeDominion.World.Misc;
using Microsoft.Xna.Framework.Media;
using Diode_Dominion.DiodeDominion.Entities.Items.Weapons;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Ores;
using Diode_Dominion.Engine.MouseInformation;

namespace Diode_Dominion.DiodeDominion.World
{
	/// <summary>
	/// This class hold entities and the tile mapping for the game. It also includes
	/// the draw and update methods to update the game state.
	/// </summary>
	public class GameWorld : GameScreen
	{
		#region Fields
		private const int MinColonists = 3;
		/// <summary>
		/// Controls random generation in the game world
		/// </summary>
		private readonly Random random;

		/// <summary>
		/// Create a menu in the game world that can be used to interact
		/// and modify the game world
		/// </summary>
		private TileInteractions tileInteractionMenu;

		private readonly EntityEventHandler entityEventHandler;

		private readonly PauseGame pauseGame;

		/// <summary>
		/// Holds the previous mouse state to make certain that the mouse is clicked
		/// and released before completing an action
		/// </summary>
		private MouseState previousMouseState;
		private MouseState currentMouseState;

		/// <summary>
		/// Used to make certain textures have been set up properly
		/// </summary>
		private bool areTexturesSetup;

		// locations of items/static entities
		private const int AxeLocationX = 1200;
		private const int AxeLocationY = 700;
		private const int HoeLocationX = 900;
		private const int HoeLocationY = 300;
		private const int PickaxeLocationX = 1010;
		private const int PickaxeLocationY = 450;
		private const int HammerLocationX = 1150;
		private const int HammerLocationY = 586;
		private const int SpaceshipLocation = 0;
		private const int SwordLocationX = 1000;
		private const int TridentLocationX = 800;
		private const int LaserHandgunLocationX = 1500;
		private const int WeaponLocationY = 500;


		#endregion

		#region Properties

		/// <summary>
		/// Mapping of the tiles that make up the game world
		/// </summary>
		public TileMapping TileMap { get; } 
		/// <summary>
		/// Used to hold any stockpiles in game
		/// </summary>
		public List<ConglomerateStockpile> Stockpiles { get; }
		/// <summary>
		/// Holds selected tiles
		/// </summary>
		public List<MapTile> SelectedTiles { get; private set; }
		/// <summary>
		/// Holds entities in the game world
		/// </summary>
		public List<Entity> Entities { get; }

		/// <summary>
		/// Holds entities in the game world
		/// </summary>
		public List<Entity> Selected { get; set; }
		/// <summary>
		/// Colonist generator for adding new colonists
		/// </summary>
		public ColonistStatRandomizer ColonistGen { get; }
		/// <summary>
		/// Holds items in the game world
		/// </summary>
		public List<Item> Items
		{
			get
			{
				// Create the list
				List<Item> items = new List<Item>();

				// Loop through the entities looking for the items
				foreach (Entity entity in Entities)
				{
					// Check if the entity is a type of item
					if (entity is Item addItem)
					{
						items.Add(addItem);
					}
				}
				return items;
			}
		}

		/// <summary>
		/// Holds animals in the game world
		/// </summary>
		public List<Animal> Animals
		{
			get
			{
				// Create the list
				List<Animal> animals = new List<Animal>();

				// Loop through the entities looking for the animals
				foreach (Entity entity in Entities)
				{
					// Check if the entity is a type of animal
					if (entity is Animal addAnimal)
					{
						animals.Add(addAnimal);
					}
				}
				return animals;
			}
		}

		/// <summary>
		/// Holds animal leaders in the game world
		/// </summary>
		public List<Animal> AnimalLeaders
		{
			get
			{
				// Create the list
				List<Animal> animalLeaders = new List<Animal>();

				// Loop through the entities looking for the animals
				foreach (Entity entity in Entities)
				{
					// Check if the entity is a type of animal and is a leader
					if (entity is Animal addAnimal && addAnimal.IsLeader)
					{
						animalLeaders.Add(addAnimal);
					}
				}
				return animalLeaders;
			}
		}

		/// <summary>
		/// Holds static entities in the game world
		/// </summary>
		public List<StaticEntity> StaticEntities
		{
			get
			{
				List<StaticEntity> statics = new List<StaticEntity>();

				foreach (Entity entity in Entities)
				{
					if (entity is StaticEntity staticEntity)
					{
						statics.Add(staticEntity);
					}
				}

				return statics;
			}
		}

		/// <summary>
		/// Holds weapons in the game world
		/// </summary>
		public List<Weapon> Weapons
		{
			get
			{
				// Create the list
				List<Weapon> weapons = new List<Weapon>();

				// Loop through the entities looking for the items
				foreach (Entity entity in Entities)
				{
					// Check if the entity is a type of item
					if (entity is Weapon addWeapon)
					{
						weapons.Add(addWeapon);
					}
				}
				return weapons;
			}
		}

		/// <summary>
		/// Hold the background texture of the world
		/// </summary>
		public Texture2D Background { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Camera WorldCamera { get; set; }
		
		#endregion
		
		/// <summary>
		/// Creates a game world object that specifies the initial entities.
		/// </summary>
		/// 
		/// <param name="entities">Initial game entities</param>
		/// <param name="seed">Seed for the game world</param>
		public GameWorld(IEnumerable<Entity> entities, int seed)
		{
			Stockpiles = new List<ConglomerateStockpile>();
			Entities = new List<Entity>(entities);
			WorldCamera = new Camera();
			TileMap = new TileMapping(seed);
			Selected = new List<Entity>();
			ColonistGen = new ColonistStatRandomizer(seed);

			random = new Random(seed);
			
			// Move the entities to different parts of the screen to see them
			for (int i = 0; i < Entities.Count; i++)
			{
				Entities[i].EntitySprite.Origin = new Vector2(100 + 100 * i, Entities[i].EntitySprite.Y);
				Entities[i].BaseAi.SetMapTiles(TileMap.Tiles);
			}

			// Create the event pass-through
			entityEventHandler = new EntityEventHandler(this);

			// Create object that can pause/resume the game
			pauseGame = new PauseGame();
		}

		/// <summary>
		/// Initialize objects that may not be needed immediately upon object instantiation.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			WorldCamera = new Camera();
			// Have an initial "move" to avoid an issue where stuff does not render until it moves
			WorldCamera.MoveBounds(0, 0);
		}

		public override void LoadContent(ContentManager contentManager)
		{
			base.LoadContent(contentManager);
			Background = contentManager.Load<Texture2D>("Backgrounds/default_background");

			Tool axe = new Tool(ToolType.AXE)
			{
				EntitySprite =
				{
					Texture = contentManager.Load<Texture2D>(TextureLocalization.Tools + "Axe"),
					Origin = new Vector2(AxeLocationX, AxeLocationY)
				}
			};

			Tool hoe = new Tool(ToolType.HOE)
			{
				EntitySprite =
				{
					Texture = contentManager.Load<Texture2D>(TextureLocalization.Tools + "Hoe"),
					Origin = new Vector2(HoeLocationX, HoeLocationY)
				}
			};

			Tool pickaxe = new Tool(ToolType.PICKAXE)
			{
				EntitySprite =
				{
					Texture = contentManager.Load<Texture2D>(TextureLocalization.Tools + "Pickaxe"),
					Origin = new Vector2(PickaxeLocationX, PickaxeLocationY)
				}
			};

			Tool hammer = new Tool(ToolType.HAMMER)
			{
				EntitySprite =
				{
					Texture = contentManager.Load<Texture2D>(TextureLocalization.Tools + "Hammer"),
					Origin = new Vector2(HammerLocationX, HammerLocationY)
				}
			};

			// placing spaceship
			Spaceship spaceship = new Spaceship
			{
				EntitySprite =
				{
					Texture = contentManager.Load<Texture2D>(TextureLocalization.Spaceship), Origin = new Vector2(SpaceshipLocation, SpaceshipLocation)
				}
			};

			Sword sword = new Sword {EntitySprite = {Origin = new Vector2(SwordLocationX, WeaponLocationY) }};
			Trident trident = new Trident {EntitySprite = {Origin = new Vector2(TridentLocationX, WeaponLocationY) }};
			LaserHandgun laserHandgun = new LaserHandgun {EntitySprite = {Origin = new Vector2(LaserHandgunLocationX, WeaponLocationY) }};

			Entities.Add(axe);
			Entities.Add(hoe);
			Entities.Add(pickaxe);
			Entities.Add(hammer);

			Entities.Add(spaceship);

			Entities.Add(sword);
			Entities.Add(trident);
			Entities.Add(laserHandgun);

			foreach (Weapon weapon in Weapons)
			{
				weapon.LoadContent(contentManager);
			}

			TileMap.LoadContent(contentManager);

			// Add the background music into the game
			Engine.Sounds.SoundManager.Instance.AddBackgroundMusic(contentManager.Load<Song>("Sound/Background/ElectroFever"));
			Engine.Sounds.SoundManager.Instance.AddBackgroundMusic(contentManager.Load<Song>("Sound/Background/EightBitAdventure"));
			Engine.Sounds.SoundManager.Instance.AddBackgroundMusic(contentManager.Load<Song>("Sound/Background/Distortion"));
			Engine.Sounds.SoundManager.Instance.AddBackgroundMusic(contentManager.Load<Song>("Sound/Background/DistortionOverdrive"));
			
			// Add all of the initial generated entities to the game world
			Entities.AddRange(new EntityGameWorldGenerator().GenerateEntities(TileMap.Tiles, random));

			// Set the tree textures
			foreach (StaticEntity staticEntity in StaticEntities)
			{
				if (staticEntity is Tree treeTextureSet)
				{
					treeTextureSet.EntitySprite.Texture = 
						contentManager.Load<Texture2D>(TextureLocalization.SampleTree);
				}
			}
			foreach (StaticEntity staticEntity in StaticEntities)
			{
				if (staticEntity is Ore oreTextureSet)
				{
					oreTextureSet.EntitySprite.Texture =
						contentManager.Load<Texture2D>(TextureLocalization.Ore);
				}
			}

			// Update the AIs so they know about the game world
			foreach (Entity entity in Entities)
			{
				entity.BaseAi.SetMapTiles(TileMap.Tiles);
			}
		}

		/// <summary>
		/// Update various objects that are part of the game world
		/// </summary>
		/// 
		/// <param name="gameTime"></param>B
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			UserInterfaceManager.Instance.Update(gameTime);

			pauseGame.Update();

			// Update the stored state of the mouse
			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

			// Check that the mouse was clicked and the it is not intersecting a UI
			if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed &&
			    !UserInterfaceManager.Instance.MouseIntersecting())
			{
				Vector2 worldPosition = Settings.MouseTransformLocation;
				
				// Reversed for loop so that the entity that is most on top is the one that is clicked
				for (int i = Entities.Count - 1; i >= 0; i--)
				{
					// Check for entity intersecting with the mouse
					if (Entities[i].EntitySprite.Bounds.Contains(worldPosition))
					{
						// Create the UI
						IUserInterface userInterface = Entities[i] is Item ? 
							EntityUiFactory.CreateUserInterface(Stockpiles, Entities, Entities[i]) : 
							EntityUiFactory.CreateUserInterface(Entities, Entities[i]);
						// Only display if a valid UI could be created
						userInterface?.Open();

						// Force i out of bounds
						i = -1;
					}
				}

				//Iterates through all conglomerate stockpiles
				foreach (StockpileZone zone in Stockpiles.SelectMany(pile => pile.StockpileZones))
				{
					//Iterate through all stockpile items to get location
					foreach (Item stockpileItem in zone.StockpileItems)
					{
						if(stockpileItem != null && stockpileItem.EntitySprite.Bounds.Contains(worldPosition))
						{
							// Create the UI
							IUserInterface userInterface = EntityUiFactory.CreateUserInterface(Entities, stockpileItem);

							// Only display if a valid UI could be created
							userInterface?.Open();
							break;
						}
					}
				}
			}
			
			// Update the map tiles
			MouseSelection.Update();
			TileMap.Update(gameTime);
			SelectedTiles = TileMap.ObtainSelectedTiles();
			// Update the tile interaction menu

			tileInteractionMenu?.Update(gameTime);


			// Allow the camera to move
			WorldCamera.Update();

			// Code that should not execute when the game is paused should go within here
			if (Settings.IsGameActive)
			{
				// Allow the AIs for each entity to update themselves
				foreach (Entity entityGroup in Entities.ToList())
				{
					if (entityGroup is Animal animal && animal.Alive == false)
					{
						Meat meatItem = new Meat(animal.AnimalType)
						{
							EntitySprite = {Origin = animal.EntitySprite.Origin}
						};
						Entities.Add(meatItem);
						animal.EntitySprite.Origin = new Vector2(-5000, -5000);
					}
					entityGroup.Update();
				}
			}

			int colonistCount = Entities.Where((t, i) => Entities.ElementAt(i) is Colonist).Count();
			if (colonistCount < MinColonists)
				Entities.Add(ColonistGen.GenerateColonist());

			// variable used to verify highest bound for animals
			int largestAnimalId = Animals.Select(animal => animal.Id).Concat(new[] {0}).Max();

			// saves id's of each of the leaders
			foreach (Animal animalGroup in Animals.Where(animalGroup => animalGroup.IsLeader))
			{
				if (animalGroup.Health == 0) // sets new leader if previous leader died
				{
					animalGroup.IsLeader = false;
					foreach (Animal animal in 
						Animals.Where(animal => animal.AnimalType == animalGroup.AnimalType && animal.Health > 0))
					{
						animal.IsLeader = true;
						animal.LeaderRandomNumber = animalGroup.LeaderRandomNumber;
						animal.SetLeader(animal.Id, largestAnimalId);
						break;
					}
				}
				else
				{
					animalGroup.SetLeader(animalGroup.Id, largestAnimalId);
				}
			}

			// sets leader for each of the animals, if not a leader
			foreach (Animal animal in Animals)
			{
				foreach (Animal leader in 
					AnimalLeaders.Where(leader => !animal.IsLeader && animal.AnimalType == leader.AnimalType))
				{
					animal.SetLeader(leader.Id, largestAnimalId);
				}
			}

			foreach (Animal animalGroup in Animals)
			{
				if (animalGroup.IsLeader && !animalGroup.HasTask) // leader movement
				{
					const int maxLocationX = 4900;
					const int maxLocationY = 4900;

					int animalMoveLocationX = animalGroup.LeaderRandomNumber.Next(0, maxLocationX);
					int animalMoveLocationY = animalGroup.LeaderRandomNumber.Next(0, maxLocationY);
					entityEventHandler.Update(new MoveEntityEvent(animalGroup.Id, animalMoveLocationX, animalMoveLocationY));
				}
				else if (!animalGroup.HasTask)
				{
					int arrayId = 0;
					for (int j = 0; j < Animals.Count; j++)
					{
						if (Animals[j].Id == Animals[j].LeaderId && Animals[j].AnimalType == animalGroup.AnimalType)
						{
							arrayId = j;
							break;
						}
					}
					(float x, float y) = animalGroup.Update(animalGroup, Animals[arrayId]);
					entityEventHandler.Update(new MoveEntityEvent(animalGroup.Id, x, y));
				}
			}
		}

		/// <summary>
		/// This method draws the sprites onto the screen at specified game time.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			// Check to see if textures have been set up
			if (!areTexturesSetup)
			{
				areTexturesSetup = true;
				SetupTextures();
			}

			// Draw stuff that should move with the camera here
			spriteBatch.Begin(transformMatrix: WorldCamera.Transform);

			// Draw the world tiles
			TileMap.Draw(gameTime, spriteBatch);

			// Draw the entities on the screen
			foreach (Entity entity in Entities)
			{
				entity?.Draw(gameTime, spriteBatch);
			}
			//Draw items in stockpile
			foreach (ConglomerateStockpile stockpile in Stockpiles)
			{
				stockpile.Draw(gameTime, spriteBatch);
			}

			UserInterfaceManager.Instance.Draw(gameTime, spriteBatch);

			spriteBatch.End();

			// Draw stuff that does not move with the camera here
			spriteBatch.Begin();

			// Draw the tile menu for the game world
			tileInteractionMenu.Draw(gameTime, spriteBatch);

			spriteBatch.End();
		}

		/// <summary>
		/// Sets up the textures so that they can be drawn to the screen
		/// </summary>
		private void SetupTextures()
		{
			tileInteractionMenu = new TileInteractions(this);

			// Load the menu items for the tile interaction menu
			tileInteractionMenu.LoadContent(Settings.Content);
		}
	}
}
