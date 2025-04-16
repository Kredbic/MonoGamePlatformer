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
        private const float MoveSpeed = 3f;
        private const float JumpForce = -10f;

        private bool _isOnGround;

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
            _isOnGround = false;

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
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Pohyb do stran
            if (keyboardState.IsKeyDown(Keys.A))
                _playerPosition.X -= MoveSpeed;
            if (keyboardState.IsKeyDown(Keys.D))
                _playerPosition.X += MoveSpeed;

            // Skákání – jednorázové nastavení velocity, pouze pokud je hráč na zemi
            if (_isOnGround && (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space)))
            {
                _playerVelocity.Y = JumpForce;
                _isOnGround = false;
            }

            // Aplikace gravitace
            _playerVelocity += _gravity;
            _playerPosition += new Vector2(0, _playerVelocity.Y);

            // Výpočty pro velikost a hranice
            float screenWidth = _graphics.PreferredBackBufferWidth;
            float screenHeight = _graphics.PreferredBackBufferHeight;
            float playerSize = SpriteSize * PlayerScale;

            // Kolize se spodní hranou
            if (_playerPosition.Y + playerSize >= screenHeight)
            {
                _playerPosition.Y = screenHeight - playerSize;
                _playerVelocity.Y = 0;
                _isOnGround = true;
            }

            // Horní hrana
            if (_playerPosition.Y < 0)
            {
                _playerPosition.Y = 0;
                _playerVelocity.Y = 0;
            }

            // Levá a pravá hrana
            if (_playerPosition.X < 0)
                _playerPosition.X = 0;
            if (_playerPosition.X + playerSize > screenWidth)
                _playerPosition.X = screenWidth - playerSize;

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
