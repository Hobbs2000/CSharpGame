using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTestGame.Src.Scenes
{
    public class GameScene : Scene
    {

        public GameScene()
        {
            clearColor = Color.Crimson;
            addRenderer(new DefaultRenderer());
        }


        public virtual void activate()
        {
        }


    }
}
