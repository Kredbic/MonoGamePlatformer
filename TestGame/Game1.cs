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

        private const float PlayerScale = 5f;
        private const int SpriteSize = 8;

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
            _gravity = new Vector2(0, 0.5f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerTexture = Content.Load<Texture2D>("player");
            _sourceRect = new Rectangle(0, 0, SpriteSize, SpriteSize);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _playerVelocity += _gravity;
            _playerPosition += _playerVelocity;

            // Hranice okna
            float screenWidth = _graphics.PreferredBackBufferWidth;
            float screenHeight = _graphics.PreferredBackBufferHeight;
            float playerSize = SpriteSize * PlayerScale;

            // Dolní okraj
            if (_playerPosition.Y + playerSize > screenHeight)
            {
                _playerPosition.Y = screenHeight - playerSize;
                _playerVelocity.Y = 0;
            }

            // Horní okraj
            if (_playerPosition.Y < 0)
            {
                _playerPosition.Y = 0;
                _playerVelocity.Y = 0;
            }

            // Pravý okraj
            if (_playerPosition.X + playerSize > screenWidth)
            {
                _playerPosition.X = screenWidth - playerSize;
                _playerVelocity.X = 0;
            }

            // Levý okraj
            if (_playerPosition.X < 0)
            {
                _playerPosition.X = 0;
                _playerVelocity.X = 0;
            }

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
                PlayerScale,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
