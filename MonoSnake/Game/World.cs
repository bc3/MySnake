using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace MonoSnake.Game;

public class World
{
    private List<Position> lastPositions = new List<Position>();

    Texture2D food;
    Texture2D empty;
    Texture2D body;
    Texture2D head;

    private Position foodPosition ;
    private Position snakePosition;
    private Direction snakeDirection;

    public World(ContentManager contentManager)
    {
        food = contentManager.Load<Texture2D>("Food");
        empty = contentManager.Load<Texture2D>("Empty");
        head = contentManager.Load<Texture2D>("Head");
        body = contentManager.Load<Texture2D>("Body");
        Reset();
    }


    private double elapsedTime = 0;
    private int speed;
    public int score;
    public bool gameOver;
    public void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (elapsedTime > speed)
        {
            var oldPosition = new Position(snakePosition.X, snakePosition.Y);
            elapsedTime = 0;
            switch (snakeDirection)
            {
                case Direction.North:
                    snakePosition.Y--;
                    break;
                case Direction.South:
                    snakePosition.Y++;
                    break;
                case Direction.East:
                    snakePosition.X++;
                    break;
                case Direction.West:
                    snakePosition.X--;
                    break;
            }
            
            if (lastPositions.Any(x => x.X == snakePosition.X && x.Y == snakePosition.Y))
            {
                gameOver = true;
            }
            
            if (snakePosition.X < 0 || snakePosition.X >= 50 || snakePosition.Y < 0 || snakePosition.Y >= 50)
            {
                gameOver = true;
            }
            
            if (snakePosition.X == foodPosition.X && snakePosition.Y == foodPosition.Y)
            {
                score += 1;
                foodPosition = new Position(50);
                lastPositions.Add(oldPosition);
            }
            lastPositions.Add(new Position(snakePosition.X, snakePosition.Y));
            if (lastPositions.Count > 1)
            {
                lastPositions.RemoveAt(0);
            }
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // spriteBatch.DrawString(Game1.fontCourier, $"{snakePosition} {string.Join(",",lastPositions)}"  , new Vector2(1, 20), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);

        
        var height = 20;
        for (var x = 0; x < 50; x++)
        {
            for (var y = 0; y < 50; y++)
            {
                spriteBatch.Draw(empty, new Rectangle(x * height, y * height, height, height), Color.White);
            }
        }
        
        spriteBatch.Draw(food, new Rectangle(foodPosition.X * height, foodPosition.Y * height, height, height), Color.White);
        
        // spriteBatch.Draw(head, new Rectangle(snakePosition.X* height, snakePosition.Y * height, height, height), null,
        //     Color.White, snakeDirection.ToFloat(), new Vector2(height / 2, height / 2),
        //     SpriteEffects.None, 0);

        foreach (var s in lastPositions)
        {
            spriteBatch.Draw(body, new Rectangle(s.X * height, s.Y * height, height, height), Color.White);

        }

        spriteBatch.Draw(head, new Rectangle(snakePosition.X * height, snakePosition.Y * height, height, height), Color.White);

    }

    public void MoveRight()
    {
        snakeDirection = Direction.East;
    }

    public void MoveLeft()
    {
        snakeDirection = Direction.West;
    }

    public void MoveUp()
    {
        snakeDirection = Direction.North;
    }

    public void MoveDown()
    {
        snakeDirection = Direction.South;
    }
    
    public void SpeedDown()
    {
       speed+=10;
    }
    
    public void SpeedUp()
    {
        if (speed > 10)
        {
            speed-=10;
        }
    }

    public void Reset()
    {
        score = 0;
        speed = 200;
        gameOver = false;
        foodPosition = new Position(50);
        snakePosition = new Position(50);
        snakeDirection = Direction.North;
        lastPositions = new List<Position>();
    }
}