using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    class Paddle : Microsoft.Xna.Framework.Game
    {
        public Texture2D texture;
        public Vector2 position;
        public PlayerIndex pNumber;
        public int width, height, speed, points;
        public Rectangle boundingBox;

        public Paddle()
        {
            texture = null;
            position = Vector2.Zero;
            pNumber = PlayerIndex.One;
            speed = 5;
        }

        public void update() { }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void Update(Rectangle mainFrame)
        {
            width = texture.Width;
            height = texture.Height;

            boundingBox = new Rectangle(((int)position.X)-1, ((int)position.Y)-1, width, height);
            if (position.Y <= 5)
                position.Y = 5;
            if (position.Y >= mainFrame.Height - 56)
                position.Y = mainFrame.Height - 56;
            if (position.X <= 5)
                position.X = 5;
            if (position.X >= mainFrame.Width - 16)
                position.X = mainFrame.Width - 16;
        }
    }
}
