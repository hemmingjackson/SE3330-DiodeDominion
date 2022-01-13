namespace Diode_Dominion.DiodeDominion.Entities.Colonists
{
    public class ColonistBuilder : IColonistBuilder
    {

        #region Properties
        public string Name { get; set; }

        public int Health { get; set; }

        public int Building { get; set; }

        public int Cooking { get; set; }

        public int Crafting { get; set; }

        public int Doctoring { get; set; }

        public int Harvesting { get; set; }

        public int Melee { get; set; }

        public int Mining { get; set; }

        #endregion

        #region Methods
        public ColonistBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ColonistBuilder WithTotalHealth(int health)
        {
            Health = health;
            return this;
        }
        public ColonistBuilder WithBuilding(int building)
        {
            Building = building;
            return this;
        }

        public ColonistBuilder WithCooking(int cooking)
        {
            Cooking = cooking;
            return this;
        }

        public ColonistBuilder WithCrafting(int crafting)
        {
            Crafting = crafting;
            return this;
        }

        public ColonistBuilder WithDoctoring(int doctoring)
        {
            Doctoring = doctoring;
            return this;
        }

        public ColonistBuilder WithHarvesting(int harvesting)
        {
            Harvesting = harvesting;
            return this;
        }

        public ColonistBuilder WithMelee(int melee)
        {
            Melee = melee;
            return this;
        }

        public ColonistBuilder WithMining(int mining)
        {
            Mining = mining;
            return this;
        }

        public Colonist Build()
        {
            return new Colonist(this);
        }
        #endregion
    }
}
