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

namespace NewTestGame.Src.Enemies.Ground_Spawn
{
    /// <summary>
    /// 
    /// </summary>
    class Enemy : Entity
    {
        public enum Animations
        {
            WalkingRight,
            jumping,
            climbing
        }


        public Sprite<Animations> enemySprite = new Sprite<Animations>();
        public TiledMap map;
        public GraphicsDevice graphics;
        public int numCollisionLayers;
        public int numObjLayers;
        public TiledMapMover mapMover;
        GroundEnemyController enemyController;
       

        public int attack = 5;
        protected int currHealth = 100;
        protected int prevHealth = 100;
        protected int recoilX = 300;
        protected int recoilY = 30;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="numCollisionLayers"></param>
        /// <param name="numObjLayers"></param>
        /// <param name="graphics"></param>
        public Enemy(TiledMap map, int numCollisionLayers, int numObjLayers, GraphicsDevice graphics)
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

            //Add a static sprite to the enemy for standing
            Subtexture tex = new Subtexture(this.scene.content.Load<Texture2D>("Sprites/DefaultSmallEnemy"));
            enemySprite.setRenderLayer(-9); 
            enemySprite.setSubtexture(tex);
            addComponent(enemySprite);

            //Add the physics collider
            BoxCollider mainCollider = new BoxCollider(-tex.sourceRect.Width / 2, -tex.sourceRect.Height / 2, tex.sourceRect.Width, tex.sourceRect.Height);
            addCollider(mainCollider);

            //Attack area
            //This is the box around the enemy that if a player is in it then they will actively attack the player
            int xMult = 15;
            int yMult = 7;
            BoxCollider attackCollider = new BoxCollider((-tex.sourceRect.Width*xMult)/2, (-tex.sourceRect.Height*yMult)/2, tex.sourceRect.Width * xMult, tex.sourceRect.Height * yMult);
            addCollider(attackCollider);

            Vector2 updatedPos = new Vector2(transform.position.X, transform.position.Y - mainCollider.height/2);
            transform.setPosition(updatedPos);

            //Create and add the mover
            mapMover = new TiledMapMover(map.getLayer<TiledTileLayer>("CollidableLayer1")); //Add the first collision layer
            for (int i = 1; i < numCollisionLayers; i++)
            {
                int num = i + 1;
                string layerName = "CollidableLayer" + num.ToString();
                mapMover.addCollisionLayer(map.getLayer<TiledTileLayer>(layerName));
            }
            addComponent(mapMover);

            //Add enemy controller
            enemyController = new GroundEnemyController(mapMover, mainCollider, attackCollider);
            addComponent(enemyController);

        }

        /// <summary>
        /// Allows for currHealth to be removed by other classes
        /// </summary>
        /// <param name="n"></param>
        public void rmvHealth(int n)
        {
            this.currHealth -= n;
        }
        /// <summary>
        /// Allows for currHealth to be added by other classes
        /// </summary>
        /// <param name="n"></param>
        public void addHealth(int n)
        {
            this.currHealth += n;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void update()
        {
            base.update();

            if (this.currHealth < this.prevHealth) //Check if the enemy has lost health since the last update
            {
                if (enemyController != null)
                {
                    enemyController.bounce(recoilY);
                    Entity plyr = this.scene.findEntity("Player");
                    if (this.transform.position.X < plyr.transform.position.X)
                        enemyController.rmvXVelocity(recoilX);                    
                    else
                        enemyController.addXVelocity(recoilX);
                }

                if (currHealth <= 0) //If the enemy has no health, kill it
                    this.destroy();
            }
            prevHealth = currHealth;
        }
    }
}
