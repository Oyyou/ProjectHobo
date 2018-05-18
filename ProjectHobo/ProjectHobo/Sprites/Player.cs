using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectHobo.Models;

namespace ProjectHobo.Sprites
{
  public class Player : Sprite
  {
    private KeyboardState _currentKey;

    private KeyboardState _previousKey;

    private bool _onGround = false;
    private bool _hasJumped = false;

    public Vector2 Velocity;

    public Player(Texture2D texture)
      : base(texture)
    {
    }

    public Player(Dictionary<string, Animation> animations) : base(animations)
    {
    }

    public override void Update(GameTime gameTime)
    {
      _previousKey = _currentKey;
      _currentKey = Keyboard.GetState();

      float v = 0f;

      if (_currentKey.IsKeyDown(Keys.A))
      {
        //Body.ApplyLinearImpulse(new Vector2(-1f, 0));
        //Body.LinearVelocity = new Vector2(1000f, 0);
        //Body.LinearVelocity = new Vector2(-250f, Body.LinearVelocity.Y);
        Body.ApplyLinearImpulse(new Vector2(-0.01f, 0));
        _animationManager?.Play(_animations["WalkingLeft"]);
        //Body.ApplyForce(new Vector2(-0.02f, 0));
        v = -5f;
      }
      else if (_currentKey.IsKeyDown(Keys.D))
      {
        Body.ApplyLinearImpulse(new Vector2(0.01f, 0));
        _animationManager?.Play(_animations["WalkingRight"]);
        //Body.ApplyForce(new Vector2(0.02f, 0));
        v = 5f;
      }
      else
      {
        _animationManager?.Stop();
        //Body.LinearVelocity = new Vector2(0f, Body.LinearVelocity.Y);
      }
      //else
      //{
      //  Body.LinearVelocity = new Vector2(0f, Body.LinearVelocity.Y);
      //  //Body.LinearVelocity = new Vector2(0f, Body.LinearVelocity.Y);
      //}

      //Body.SetTransform(new Vector2(Body.Position.X + v, Body.Position.Y), 0f);

      _hasJumped = false;

      if ((_previousKey.IsKeyUp(Keys.Space) &&
          _currentKey.IsKeyDown(Keys.Space)) && 
          !_hasJumped)
      {
        Console.WriteLine("Jumping");
        Body.ApplyLinearImpulse(new Vector2(0, 0.05f));
        _onGround = false;
        //_hasJumped = true;
      }

      if (!_onGround)
        Velocity.Y += 0.3f;

      _animationManager?.Update(gameTime);
    }

    public override void PostUpdate(GameTime gameTime)
    {

    }

    public override void OnCollide(Sprite sprite)
    {

    }
  }
}
