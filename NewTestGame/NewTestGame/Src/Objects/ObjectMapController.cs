using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTestGame.Src.Objects
{
    /// <summary>
    /// This is a Map Controller meant to be added to items and objects
    /// Allows for movement of items and objects that follows the game physics and collisions
    /// </summary>
    class ObjectMapController : Component, IUpdatable
    {
        public float maxVelocity = 300;
        public float gravity;
        public float momentumLoss = 25;

        private bool gravityState = true;

        public TiledMapMover _mover;
        BoxCollider _collider;
        public Vector2 _velocity;
        TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();

        /// <summary>
        /// 
        /// </summary>
        public ObjectMapController(float gravity)
        {
            this.gravity = gravity;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            _mover = entity.getComponent<TiledMapMover>();
            _collider = entity.getCollider<BoxCollider>();
        }


        /// <summary>
        /// 
        /// </summary>
        public void update()
        {
            //Apply gravity
            if (gravityState)
                _velocity.Y += gravity * Time.deltaTime;

            if ((_mover != null) && (_collider != null))
                _mover.move(_velocity * Time.deltaTime, _collider, _collisionState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounceHeight"></param>
        public void bounce(float bounceHeight)
        {
            _velocity.Y = (float)(-Math.Sqrt(2 * bounceHeight * gravity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void toggleGravity(bool state)
        {
            gravityState = state;
        }
    }
}
