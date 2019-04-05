using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace NewTestGame.Src.Scenes
{
    public class MainSceneManager : SceneManager
    {
        public GameScene gameScene;
        public UIScene uiScene;

        public bool uiActive = false;

        int count = 1;

        public MainSceneManager(GameScene gameScene, UIScene uiScene, bool uiStartActive)
        {
            this.gameScene = gameScene;

            this.uiScene = uiScene;

            uiActive = uiStartActive;
            if (uiStartActive)
            {
                gameScene.pause();
                this.uiScene.activate();
                uiScene.unPause();
                Core.scene = uiScene;
            }
            else
            {
                uiScene.pause();
                this.gameScene.activate();
                gameScene.unPause();
                Core.scene = gameScene;
                count = 2;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void update()
        {
            base.update();

            if (Input.isKeyPressed(Keys.F1))
            {

                if (count % 2 == 0)
                {
                    gameScene.pause();
                    uiScene.unPause();
                    uiScene.activate();
                    Core.scene = uiScene;
                }
                else
                {
                    uiScene.pause();
                    gameScene.unPause();
                    gameScene.activate();
                    Core.scene = gameScene;
                }

                count++;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void switchToGameScene()
        {
            uiScene.pause();
            gameScene.unPause();
            gameScene.activate();
            Core.scene = gameScene;

            count = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public void switchToUIScene()
        {
            gameScene.pause();
            uiScene.unPause();
            uiScene.activate();
            Core.scene = uiScene;

            count = 3;
        }
    }
}
