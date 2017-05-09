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
using System.Threading;
namespace AnimatedSprites
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private bool marioCaminando = false;
        private String ip = "148.226.81.145";

        int collisionRectOffsetBall = 1;
        int collisionRectOffsetMarioCaminando = 4;

        SoundEffect soundEffect;
        GraphicsDeviceManager graphics;
        SpriteBatch sprite;
         // Texture stuff

        // Asteroide
        Texture2D textureBall;
        Point frameSizeBall = new Point(60, 60);
        Point currentFrameBall = new Point(0, 0);
        Point sheetSizeBall = new Point(1, 4);
        int timeSinceLastFrameBall = 0;
        const int millisecondsPerFrameBall = 25;
        //Vector2 positionBall = new Vector2(100, 100);
       
        Vector2 positionDrawBall = new Vector2(600, 350);


        //Fondo
        Texture2D texture;
        Point frameSize = new Point(1200, 600);
        Point currentFrame = new Point(0, 0);
        Vector2 positionDraw = new Vector2(0, 0);

        // Mario
        /*
        Texture2D textureMarioCaminando;
        Point currentFrameMarioCaminando = new Point(0, 0);
        Point frameSizeMarioCaminando = new Point(17, 32);
        int timeSinceLastFrameMarioCaminando = 0;
        const int millisecondsPerFrameMarioCaminando = 30;
        Point sheetSizeMarioCaminando = new Point(1, 4);
        Vector2 positionDrawMario = new Vector2(100, 150);
        const float speedMarioCaminando = 5;
         */
        Texture2D textureMarioCaminando;
        Point currentFrameMarioCaminando = new Point(0, 0);
        Point frameSizeMarioCaminando = new Point(17, 32);
        int timeSinceLastFrameMarioCaminando = 0;
        const int millisecondsPerFrameMarioCaminando = 30;
        Point sheetSizeMarioCaminando = new Point(3, 1);
        Vector2 positionDrawMario = new Vector2(100, 150);
        const float speedMarioCaminando = 5;

        Texture2D textureMarioDetenido;
        Point currentFrameMarioDetenido = new Point(0, 0);
        Point frameSizeMarioDetenido = new Point(17, 32);
        int timeSinceLastFrameMarioDetenido = 0;
        const int millisecondsPerFrameMarioDetenido = 30;
        Point sheetSizeMarioDetenido = new Point(1, 1);

        /*

        Texture2D textureMarioDetenido;
        Point currentFrameMarioDetenido = new Point(0, 0);  
        Point frameSizeMarioDetenido = new Point(17, 32);
        int timeSinceLastFrameMarioDetenido = 0;
        const int millisecondsPerFrameMarioDetenido = 30;
        Point sheetSizeMarioDetenido = new Point(1, 1);
         */ 
       
        const float speedMarioDetenido = 5;

        // Framerate stuff
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 50;

        //Control de botones
        GamePadState gamepadLast;
        KeyboardState keyboardLast;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1200;
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
            
            sprite = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>(@"images\Background");
            textureMarioCaminando = Content.Load<Texture2D>(@"images\marioSprite1");
            textureMarioDetenido = Content.Load<Texture2D>(@"images\marioStatic");
            textureBall = Content.Load<Texture2D>(@"images\asset_0028");
            soundEffect = Content.Load<SoundEffect>(@"sounds\space");
            soundEffect.Play();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Console.WriteLine("Saliendo");
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Animación inicial
        /// </summary>
        /// <param name="gameTime"></param>
        /// <summary>
        /// Explosivo
        /// </summary>
        /// <param name="gameTime"></param>
        private void Ball(GameTime gameTime)
        {
            // ball animation and framerate stuff
            timeSinceLastFrameBall += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrameBall > millisecondsPerFrameBall)
            {
                timeSinceLastFrameBall -= millisecondsPerFrameBall;
                // Advance to the next frame
                ++currentFrameBall.X;
                if (currentFrameBall.X >= sheetSizeBall.X)
                {
                    currentFrameBall.X = 0;
                    ++currentFrameBall.Y;
                    if (currentFrameBall.Y >= sheetSizeBall.Y)
                        currentFrameBall.Y = 0;
                }
            }
        }


        private Vector2 ValidarObjeto(Vector2 obj, Point frameSize)
        {
            // If threerings is off the screen, move it back into the game window
            if (obj.X < 0)
                obj.X = 0;
            if (obj.Y < 0)
                obj.Y = 0;
#if WINDOWS
            if (obj.X > Window.ClientBounds.Width - frameSize.X)
                obj.X = Window.ClientBounds.Width - frameSize.X;
            if (obj.Y > Window.ClientBounds.Height - frameSize.Y)
                obj.Y = Window.ClientBounds.Height - frameSize.Y;
#else
                if (obj.X >  (graphics.GraphicsDevice.Viewport.Width-20) - frameSize.X)
                    obj.X = (graphics.GraphicsDevice.Viewport.Width-20) - frameSize.X;
                if (obj.Y > (graphics.GraphicsDevice.Viewport.Height-20) - frameSize.Y)
                    obj.Y = (graphics.GraphicsDevice.Viewport.Height-20) - frameSize.Y;
   
#endif
            return obj;
        }

        /// <summary>
        /// MarioNormal....
        /// </summary>
        /// <param name="gameTime"></param>
        private void MarioCaminando(GameTime gameTime)
        {
            timeSinceLastFrameMarioCaminando += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrameMarioCaminando > millisecondsPerFrameMarioCaminando)
            {
                timeSinceLastFrameMarioCaminando -= millisecondsPerFrameMarioCaminando;

                ++currentFrameMarioCaminando.X;
                if (currentFrameMarioCaminando.X >= sheetSizeMarioCaminando.X)
                {
                    currentFrameMarioCaminando.X = 0;
                    ++currentFrameMarioCaminando.Y;
                    if (currentFrameMarioCaminando.Y >= sheetSizeMarioCaminando.Y)
                        currentFrameMarioCaminando.Y = 0;
                }
            }
            positionDrawMario = ValidarObjeto(positionDrawMario, frameSizeMarioCaminando);
        }

        private void MarioDetenido(GameTime gameTime)
        {
            timeSinceLastFrameMarioDetenido += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrameMarioDetenido > millisecondsPerFrameMarioDetenido)
            {
                timeSinceLastFrameMarioDetenido -= millisecondsPerFrameMarioDetenido;

                ++currentFrameMarioDetenido.X;
                if (currentFrameMarioDetenido.X >= sheetSizeMarioDetenido.X)
                {
                    currentFrameMarioDetenido.X = 0;
                    ++currentFrameMarioDetenido.Y;
                    if (currentFrameMarioDetenido.Y >= sheetSizeMarioDetenido.Y)
                        currentFrameMarioDetenido.Y = 0;
                }
            }
            positionDrawMario = ValidarObjeto(positionDrawMario, frameSizeMarioDetenido);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                this.Exit();

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
               
            }

            if (marioCaminando)
            {
                MarioCaminando(gameTime);
            }
            else
            {
                MarioDetenido(gameTime);
            }
            Ball(gameTime);
            if (Collide())
                Exit();
         
            // Move threerings based on keyboard input
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed
                || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                marioCaminando = true;
                positionDrawMario.X -= speedMarioCaminando;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
            {

                marioCaminando = true;
                positionDrawMario.X += speedMarioCaminando;
            }
            else if (keyboardState.IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
            {

                marioCaminando = true;
                positionDrawMario.Y -= speedMarioCaminando;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
            {
                marioCaminando = true;
                positionDrawMario.Y += speedMarioCaminando;
            }
            else
            {
                marioCaminando = false;
            }
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (gamepadState.Buttons.A == ButtonState.Pressed)
            {
                gamepadLast = gamepadState;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (!keyboardLast.IsKeyDown(Keys.A))
                {
                    Encender();
                    Console.WriteLine("Imprimir A");
                }
                keyboardLast = keyboardState;
            }

            if ((gamepadLast.Buttons.A == ButtonState.Released && gamepadState.Buttons.A == ButtonState.Pressed))
            {
                    GamePad.SetVibration(PlayerIndex.One, 1f, 1f);

                    Encender();
            }
            else
            {
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }

            keyboardLast = keyboardState;
            gamepadLast = gamepadState;
           
            base.Update(gameTime);
        }

        private void AnimationSprite()
        {
            sprite.Draw(texture, positionDraw,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X,
                    frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);
        }


        private void Encender()
        {
            ClienteUtils c = new ClienteUtils();
            Thread t = new Thread(delegate()
            {
                try
                {
                    //192,168,43,91
                  //  ClienteUtils.Send("192.168.1.177", 80, "5\n\n");
                    ClienteUtils.Send(ip, 80, "5\n\n");
                    
                    Console.WriteLine("Enviando paquete....");
                }
                catch { }
            });
            t.Start();
        }

        private void MarioCaminandoSprite()
        {
            sprite.Draw(textureMarioCaminando, positionDrawMario,
                new Rectangle(currentFrameMarioCaminando.X * frameSizeMarioCaminando.X,
                    currentFrameMarioCaminando.Y * frameSizeMarioCaminando.Y,
                    frameSizeMarioCaminando.X,
                    frameSizeMarioCaminando.Y),
                Color.White, 0, Vector2.Zero,
               2, SpriteEffects.None, 0);
        }

        private void MarioDetenidoSprite()
        {
            sprite.Draw(textureMarioDetenido, positionDrawMario,
                new Rectangle(currentFrameMarioDetenido.X * frameSizeMarioDetenido.X,
                    currentFrameMarioDetenido.Y * frameSizeMarioDetenido.Y,
                    frameSizeMarioDetenido.X,
                    frameSizeMarioDetenido.Y),
                Color.White, 0, Vector2.Zero,
               2, SpriteEffects.None, 0);
        }
        private void BallSprite()
        {
            sprite.Draw(textureBall, positionDrawBall,
                new Rectangle(currentFrameBall.X * frameSizeBall.X,
                    currentFrameBall.Y * frameSizeBall.Y,
                    frameSizeBall.X,
                    frameSizeBall.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            AnimationSprite();
            BallSprite();
            if (marioCaminando)
                MarioCaminandoSprite();
            else
                MarioDetenidoSprite();
            sprite.End();
            base.Draw(gameTime);
        }

        protected bool Collide()
        {
            Rectangle marioRect = new Rectangle(
                (int)positionDrawMario.X + collisionRectOffsetMarioCaminando,
                (int)positionDrawMario.Y + collisionRectOffsetMarioCaminando,
                frameSizeMarioCaminando.X - (collisionRectOffsetMarioCaminando * 2),
                frameSizeMarioCaminando.Y - (collisionRectOffsetMarioCaminando * 2));
            Rectangle ballRect = new Rectangle(
                (int)positionDrawBall.X + collisionRectOffsetBall,
                (int)positionDrawBall.Y + collisionRectOffsetBall,
                frameSizeBall.X - (collisionRectOffsetBall * 2),
                frameSizeBall.Y - (collisionRectOffsetBall * 2));

            return marioRect.Intersects(ballRect);

        }

      

    }
}
