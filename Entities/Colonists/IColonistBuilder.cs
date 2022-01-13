namespace Diode_Dominion.DiodeDominion.Entities.Colonists
{
    public interface IColonistBuilder
    {
        ColonistBuilder WithName(string name);
        ColonistBuilder WithTotalHealth(int health);
        ColonistBuilder WithMining(int mining);
        ColonistBuilder WithBuilding(int building);
        ColonistBuilder WithMelee(int melee);
        ColonistBuilder WithHarvesting(int harvesting);
        ColonistBuilder WithCrafting(int crafting);
        ColonistBuilder WithCooking(int cooking);
        ColonistBuilder WithDoctoring(int doctoring);
        Colonist Build();
    }
}
