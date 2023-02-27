using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.TextureAtlases;
using MonoSnake.Game;

namespace MonoSnake;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private static SpriteFont _fontCourier;
    private Song _background;
   

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
        _fontCourier = Content.Load<SpriteFont>("Courier");
        _background = Content.Load<Song>("game_music");
      
        _world = new World(Content);
        
        MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        MediaPlayer.Play(this._background);
         
        MediaPlayer.IsRepeating = true;
        
        
        base.Initialize();
    }
    void MediaPlayer_MediaStateChanged(object sender, System.
        EventArgs e)
    {
        // 0.0f is silent, 1.0f is full volume
        MediaPlayer.Volume = 0.8f;
          
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _world.LoadContent(Content);
    }
    
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
        
        if (Keyboard.GetState().IsKeyDown(Keys.Space) && _world.GameOver)
        {
            _world.Reset();
        }
        
        _world.Update(gameTime);

        base.Update(gameTime);
    }

    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _spriteBatch.Begin();

        if (!_world.GameOver)
        {
            _world.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(_fontCourier, $"Score: {_world.Score}"  , new Vector2(1, 1), Color.Yellow, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
        
        }
        else
        {
            GraphicsDevice.Clear(Color.Red);
            _spriteBatch.DrawString(_fontCourier, $"Game over, score was: {_world.Score}."  , new Vector2(250, 250), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
            _spriteBatch.DrawString(_fontCourier, $"Press space to restart ..."  , new Vector2(250, 280), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);

        }
        _spriteBatch.End();
       
        base.Draw(gameTime);
    }

}