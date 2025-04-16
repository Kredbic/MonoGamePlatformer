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
        private Rectangle _sourceRect;

        private Vector2 _playerPosition;
        private Vector2 _playerVelocity;
        private Vector2 _gravity;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _playerPosition = new Vector2(100, 100);
            _playerVelocity = Vector2.Zero;
            _gravity = new Vector2(0, 0.5f); // gravitace dolů

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerTexture = Content.Load<Texture2D>("player");
            _sourceRect = new Rectangle(0, 0, 8, 8);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Aplikace gravitace na rychlost
            _playerVelocity += _gravity;

            // Aplikace rychlosti na pozici
            _playerPosition += _playerVelocity;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(
                _playerTexture,
                _playerPosition,
                _sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                5f,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
