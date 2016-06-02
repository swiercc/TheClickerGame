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

namespace theClickerGame
{
    public enum GameState
    {
        TITLE_SCREEN,
        TIME_SELECTION,
        PLAYING,
        GAMEOVER,  // Only displays button to see score
        SHOWSCORE,
        HIGHSCORE
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState state;

        Texture2D numberSpritesheet;

        int clickTimes = 0;
        float timeRemaining;
        bool keyDown = false;
        int highScore;

        Texture2D titleScreen;
        Texture2D TimeSelection;
        Texture2D gameoverScreen;
        Texture2D Playing;
        Texture2D Showscore;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            state = GameState.TITLE_SCREEN;

            numberSpritesheet = Content.Load<Texture2D>(@"numberSpritesheet");

            titleScreen = Content.Load<Texture2D>(@"TitleScreen");
            TimeSelection = Content.Load<Texture2D>(@"TimeSelection");
            gameoverScreen = Content.Load<Texture2D>(@"hitSpaceToContinue");
            Playing = Content.Load<Texture2D>(@"Playing");
            Showscore = Content.Load<Texture2D>(@"Showscore");
        }

        public void DrawNumber(SpriteBatch sb, int num, int x, int y)
        {
            DrawNumber(sb, num, x, y, 1.0f, -1);
        }

        public void DrawNumber(SpriteBatch sb, int num, int x, int y, float scale, int padsize)
        {
            String text = num.ToString();
            int spacing = 0;

            if (padsize != -1)
                text = text.PadLeft(padsize, '0');

         
            for (int i = 0; i < text.Length; i++)
            {
                int digit = Convert.ToInt32(text[i].ToString());
                Rectangle rect = Rectangle.Empty;

                switch (digit)
                {
                    case 0:
                        rect = new Rectangle(0, 142, 90, 105);
                        break;
                    case 1:
                        rect = new Rectangle(99, 142, 72, 105);
                        break;
                    case 2:
                        rect = new Rectangle(176, 142, 84, 105);
                        break;
                    case 3:
                        rect = new Rectangle(269, 142, 91, 105);
                        break;
                    case 4:
                        rect = new Rectangle(364, 142, 85, 105);
                        break;
                    case 5:
                        rect = new Rectangle(453, 142, 95, 105);
                        break;
                    case 6:
                        rect = new Rectangle(551, 142, 91, 105);
                        break;
                    case 7:
                        rect = new Rectangle(639, 142, 95, 105);
                        break;
                    case 8:
                        rect = new Rectangle(736, 142, 92, 105);
                        break;
                    case 9:
                        rect = new Rectangle(830, 142, 92, 105);
                        break;
                    
                }

                sb.Draw(numberSpritesheet, new Rectangle(x+spacing, y, (int)(rect.Width*scale), (int)(rect.Height*scale)), rect, Color.White);
                spacing += (int)(rect.Width*scale) + 0;

            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState kb = Keyboard.GetState();

            if (state == GameState.TITLE_SCREEN)
            {
                if (kb.IsKeyDown(Keys.Space))
                {
                    state = GameState.TIME_SELECTION;
                }
            }
            else if (state == GameState.TIME_SELECTION)
            {
                clickTimes = 0;
                // Write code to handle the time selection
                if (kb.IsKeyDown(Keys.D1))
                {
                    
                    timeRemaining = 5 * 1000f;
                    state = GameState.PLAYING;
                }
                else if (kb.IsKeyDown(Keys.D2))
                {
                    timeRemaining = 10 * 1000f;
                    state = GameState.PLAYING;
                }
                else if (kb.IsKeyDown(Keys.D3))
                {
                    timeRemaining = 30 * 1000f;
                    state = GameState.PLAYING;
                }
            }
            else if (state == GameState.PLAYING)
            {
                
                //spacebar sprite   if space bar pressed sprite moves down
                if (keyDown && !kb.IsKeyDown(Keys.Space))
                    keyDown = false;

                if (!keyDown && kb.IsKeyDown(Keys.Space))
                {
                    clickTimes += 1;
                    keyDown = true;
                }
                
                
                timeRemaining -= (float)gameTime.ElapsedGameTime.Milliseconds;

                if (timeRemaining <= 0)
                {
                    state = GameState.GAMEOVER;
                }
            }
            else if (state == GameState.GAMEOVER)
            {
                highScore = clickTimes;
                
                if (keyDown && !kb.IsKeyDown(Keys.Space))
                    keyDown = false;

                if (!keyDown && kb.IsKeyDown(Keys.Space))
                {
                    state = GameState.SHOWSCORE;
                    keyDown = true;
                }
                
            }
            else if (state == GameState.SHOWSCORE)
            {
                
                if (kb.IsKeyDown(Keys.Enter))
                {                    
                    state = GameState.TIME_SELECTION;
                    
                    
                }
                else if (kb.IsKeyDown(Keys.H))
                {
                    state = GameState.HIGHSCORE;
                    
                    
                }
            }
            else if (state == GameState.HIGHSCORE)
            {
                if (keyDown && !kb.IsKeyDown(Keys.Space))
                    keyDown = false;

                if (!keyDown && kb.IsKeyDown(Keys.Space))
                {
                    state = GameState.TIME_SELECTION;
                    
                    
                }
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

            

            

            if (state == GameState.TITLE_SCREEN)            
            {
                spriteBatch.Begin();
                spriteBatch.Draw(titleScreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
                spriteBatch.End();
            }
            else if (state == GameState.TIME_SELECTION)
            {
                
                spriteBatch.Begin();
                spriteBatch.Draw(TimeSelection,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
          
                spriteBatch.End();

                
            }
            else if (state == GameState.PLAYING)
            {

                GraphicsDevice.Clear(Color.Goldenrod);
                spriteBatch.Begin();
                spriteBatch.Draw(Playing,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);

                //                      How much time remaining    x    y  scale number lengths
                DrawNumber(spriteBatch, (int)(timeRemaining/10f), 310, 400, 0.7f, 3);
                spriteBatch.End();
            }
            else if (state == GameState.GAMEOVER)
            {
                //background that says hit space to see score
                GraphicsDevice.Clear(Color.LemonChiffon);
                spriteBatch.Begin();
                spriteBatch.Draw(gameoverScreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
                 
                spriteBatch.End();
            }
            else if (state == GameState.SHOWSCORE)
            {
                GraphicsDevice.Clear(Color.LightCoral);
                spriteBatch.Begin();
                spriteBatch.Draw(Showscore,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
                DrawNumber(spriteBatch, clickTimes, 310, 200);
                spriteBatch.End();
            }
            else if (state == GameState.HIGHSCORE)
            {
                GraphicsDevice.Clear(Color.LightCoral);
                spriteBatch.Begin();
                spriteBatch.Draw(Showscore,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
                DrawNumber(spriteBatch, highScore, 310, 200);
                spriteBatch.End();
            }

            

            base.Draw(gameTime);
        }
    }
}
