using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Geometries
{
    public class Polygon : Geometry
    {
        public List<Vector2> vertices { get; set; }

        public Polygon(List<Vector2> vertices)
        {
            this.vertices = vertices;
            Circle tempCircle = FindSmallestCircle(this.vertices);
            radius = tempCircle.radius;
            center = tempCircle.center;
        }

        public override bool Collides(Geometry other)
        {
            if(other is Circle otherCircle)
            {
                return otherCircle.Collides(this);
            }
            else if (other is Polygon otherPolygon)
            {
                List<Vector2> vertices1 = vertices;
                List<Vector2> vertices2 = otherPolygon.vertices;

                // Check for edge-edge intersection
                for (int i = 0; i < vertices1.Count; i++)
                {
                    Vector2 p1 = vertices1[i];
                    Vector2 p2 = vertices1[(i + 1) % vertices1.Count];

                    for (int j = 0; j < vertices2.Count; j++)
                    {
                        Vector2 q1 = vertices2[j];
                        Vector2 q2 = vertices2[(j + 1) % vertices2.Count];

                        if (DoIntersect(p1, p2, q1, q2))
                        {
                            return true;
                        }
                    }
                }
                // Check if one polygon is contained within the other
                if (IsPolygonInside(vertices1, vertices2) || IsPolygonInside(vertices2, vertices1))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool DoIntersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
        {
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            // Special cases
            if (o1 == 0 && OnSegment(p1, p2, q1))
            {
                return true;
            }

            if (o2 == 0 && OnSegment(p1, q2, q1))
            {
                return true;
            }

            if (o3 == 0 && OnSegment(p2, p1, q2))
            {
                return true;
            }

            if (o4 == 0 && OnSegment(p2, q1, q2))
            {
                return true;
            }

            return false;
        }

        private static int Orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (Math.Abs(val) < 0.0001)
            {
                return 0;  // Collinear
            }

            return (val > 0) ? 1 : 2;  // Clockwise or counterclockwise
        }
        private static bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min (p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
            {
                return true;
            }

            return false;
        }

        private static bool IsPolygonInside(List<Vector2> polygon1, List<Vector2> polygon2)
        {
            foreach (Vector2 vertex in polygon1)
            {
                if (!IsPointInside(vertex, polygon2))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsPointInside(Vector2 point, List<Vector2> polygon)
        {
            int count = polygon.Count;
            bool inside = false;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                if ((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y) &&
                    point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private Circle FindSmallestCircle(List<Vector2> vertices)
        {
            // If the list is empty, return null
            if (vertices.Count == 0)
            {
                return null;
            }

            // If there's only one point, the smallest circle is centered on that point with radius 0
            if (vertices.Count == 1)
            {
                return new Circle(vertices[0], 0);
            }

            // Randomize the order of the points
            Random random = new Random();
            List<Vector2> shuffledPoints = new List<Vector2>(vertices);
            for (int i = shuffledPoints.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Vector2 temp = shuffledPoints[i];
                shuffledPoints[i] = shuffledPoints[j];
                shuffledPoints[j] = temp;
            }

            // Create an initial circle using the first two points
            Circle smallestCircle = CreateCircleFromPoints(shuffledPoints[0], shuffledPoints[1]);

            // Iterate over the remaining points and adjust the circle if necessary
            for (int i = 2; i < shuffledPoints.Count; i++)
            {
                Vector2 currentPoint = shuffledPoints[i];

                // If the current point is outside the current circle, create a new circle using this point
                if (!IsPointInsideCircle(currentPoint, smallestCircle))
                {
                    smallestCircle = CreateCircleFromPoints(shuffledPoints[0], currentPoint);

                    // Find the smallest circle that contains both the current point and any previously processed points
                    for (int j = 1; j < i; j++)
                    {
                        Vector2 point = shuffledPoints[j];
                        if (!IsPointInsideCircle(point, smallestCircle))
                        {
                            smallestCircle = CreateCircleFromPoints(point, currentPoint);

                            // Find the smallest circle that contains the current point, the point at index j, and any previously processed points
                            for (int k = 0; k < j; k++)
                            {
                                Vector2 p = shuffledPoints[k];
                                if (!IsPointInsideCircle(p, smallestCircle))
                                {
                                    smallestCircle = CreateCircleFromPoints(p, point, currentPoint);
                                }
                            }
                        }
                    }
                }
            }

            return smallestCircle;
        }

        private Circle CreateCircleFromPoints(Vector2 p1, Vector2 p2)
        {
            float centerX = (p1.X + p2.X) / 2;
            float centerY = (p1.Y + p2.Y) / 2;
            float radius = DistanceBetweenPoints(p1, p2) / 2;

            return new Circle (new Vector2 (centerX,centerY), radius );
        }

        private Circle CreateCircleFromPoints(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float centerX = ((p1.X * p1.X + p1.Y * p1.Y) * (p2.Y - p3.Y) + (p2.X * p2.X + p2.Y * p2.Y) * (p3.Y - p1.Y) + (p3.X * p3.X + p3.Y * p3.Y) * (p1.Y - p2.Y)) / (2 * (p1.X * (p2.Y - p3.Y) - p1.Y * (p2.X - p3.X) + p2.X * p3.Y - p2.Y * p3.X));
            float centerY = ((p1.X * p1.X + p1.Y * p1.Y) * (p3.X - p2.X) + (p2.X * p2.X + p2.Y * p2.Y) * (p1.X - p3.X) + (p3.X * p3.X + p3.Y * p3.Y) * (p2.X - p1.X)) / (2 * (p1.X * (p2.Y - p3.Y) - p1.Y * (p2.X - p3.X) + p2.X * p3.Y - p2.Y * p3.X));
            float radius = DistanceBetweenPoints(new Vector2 { X = centerX, Y = centerY }, p1);

            return new Circle(new Vector2(centerX, centerY), radius);
        }

        private bool IsPointInsideCircle(Vector2 point, Circle circle)
        {
            float distance = DistanceBetweenPoints(point, circle.center);
            return distance <= circle.radius;
        }

        private float DistanceBetweenPoints(Vector2 p1, Vector2 p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
