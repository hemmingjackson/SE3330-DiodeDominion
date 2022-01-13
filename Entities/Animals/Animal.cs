using Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Diode_Dominion.DiodeDominion.World.Tiles;
using System.Collections.Generic;

namespace Diode_Dominion.DiodeDominion.Entities.Animals
{
    /// <summary>
    /// Animal entities that roam the world and can be interacted
    /// with by colonists
    /// </summary>
    public class Animal : Entity
    {
        #region Properties

        /// <summary>
        /// Type the animal is
        /// </summary>
        public AnimalType AnimalType { get; set; }

        /// <summary>
        /// True if the animal is a herd leader
        /// </summary>
        public Boolean IsLeader { get; set; }

        /// <summary>
        /// Id of the leader of the herd (5 herd for each animal type)
        /// </summary>
        public int LeaderId { get; set; }

        /// <summary>
        /// Random number for each herd leader used to all have different
        /// paths
        /// </summary>
        public Random LeaderRandomNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates animal using type and location
        /// </summary>
        /// <param name="animalType">Type of animal</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        //public Animal(AnimalType animalType, float x, float y)
        public Animal(AnimalType animalType, float x, float y, MapTile[,] tiles)
        {
            AnimalType = animalType;
            this.Name = animalType.ToString();
            EntitySprite.Origin = new Vector2(x, y);
            MovementSpeed = 0.1f;
            BaseAi = new AnimalAI(this, tiles);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the leader of the herd
        /// </summary>
        /// <param name="id">id of the leader</param>
        public virtual void SetLeader(int id, int animalListCount)
        {
            if (id <= 0 || id > animalListCount + 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                this.LeaderId = id;
            }
        }

        /// <summary>
        /// Loads correct sprite for entity
        /// </summary>
        /// <param name="content">Content manager that can import textures</param>
        public virtual void LoadContent(ContentManager content)
        {
            EntitySprite = new Sprite(EntitySprite.Origin, content.Load<Texture2D>(TextureLocalization.Animals +
                this.Name[0] + this.Name.ToString().Substring(1).ToLowerInvariant()));
        }

        /// <summary>
        /// Updates entity origin to follow its leader
        /// </summary>
        /// <param name="animal">follower animal</param>
        /// <param name="leader">leader animal</param>
        /// <returns>leader's origin</returns>
        public virtual Vector2 Update(Animal animal, Animal leader)
        {
            return leader.EntitySprite.Origin;
        }

        #endregion
    }
}