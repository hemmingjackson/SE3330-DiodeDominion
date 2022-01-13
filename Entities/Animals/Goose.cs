﻿using Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs;
using Microsoft.Xna.Framework;
using System;
using Diode_Dominion.DiodeDominion.World.Tiles;
using System.Collections.Generic;

namespace Diode_Dominion.DiodeDominion.Entities.Animals
{
    /// <summary>
    /// Type of animal entities that roam the world and can be interacted
    /// with by colonists
    /// </summary>
    public class Goose : Animal
    {
        #region Fields

        /// <summary>
        /// Added distance between animal and its leader
        /// </summary>
        private const int LocationBuffer = 45;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates animal using location
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="tiles"></param>
        public Goose(float x, float y, MapTile[,] tiles) : base(AnimalType.GOOSE, x, y, tiles)
        {
            EntitySprite.Origin = new Vector2(x, y);
            MovementSpeed = 0.25f;
            BaseAi = new AnimalAI(this, tiles);
        }

        /// <summary>
        /// Creates animal using location, if leader or not, and randomNumber previously generated
        /// This constructor is generally used in creating the leaders.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="value">true or false depending on if isLeader</param>
        /// <param name="random">random number previously generated, used for movement</param>
        /// <param name="tiles"></param>
        public Goose(float x, float y, bool value, Random random, MapTile[,] tiles) : base(AnimalType.GOOSE, x, y, tiles)
        {
            EntitySprite.Origin = new Vector2(x, y);
            MovementSpeed = 0.25f;
            this.LeaderRandomNumber = random;
            this.IsLeader = value;
            BaseAi = new AnimalAI(this, tiles);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates entity origin to follow its leader
        /// </summary>
        /// <param name="animal">follower animal</param>
        /// <param name="leader">leader animal</param>
        /// <returns>leader's origin</returns>
        public override Vector2 Update(Animal animal, Animal leader)
        {
            return new Vector2(leader.EntitySprite.Origin.X + LocationBuffer, leader.EntitySprite.Origin.Y + LocationBuffer);
        }

        #endregion
    }
}