using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework.Graphics;
using NewTestGame.Src.Objects;
using Microsoft.Xna.Framework;
using NewTestGame.Src.Scenes;

namespace NewTestGame.Src.Objects.Items
{
    /// <summary>
    /// Items are equipable and un-equipable 
    /// Items can be dynamically destroyed and created 
    /// They are meant to interact with players and enemies
    /// Not based off the Map like Objects
    /// </summary>
    class Item : Entity
    {
        public int spawnTime = 0;
        protected int x_off, y_off; //The amount on screen that an item will be offset when a player has it equipped 

        /// <summary>
        /// 
        /// </summary>
        public enum States
        {
            equipped,
            dropped
        }
        public States state;

        public enum Types
        {
            weapon,
            passive
        }
        public Types type;

        public enum Actions
        {
            attack, 
            idle
        }
        public Actions currentAction;


        /// <summary>
        /// 
        /// </summary>
        public override void update()
        {
            base.update();

            //spawnTime is the amount of time that nothing can be done to the item
            if (spawnTime > 0)
            {
                spawnTime--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void onEquipped()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void onUnEquipped()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getXOffset()
        {
            return x_off;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getYOffset()
        {
            return y_off;
        }
    }
}
