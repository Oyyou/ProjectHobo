using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectHobo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHobo.Managers
{
  public class AnimationManager
  {
    private Animation _animation;

    private float _timer;

    public Vector2 Position { get; set; }

    public Vector2 Scale { get; set; }

    public float Rotation { get; set; }

    public AnimationManager(Animation animation)
    {
      _animation = animation;
      Console.WriteLine(_animation.FrameWidth + "x" + _animation.FrameHeight);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_animation.Texture,
                       Position,
                       new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                     0,
                                     _animation.FrameWidth,
                                     _animation.FrameHeight),
                       Color.White,
                       Rotation,
                       new Vector2(_animation.FrameWidth / 2, _animation.FrameHeight / 2),
                       Scale / new Vector2(_animation.FrameWidth / 2, _animation.FrameHeight / 2),
                       SpriteEffects.FlipVertically,
                       0);
    }

    public void Play(Animation animation)
    {
      if (_animation == animation)
        return;

      _animation = animation;

      _animation.CurrentFrame = 0;

      _timer = 0;
    }

    public void Stop()
    {
      _timer = 0f;

      _animation.CurrentFrame = 0;
    }

    public void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer > _animation.FrameSpeed)
      {
        _timer = 0f;

        _animation.CurrentFrame++;

        if (_animation.CurrentFrame >= _animation.FrameCount)
          _animation.CurrentFrame = 0;
      }
    }
  }
}
