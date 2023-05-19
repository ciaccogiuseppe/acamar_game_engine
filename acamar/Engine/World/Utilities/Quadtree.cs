using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using acamar.Engine.World.Geometries;
using acamar.Engine.World.Physics;

namespace acamar.Engine.World.Utilities
{
    public class Quadtree
    {
        private const int MAX_BODIES = 4;
        private const int MAX_LEVELS = 6;

        private int level;
        private List<PhysicalBody> bodies;

        private Quadtree[] nodes;

        private float x;
        private float y;
        private float width;
        private float height;

        public Quadtree(int level, float x, float y, float width, float height)
        {
            this.level = level;
            bodies = new List<PhysicalBody>();
            nodes = new Quadtree[4];
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void Clear()
        {
            bodies.Clear();

            for(int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        public void Split()
        {
            float subWidth = width / 2;
            float subHeight = height / 2;
            float x = this.x;
            float y = this.y;

            nodes[0] = new Quadtree(level + 1, x + subWidth, y, subWidth, subHeight);
            nodes[1] = new Quadtree(level + 1, x, y, subWidth, subHeight);
            nodes[2] = new Quadtree(level + 1, x, y + subHeight, subWidth, subHeight);
            nodes[3] = new Quadtree(level + 1, x + subWidth, y + subHeight, subWidth, subHeight);
        }

        private int GetIndex(PhysicalBody body)
        {
            int index = -1;
            float verticalMidpoint = x + (width / 2f);
            float horizontalMidpoint = y + (height / 2f);

            bool topQuadrant = (body.geometry.center.Y + body.geometry.radius < horizontalMidpoint);
            bool bottomQuadrant = (body.geometry.center.Y - body.geometry.radius > horizontalMidpoint);
            // Check if the polygon can completely fit within the left quadrants
            if (body.geometry.center.X - body.geometry.radius < verticalMidpoint && body.geometry.center.X + body.geometry.radius < verticalMidpoint)
            {
                if (topQuadrant)
                    index = 1;
                else if (bottomQuadrant)
                    index = 2;
            }
            // Check if the polygon can completely fit within the right quadrants
            else if (body.geometry.center.X - body.geometry.radius > verticalMidpoint)
            {
                if (topQuadrant)
                    index = 0;
                else if (bottomQuadrant)
                    index = 3;
            }
            return index;
        }


        public void Insert(PhysicalBody body)
        {
            if (nodes[0] != null)
            {
                int index = GetIndex(body);
                if (index != -1)
                {
                    nodes[index].Insert(body);
                    return;
                }
            }

            bodies.Add(body);

            if (bodies.Count > MAX_BODIES && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                    Split();

                int i = 0;
                while (i < bodies.Count)
                {
                    int index = GetIndex(bodies[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(bodies[i]);
                        bodies.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        public void Update(PhysicalBody body)
        {
            //TODO: if still in the same box no remove/insert
            bool removed = Remove(body);
            if (removed)
            {
                Insert(body);
            }
        }

        public bool Remove(PhysicalBody body)
        {
            bool removed = bodies.Remove(body);

            if (!removed && nodes != null)
            {
                foreach (Quadtree node in nodes)
                {
                    removed |= node.Remove(body);
                    if (removed)
                        break;
                }
            }

            if (removed && nodes != null && CountTotalElements(nodes) <= MAX_BODIES)
            {
                Flatten();
            }

            return removed;
        }

        private int CountTotalElements(Quadtree[] nodes)
        {
            int count = 0;
            foreach (Quadtree node in nodes)
            {
                if (node != null)
                {
                    count += node.CountTotalElements(node.nodes);
                    count += node.bodies.Count;
                }
            }
            return count;
        }

        private void Flatten()
        {
            List<PhysicalBody> allElements = new List<PhysicalBody>();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    allElements.AddRange(nodes[i].bodies);
                    nodes[i].bodies.Clear();
                    nodes[i].nodes = null;
                }
            }
            bodies.AddRange(allElements);
        }


        public List<PhysicalBody> Retrieve(List<PhysicalBody> result, PhysicalBody body)
        {
            int index = GetIndex(body);
            if (index != -1 && nodes[0] != null)
                nodes[index].Retrieve(result, body);

            result.AddRange(bodies);

            return result;
        }

    }
}
