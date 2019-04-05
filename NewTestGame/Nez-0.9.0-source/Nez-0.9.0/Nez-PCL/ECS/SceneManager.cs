using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace Nez
{
    public class SceneManager : IUpdatable
    {
        public bool enabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int updateOrder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void update()
        {
            //Stuff added here when this class is derived
        }
    }
}
