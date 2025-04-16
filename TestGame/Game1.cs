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
                "1x300",
                "45443",
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

            Vector2 newPosition = _playerPosition;
            Vector2 velocity = _playerVelocity;

            if (keyboard.IsKeyDown(Keys.A))
                newPosition.X -= MoveSpeed;
            if (keyboard.IsKeyDown(Keys.D))
                newPosition.X += MoveSpeed;

            if (_isOnGround && (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Space)))
            {
                velocity.Y = JumpForce;
                _isOnGround = false;
            }

            velocity += _gravity;
            newPosition.Y += velocity.Y;

            Rectangle futureBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, PlayerSize, PlayerSize);

            _isOnGround = false;

            // Dolní kolize
            if (velocity.Y >= 0)
            {
                Rectangle feet = new Rectangle(futureBounds.X, futureBounds.Y + PlayerSize, PlayerSize, 1);
                if (IsColliding(feet))
                {
                    newPosition.Y = (float)Math.Floor((newPosition.Y + PlayerSize) / TileSize) * TileSize - PlayerSize;
                    velocity.Y = 0;
                    _isOnGround = true;
                }
            }

            // Horní kolize
            if (velocity.Y < 0)
            {
                Rectangle head = new Rectangle(futureBounds.X, futureBounds.Y, PlayerSize, 1);
                if (IsColliding(head))
                {
                    newPosition.Y = (float)Math.Floor(newPosition.Y / TileSize + 1) * TileSize;
                    velocity.Y = 0;
                }
            }

            // Levá kolize
            Rectangle left = new Rectangle(futureBounds.X, futureBounds.Y, 1, PlayerSize);
            if (IsColliding(left))
            {
                newPosition.X = (float)Math.Floor(newPosition.X / TileSize + 1) * TileSize;
            }

            // Pravá kolize
            Rectangle right = new Rectangle(futureBounds.X + PlayerSize, futureBounds.Y, 1, PlayerSize);
            if (IsColliding(right))
            {
                newPosition.X = (float)Math.Floor((newPosition.X + PlayerSize) / TileSize) * TileSize - PlayerSize;
            }

            _playerPosition = newPosition;
            _playerVelocity = velocity;

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
                        _map[y, x] = 0;
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
                throw new Exception($"Mapa musí obsahovat právě jedno 'x'. Nalezeno: {playerFound}");
        }

        private bool IsColliding(Rectangle area)
        {
            int startX = area.Left / TileSize;
            int endX = (area.Right - 1) / TileSize;
            int startY = area.Top / TileSize;
            int endY = (area.Bottom - 1) / TileSize;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (y >= 0 && y < _map.GetLength(0) && x >= 0 && x < _map.GetLength(1))
                    {
                        if (_map[y, x] != 0)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
