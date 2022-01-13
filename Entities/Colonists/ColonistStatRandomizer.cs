using System;
using Diode_Dominion.Engine.Entities;

namespace Diode_Dominion.DiodeDominion.Entities.Colonists
{
	public class ColonistStatRandomizer
	{
		#region Fields

		/// <summary>
		/// Max amount of health a colonist can start with
		/// </summary>
		private const int MaxHealth = 40;

		/// <summary>
		/// Max amount of health a colonist can start with
		/// </summary>
		private const int MinHealth = 30;

		/// <summary>
		/// The skill cap for the total amount of points a colonist can invest in 
		/// one single skill	
		/// </summary>
		private const int PointCap = 10;

		/// <summary>
		/// Total starting points for a colonist
		/// </summary>
		private const int NumberOfSkillPoints = 25;

		/// <summary>
		/// Used to pick a random number
		/// </summary>
		private Random rand;

		#endregion

		#region Properties

		public int ColonistPointsLeft;

		/// <summary>
		/// Holds the skills that a colonist will have
		/// </summary>
		public int [] ListOfSkills { get; set; }
		/// <summary>
		/// Name of the colonist
		/// </summary>
		
		public string NameOfColonist { get; set; }

		/// <summary>
		/// Health of colonist
		/// </summary>
		public int ColonistHealth { get; set; }

		#endregion

		#region Constructors

		public ColonistStatRandomizer(int seed)
		{
			rand = new Random(seed);
			ListOfSkills = new int[Enum.GetNames(typeof(SkillTypes)).Length];
			RandomizeStats();
		}
		/// <summary>
		/// Copy constructor for the colonist stat randomization
		/// </summary>
		/// <param name="randomizer"></param>
		public ColonistStatRandomizer(ColonistStatRandomizer randomizer)
		{
			ColonistHealth = randomizer.ColonistHealth;
			NameOfColonist = randomizer.NameOfColonist;
			ColonistPointsLeft = randomizer.ColonistPointsLeft;
			ListOfSkills = new int[Enum.GetNames(typeof(SkillTypes)).Length];
			rand = randomizer.rand;
			for(int i = 0; i < ListOfSkills.Length; i++ )
			{
				ListOfSkills[i] = randomizer.ListOfSkills[i];
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Where colonist names are being generated
		/// </summary>
		public void RandomizeStats()
		{
			for (int i = 0; i < ListOfSkills.Length; i++)
			{
				ListOfSkills[i] = 0;
			}

			NameOfColonist = GenerateName();
			for (int i = 0; i < NumberOfSkillPoints; i++)
			{
				bool skillPicked = false;
				while (!skillPicked)
				{
					int pickedSkill = rand.Next(ListOfSkills.Length);
					if (ListOfSkills[pickedSkill] < PointCap)
					{
						ListOfSkills[pickedSkill]++;
						skillPicked = true;
					}
				}

			}
			ColonistHealth = rand.Next(MinHealth, MaxHealth);
			ColonistPointsLeft = 0;
		}
		/// <summary>
		/// Creates a random name from a list of predefined first and last names
		/// </summary>
		/// <returns></returns>
		private string GenerateName()
		{
			return ColonistNames.FirstNames[rand.Next(ColonistNames.FirstNames.Length)] 
				+ " "
				+ ColonistNames.LastNames[rand.Next(ColonistNames.LastNames.Length)];
		}
		/// <summary>
		/// Returns a built colonist that has the current stats held in 
		/// this class
		/// </summary>
		/// <returns>Built colonist</returns>
		public Colonist CreateColonist()
		{
			IColonistBuilder builder = new ColonistBuilder();
			builder.WithName(NameOfColonist)
				.WithTotalHealth(ColonistHealth)
				.WithBuilding(ListOfSkills[(int)SkillTypes.BUILDING])
				.WithCooking(ListOfSkills[(int)SkillTypes.COOKING])
				.WithCrafting(ListOfSkills[(int)SkillTypes.CRAFTING])
				.WithDoctoring(ListOfSkills[(int)SkillTypes.DOCTORING])
				.WithHarvesting(ListOfSkills[(int)SkillTypes.HARVESTING])
				.WithMelee(ListOfSkills[(int)SkillTypes.MELEE])
				.WithMining(ListOfSkills[(int)SkillTypes.MINING]);
			return builder.Build();
		}
		/// <summary>
		/// Returns a random colonist object with with randomly generated stats and name
		/// </summary>
		/// <returns></returns>
		public Colonist GenerateColonist()
		{
			IColonistBuilder builder = new ColonistBuilder();
			RandomizeStats();
			builder.WithName(NameOfColonist)
				.WithTotalHealth(ColonistHealth)
				.WithBuilding(ListOfSkills[(int)SkillTypes.BUILDING])
				.WithCooking(ListOfSkills[(int)SkillTypes.COOKING])
				.WithCrafting(ListOfSkills[(int)SkillTypes.CRAFTING])
				.WithDoctoring(ListOfSkills[(int)SkillTypes.DOCTORING])
				.WithHarvesting(ListOfSkills[(int)SkillTypes.HARVESTING])
				.WithMelee(ListOfSkills[(int)SkillTypes.MELEE]);
			return builder.Build();
		}
		/// <summary>
		/// Sets the stats of a colonistRandomizer to the stats in the passed object
		/// </summary>
		/// <param name="randomizer"></param>
		public void SetStats(ColonistStatRandomizer randomizer)
		{
			ColonistHealth = randomizer.ColonistHealth;
			NameOfColonist = randomizer.NameOfColonist;
			ColonistPointsLeft = randomizer.ColonistPointsLeft;
			ListOfSkills = new int[Enum.GetNames(typeof(SkillTypes)).Length];
			rand = randomizer.rand;
			for (int i = 0; i < ListOfSkills.Length; i++)
			{
				ListOfSkills[i] = randomizer.ListOfSkills[i];
			}
		}
	}
	#endregion
}
