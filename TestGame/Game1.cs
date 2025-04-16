using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _playerTexture;
        private Rectangle _sourceRect;  // Výřez 8x8 z levého horního rohu
        private Vector2 _playerPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Umístění hráče někde na obrazovce
            _playerPosition = new Vector2(100, 100);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Načteme spritesheet
            _playerTexture = Content.Load<Texture2D>("player");

            // Výřez 8x8 z levého horního rohu
            _sourceRect = new Rectangle(0, 0, 8, 8);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Vykresli hráče
            _spriteBatch.Draw(
                _playerTexture,
                _playerPosition,
                _sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                5f, // zvětšíme 5x pro viditelnost
                SpriteEffects.None,
                0f
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}