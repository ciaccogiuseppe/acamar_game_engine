using acamar.Engine.World.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Physics
{
    internal class CollisionHandler
    {
        private Quadtree quadtree;

        public CollisionHandler(float x, float y, float width, float height)
        {
            quadtree = new Quadtree(0, x, y, width, height);
        }

        public void InsertBody(PhysicalBody body)
        {
            quadtree.Insert(body);
        }

        public void RemoveBody(PhysicalBody body)
        {
            quadtree.Remove(body);
        }

        public void UpdateBody(PhysicalBody body)
        {
            quadtree.Update(body);
        }

        private void ResolveCollision(PhysicalBody body, PhysicalBody other)
        {
            throw new NotImplementedException();
        }

        public void DetectCollisions(float timeStep)
        {
            List<PhysicalBody> allBodies = new List<PhysicalBody>();
            List<PhysicalBody> potentialCollisions = new List<PhysicalBody>();

            quadtree.Retrieve(allBodies, null);

            foreach(PhysicalBody body in allBodies)
            {
                potentialCollisions.Clear();
                quadtree.Retrieve(potentialCollisions, body);

                foreach(PhysicalBody other in potentialCollisions)
                {
                    if(body != other && body.Collides(other))
                    {
                        ResolveCollision(body, other);
                    }
                }
                
            }
        }
    }
}
