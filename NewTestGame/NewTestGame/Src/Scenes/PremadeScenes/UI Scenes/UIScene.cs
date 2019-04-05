using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTestGame.Src.Scenes
{
    public class UIScene : Scene
    {

        public virtual void activate()
        {
            //Scene.setDefaultDesignResolution(Screen.monitorWidth, Screen.monitorHeight, SceneResolutionPolicy.NoBorder);
            this.designResolutionSize = new Point(Screen.monitorWidth, Screen.monitorHeight);
        }
    }
}
