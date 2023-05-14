using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace acamar.Engine.World.Graphics
{
    public abstract class DrawableSurface
    {
        protected Texture2D texture;
        protected int pos_x;
        protected int pos_y;
        protected int pos_z;

        protected float opacity = 1.0f;

        protected Rectangle destRec;
        protected Rectangle sourceRec;

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(
                texture,
                destRec,
                sourceRec, 
                Color.White * opacity, 
                0.0f, 
                new Vector2(0, 0),
                SpriteEffects.None,
                1 - (float)pos_z / (float)Globals.LAYERS);
        }

        public abstract void Update();

    }
}
