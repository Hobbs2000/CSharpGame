using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;


namespace NewTestGame.Src.Objects
{

    /// <summary>
    /// A MapObject is an object from the map. They are mostly static in terms of positioning and have set starting positions
    /// Different from an item 
    /// Example: A ladder
    /// NOT an example: A sword
    /// </summary>
    class MapObject : Entity
    {
        //All the different types of objects
        public enum ObjectType
        {
            chest,
            door,
            ladder
        }

        public ObjectType type;
        public TiledObject obj;
        public Scene scene;

        public MapObject(TiledObject obj, Scene scene)
        {
            this.obj = obj;
            this.scene = scene;
            this.transform.setPosition(new Vector2(obj.x, obj.y));
            initialize();
        }

        public virtual void initialize()
        {
            addCollider(new BoxCollider(0, 0, obj.width, obj.height));
            this.attachToScene(scene);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void interact()
        {

        }

    }
}
