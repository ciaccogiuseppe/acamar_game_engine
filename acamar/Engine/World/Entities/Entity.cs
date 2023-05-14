using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using acamar.Engine.World.Graphics;
using acamar.Engine.World.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Entities
{
    public class Entity
    {
        protected int entid;

        protected int pos_x;
        protected int pos_y;
        protected int pos_z;

        protected bool active;

        protected DrawableSurface drawableSurface;
        protected PhysicalBody physicalObject;


        public void Update()
        {
            physicalObject.Update();
            drawableSurface.Update();
        }
        //protected string name;
        //protected int entid;
        //protected int posx;
        //protected int posy;
        //protected int dir;
        //protected int sprid;
        //protected int width = 400;
        //protected int height = 10;
        //protected int currentAnimation = 0;
        //protected int[] animationLength = new int[Globals.MAXANIMNUMBER];
        //protected bool[] animationLoop = new bool[Globals.MAXANIMNUMBER];
        //protected int[] animationStep = new int[Globals.MAXANIMNUMBER];
        //protected int animationCount = 0;
        ////protected int animationLength = 2;
        ////protected bool animationLoop = true;
        //protected bool moving = false;
        //protected bool locked = false;
        //protected bool enabled = true;
        //protected bool transparent = false;
        //protected int layer = 1;
        //protected Texture2D texture;
        //protected Rectangle destRec;
        //protected Rectangle sourceRec;
        //protected Rectangle collRec;

        //protected float opacity = 1.0f;
        //protected float fadeStep = 0.1f;
        //protected bool fading;


        //protected List<Event> events = new List<Event>();
        //protected List<Event> activeEvents = new List<Event>();
        //protected bool active = true; //to activate/deactivate entity
        //protected bool animActive = false; //activate/deactive animation
        //protected bool loopAnim = false; //loop/nonloop animation

        //protected bool collidable = true;
        //protected int cPosx;
        //protected int cPosy;
    }
}
