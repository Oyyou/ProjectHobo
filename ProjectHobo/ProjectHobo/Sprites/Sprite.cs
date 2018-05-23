using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectHobo.Managers;
using ProjectHobo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Dynamics;

namespace ProjectHobo.Sprites
{
  public class Sprite : Component
  {
    protected AnimationManager _animationManager;

    protected Dictionary<string, Animation> _animations;

    protected float _layer;

    protected float _rotation;

    protected Vector2 _scale;

    public Body Body;

    public Color Colour { get; set; }

    public float Density { get; set; }

    public float Height
    {
      get
      {
        if (Texture != null)
          return Texture.Height;
        else if (_animationManager != null)
          return _animationManager.CurrentFrameHeight;

        throw new Exception("Can't determine height");
      }
    }

    public float Layer
    {
      get { return _layer; }
      set
      {
        _layer = value;

        if (_animationManager != null)
          _animationManager.Layer = _layer;
      }
    }

    public Texture2D Texture { get; private set; }

    public float Rotation
    {
      get { return _rotation; }
      set
      {
        _rotation = value;
        Body.Rotation = _rotation;

        if (_animationManager != null)
          _animationManager.Rotation = _rotation;
      }
    }

    public Vector2 Scale
    {
      get { return _scale; }
      set
      {
        _scale = value;

        if (_animationManager != null)
          _animationManager.Scale = _scale;
      }
    }

    public float Width
    {
      get
      {
        if (Texture != null)
          return Texture.Width;
        else if (_animationManager != null)
          return _animationManager.CurrentFrameWidth;

        throw new Exception("Can't determine width");
      }
    }

    public Sprite(Dictionary<string, Animation> animations) :
      this(texture: null)
    {
      _animations = animations;
      _animationManager = new AnimationManager(_animations.FirstOrDefault().Value);
    }

    public Sprite(Texture2D texture)
    {
      Texture = texture;

      Scale = new Vector2(1f, 1f);
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void PostUpdate(GameTime gameTime)
    {

    }

    protected virtual void SetAnimation(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (_animationManager != null)
      {
        SetAnimation(gameTime);

        _animationManager.Position = Body.Position;
        _animationManager.Draw(spriteBatch);
      }
      else
      {
        spriteBatch.Draw(Texture,
          Body.Position, 
          null, 
          Colour, 
          Body.Rotation, 
          new Vector2(Texture.Width / 2, Texture.Height / 2), 
          Scale / new Vector2(Texture.Width, Texture.Height), 
          SpriteEffects.FlipVertically, 
          _layer);
      }
    }

    public virtual void OnCollide(Sprite sprite)
    {

    }
  }
}
