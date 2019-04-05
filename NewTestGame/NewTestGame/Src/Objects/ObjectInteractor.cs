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

namespace NewTestGame.Src.Objects
{
    /// <summary>
    /// Added to player entity to check for interactions between player and object
    /// </summary>
    class ObjectInteractor : Component, IUpdatable
    {
        public BoxCollider playerCollider;
        //public List<Object> objects = new List<Object>();
        public ObjectParser objParser;
        public Scene scene;
        public bool onLadder = false;

        public ObjectInteractor(Scene scene, TiledMap map, int numObjLayers) : base()
        {
            this.scene = scene;

            //Add all the objects from every object layer
            List<TiledObjectGroup> objLayers = new List<TiledObjectGroup>();
            for (int i = 0; i < numObjLayers; i++)
            {
                int num = i + 1;
                string layerName = "ObjLayer" + num.ToString();
                objLayers.Add(map.getObjectGroup(layerName));
            }

            //Setup the parser for the objects
            this.objParser = new ObjectParser(scene, map, objLayers);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            this.playerCollider = entity.getCollider<BoxCollider>();
        }

        /// <summary>
        /// Checks for interactions between the player and Objects every frame
        /// </summary>
        public void update()
        {
            //This is for objects that can only be interacted with when the player presses the E key
            if (Input.isKeyPressed(Keys.E))
            {
                #region Chest interactions
                //Handle interaction with Chests
                for (int i = 0; i < objParser.getChests().Count; i++)
                {
                    if (playerCollider.overlaps(objParser.getChests()[i].getCollider<BoxCollider>()))
                        objParser.getChests()[i].interact();
                }
                #endregion
                #region Door interactions
                //Handles interaction with Doors
                for (int i = 0; i < objParser.getDoors().Count; i++)
                {
                    if (playerCollider.overlaps(objParser.getDoors()[i].getCollider<BoxCollider>()))
                        objParser.getDoors()[i].interact();
                }
                #endregion
            }

            #region Handle interacting with Ladders
            //When W is held down while in the area of a ladder, then move the player up
            if (Input.isKeyDown(Keys.W))
            {
                for (int i = 0; i < objParser.getLadders().Count; i++)
                {
                    if (playerCollider.overlaps(objParser.getLadders()[i].getCollider<BoxCollider>()))
                    {
                        //Move the player up
                        Vector2 move = new Vector2(0, -5);
                        entity.getComponent<TiledMapMover>().move(move, playerCollider, new TiledMapMover.CollisionState());
                        onLadder = true;
                        break;
                    }
                    onLadder = false;
                }
            }
            else
                onLadder = false;
            #endregion
        }

    }
}
