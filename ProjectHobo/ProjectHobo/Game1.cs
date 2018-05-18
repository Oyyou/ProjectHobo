using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectHobo.Models;
using ProjectHobo.Sprites;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace ProjectHobo
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private BasicEffect _spriteBatchEffect;

    private World _world;

    private List<Sprite> _sprites;

    private Matrix _projection;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      _spriteBatchEffect = new BasicEffect(graphics.GraphicsDevice);
      _spriteBatchEffect.TextureEnabled = true;

      _world = new World();

      var square = Content.Load<Texture2D>("Square");

      _sprites = new List<Sprite>();

      var playerAnimations = new Dictionary<string, Animation>()
      {
        { "WalkingLeft", new Animation(Content.Load<Texture2D>("Player/WalkingLeft"), 3) },
        { "WalkingRight", new Animation(Content.Load<Texture2D>("Player/WalkingRight"), 3) },
      };

      var playerBody = _world.CreateRectangle(0.3125f, 0.46875f, 0.1f, new Vector2(1, 1.5f));
      playerBody.BodyType = BodyType.Dynamic;
      playerBody.FixedRotation = true;
      playerBody.SetFriction(0.2f);
      //playerBody.SetRestitution(0.0f);


      _sprites.Add(new Player(playerAnimations)
      {
        Colour = Color.Red,
        Body = playerBody,
        Scale = new Vector2(0.3125f, 0.46875f)
      });

      //for (int i = 0; i < 1; i++)
      {
        var position = new Vector2(0, -1.25f);
        var scale = new Vector2(8f, 1f);
        var body = _world.CreateRectangle(scale.X, scale.Y, 1f, position);
        body.BodyType = BodyType.Static;
        body.SetRestitution(0.0f);
        body.SetFriction(0.2f);

        _sprites.Add(new Sprite(square)
        {
          Body = body,
          Scale = scale,
        });
      }
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      // update camera View Projection
      var vp = GraphicsDevice.Viewport;
      var cameraZoomFactor = 12.5f / vp.Width; // zoom out to about ~12.5 meters wide
      _projection = Matrix.CreateOrthographic(vp.Width * cameraZoomFactor, vp.Height * cameraZoomFactor, 0f, -1f);

      foreach (var sprite in _sprites)
        sprite.Update(gameTime);

      PostUpdate(gameTime);

      _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

      base.Update(gameTime);
    }

    private void PostUpdate(GameTime gameTime)
    {
      //foreach (var leftSprite in _sprites)
      //{
      //  foreach (var rightSprite in _sprites)
      //  {
      //    // Don't do anything if they're the same sprite!
      //    if (leftSprite == rightSprite)
      //      continue;

      //    // Don't do anything if they're not colliding
      //    if (!leftSprite.Rectangle.Intersects(rightSprite.Rectangle))
      //      continue;

      //    leftSprite.OnCollide(rightSprite);
      //  }
      //}

      foreach (var sprite in _sprites)
        sprite.PostUpdate(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _spriteBatchEffect.Projection = _projection;
      spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, RasterizerState.CullClockwise, _spriteBatchEffect);

      foreach (var sprite in _sprites)
        sprite.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
