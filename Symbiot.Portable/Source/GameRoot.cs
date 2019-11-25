using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

using MGCore.UI;
using Symbiot.Portable.Source.Controllers;

namespace Symbiot.Portable.Source
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Game
    {
        public static GameRoot Instance;

        public enum Platform
        { PC, Android, IOS }
        public static Platform CurrentPlatform { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private InputController inputController;
        private UIManager uiManager;
        private GameController gameController;

        public delegate void Delegate_OnDraw(SpriteBatch spriteBatch);
        public delegate void Delegate_OnUpdate(GameTime gameTime);
        public delegate void Delegate_OnLoadContent(Game game);

        public Delegate_OnDraw OnDraw;
        public Delegate_OnUpdate OnUpdate;
        public Delegate_OnLoadContent OnLoad;

        public GameRoot(Platform platform)
        {
            Instance = this;
            CurrentPlatform = platform;
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
            if (CurrentPlatform == Platform.Android || CurrentPlatform == Platform.IOS)
            {
                graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                graphics.IsFullScreen = true;
            }
            else
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferHeight = 1000;
                graphics.PreferredBackBufferWidth = 1000 * 9 / 16;
            }

            graphics.ApplyChanges();

            inputController = new InputController();
            uiManager = new UIManager();
            gameController = new GameController();

            

            this.IsMouseVisible = true;

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

            OnLoad(this);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            inputController.OnUpdade();

            OnUpdate(gameTime);

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
            OnDraw(spriteBatch);
            uiManager.OnDraw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
