using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using MonoSnake.Game;

namespace MonoSnake;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static SpriteFont fontCourier;

    private World _world;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1000;
        _graphics.PreferredBackBufferHeight = 1000;
        Content.RootDirectory = "Content";
      
     
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Console.WriteLine("Going to start game");
        fontCourier = Content.Load<SpriteFont>("Courier");
        _world = new World(Content);
        
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
      

        // TODO: use this.Content to load your game content here
    }
    
    private string dir;


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _world.MoveRight();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _world.MoveLeft();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _world.MoveUp();
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _world.MoveDown();
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            _world.SpeedDown();
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.U))
        {
            _world.SpeedUp();
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && _world.gameOver)
        {
            _world.Reset();
        }
        
        _world.Update(gameTime);

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _spriteBatch.Begin();

        if (!_world.gameOver)
        {
            _world.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(fontCourier, $"Score: {_world.score}"  , new Vector2(1, 1), Color.Yellow, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
        
        }
        else
        {
            GraphicsDevice.Clear(Color.Red);
            _spriteBatch.DrawString(fontCourier, $"Game over, score was: {_world.score}."  , new Vector2(250, 250), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            _spriteBatch.DrawString(fontCourier, $"Press space to restart ..."  , new Vector2(250, 280), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);

        }
        _spriteBatch.End();

       
        base.Draw(gameTime);
    }
    

}