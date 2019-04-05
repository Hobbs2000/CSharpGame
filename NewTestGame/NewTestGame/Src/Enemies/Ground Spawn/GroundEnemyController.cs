using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Tiled;
using Microsoft.Xna.Framework;
using NewTestGame.Src.Scenes;
using NewTestGame.Src.Objects.Items;

namespace NewTestGame.Src.Enemies.Ground_Spawn
{
    /// <summary>
    /// Ground enemies move differently than flying enemies
    /// </summary>
    class GroundEnemyController : Component, IUpdatable
    {
        public int speed = 20;
        public Vector2 _velocity = new Vector2(0, 0);
        public int gravity = 1500;
        public bool bouncing = false;

        public TiledMapMover enemyMover;
        public BoxCollider enemyCollider;
        public BoxCollider enemyAttCollider;
        TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();

        /// <summary>
        /// 
        /// </summary>
        public GroundEnemyController(TiledMapMover mover, BoxCollider physicsCollider, BoxCollider attackCollider)
        {
            this.enemyMover = mover;
            this.enemyCollider = physicsCollider;
            this.enemyAttCollider = attackCollider;
        }



        /// <summary>
        /// 
        /// </summary>
        public void update()
        {
            if (!enabled)
            {
                Debug.log("Enemy cont is Disabled!");
                return;
            }

            #region _velocity.X Stuff
            //If the enemy's attack collider overlaps the players collider, then move towards player
            if (enemyAttCollider.overlaps(entity.scene.findEntity("Player").getCollider<BoxCollider>()))
            {
                if (this.transform.position.X < entity.scene.findEntity("Player").transform.position.X)
                {
                    if (_velocity.X < 200)
                        _velocity.X += speed;
                    else if (_velocity.X >= 200)
                        _velocity.X = 200;
                }
                else if (this.transform.position.X > entity.scene.findEntity("Player").transform.position.X)
                {
                    if (_velocity.X > -200)
                        _velocity.X -= speed;
                    else if (_velocity.X < -200)
                        _velocity.X = -200;
                }
            }
            else
                _velocity.X = 0;
            #endregion

            #region _velocity.Y Stuff
            _velocity.Y += gravity * Time.deltaTime; //Apply gravity

            if (_collisionState.below && !bouncing) 
                _velocity.Y = 0;
            if (bouncing)
                bouncing = false; //Have to set bouncing back to false so Y velocity does not build up when back on ground
            #endregion

            //Finally, move the enemy
            enemyMover.move(_velocity * Time.deltaTime, enemyCollider, _collisionState);
        }


        /// <summary>
        /// Will launch the enemy up bounceHeight amount
        /// </summary>
        /// <param name="bounceHeight"></param>
        public void bounce(float bounceHeight)
        {
            bouncing = true;
            _velocity.Y = (float)(-Math.Sqrt(2 * bounceHeight * gravity));
        }

        /// <summary>
        /// Adds n amount of X velocity
        /// </summary>
        /// <param name="n"></param>
        public void addXVelocity(int n)
        {
            _velocity.X += n;
        }

        /// <summary>
        /// Removes n amount of X velocity
        /// </summary>
        /// <param name="n"></param>
        public void rmvXVelocity(int n)
        {
            _velocity.X -= n;
        }
    }
}
