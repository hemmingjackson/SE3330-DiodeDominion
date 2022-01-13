using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Textures;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.Entities.Items
{
    public sealed class Meat : Item
    {
        #region Fields
        public AnimalType FoodType { get; }
        public double HealthValue { get; }
        #endregion

        #region Constructors
        public Meat(AnimalType animalType)
        {
            FoodType = animalType;
            ItemType = ItemType.FOOD;
            HealthValue = 10.0;
            LoadContent(Settings.Content);
        }
        #endregion

        public void LoadContent(ContentManager content)
        {
	        if (FoodType == AnimalType.ANTEATER)
	        {
		        EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.AnteaterMeat);
	        }
	        else if (FoodType == AnimalType.CAPYBARA)
	        {
		        EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.CapybaraMeat);
	        }
	        else if (FoodType == AnimalType.GIRAFFE)
	        {
		        EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.GiraffeMeat);
	        }
	        else if (FoodType == AnimalType.GOOSE)
	        {
		        EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.GooseMeat);
	        }
	        else if (FoodType == AnimalType.OSTRICH)
	        {
		        EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.OstrichMeat);
	        }
	        else
	        {
	        }
        }
    }
}
