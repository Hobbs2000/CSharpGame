using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Nez.Textures;
using NewTestGame.Src.Objects.Items;
using NewTestGame.Src.Scenes;

namespace NewTestGame.Src.Objects.Interactables.Chests
{
    /// <summary>
    /// 
    /// </summary>
    class Chest : MapObject
    {
        public enum State
        {
            closed,
            open
        }
        public State state = State.closed;

        public Sprite openChest = new Sprite();
        public Sprite closedChest = new Sprite();
        public bool untouched = true;


        public Chest(TiledObject obj, Scene scene) : base(obj, scene)
        {
            type = ObjectType.chest; //Set the object type to a chest
        }

        public Chest(MapObject baseObj) : this(baseObj.obj, baseObj.scene)
        {
        }

        public override void initialize()
        {
            base.initialize();

            //Load in and add the closedDoor sprite
            Subtexture closedTex = new Subtexture(scene.content.Load<Texture2D>("Sprites/closedChest"));
            closedChest.setSubtexture(closedTex);
            closedChest.setEnabled(true);
            addComponent(closedChest);
            closedChest.setRenderLayer(0);
            closedChest.transform.position = new Vector2(obj.x + obj.width / 2, (obj.y + obj.height / 2));
            getCollider<BoxCollider>().setLocalOffset(new Vector2(0, 0));

            //Load in and add the openChest sprite
            //Starts off disabled because the chest starts closed
            Subtexture tex = new Subtexture(scene.content.Load<Texture2D>("Sprites/OpenChest"));
            openChest.setSubtexture(tex);
            openChest.setEnabled(false);
            addComponent(openChest);
            openChest.setRenderLayer(0);
            openChest.transform.position = new Vector2(obj.x + obj.width / 2, (obj.y + obj.height / 2));
            getCollider<BoxCollider>().setLocalOffset(new Vector2(0, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void interact()
        {
            if (state == State.closed)
            {
                state = State.open;
                openChest.setEnabled(true);
                closedChest.setEnabled(false);

                //This is testing spawining an item when the player first opens a chest
                if (untouched == true)
                {
                    Sword sword = new Sword(Core.graphicsDevice);
                    sword.transform.position = new Vector2(this.transform.position.X, this.transform.position.Y-4);
                    scene.addEntity(sword);

                    //Add the sword to the list of items in the MasterScene
                    MasterScene mScene = scene as MasterScene;
                    mScene.items.Add(sword);

                    untouched = false;
                }
            }
            else if (state == State.open)
            {
                state = State.closed;
                openChest.setEnabled(false);
                closedChest.setEnabled(true);
            }
        }
    }
}
