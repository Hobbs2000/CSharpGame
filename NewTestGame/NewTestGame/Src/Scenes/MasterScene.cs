using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using MonoGame.Utilities;
using Nez;
using Nez.Tiled;
using Nez.Sprites;
using Nez.Textures;
using NewTestGame.Src.Objects;
using NewTestGame.Src.Enemies;
using Nez.UI;
using NewTestGame.Src.Utils;
using NewTestGame.Src.Objects.Items;

namespace NewTestGame.Src.Scenes
{
    class MasterScene : GameScene
    {
        public int numOfCollisionLayers;
        public int numOfObjLayers;
        public string mapName;
        public GraphicsDevice graphicsDevice;
        public TiledMap tileMap;

        public List<Item> items = new List<Item>();

        public bool settingsEnabled = false;

        public List<TiledObjectGroup> objectLayers = new List<TiledObjectGroup>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="totalNumOfLayers"></param>
        /// <param name="numOfCollisionLayers"></param>
        /// <param name="numOfObjLayers"></param>
        public MasterScene(GraphicsDevice graphicsDevice, string mapName, int numOfCollisionLayers, int numOfObjLayers)
        {
            this.graphicsDevice = graphicsDevice;
            this.numOfCollisionLayers = numOfCollisionLayers;
            this.numOfObjLayers = numOfObjLayers;
            this.mapName = mapName;


            /*I commented out the initialize call in the Scene source code 
            Because it was getting called by the base Scene constructor 
            Before the MasterScene constructor was called and this caused
            certain variables to not be set. So now I have to call it here :(  
            */
            initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void initialize()
        {

            clearColor = Color.LightSkyBlue;
            addRenderer(new DefaultRenderer());

            #region setup everything for Tiled Map
            tileMap = content.Load<TiledMap>(this.mapName);
            var tileEntity = createEntity("Tiled-Entity");
            //Add the collidable layers to the tile entity
            //Have to do this for all collision layers for physics to properly work
            for (int i = 0; i <= numOfCollisionLayers; i++)
            {
                int num = i + 1;
                string layerName = "CollidableLayer" + num.ToString(); //All layers that contain tiles that have collision must be named "CollidableLayer" followed by a number 
                tileEntity.addComponent(new TiledMapComponent(tileMap, layerName)); //                                               the numbers must be increasing sequentially 
            }
            //Fill the object layers list
            for (int i = 0; i < numOfObjLayers; i++)
            {
                int num = i + 1;
                string layerName = "ObjLayer" + num.ToString(); //All object layers must be named "ObjLayer" followed by a number. The numbers must be increasing sequentially
                objectLayers.Add(tileMap.getObjectGroup(layerName));
            }
            #endregion

            #region setup player and add to scene
            //Find the spawn point in the object layers list then set the players position to it
            TiledObject spawn = null;
            foreach (TiledObjectGroup objectLayer in objectLayers)
            {
                if (objectLayer.objectWithName("SpawnPoint") != null)
                {
                    spawn = objectLayer.objectWithName("SpawnPoint");
                }
            }
            //var player = createEntity("Player");
            Player player = new Player(tileMap, numOfCollisionLayers, numOfObjLayers, graphicsDevice);
            player.name = "Player"; //Set the name of the player so it can be easily found later
            addEntity(player);
            player.transform.setPosition(new Vector2(spawn.x, spawn.y));
            #endregion

            #region Add and setup a camera to follow the player
            //Add and setup camera
            var playerCamera = new FollowCamera(player, player.scene.camera);
            playerCamera.Y_offset = 0;
            player.addComponent(playerCamera);
            //Try to set deadzone of playerCamera
            Core.schedule(0.03f, t => playerCamera.setCenteredDeadzone(50, 50));
            #endregion

            #region Start the enemy spawner
            //Start spawning enemies with the enemy spawner
            EnemySpawner spawner = new EnemySpawner(tileMap, numOfCollisionLayers, numOfObjLayers, graphicsDevice);
            addEntity(spawner);
            #endregion

        }
    }
}
