using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Geometries
{
    public abstract class Geometry
    {   
        public Vector2 position;            // center position
        public float radius;                // max radius
        public abstract bool Collides(Geometry other);
    }
}
