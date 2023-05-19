using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Geometries
{
    public class Circle : Geometry
    {

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public override bool Collides(Geometry other)
        {
            if(other is Circle otherCircle)
            {
                float dx = center.X - otherCircle.center.X;
                float dy = center.Y - otherCircle.center.Y;
                float distance2 = dx * dx + dy * dy;
                float radiusSum = radius + otherCircle.radius;
                return distance2 < radiusSum * radiusSum;
            }
            else if (other is Polygon otherPolygon)
            {
                for(int i = 0; i < otherPolygon.vertices.Count; i++)
                {
                    Vector2 v1 = otherPolygon.vertices[i];
                    Vector2 v2 = otherPolygon.vertices[(i+1)% otherPolygon.vertices.Count];
                    if (DistanceToSegment(center, v1, v2) < radius)
                    {
                        return true;
                    }
                }
                // Check if the circle is inside the polygon
                if (PointInPolygon(center, otherPolygon.vertices))
                    {
                        return true;
                    }
                    return false;
                }
            return false;
        }

        private static float DistanceToSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            Vector2 v = segmentEnd - segmentStart;
            Vector2 w = point - segmentStart;

            float lengthSquared = v.X * v.X + v.Y * v.Y;
            float t = Math.Max(0, Math.Min(1, Vector2.Dot(w, v) / lengthSquared));

            Vector2 projection = segmentStart + v * t;
            float distance = Vector2.Distance(point, projection);

            return distance;
        }

        private static bool PointInPolygon(Vector2 point, List<Vector2> vertices)
        {
            bool inside = false;

            for (int i = 0, j = vertices.Count - 1; i < vertices.Count; j = i++)
            {
                Vector2 vi = vertices[i];
                Vector2 vj = vertices[j];

                if ((vi.Y > point.Y) != (vj.Y > point.Y) &&
                    point.X < (vj.X - vi.X) * (point.Y - vi.Y) / (vj.Y - vi.Y) + vi.X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}
