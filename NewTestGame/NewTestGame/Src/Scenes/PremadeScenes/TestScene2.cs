using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using NewTestGame.Src.Scenes;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Nez;

namespace NewTestGame.Src.Scenes.PremadeScenes
{
    /// <summary>
    /// A premade scene that uses the Tiled map TestMap2
    /// </summary>
    class TestScene2 : MasterScene
    {
        Song s1;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public TestScene2(GraphicsDevice graphicsDevice) : base(graphicsDevice, "Tiled Maps/TestMap2", 1, 1)
        {
            s1 = content.Load<Song>("Sound/Music/Chiptune Stronger than you");
        }

        
        public override void activate()
        {
            base.activate();

            //setDefaultDesignResolution(600, 400, SceneResolutionPolicy.NoBorder);
            this.designResolutionSize = new Point(600, 400);

            MediaPlayer.Volume = .1f;
            //MediaPlayer.Play(s1);
        }
    }
}
