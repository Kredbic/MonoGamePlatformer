using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TestGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _spriteSheet;

        private const int TileSize = 8;
        private const int SpriteOffsetY = 8;

        // === Hráč ===
        private Vector2 _playerPosition;
        private Vector2 _playerVelocity;
        private Vector2 _gravity;
        private bool _isOnGround;
        private const int PlayerSize = 8;
        private const float MoveSpeed = 2.5f;
        private const float JumpForce = -6f;
        private const float GravityForce = 0.4f;

        // === Mapa ===
        private int[,] _map;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _gravity = new Vector2(0, GravityForce);
            _playerVelocity = Vector2.Zero;

            string[] level = {
                "00000",
                "13x00",
                "45223",
                "78889"
            };

            LoadMapFromStringArray(level);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteSheet = Content.Load<Texture2D>("player");
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboard.IsKeyDown(Keys.A))
                _playerPosition.X -= MoveSpeed;
            if (keyboard.IsKeyDown(Keys.D))
                _playerPosition.X += MoveSpeed;

            if (_isOnGround && (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Space)))
            {
                _playerVelocity.Y = JumpForce;
                _isOnGround = false;
            }

            _playerVelocity += _gravity;
            _playerPosition += new Vector2(0, _playerVelocity.Y);

            float screenHeight = _graphics.PreferredBackBufferHeight;
            if (_playerPosition.Y + PlayerSize >= screenHeight)
            {
                _playerPosition.Y = screenHeight - PlayerSize;
                _playerVelocity.Y = 0;
                _isOnGround = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            for (int y = 0; y < _map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.GetLength(1); x++)
                {
                    int tileId = _map[y, x];
                    if (tileId > 0)
                    {
                        Rectangle src = GetSourceRectFromTileId(tileId);
                        Vector2 pos = new Vector2(x * TileSize, y * TileSize);
                        _spriteBatch.Draw(_spriteSheet, pos, src, Color.White);
                    }
                }
            }

            Rectangle playerSrc = new Rectangle(0, 0, 8, 8);
            _spriteBatch.Draw(_spriteSheet, _playerPosition, playerSrc, Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private Rectangle GetSourceRectFromTileId(int id)
        {
            int index = id - 1;
            int col = index % 3;
            int row = index / 3;
            return new Rectangle(col * TileSize, SpriteOffsetY + row * TileSize, TileSize, TileSize);
        }

        private void LoadMapFromStringArray(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;
            _map = new int[rows, cols];

            int playerFound = 0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    char c = lines[y][x];

                    if (c == 'x')
                    {
                        _playerPosition = new Vector2(x * TileSize, y * TileSize);
                        _map[y, x] = 0; // není to tile
                        playerFound++;
                    }
                    else if (char.IsDigit(c))
                    {
                        _map[y, x] = int.Parse(c.ToString());
                    }
                    else
                    {
                        _map[y, x] = 0;
                    }
                }
            }

            if (playerFound != 1)
            {
                throw new Exception($"Mapa musí obsahovat právě jedno 'x' (hráč). Nalezeno: {playerFound}");
            }
        }
    }
}
