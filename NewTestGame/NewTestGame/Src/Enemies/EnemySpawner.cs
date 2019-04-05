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
using NewTestGame.Src.Enemies.Ground_Spawn;


namespace NewTestGame.Src.Enemies
{
    /// <summary>
    /// Will spawn enemies
    /// Has to be an entity so it can be added to a scene (there might be a better way for updatable things to be directly added to the scene)
    /// </summary>
    class EnemySpawner : Entity
    {
        public TiledMap map;
        public GraphicsDevice graphics;
        public int numCollisionLayers;
        public int numObjLayers;
        public List<TiledObject> objs = new List<TiledObject>();


        /// <summary>
        /// 
        /// </summary>
        public EnemySpawner(TiledMap map, int numCollisionLayers, int numObjLayers, GraphicsDevice graphics)
        {
            this.map = map;
            this.numCollisionLayers = numCollisionLayers;
            this.numObjLayers = numObjLayers;
            this.graphics = graphics;

            //Get all the objects
            for (int i = 0; i < numCollisionLayers; i++)
            {
                int num = i + 1;
                string layerName = "ObjLayer" + num.ToString();
                TiledObjectGroup group = map.getObjectGroup(layerName);
                if (group.getObjectsOfType("SpawnGround") != null)
                {
                    foreach (TiledObject obj in group.getObjectsOfType("SpawnGround"))
                    {
                        objs.Add(obj);
                    }
                }


            }
        }


        /// <summary>
        /// Will randomly add enemies 
        /// </summary>
        public override void update()
        {
            base.update();
            if (this.scene != null)
            {
                //if chance is true, then start process of creating an enemy
                if (Nez.Random.chance(.05f))
                {
                    Enemy enemy = new Enemy(map, numCollisionLayers, numObjLayers, graphics);

                    TiledObject spawnObj = null;

                    for (int i = 0; i < numCollisionLayers; i++)
                    {
                        int num = i + 1;
                        string layerName = "ObjLayer" + num.ToString();
                        TiledObjectGroup group = map.getObjectGroup(layerName);
                        //Gets the Obj to spawn at (Not correct right now because it will just end up grabbing a random SpawnGround obj from the LAST obj layer, when it should be a random layer)
                        spawnObj = group.getObjectsOfType("SpawnGround")[Nez.Random.range(0, group.getObjectsOfType("SpawnGround").Count)];
                    }

                    int xSpawn = Nez.Random.range(spawnObj.x, spawnObj.x + spawnObj.width);
                    int ySpawn = (int)(spawnObj.y) - 1;

                    //Finally move the enemy to its psuedo spawn position (entity has to move itself to the correct spawn based off its collider)
                    enemy.transform.setPosition(new Vector2(xSpawn, ySpawn));

                    this.scene.addEntity(enemy);

                }
            }

            
        }


    }
}
