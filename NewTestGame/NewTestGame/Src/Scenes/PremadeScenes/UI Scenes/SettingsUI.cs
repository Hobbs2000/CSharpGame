using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTestGame.Src.Scenes.PremadeScenes.UI_Scenes
{
    class SettingsUI : UIScene
    {

        public SettingsUI()
        {
            clearColor = Color.LightSkyBlue;
            addRenderer(new DefaultRenderer());
        }

    }
}


