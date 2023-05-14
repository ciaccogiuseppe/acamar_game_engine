using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Physics
{
    public interface PhysicalBody
    {
        public bool Collides(PhysicalBody other);
        public bool Update();
    }
}
