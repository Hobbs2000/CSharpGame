using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;
using Nez.Sprites;
using NewTestGame.Src.Objects;
using Nez.UI;
using NewTestGame.Src.Objects.Items;

namespace NewTestGame.Src
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerController : Component, IUpdatable
    {
        public float moveSpeed = 20;
        public float maxVelocity = 300;
        public float gravity = 1500;
        public float jumpHeight = 16 * 4;
        public float momentumLoss = 25;

        TiledMapMover _mover;
        BoxCollider _collider;
        public Vector2 _velocity;
        TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();

        ColliderTriggerHelper _triggerHelper;

        private bool isPaused = false;

        Player p;

        public event Action<PlayerController> onClick;

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();

            _mover = this.getComponent<TiledMapMover>();
            _collider = entity.getCollider<BoxCollider>();
            _triggerHelper = new ColliderTriggerHelper(entity);

            p = entity as Player;

        }

        /// <summary>
        /// 
        /// </summary>
        public void pause()
        {
            isPaused = true;
        }


        /// <summary>
        /// 
        /// </summary>
        public void resume()
        {
            isPaused = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void update()
        {

            if (!enabled)
            {
                Debug.log("Player cont is Disabled!");
                return;
            }

            if (Input.isKeyDown(Keys.D)) //Moving right
            {
                if (_velocity.X < 0)
                    _velocity.X += (moveSpeed + momentumLoss); //Combine momentum loss and movespeed if previously going left
                else
                    _velocity.X += moveSpeed; //Just add movespeed if starting from standstill

                if (_velocity.X > maxVelocity)
                    _velocity.X = maxVelocity;

                if (!entity.getComponent<Sprite<Player.Animations>>().isAnimationPlaying(Player.Animations.WalkingRight))
                {
                    entity.getComponent<Sprite<Player.Animations>>().play(Player.Animations.WalkingRight);
                }
            }
            else if (Input.isKeyDown(Keys.A)) //Moving left
            {
                if (_velocity.X > 0)
                    _velocity.X -= (moveSpeed + momentumLoss); //Combine momentum loss and movespeed if previously going right
                else
                    _velocity.X -= moveSpeed; //Just subtract movespeed if starting from standstill

                if (_velocity.X < (-maxVelocity))
                {
                    _velocity.X = -maxVelocity;
                }

                if (entity.getComponent<Sprite<Player.Animations>>().isAnimationPlaying(Player.Animations.WalkingRight))
                {
                    entity.getComponent<Sprite<Player.Animations>>().stop();
                }

            }
            else
            {

                //When the player is not moving left or right, go into standing position
                entity.getComponent<Sprite<Player.Animations>>().play(Player.Animations.standing);


                //Now account for the players momentum when they release the controls
                if (Input.isKeyUp(Keys.D))
                {
                    if (_velocity.X >= 0)
                    {
                        _velocity.X -= momentumLoss;
                        if (_velocity.X < 0)
                            _velocity.X = 0;
                    }

                }
                if (Input.isKeyUp(Keys.A))
                {
                    if (_velocity.X <= 0)
                    {
                        _velocity.X += momentumLoss;
                        if (_velocity.X > 0)
                            _velocity.X = 0;
                    }

                }
            }

            //When D key is released and A was up, stop the WalkingRight animation and disable it, then enable the static standing sprite
            if (Input.isKeyReleased(Keys.D) && Input.isKeyUp(Keys.A))
            {
                if (entity.getComponent<Sprite<Player.Animations>>().isAnimationPlaying(Player.Animations.WalkingRight))
                {
                    entity.getComponent<Sprite<Player.Animations>>().stop();
                }
            }



            //Check for jumping
            if (_collisionState.below && Input.isKeyPressed(Keys.Space))
            {
                _velocity.Y = (float)(-Math.Sqrt(2 * jumpHeight * gravity));
            }

            //If the player is not on a ladder, apply gravity
            if (!entity.getComponent<ObjectInteractor>().onLadder)
                _velocity.Y += gravity * Time.deltaTime;
            else
                _velocity.Y = 0;

            //Finally, move the player
            _mover.move(_velocity * Time.deltaTime, _collider, _collisionState);


            //Do not allow player to build up velocity when colliding
            if (_collisionState.right || _collisionState.left)
                _velocity.X = 0;
            if (_collisionState.above || _collisionState.below)
                _velocity.Y = 0;


            if (p.weapon != null)
            {
                p.weapon.transform.position = new Vector2(p.transform.position.X + p.weapon.getXOffset(), p.transform.position.Y + p.weapon.getYOffset());

                if (Input.leftMouseButtonPressed)
                {
                    Sword sw = p.weapon as Sword;
                    sw.attack();
                }
            }
        }

    }
}
