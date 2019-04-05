using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;
using Nez.Textures;

using Nez.UI;
using Nez.Audio;


using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Microsoft.Xna.Framework;
using System;

using NewTestGame.Src.Scenes.PremadeScenes;

namespace NewTestGame.Src.Scenes
{
    public class StartScreenUIScene : UIScene
    {
        public MainSceneManager manager;

        public Song s1;

        public StartScreenUIScene()
        {
            initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public void attachManager(MainSceneManager controlManager)
        {
            this.manager = controlManager;
        }


        public override void initialize()
        {
            s1 = content.Load<Song>("Sound/Music/WelcomeToTheJungle");

            addRenderer(new RenderLayerExcludeRenderer(0, 999));
            addRenderer(new ScreenSpaceRenderer(10, 999));


            var canvas = createEntity("ui")
                .addComponent(new UICanvas());
            canvas.setRenderLayer(999);

            var table = canvas.stage.addElement(new Table())
                              .setFillParent(true);
            table.defaults().setPadTop(20);

            var bar = new ProgressBar(0.0f, 1.0f, 0.01f, false, ProgressBarStyle.create(Color.Black, Color.White));
            table.add(bar);

            table.row();

            var slider = new Slider(0.0f, 1.0f, 0.01f, false, SliderStyle.create(Color.DarkGray, Color.LightYellow));
            table.add(slider);

            table.row();

            var button = new Button(ButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));

            button.onClicked += Button_onClicked;

            table.add(button).setMinWidth(100).setMinHeight(30);


            canvas.isFullScreen = true;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void activate()
        {
            base.activate();

            MediaPlayer.Volume = 0.3f;
            //MediaPlayer.Play(s1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void Button_onClicked(Button obj)
        {
            
            if (manager != null)
            {
                manager.switchToGameScene();
            }
        }


    }
}