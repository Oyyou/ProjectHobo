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

    protected float _rotation;

    protected Vector2 _scale;

    public Body Body;

    public Color Colour { get; set; }

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

    public Sprite(Dictionary<string, Animation> animations)
    {
      _animations = animations;
      _animationManager = new AnimationManager(_animations.FirstOrDefault().Value);

      Colour = Color.White;

      Scale = new Vector2(1f, 1f);
    }

    public Sprite(Texture2D texture)
    {
      Texture = texture;

      Colour = Color.White;

      Scale = new Vector2(1f, 1f);
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override void PostUpdate(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (_animationManager != null)
      {
        _animationManager.Position = Body.Position;
        _animationManager.Draw(spriteBatch);
      }
      else
      {
        spriteBatch.Draw(Texture, Body.Position, null, Colour, Body.Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), Scale / new Vector2(Texture.Width, Texture.Height), SpriteEffects.FlipVertically, 0f);
      }
    }

    public virtual void OnCollide(Sprite sprite)
    {

    }
  }
}
