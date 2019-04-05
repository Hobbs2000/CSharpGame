using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.PhysicsShapes;
using NewTestGame.Src.Objects;
using NewTestGame.Src.Objects.Interactables;
using NewTestGame.Src.Scenes;

namespace NewTestGame.Src.Objects.Items
{
    class ItemHandler : Component, IUpdatable
    {

        /// <summary>
        /// This is meant to be added to a player
        /// It handles determing if a player interacts with an item
        /// Also handles what happens when they player interacts with it 
        /// </summary>
        public void update()
        {
            //Check every item currently in the scene and look for an interaction
            MasterScene mScene = entity.scene as MasterScene;
            for (int i = 0; i < mScene.items.Count; i++)
            {
                //Don't bother with it if it is already equipped by a player
                if ((mScene.items[i].state == Item.States.dropped) && (entity.getCollider<BoxCollider>().overlaps(mScene.items[i].getCollider<BoxCollider>()) && (mScene.items[i].spawnTime <= 0)))
                {
                    mScene.items[i].state = Item.States.equipped;
                    Player p = entity as Player;
                    p.weapon = mScene.items[i];
                    mScene.items[i].onEquipped();
                    break;
                }
            }
        }
    }
}
