using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;


namespace NewTestGame.Src.Utils 
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyListenerEntity : Entity
    {
        //public EventHandler keyPressed;

        public event Action onKeyPressed;

        public Keys key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listenKey"></param>
        public KeyListenerEntity(Keys listenKey)
        {
            this.key = listenKey;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void update()
        {
            base.update();

            if (onKeyPressed != null)
            {
                if (Input.isKeyPressed(this.key))
                {
                    onKeyPressed();
                }
            }
        }      
    }


}
