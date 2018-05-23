using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectHobo.Models;
using ProjectHobo.Sprites;
using System.Collections.Generic;
using System.Linq;
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
        { "Idle", new Animation(Content.Load<Texture2D>("Player/Hobo_Idle"), 2) { FrameSpeed = 0.7f, } },
        { "Running", new Animation(Content.Load<Texture2D>("Player/Hobo_Running"), 4) },
        { "Jumping", new Animation(Content.Load<Texture2D>("Player/Hobo_Jumping"), 3) },
        { "Falling", new Animation(Content.Load<Texture2D>("Player/Hobo_Falling"), 3) },
        { "Sliding", new Animation(Content.Load<Texture2D>("Player/Hobo_Sliding"), 3) },
      };

      var players = new List<Player>()
      {
        //new Player(square),
        new Player(playerAnimations),
      };

      foreach (var player in players)
      {
        var size = new Vector2(player.Width, player.Height);
        var playerScale = new Vector2(size.X / 64, size.Y / 64);
        var density = (playerScale.X + playerScale.Y) / 10;
        System.Console.WriteLine("Size: " + size);
        System.Console.WriteLine("Scale: " + playerScale);
        System.Console.WriteLine("Density: " + density);

        var playerBody = _world.CreateRectangle(playerScale.X, playerScale.Y, density, new Vector2(1, 2.5f));
        playerBody.BodyType = BodyType.Dynamic;
        playerBody.FixedRotation = true;
        playerBody.SetFriction(0.1f);
        playerBody.SetCollisionCategories(Category.Cat20);
        playerBody.SetCollidesWith(Category.Cat1 | Category.Cat2);

        player.Body = playerBody;
        player.Density = density;
        player.Scale = playerScale;
        player.Layer = 0.5f;

        _sprites.Add(player);
      }

      var collidables = new[]
      {
        new
        {
          Position = new Vector2(0, -3),
          Scale = new Vector2(10f, 1f),
          Color = Color.Blue,
          Category = Category.Cat1,
        },
        new
        {
          Position = new Vector2(-4, 0),
          Scale = new Vector2(1f, 3f),
          Color = Color.Red,
          Category = Category.Cat2,
        },
        new
        {
          Position = new Vector2(4, 0),
          Scale = new Vector2(1f, 4f),
          Color = Color.Red,
          Category = Category.Cat2,
        },
        new
        {
          Position = new Vector2(-7, 1),
          Scale = new Vector2(5f, 1f),
          Color = Color.Red,
          Category = Category.Cat1,
        },
      };

      foreach (var collidable in collidables)
      {
        var body = _world.CreateRectangle(collidable.Scale.X, collidable.Scale.Y, 1f, collidable.Position);
        body.SetCollisionCategories(collidable.Category);
        body.BodyType = BodyType.Static;
        body.SetRestitution(0.0f);
        body.SetFriction(0.2f);

        _sprites.Add(new Sprite(square)
        {
          Body = body,
          Colour = collidable.Color,
          Scale = collidable.Scale,
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
      spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, RasterizerState.CullClockwise, _spriteBatchEffect);

      foreach (var sprite in _sprites)
        sprite.Draw(gameTime, spriteBatch);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
