using acamar.Engine.World.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Physics
{
    public abstract class PhysicalBody
    {
        public Geometry geometry;                        //Relative coordinates + Translation vector
        protected PhysicalParameters currentParameters;
        protected PhysicalParameters nextParameters;
        
        public bool updated = false;



        public bool Collides(PhysicalBody other) {
            return geometry.Collides(other.geometry);
        }

        public virtual void Update() { }

        public virtual void ApplyUpdate()
        {
            currentParameters = nextParameters;
            updated = true;
        }
    }
}
