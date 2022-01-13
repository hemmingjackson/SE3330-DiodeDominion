using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.Items.Weapons;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.DiodeDominion.World;
using Diode_Dominion.Engine.Entities;
using Diode_Dominion.Engine.GameTimer;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Type of action to deposit an item into a stockpile
	/// </summary>
	internal class Fight : IAction
	{
        #region Fields

        private const int RATIO_CHANCE_WEAK_MELEE = 3;
        private const int RATIO_CHANCE_AVERAGE_MELEE = 6;
        private const int MAX_HP_DAMAGE = 2;
        private const double LOW_DAMAGE = 0.4;
        private const double MEDIUM_DAMAGE = 0.6;
        private const double HIGH_DAMAGE = 0.8;
        private const float WAIT_TIME = 0.1f;
        public Entity Attacker { get; }
        public Entity Target { get; set; }

        public Weapon Weapon { get; set; }
        public double Damage { get; set; }
        private Random RandomNum { get; set; }

        private Melee Melee { get; set; }
        public bool Success { get; set; }
        private float FightStartTime { get; set; }
        #endregion

        #region Constructors
        /// <summary>
		/// Used to create a Fight action instance.
		/// </summary>
		/// <param name="attacker">The entity object that is attacking the target</param>
		/// <param name="target">The target entity that is has to fight off the attacker</param>
        /// /// <param name="weapon">The weapon that the current attacker is weilding</param>
		/// <param name="previousTime">A float that represents a GameTimer time for updating purposes</param>
		/// <param name="damage">An optional parameter used to set the initial damage at the beginning of a fight</param>
        public Fight(Entity attacker, Entity target, Weapon weapon, float previousTime, double damage = 0)
        {
            Attacker = attacker;
            Target = target;
            Weapon = weapon;
            Damage = damage;
            RandomNum = new Random();
            Success = false;
            FightStartTime = previousTime;
            Melee = new Melee(Target.Health, Weapon);
        }

        #endregion
        /// <summary>
		/// Used as the main method in the Fight class.  Each round of the fight
        /// is handled by this method.
		/// </summary>
        public void DetermineDamage()
        {
            //Resets the sprite for each entity back to normal (not taking damage).
            UpdateSprite(Attacker);
            UpdateSprite(Target);
            //The attacker is given the opportunity to inflict damage on the target
            AttackerAttemptAttack();
            //If the attacker's attempt is unsuccessful, the target is given the opportunity to retaliate.
            if (Success == false)
            {
                TargetRetaliateAttack();
            }
        }

        /// <summary>
		/// This method is used to handle the attacker's actions in the current round of the fight.
		/// </summary>
        private void AttackerAttemptAttack()
        {
            Success = false;
            double num = RandomNum.NextDouble();
            if (Attacker is Colonist)
            {
                //Handles an attacker that is not a very experienced fighter
                if (((Colonist)Attacker).Skills.SkillAndLevel[SkillTypes.MELEE] <= RATIO_CHANCE_WEAK_MELEE)
                {
                    if (num < LOW_DAMAGE)
                    {
                        if (Weapon != null)
                        {
                            Damage = Melee.CalculateTargetEntityDamage();
                        }
                        else
                        {
                            Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                        }
                        Target.Health -= Damage;
                        Success = true;
                        UpdateSpriteHurt(Target);
                    }
                }
                //Handles an attacker that ihas some fighting experience
                else if (((Colonist)Attacker).Skills.SkillAndLevel[SkillTypes.MELEE] <= RATIO_CHANCE_AVERAGE_MELEE)
                {
                    if (num < MEDIUM_DAMAGE)
                    {
                        if (Weapon != null)
                        {
                            Damage = Melee.CalculateTargetEntityDamage();
                        }
                        else
                        {
                            Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                        }
                        Target.Health -= Damage;
                        Success = true;
                        UpdateSpriteHurt(Target);
                    }
                }
                //Handles an expert fighter
                else
                {
                    if (num < HIGH_DAMAGE)
                    {
                        if (Weapon != null)
                        {
                            Damage = Melee.CalculateTargetEntityDamage();
                        }
                        else
                        {
                            Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                        }
                        Target.Health -= Damage;
                        Success = true;
                        UpdateSpriteHurt(Target);
                    }
                }
            }
            //Handles any entity that is not a colonist
            else
            {
                Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                Attacker.Health -= Damage;
                Success = true;
                UpdateSpriteHurt(Target);
            }
            IsDead(Target);
        }

        private void TargetRetaliateAttack()
        {
            double num = RandomNum.NextDouble();
            if (Attacker is Colonist)
            {
                //Handles retaliation against a weak fighter
                if (((Colonist)Attacker).Skills.SkillAndLevel[SkillTypes.MELEE] <= RATIO_CHANCE_WEAK_MELEE)
                {
                    if (num < 0.4)
                    {
                        Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                        Attacker.Health -= Damage;
                        UpdateSpriteHurt(Attacker);
                    }
                }
                //Handles retaliation against an average fighter
                else if (((Colonist)Attacker).Skills.SkillAndLevel[SkillTypes.MELEE] <= RATIO_CHANCE_AVERAGE_MELEE)
                {
                    if (num < 0.6)
                    {
                        Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                        Attacker.Health -= Damage;
                        UpdateSpriteHurt(Attacker);
                    }
                }
                //Handles retaliation against an experienced fighter
                else
                {
                    if (num < 0.8)
                    {
                        Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                        Attacker.Health -= Damage;
                        UpdateSpriteHurt(Attacker);
                    }
                }
            }
            //Handles retaliation against a non-colonist
            else
            {
                Damage = RandomNum.Next(0, MAX_HP_DAMAGE);
                Attacker.Health -= Damage;
                UpdateSpriteHurt(Attacker);
            }
            IsDead(Attacker);
        }

        /// <summary>
		/// Determines whether or not the current entity can still fight or if they are dead.
		/// </summary>
		/// 
		/// <param name="entity">Entity whos status is being determined</param>
        private void IsDead(Entity entity)
        {
            if (entity.Health <= 0 && entity.Alive)
            {
                entity.Health = 0;
                entity.Alive = false;
            }
        }

        /// <summary>
		/// Updates the sprite of the entity to reflect damage being inflicted
		/// </summary>
		/// 
		/// <param name="entity">Entity whos status is being determined</param>
        private void UpdateSpriteHurt(Entity entity)
        {
            if (entity is Animal)
            {
                switch (((Animal)entity).AnimalType)
                {
                    case AnimalType.ANTEATER:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.AnteaterHurt);
                            break;
                        }
                    case AnimalType.CAPYBARA:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.CapybaraHurt);
                            break;
                        }
                    case AnimalType.GIRAFFE:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.GiraffeHurt);
                            break;
                        }
                    case AnimalType.GOOSE:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.GooseHurt);
                            break;
                        }
                    case AnimalType.OSTRICH:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.OstrichHurt);
                            break;
                        }
                    default: break;
                }
            }
            else
            {
                entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.ColonistHurt);
            }
            
        }

        /// <summary>
        /// Updates the sprite of the entity to reflect the beginning of a new fight round (Returns sprite back to normal)
        /// </summary>
        /// 
        /// <param name="entity">Entity whos status is being determined</param>
        private void UpdateSprite(Entity entity)
        {
            if (entity is Animal)
            {
                switch (((Animal)entity).AnimalType)
                {
                    case AnimalType.ANTEATER:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.Animals + "Anteater");
                            break;
                        }
                    case AnimalType.CAPYBARA:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.Animals + "Capybara");
                            break;
                        }
                    case AnimalType.GIRAFFE:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.Animals + "Giraffe");
                            break;
                        }
                    case AnimalType.GOOSE:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.Animals + "Goose");
                            break;
                        }
                    case AnimalType.OSTRICH:
                        {
                            entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.Animals + "Ostrich");
                            break;
                        }
                    default: break;
                }
            }
            else
            {
                entity.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.ColonistSprite);
            }
        }

        /// <summary>
        /// Updates the sprite of the entity to reflect the beginning of a new fight round (Returns sprite back to normal)
        /// </summary>
        ///
        /// <returns>Returns whether or not both targets are still alive.  If yes, the fight continues. If not, the fight is over.</returns>
        public bool Update()
        {
            if (GameTimer.Time - FightStartTime > WAIT_TIME)
            {
                FightStartTime = GameTimer.Time;
                DetermineDamage();
                if (Attacker.Alive && Target.Alive)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
