using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Clicker
{
    public class Platform
    {
        //States for platforms
        public enum PlatformType
        {
            incollidable,

            collidable,

            verticallyCollidable

        }

        public PlatformType PlatState;

        public string PlatTex;
        public Rectangle PlatRec;
        public const int WIDTH = 25;
        public const int HEIGHT = 50;

        public Vector2 getSize()
        {
            return new Vector2(Convert.ToInt32(PlatRec.X), Convert.ToInt32(PlatRec.Y));
        }

        //Construct new platform.
        public Platform(string Tex, Rectangle Rec, PlatformType platType)
        {
            PlatState = platType;
            PlatRec = Rec;
            PlatTex = Tex;
        }
    }
}
