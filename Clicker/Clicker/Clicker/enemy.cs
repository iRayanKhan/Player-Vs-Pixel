using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Clicker {
    class enemy {
        private Vector2 position;
        private Vector2 moveSpeeds;
        private Rectangle range;
        private int timer;
        private GameTime gT;

        public enemy(Vector2 position) {
            this.position = position;
            moveSpeeds = new Vector2(1, 1);
            range = new Rectangle((int)position.X, (int)position.Y, 64, 64);
            timer = 10;
            gT = new GameTime();
        }

        public void Update(GameTime gameTime, Player p) {
            Move(p);
            if (clickInRange(gameTime, p)) {
                p.setDead();
            }
        }

        private bool clickInRange(GameTime gameTime, Player p) {

            if (!p.localBounds.Intersects(range)) {
                return false;
            }
            else if (timer != 0) {
                if (gT.ElapsedGameTime == new TimeSpan(0, 0, 2)) {
                    timer--;
                }
                return false;
            }

            return true;
        }

        private void Move(Player p) {
            if (p.Position.X > position.X)
                position.X += moveSpeeds.X;
            else if (p.Position.X < position.X)
                position.X -= moveSpeeds.X;

            if (p.Position.Y > position.Y)
                position.Y += moveSpeeds.Y;
            else if (p.Position.Y < position.Y)
                position.Y -= moveSpeeds.Y;

            range.X = (int)position.X;
            range.Y = (int)position.Y;
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D tex) {
            spriteBatch.Draw(tex, range, Color.White);
        }

    }
}
