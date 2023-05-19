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
        CollisionHandler collisionHandler;
        

        public void Update()
        {
            ComputeInteractions();
            BodiesUpdate();     //apply actual update
        }

        public void ComputeInteractions()
        {
            //bodies.ForEach(b => b.Update());            //compute next state without applying it
            collisionHandler.DetectCollisions(1);
        }


        public void BodiesUpdate()
        {
            bodies.ForEach(b => b.ApplyUpdate());
        }
    }
}
