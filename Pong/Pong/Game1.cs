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

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont p1Font, p2Font, titleFont, winnerFont;
        Vector2 p1FontPos, p2FontPos, titleFontPos, winnerFontPos;
        KeyboardState keyboard;

        int numPlayers;
        Boolean cont;

        Paddle p1;
        Paddle p2;
        Ball ball;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 760;
            graphics.PreferredBackBufferWidth = 1024;

            this.IsFixedTimeStep = false;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            numPlayers = -1;

            p1 = new Paddle();
            p2 = new Paddle();

            ball = new Ball();

            base.Initialize();
        }

        Texture2D myBackground;
        Rectangle mainFrame;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            cont = true;

            myBackground = Content.Load<Texture2D>("background");
            p1.texture = Content.Load<Texture2D>("paddle");
            p2.texture = Content.Load<Texture2D>("paddle");
            ball.texture = Content.Load<Texture2D>("ball");
            p2Font = Content.Load<SpriteFont>("scoreFont");
            p1Font = Content.Load<SpriteFont>("scoreFont");
            titleFont = Content.Load<SpriteFont>("scoreFont");
            winnerFont = Content.Load<SpriteFont>("scoreFont");
            p1FontPos = new Vector2(54, 54);
            p2FontPos = new Vector2(GraphicsDevice.Viewport.Width - 54, 54);            
            titleFontPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            winnerFontPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //set starting position for our paddles
            p1.position.X = 50;
            p1.position.Y = graphics.GraphicsDevice.Viewport.Height / 2 - (p1.height / 2);
            p2.position.X = graphics.GraphicsDevice.Viewport.Width - 50 - (p2.width);
            p2.position.Y = graphics.GraphicsDevice.Viewport.Height / 2 - (p2.height / 2);

            //set starting position for ball
            ball.Reset(mainFrame);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            this.Exit();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.UnloadContent();

            if (keyboard.IsKeyDown(Keys.Escape))
                this.UnloadContent();

            if (keyboard.IsKeyDown(Keys.C) && !cont)
            {
                cont = true;
                this.Draw(gameTime);
            }

            if (cont)
            {
                if (numPlayers == -1)
                {
                    if (keyboard.IsKeyDown(Keys.D1))
                        numPlayers = 1;
                    if (keyboard.IsKeyDown(Keys.D2))
                        numPlayers = 2;
                }

                if (numPlayers == 1)
                    singleplayerUpdate(gameTime);
                if (numPlayers == 2)
                    multiplayerUpdate(gameTime);


                //Allows the game to start
                if (numPlayers != -1)
                    if (keyboard.IsKeyDown(Keys.Space))
                        ball.start = true;
            }

            if (keyboard.IsKeyDown(Keys.C) && !cont)
            {
                cont = true;
                this.Initialize();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(myBackground, mainFrame, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            p1.draw(spriteBatch);
            p2.draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            ball.draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
            
            //p1 score
            spriteBatch.Begin();
            string p1ScoreOutput = p1.points.ToString();
            Vector2 p1FontOrigin = p1Font.MeasureString(p1ScoreOutput) / 2;
            spriteBatch.DrawString(p1Font, p1ScoreOutput, p1FontPos, Color.Green,
                0, p1FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();

            //p2 score
            spriteBatch.Begin();
            string p2ScoreOutput = p2.points.ToString();
            Vector2 p2FontOrigin = p2Font.MeasureString(p2ScoreOutput) / 2;
            spriteBatch.DrawString(p2Font, p2ScoreOutput, p2FontPos, Color.Green,
                0, p2FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();

            if (p1.points == 7)
            {
                numPlayers = -1;
                cont = false;
                spriteBatch.Begin();
                string winner = "Player 1 wins! Play again? \n [C] = Continue [Esc] = Exit";
                Vector2 winnerFontOrigin = winnerFont.MeasureString(winner) / 2;
                spriteBatch.DrawString(winnerFont, winner, winnerFontPos, Color.Green,
                    0, winnerFontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }
            else if (p2.points == 7)
            {
                numPlayers = -1;
                cont = false;
                spriteBatch.Begin();
                string winner = "Player 2 wins! Play again? \n [C] = Continue [Esc] = Exit";
                Vector2 winnerFontOrigin = winnerFont.MeasureString(winner) / 2;
                spriteBatch.DrawString(winnerFont, winner, winnerFontPos, Color.Green,
                    0, winnerFontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }

            if (numPlayers == -1 && cont)
            {
                spriteBatch.Begin();
                string title = "         Welcome to Pong \n  [1] = 1 Player    [2] = 2 Players";
                Vector2 titleFontOrigin = titleFont.MeasureString(title) / 2;
                spriteBatch.DrawString(titleFont, title, titleFontPos, Color.Green,
                    0, titleFontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        protected void singleplayerUpdate(GameTime gameTime)
        {
            //p1 controls
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
                p1.position.X -= p1.speed;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
                p1.position.X += p1.speed;
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
                p1.position.Y += p1.speed;
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
                p1.position.Y -= p1.speed;

            if (p1.position.X >= mainFrame.Width / 4 - 13)
                p1.position.X = mainFrame.Width / 4 - 13;
            if (p2.position.X <= ((mainFrame.Width / 4) * 3) + 3)
                p2.position.X = ((mainFrame.Width / 4) * 3) + 3;

            //AI move
            if (ball.position.X >= mainFrame.Width / 2 && ball.right)
            {
                if (ball.position.X < p2.position.X)
                {
                    if (p2.position.X > ball.position.X)
                        if (p2.position.X > ((mainFrame.Width / 4) * 3) + 3)
                            p2.position.X -= p2.speed;
                        else
                            p2.position.X = ((mainFrame.Width / 4) * 3) + 3;
                    else if (p2.position.X <= graphics.GraphicsDevice.Viewport.Width - 50 - (p2.width))
                        p2.position.X += p2.speed;

                    if (p2.position.X > ball.position.X)
                        if (p2.position.Y <= ball.position.Y)
                            p2.position.Y += p2.speed;
                        else
                            p2.position.Y -= p2.speed;
                }
            }
            else
            {
                if (p2.position.X < graphics.GraphicsDevice.Viewport.Width - 50 - (p2.width))
                    p2.position.X += p2.speed;
            }

            p1.Update(mainFrame);
            p2.Update(mainFrame);
            ball.Update(mainFrame, p1, p2);
        }

        protected void multiplayerUpdate(GameTime gameTime)
        {
            //p1 controls
            if (keyboard.IsKeyDown(Keys.A))
                p1.position.X -= p1.speed;
            if (keyboard.IsKeyDown(Keys.D))
                p1.position.X += p1.speed;
            if (keyboard.IsKeyDown(Keys.S))
                p1.position.Y += p1.speed;
            if (keyboard.IsKeyDown(Keys.W))
                p1.position.Y -= p1.speed;

            //p2 controls
            if (keyboard.IsKeyDown(Keys.Left))
                p2.position.X -= p2.speed;
            if (keyboard.IsKeyDown(Keys.Right))
                p2.position.X += p2.speed;
            if (keyboard.IsKeyDown(Keys.Down))
                p2.position.Y += p2.speed;
            if (keyboard.IsKeyDown(Keys.Up))
                p2.position.Y -= p2.speed;

            if (p1.position.X >= mainFrame.Width / 4 - 13)
                p1.position.X = mainFrame.Width / 4 - 13;
            if (p2.position.X <= ((mainFrame.Width / 4) * 3) + 3)
                p2.position.X = ((mainFrame.Width / 4) * 3) + 3;

            p1.Update(mainFrame);
            p2.Update(mainFrame);
            ball.Update(mainFrame, p1, p2);
        }
    }
}