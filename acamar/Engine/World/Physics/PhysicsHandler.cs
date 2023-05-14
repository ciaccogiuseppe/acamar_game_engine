using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Physics
{
    public class PhysicsHandler
    {
        List<PhysicalBody> bodies;

        public void Update()
        {
            ComputeInteractions();
            BodiesUpdate();
        }

        public void ComputeInteractions()
        {

        }

        public void BodiesUpdate()
        {
            bodies.ForEach(b => b.Update());
        }
    }
}
