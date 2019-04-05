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
using NewTestGame.Src.Scenes;
using NewTestGame.Src.Objects.Items;

namespace NewTestGame.Src
{
    /// <summary>
    /// 
    /// </summary>
    class Player : Entity
    {
        public enum Animations
        {
            standing,
            WalkingRight,
            jumping, 
            climbing
        }


        public  Sprite<Animations> playerSprite = new Sprite<Animations>();
        public Sprite staticPlayerSprite = new Sprite();
        public  SpriteAnimation walkingAnimation = new SpriteAnimation();
        public TiledMap map;
        public GraphicsDevice graphics;
        public int numCollisionLayers;
        public int numObjLayers;

        public Item weapon; //The player gets one weapon at a time

        public int health = 100;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="numCollisionLayers"></param>
        /// <param name="numObjLayers"></param>
        public Player(TiledMap map, int numCollisionLayers, int numObjLayers, GraphicsDevice graphics)
        {
            this.map = map;
            this.numCollisionLayers = numCollisionLayers;
            this.numObjLayers = numObjLayers;
            this.graphics = graphics;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onAddedToScene()
        {
            base.onAddedToScene();
            
            //Add a static sprite to the player for standing
            Subtexture tex = new Subtexture(this.scene.content.Load<Texture2D>("Sprites/StandingCharacter"));
            playerSprite.setRenderLayer(-99); //Make sure that the player sprite is always on top
            playerSprite.setSubtexture(tex);
            addComponent(playerSprite);


            //Set animation sprite from spritesheet
            Texture2D spriteSheet = this.scene.content.Load<Texture2D>("Sprites/WalkingSpriteSheet");
            int subSpriteWidth = 18;
            int subSpriteHeight = 23;
            //Goes through 4 times because there are 4 walking animation frames
            //Gets the sub sprite images out of the spritesheet
            for (int i = 0; i < 4; i++)
            {;
                Rectangle sourceRectangle = new Rectangle((i*subSpriteWidth), 0, subSpriteWidth, subSpriteHeight);
                Texture2D cropTexture = new Texture2D(graphics, sourceRectangle.Width, sourceRectangle.Height);
                Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                spriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);
                walkingAnimation.addFrame(new Subtexture(cropTexture));
            }
            playerSprite.addAnimation(Animations.WalkingRight, walkingAnimation);


            playerSprite.addAnimation(Animations.standing, new SpriteAnimation().addFrame(tex)); //Set the standing animation (right now it is just a single frame)

            //Add the controller and mover and add collision layers to the mover
            addComponent<PlayerController>();
            TiledMapMover mapMover = new TiledMapMover(map.getLayer<TiledTileLayer>("CollidableLayer1")); //Add the first collision layer
            for (int i = 1; i < numCollisionLayers; i++)
            {
                int num = i + 1;
                string layerName = "CollidableLayer" + num.ToString();
                mapMover.addCollisionLayer(map.getLayer<TiledTileLayer>(layerName));
            }
            addComponent(mapMover);
            addCollider(new BoxCollider(-tex.sourceRect.Width / 2, -tex.sourceRect.Height / 2, tex.sourceRect.Width, tex.sourceRect.Height));

            //Add an ObjectInteractor
            addComponent(new ObjectInteractor(this.scene, map, numObjLayers));

            //Add ItemHandler 
            addComponent(new ItemHandler());
        }


        /// <summary>
        /// 
        /// </summary>
        public void damage(int amount)
        {
            health -= amount;
        }
    }
}
