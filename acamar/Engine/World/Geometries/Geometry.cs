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
        public Vector2 center { get; set; } // center position
        public float radius { get; set; }   // max radius
        public abstract bool Collides(Geometry other);
    }
}
