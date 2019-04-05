using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using NewTestGame.Src.Objects.Interactables.Chests;
using NewTestGame.Src.Objects.Interactables.Doors;

namespace NewTestGame.Src.Objects
{
    /// <summary>
    /// Goes through the map and parses the objects into correct derived object classes
    /// Whenever a new object is created for the game, a get method for a list of those objects must be put here
    /// This also holds the different objects
    /// </summary>
    class ObjectParser : Component
    {
        private List<TiledObjectGroup> objLayers;
        private TiledMap map;
        private Scene scene;

        //All the objects that can be retrieved
        List<Chest> chests = new List<Chest>();
        List<Door> doors = new List<Door>();
        List<Ladder> ladders = new List<Ladder>();

        /// <summary>
        /// Gets all the objects of type chest and returns a list of chests
        /// </summary>
        /// <param name="map"></param>
        /// <param name="numCollisionLayers"></param>
        public ObjectParser(Scene scene, TiledMap map, List<TiledObjectGroup> objLayers)
        {
            this.scene = scene;
            this.map = map;
            this.objLayers = objLayers;

            initialize();
        }

        /// <summary>
        /// Parses all the objects in the map into the current derived object classes
        /// </summary>
        private void initialize()
        {
            //Goes through every object layer and extracts each object and creates an appropriate object based from it
            foreach (TiledObjectGroup objLayer in objLayers)
            {
                #region Gets Chest objects
                //Setup the chests
                if (objLayer.getObjectsOfType("Chest") != null)
                {
                    List<TiledObject> objects = objLayer.getObjectsOfType("Chest");
                    foreach (TiledObject baseObj in objects)
                        chests.Add(new Chest(baseObj, scene));
                }
                #endregion
                #region Gets Door objects
                //Setup the doors
                if (objLayer.getObjectsOfType("Door") != null)
                {
                    List<TiledObject> objects = objLayer.getObjectsOfType("Door");
                    foreach (TiledObject baseObj in objects)
                    {
                        doors.Add(new Door(baseObj, scene));
                    }
                }
                #endregion
                #region Gets Ladder objects
                //Setup the ladders
                if (objLayer.getObjectsOfType("Ladder") != null)
                {
                    List<TiledObject> objects = objLayer.getObjectsOfType("Ladder");
                    foreach (TiledObject baseObj in objects)
                    {
                        ladders.Add(new Ladder(baseObj, scene));
                    }
                }
                #endregion
            }


        }

        /// <summary>
        /// Returns the list of Chest objects
        /// </summary>
        /// <returns></returns>
        public List<Chest> getChests()
        {
            return chests;
        }

        /// <summary>
        /// Returns the list of Door objects
        /// </summary>
        /// <returns></returns>
        public List<Door> getDoors()
        {
            return doors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Ladder> getLadders()
        {
            return ladders;
        }

    }
}
