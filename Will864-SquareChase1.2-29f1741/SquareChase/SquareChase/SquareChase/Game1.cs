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

namespace SquareChase
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        Song soundEngine;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private Vector2 origin;
        private Vector2 screenpos;
        private MouseState previousMouseState;

        private float RotationAngle;
        enum GameStates { TitleScreen, Playing };
        GameStates gameState = GameStates.Playing;

        Random rand = new Random();
        Texture2D titleScreen;
        Texture2D squareTexture;
        Rectangle currentSquare;
        int playerScore = 0;
        float timeRemaining = 0.0f;
        float TimePerSquare = 2f;

        Color[] colors = new Color[3] { Color.Red, Color.Green, Color.Blue };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // graphics.IsFullScreen = true;   <---------add this for full screen
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
            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// private Texture2D SpriteTexture;



        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            squareTexture = Content.Load<Texture2D>(@"SQUARE");
            soundEngine = Content.Load<Song>(@"chippy");

            Viewport viewport = graphics.GraphicsDevice.Viewport;
            origin.X = squareTexture.Width / 2;
            origin.Y = squareTexture.Height / 2;
            screenpos.X = viewport.Width / 2;
            screenpos.Y = viewport.Height / 2;

            font = Content.Load<SpriteFont>("SpriteFont1");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(soundEngine);

            titleScreen = Content.Load<Texture2D>(@"TitleScreen");


            // TODO: use this.Content to load your game content here
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
        /// 

        //public bool MouseWithinRotatedRectangle(Rectangle area, Vector2 mousePosition, float angleRotation)
        //{
        //    var newMousePos = mousePosition - new Vector2(area.Location.X, area.Location.Y);
        //    newMousePos = new Vector2((float)(Math.Cos(-angleRotation) * newMousePos.X - Math.Sin(-angleRotation) * newMousePos.Y),
        //                              (float)(Math.Sin(-angleRotation) * newMousePos.X + Math.Cos(-angleRotation) * newMousePos.Y));
        //    newMousePos += new Vector2(area.Left + area.Width / 2, area.Top + area.Height / 2);
        //    return area.Contains((int)newMousePos.X, (int)newMousePos.Y);
        //}


        public bool MouseWithinRotatedRectangle(Rectangle rect, Vector2 mousePosition, float angleRotation)
        {
            var newMousePos = mousePosition - new Vector2(rect.Location.X, rect.Location.Y);
            newMousePos = new Vector2((float)(Math.Cos(-angleRotation) * newMousePos.X - Math.Sin(-angleRotation) * newMousePos.Y),
                                      (float)(Math.Sin(-angleRotation) * newMousePos.X + Math.Cos(-angleRotation) * newMousePos.Y));
            newMousePos += new Vector2(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            return rect.Contains((int)newMousePos.X, (int)newMousePos.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            RotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

           // RotationAngle = MathHelper.PiOver4 / 2;


            MouseState mouse = Mouse.GetState();
            // TODO: Add your update logic here

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        playerScore = 0;
                        gameState = GameStates.Playing;
                    }
                    break;

                case GameStates.Playing:

                    //for the placement and size of the Sqaures

                    //currentSquare = new Rectangle(
                    //    55,
                    //    55,
                    //    75, 75);
                    //timeRemaining = TimePerSquare;

                    //if score is below 4
                    if (timeRemaining == 0.0f && playerScore <= 3)
                    {
                        currentSquare = new Rectangle(
                            rand.Next(0, this.Window.ClientBounds.Width - 55),
                            rand.Next(0, this.Window.ClientBounds.Height - 55),
                            75, 75);
                        timeRemaining = TimePerSquare;
                    }
                    //if score is below 7
                    if (timeRemaining == 0.0f && playerScore > 3 && playerScore <= 6)
                    {
                        currentSquare = new Rectangle(
                            rand.Next(0, this.Window.ClientBounds.Width - 55),
                            rand.Next(0, this.Window.ClientBounds.Height - 55),
                            55, 55);
                        timeRemaining = TimePerSquare - (TimePerSquare * .50f);
                    }
                    //if score is below 9
                    if (timeRemaining == 0.0f && playerScore > 6 && playerScore <= 9)
                    {
                        currentSquare = new Rectangle(
                            rand.Next(0, this.Window.ClientBounds.Width - 55),
                            rand.Next(0, this.Window.ClientBounds.Height - 55),
                            25, 25);
                        timeRemaining = TimePerSquare - (TimePerSquare * .70f);
                    }
                    //if the score is above 9
                    if (timeRemaining == 0.0f && playerScore > 9)
                    {
                        currentSquare = new Rectangle(
                            rand.Next(0, this.Window.ClientBounds.Width - 55),
                            rand.Next(0, this.Window.ClientBounds.Height - 55),
                            5, 5);
                        timeRemaining = TimePerSquare;
                    }

                    //for the mouse clicks
                    //MouseState mouse = Mouse.GetState();
                    //if (MouseWithinRotatedRectangle(currentSquare, new Vector2(mouse.X, mouse.Y), RotationAngle) && mouse.LeftButton == ButtonState.Pressed)

                    if (MouseWithinRotatedRectangle(currentSquare, new Vector2(mouse.X, mouse.Y), RotationAngle) && mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {

                        playerScore++;
                        timeRemaining = 0.0f;
                    }
                    /*
                        if (playerScore > 3 && playerScore <= 6)
                        {
                            currentSquare = new Rectangle(
                                rand.Next(0, this.Window.ClientBounds.Width - 55),
                                rand.Next(0, this.Window.ClientBounds.Height - 55),
                                55, 55);
                            timeRemaining = TimePerSquare - (TimePerSquare * .50f);
                        }

                        if (playerScore > 6 && playerScore <= 9)
                        {
                            currentSquare = new Rectangle(rand.Next(0, this.Window.ClientBounds.Width - 55),
                            rand.Next(0, this.Window.ClientBounds.Height - 55),
                            25, 25);
                            timeRemaining = TimePerSquare - (TimePerSquare * .70f);
                        }

                        else if (playerScore > 9)
                        {
                            currentSquare = new Rectangle(rand.Next(0, this.Window.ClientBounds.Width - 55),
                            rand.Next(0, this.Window.ClientBounds.Height - 55),
                            5, 5);
                            timeRemaining = TimePerSquare;
                        }
                    */
                    

                    timeRemaining = MathHelper.Max(0, timeRemaining -
                        (float)gameTime.ElapsedGameTime.TotalSeconds);



                    this.Window.Title = "Score : " + playerScore.ToString();

                    break;
            }
            previousMouseState = mouse;
            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            origin.X = squareTexture.Width / 2;
            origin.Y = squareTexture.Height / 2;

            //origin.X = 0;
            //origin.Y = 0;

            // TODO: Add your drawing code here



            if (gameState == GameStates.Playing)
            {
                //spriteBatch.Begin();
                //spriteBatch.Draw(squareTexture, currentSquare, colors[playerScore % 3]);


                spriteBatch.Begin();
                spriteBatch.Draw(squareTexture, currentSquare, null, colors[playerScore % 3], RotationAngle, origin, 0, 0);


                //For when the player wins the game....Hoooray!!!!!
                if (playerScore > 9)
                {
                    spriteBatch.DrawString(font, "You Win HOme Slice@!", new Vector2(5.0f, 1.0f), Color.White);
                }

                spriteBatch.End();
            }
            if (gameState == GameStates.TitleScreen)
            {
                if (gameState == GameStates.TitleScreen)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(titleScreen,
                    new Rectangle(0, 0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height),
                    Color.White);

                    spriteBatch.End();
                }
            }
            base.Draw(gameTime);
        }


    }
}
