using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    class Ball : Microsoft.Xna.Framework.Game
    {
        public Texture2D texture;
        public Vector2 position;
        public int width, height, speedX, speedY;
        public Rectangle boundingBox;
        public Boolean right, start, up;
        private Random r;

        public Ball()
        {
            texture = null;
            position = Vector2.Zero;
            r = new Random();
            speedX = 5;
            speedY = 5;
            if (r.Next(100) > 50)
                right = true;
            else
                right = false;
            if (r.Next(100) > 50)
                up = true;
            else
                up = false;
            start = false;
        }

        public void update() { }

        public void draw(SpriteBatch spriteBatch) { spriteBatch.Draw(texture, position, Color.White); }

        public void Update(Rectangle mainFrame, Paddle p1, Paddle p2)
        {
            if(start)
            {
                width = texture.Width;
                height = texture.Height;

                boundingBox = new Rectangle(((int)position.X), ((int)position.Y), width, height);
                if (boundingBox.Intersects(p1.boundingBox))
                    right = true;
                if (boundingBox.Intersects(p2.boundingBox))
                    right = false;

                if (position.Y <= 5)
                {
                    position.Y = 5;
                    up = true;
                }
                if (position.Y >= mainFrame.Height - 16)
                {
                    position.Y = mainFrame.Height - 16;
                    up = false;
                }
                if (position.X <= 5)
                {
                    position.X = 5;
                    right = true;
                    p2.points++;
                    Reset(mainFrame);
                }
                if (position.X >= mainFrame.Width - 15)
                {
                    position.X = mainFrame.Width - 15;
                    right = false;
                    p1.points++;
                    Reset(mainFrame);
                }

                if (right)
                    position.X += speedX;
                else
                    position.X -= speedX;

                if (up)
                    position.Y += speedY;
                else
                    position.Y -= speedY;
            }
        }

        public Ball Clone()
        {
            Ball newBall = new Ball();

            newBall.position = this.position;
            newBall.width = this.width;
            newBall.height = this.height;
            newBall.speedX = this.speedX;
            newBall.speedY = this.speedY;
            newBall.boundingBox = this.boundingBox;
            newBall.right = this.right;
            newBall.start = this.start;
            newBall.up = this.up;

            return newBall;
        }

        public void Reset(Rectangle mainFrame)
        {
            start = false;
            position.X = mainFrame.Width / 2 - 10;
            position.Y = mainFrame.Height / 2;
        }
    }
}
