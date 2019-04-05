using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework.Graphics;
using NewTestGame.Src.Objects;
using Microsoft.Xna.Framework;

namespace NewTestGame.Src
{
    /// <summary>
    /// 
    /// </summary>
    class Sword : Entity
    {
        public enum Animations
        {
            swing
        }

        public Sprite<Animations> swordSprite = new Sprite<Animations>();
        public SpriteAnimation swingAnimation = new SpriteAnimation();
        public GraphicsDevice graphics;

        public Sword(GraphicsDevice graphics)
        {
            this.graphics = graphics;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onAddedToScene()
        {
            base.onAddedToScene();

            
            //Set animation sprite from spritesheet
            Texture2D spriteSheet = this.scene.content.Load<Texture2D>("Sprites/DefaultSwordSpinningSpriteSheet");
            int subSpriteWidth = 11;
            int subSpriteHeight = 24;
            //Goes through 4 times because there are 4 walking animation frames
            //Gets the sub sprite images out of the spritesheet
            for (int i = 0; i < 4; i++)
            {
                ;
                Rectangle sourceRectangle = new Rectangle((i * subSpriteWidth), 0, subSpriteWidth, subSpriteHeight);
                Texture2D cropTexture = new Texture2D(graphics, sourceRectangle.Width, sourceRectangle.Height);
                Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                spriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);
                swingAnimation.addFrame(new Subtexture(cropTexture));
            }
            swordSprite.addAnimation(Animations.swing, swingAnimation);


            swordSprite.setRenderLayer(-10); //Make sure that the player sprite is always on top
            addComponent(swordSprite);

            swordSprite.play(Animations.swing);

        }
    }
}
