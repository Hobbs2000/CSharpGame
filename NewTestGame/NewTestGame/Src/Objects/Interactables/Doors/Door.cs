using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using Nez.Textures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;

namespace NewTestGame.Src.Objects.Interactables.Doors
{
    class Door : MapObject
    {
        public enum DoorState
        {
            open, 
            closed
        }
        public DoorState state = DoorState.open;

        public Sprite openDoor = new Sprite();
        public Sprite closedDoor = new Sprite();


        public Door(TiledObject obj, Scene scene) : base(obj, scene)
        {
            type = ObjectType.door; //Set the object type to a door
        }

        public override void initialize()
        {
            base.initialize();

            //Load in and add the closedDoor sprite
            Subtexture closedTex = new Subtexture(scene.content.Load<Texture2D>("Sprites/closedDoor"));
            closedDoor.setSubtexture(closedTex);
            closedDoor.setEnabled(true);
            addComponent(closedDoor);
            closedDoor.setRenderLayer(0);
            closedDoor.transform.position = new Vector2(obj.x + obj.width / 2, (obj.y + obj.height / 2) + 1);
            getCollider<BoxCollider>().setLocalOffset(new Vector2(0, 0));//                  add one here ^ because it is up by for some reason otherwise (moves this down by one in game pixel)

            //Load in and add the openDoor sprite
            //Starts off disabled because the door starts closed
            Subtexture openTex = new Subtexture(scene.content.Load<Texture2D>("Sprites/openDoor"));
            openDoor.setSubtexture(openTex);
            openDoor.setEnabled(false);
            addComponent(openDoor);
            openDoor.setRenderLayer(0); 
            openDoor.transform.position = new Vector2(obj.x + obj.width / 2, (obj.y + obj.height / 2) + 1);
            getCollider<BoxCollider>().setLocalOffset(new Vector2(0, 0));//                add one here ^ because it is up by for some reason otherwise (moves this down by one in game pixel)
        }

        public override void interact()
        {
            base.interact();

            if (state == DoorState.closed)
            {
                state = DoorState.open;
                openDoor.setEnabled(true);
                closedDoor.setEnabled(false);
            }
            else if (state == DoorState.open)
            {
                state = DoorState.closed;
                openDoor.setEnabled(false);
                closedDoor.setEnabled(true);
            }
        }
    }
}
