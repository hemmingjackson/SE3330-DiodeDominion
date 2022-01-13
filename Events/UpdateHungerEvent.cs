using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.Engine.Entities;
using Diode_Dominion.Engine.Events;
using Diode_Dominion.Engine.GameTimer;

namespace Diode_Dominion.DiodeDominion.Events
{
    public class UpdateHungerEvent : BaseEvent
    {
        #region Fields

        private const double RechargeDecrementTime = 10;
        public Colonist Colonist { get; }

        #endregion

        #region Constructors

        public UpdateHungerEvent(Colonist colonist)
        {
            Colonist = colonist;
        }

        #endregion

        public void UpdateColonistHunger()
        {
            if ((GameTimer.Time - GameTimer.GameStartTime) / RechargeDecrementTime > Colonist.PreviousTime)
            {
                if (Colonist.TimeTillRecharge != 0)
                {
                    UpdateRemainingHunger();
                }
                else
                {
                    UpdateRemainingHealth();
                }
            }
        }

        private void UpdateRemainingHunger()
        {
            Colonist.TimeTillRecharge -= 1;
            Colonist.PreviousTime++;
            if (Colonist.TimeTillRecharge / Colonist.MaxHealth < 0.5)
            {
                Colonist.Mood = MoodTypes.HUNGRY;

            }
        }

        private void UpdateRemainingHealth()
        {
            Colonist.TimeTillRecharge = 0;
            Colonist.Mood = MoodTypes.MURDEROUS;
            Colonist.Health -= 2;
            Colonist.PreviousTime++;
            if (Colonist.Health <= 0)
            {
                Colonist.Health = 0;
                Colonist.Alive = false;
            }
        }
    }
}