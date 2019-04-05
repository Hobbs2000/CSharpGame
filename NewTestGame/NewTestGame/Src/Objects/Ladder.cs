using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;

namespace NewTestGame.Src.Objects
{
    /// <summary>
    /// 
    /// </summary>
    class Ladder : MapObject 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="scene"></param>
        public Ladder(TiledObject obj, Scene scene) : base(obj, scene)
        {
            type = ObjectType.ladder; //Set the object type to a door
        }
    }
}
