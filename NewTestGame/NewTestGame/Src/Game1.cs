using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using NewTestGame.Src.Scenes.PremadeScenes;
using NewTestGame.Src.Scenes;
using Nez.UI;
using NewTestGame.Src.Scenes.PremadeScenes.UI_Scenes;

namespace NewTestGame.Src
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Core
    {

        public Game1() : base(Screen.monitorWidth, Screen.monitorHeight)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();

            Scene.setDefaultDesignResolution(Screen.monitorWidth, Screen.monitorHeight, Scene.SceneResolutionPolicy.NoBorder);

            StartScreenUIScene startSceneUI = new StartScreenUIScene();
            TestScene2 gameScene = new TestScene2(Core.graphicsDevice);

            SettingsUI settings = new SettingsUI();
            GameScene testGameScene = new GameScene();

            MainSceneManager mainManager = new MainSceneManager(gameScene, startSceneUI, false);



            startSceneUI.attachManager(mainManager);

            this.sceneManager = mainManager;



            Screen.isFullscreen = true;
        }

    }
}
