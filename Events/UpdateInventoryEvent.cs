using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
    public class UpdateInventoryEvent: BaseEvent
    {
        public Colonist Colonist;
        public Item Item;

        public UpdateInventoryEvent(Colonist colonist, Item item)
        {
            Colonist = colonist;
            Item = item;
        }
    }
}
