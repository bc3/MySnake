using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace MonoSnake.Game;

public class World
{
    private List<Position> _lastPositions;
    private List<Position> _walls;

    Texture2D _food;
    Texture2D _empty;
    Texture2D _body;
    Texture2D _head;

    private Position _foodPosition ;
    private Position _snakePosition;
    private Direction _snakeDirection;

    private double _elapsedTime;
    private int _speed;
    public int Score;
    public bool GameOver;
    private int _max;
    
    private List<SoundEffect> _soundEffects = new List<SoundEffect>();

    public World(ContentManager contentManager)
    {
        Reset();
    }

    public void LoadContent(ContentManager contentManager)
    {
        _food = contentManager.Load<Texture2D>("Food");
        _empty = contentManager.Load<Texture2D>("Empty");
        _head = contentManager.Load<Texture2D>("Head");
        _body = contentManager.Load<Texture2D>("Body");
        _soundEffects.Add(contentManager.Load<SoundEffect>("apple_bite.ogg"));
        _soundEffects.Add(contentManager.Load<SoundEffect>("qubodup-crash"));
    }


    public void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_elapsedTime > _speed)
        {
            var oldPosition = new Position(_snakePosition.X, _snakePosition.Y);
            _elapsedTime = 0;
            switch (_snakeDirection)
            {
                case Direction.North:
                    _snakePosition.Y--;
                    break;
                case Direction.South:
                    _snakePosition.Y++;
                    break;
                case Direction.East:
                    _snakePosition.X++;
                    break;
                case Direction.West:
                    _snakePosition.X--;
                    break;
            }
            
            if (_lastPositions.Any(x => x.X == _snakePosition.X && x.Y == _snakePosition.Y))
            {
                _soundEffects[1].Play(0.75f, 0.0f, 0.0f);
                GameOver = true;
            }
            
            if (_walls.Any(x => x.X == _snakePosition.X && x.Y == _snakePosition.Y))
            {
                _soundEffects[1].Play(0.75f, 0.0f, 0.0f);
                GameOver = true;
            }
            
            if (_snakePosition.X < 0 || _snakePosition.X >= 50 || _snakePosition.Y < 0 || _snakePosition.Y >= 50)
            {
                _soundEffects[1].Play(0.75f, 0.0f, 0.0f);
                GameOver = true;
            }
            
            if (_snakePosition.X == _foodPosition.X && _snakePosition.Y == _foodPosition.Y)
            {
                Score += 1;
                AddFood();
                _lastPositions.Add(oldPosition);
                _soundEffects[0].Play(0.75f, 0.0f, 0.0f);
            }
            _lastPositions.Add(new Position(_snakePosition.X, _snakePosition.Y));
            if (_lastPositions.Count > 1)
            {
                _lastPositions.RemoveAt(0);
            }
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // spriteBatch.DrawString(Game1.fontCourier, $"{snakePosition} {string.Join(",",lastPositions)}"  , new Vector2(1, 20), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);

        
        var height = 1000 / _max;
        for (var x = 0; x < _max; x++)
        {
            for (var y = 0; y < _max; y++)
            {
                spriteBatch.Draw(_empty, new Rectangle(x * height, y * height, height, height), Color.White);
            }
        }
        
        spriteBatch.Draw(_food, new Rectangle(_foodPosition.X * height, _foodPosition.Y * height, height, height), Color.White);
        
        // spriteBatch.Draw(head, new Rectangle(snakePosition.X* height, snakePosition.Y * height, height, height), null,
        //     Color.White, snakeDirection.ToFloat(), new Vector2(height / 2, height / 2),
        //     SpriteEffects.None, 0);

        foreach (var s in _lastPositions)
        {
            spriteBatch.Draw(_body, new Rectangle(s.X * height, s.Y * height, height, height), Color.White);

        }

        foreach (var s in _walls)
        {
            spriteBatch.FillRectangle(new Rectangle(s.X * height, s.Y * height, height, height), new Color(63,58,86));

        }

        spriteBatch.Draw(_head, new Rectangle(_snakePosition.X * height, _snakePosition.Y * height, height, height), Color.White);

    }

    public void MoveRight()
    {
        _snakeDirection = Direction.East;
    }

    public void MoveLeft()
    {
        _snakeDirection = Direction.West;
    }

    public void MoveUp()
    {
        _snakeDirection = Direction.North;
    }

    public void MoveDown()
    {
        _snakeDirection = Direction.South;
    }
    
    public void SpeedDown()
    {
       _speed+=10;
    }
    
    public void SpeedUp()
    {
        if (_speed > 10)
        {
            _speed-=10;
        }
    }

    public void Reset()
    {
        Score = 0;
        _speed = 200;
        GameOver = false;
        _max = 50;
        _elapsedTime = 0;
        AddWalls();
        AddFood();
        AddSnake();
    }

    private void AddSnake()
    {
        var snakeAdded = false;
        while(!snakeAdded)
        {
            var pos = new Position(_max);
            if (!_walls.Any(x => x.X == pos.X && x.Y == pos.Y))
            {
                _snakePosition = pos;
                snakeAdded = true;
            }
        }
        _snakeDirection = Direction.North;
        _lastPositions = new List<Position>();
    }

    private void AddWalls()
    {
        _walls = new List<Position>();
        int i = 0;
        while (i <= _max * 2)
        {
            var pos = new Position(_max);
            if (!_walls.Any(x => x.X == pos.X && x.Y == pos.Y))
            {
                _walls.Add(pos);
                i++;
            }
        }
    }

    private void AddFood()
    {
        var foodAdded = false;
        while(!foodAdded)
        {
            var pos = new Position(_max);
            if (!_walls.Any(x => x.X == pos.X && x.Y == pos.Y))
            {
                _foodPosition = pos;
                foodAdded = true;
            }
        }
    }
}