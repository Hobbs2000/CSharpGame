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
using NewTestGame.Src.Enemies.Ground_Spawn;

namespace NewTestGame.Src.Objects.Items
{
    /// <summary>
    /// 
    /// </summary>
    class Sword : Item
    {
        public enum Animations
        {
            idle,
            held,
            swing
        }

        public Sprite<Animations> swordSprite = new Sprite<Animations>();
        public SpriteAnimation idleAnimation = new SpriteAnimation();
        public SpriteAnimation swingAnimation = new SpriteAnimation();
        public GraphicsDevice graphics;
        ObjectMapController controller;

        protected int ATKDURATION = 10;
        protected int atkTimer = 0;
        protected int dmg = 50;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        public Sword(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            type = Types.weapon;
            currentAction = Actions.idle;

            x_off = 3;
            y_off = 1;
        }

        /// <summary>
        /// Does all the stuff needed when essentially spawning in
        /// </summary>
        public override void onAddedToScene()
        {
            base.onAddedToScene();

            state = States.dropped;

            MasterScene mScene = this.scene as MasterScene;

            swordSprite.setRenderLayer(-9);
            addComponent(swordSprite);

            Rectangle sourceRectangle;
            Texture2D cropTexture;
            Color[] data;

            #region Set Idle Animation
            //Set idle animation sprite from spritesheet
            Texture2D spriteSheet = this.scene.content.Load<Texture2D>("Sprites/DefaultSwordSpinningSpriteSheet");
            int subSpriteWidth = 11;
            int subSpriteHeight = 24;
            //Goes through 4 times because there are 4 walking animation frames
            //Gets the sub sprite images out of the spritesheet
            for (int i = 0; i < 4; i++)
            {
                sourceRectangle = new Rectangle((i * subSpriteWidth), 0, subSpriteWidth, subSpriteHeight);
                cropTexture = new Texture2D(graphics, sourceRectangle.Width, sourceRectangle.Height);
                data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                spriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);
                idleAnimation.addFrame(new Subtexture(cropTexture));
            }
            swordSprite.addAnimation(Animations.idle, idleAnimation);
            #endregion

            //Then play the idle animation because it just spawned in
            swordSprite.play(Animations.idle);


            Texture2D swingSpriteSheet = this.scene.content.Load<Texture2D>("Sprites/DefaultSwordSpriteSheet");
            subSpriteWidth = 22;
            subSpriteHeight = 24;
            #region Set Held Animation
            //Add the sprite animation for the sword being held
            sourceRectangle = new Rectangle(0, 0, subSpriteWidth, subSpriteHeight);
            cropTexture = new Texture2D(graphics, sourceRectangle.Width, sourceRectangle.Height);
            data = new Color[sourceRectangle.Width * sourceRectangle.Height];
            swingSpriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
            cropTexture.SetData(data);
            //Subtexture heldSprite = new Subtexture(this.scene.content.Load<Texture2D>("Sprites/DefaultSword"));
            swordSprite.addAnimation(Animations.held, new SpriteAnimation().addFrame(new Subtexture(cropTexture)));
            #endregion

            #region Set Swing animation
            //Add the swinging animation
            //Goes through 4 times because there are 4 walking animation frames
            //Gets the sub sprite images out of the spritesheet for the swinging
            for (int i = 0; i < 4; i++)
            {
                sourceRectangle = new Rectangle((i * subSpriteWidth), 0, subSpriteWidth, subSpriteHeight);
                cropTexture = new Texture2D(graphics, sourceRectangle.Width, sourceRectangle.Height);
                data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                swingSpriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);
                cropTexture.SetData(data);
                swingAnimation.addFrame(new Subtexture(cropTexture));
            }
            swingAnimation.setLoop(false);
            swingAnimation.setFps(20);
            swordSprite.addAnimation(Animations.swing, swingAnimation);
            #endregion

            #region Add the controller so the object can move with physics
            //Add the controller and mover and add collision layers to the mover
            TiledMapMover mapMover = new TiledMapMover( mScene.tileMap.getLayer<TiledTileLayer>("CollidableLayer1")); //Add the first collision layer
            for (int i = 1; i < mScene.numOfCollisionLayers ; i++)
            {
                int num = i + 1;
                string layerName = "CollidableLayer" + num.ToString();
                mapMover.addCollisionLayer(mScene.tileMap.getLayer<TiledTileLayer>(layerName));
            }
            addComponent(mapMover);

            //Add controller
            controller = new ObjectMapController(1500);
            addComponent(controller);
            #endregion 
            addCollider(new BoxCollider(-swordSprite.width / 2, -swordSprite.height / 2, swordSprite.width, swordSprite.height));

            //Do little bounce when spawning in
            controller.bounce(20);

            spawnTime = 25;
        }


        List<Entity> alreadyAtkd = new List<Entity>();
        /// <summary>
        /// 
        /// </summary>
        public override void update()
        {
            base.update();

            if (swordSprite.isPlaybackDone() && atkTimer <= 0)
            {
                currentAction = Actions.idle;
                swordSprite.play(Animations.held);
            }


            if (atkTimer > 0)
            {

                MasterScene mScene = scene as MasterScene;
                List<Entity> allEnemies = mScene.entities.entitiesOfType<Enemy>();
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (state == States.equipped && type == Types.weapon && //The item has to be equipped and a weapon
                        !alreadyAtkd.Contains(allEnemies[i]) &&             //Will not attack an enemy that has already been attacked during this attack sequence  
                        allEnemies[i].getCollider<BoxCollider>() != null && //Check to see if the enemies collider is null
                        currentAction == Item.Actions.attack &&             //The items current action has to be attack  
                        getCollider<BoxCollider>().overlaps(allEnemies[i].getCollider<BoxCollider>())) //Finally check to see if the weapons collider overlaps the enemies
                    {
                        alreadyAtkd.Add(allEnemies[i]); //Add this enemy to the alreadyAtkd list so it is not hurt duing this sequence again
                        //View the enemy as a type of Enemy not just Entity
                        Enemy enm = allEnemies[i] as Enemy;
                        enm.rmvHealth(dmg);
                    }
                }
                atkTimer--;
            }

        }

        /// <summary>
        /// Goes through every entity that is of type entity and 
        /// checks to see if it comes into contact with the sword
        /// </summary>
        public void attack()
        {
            if (atkTimer <= 0) //Only attack if not currently attacking
            {
                alreadyAtkd.Clear();
                atkTimer = ATKDURATION; //Since attacking takes time, the timer is set 
                currentAction = Actions.attack; //Set the action
                swordSprite.play(Sword.Animations.swing); //Play the attack animation for this sword
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void onEquipped()
        {
            base.onEquipped();

            controller.setEnabled(false);
            swordSprite.setRenderLayer(-100);
            swordSprite.play(Sword.Animations.held);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onUnEquipped()
        {
            base.onUnEquipped();
        }


    }
}
